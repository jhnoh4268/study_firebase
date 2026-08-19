using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MailManager : MonoBehaviour
{
    private static MailManager _instance = null;
    public static MailManager Instance => _instance;

    private FirebaseMailRepository _repository;

    private List<MailData> _mails = new List<MailData>();
    public IReadOnlyList<MailData> Mails => _mails;

    public event Action OnMailChanged = null;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(_instance);
            return;
        }

        _instance = this;

        _repository = new FirebaseMailRepository();
    }


    private void Start()
    {
        CheckMailLoopAsync().Forget();
    }
    private async UniTask CheckMailLoopAsync()
    {
        while(true)
        {
            await GetMyMailsAsync(10);
            await UniTask.Delay(3000);
        }
    }

    public async UniTask<List<MailData>> GetMyMailsAsync(int count)
    {
        string userId = FirebaseAuthService.Instance.UserId;

        _mails = await _repository.GetMyMailsAsync(userId, count);
        OnMailChanged?.Invoke();

        return _mails;
    }

    public async UniTask MarkReadAsync(MailData mail)
    {
        if(mail.IsRead == true) return;
        mail.IsRead = true;

        string userId = FirebaseAuthService.Instance.UserId;
        await _repository.SaveMailAsync(userId, mail);
    }

    public async UniTask<bool> ClaimMailAsync(MailData mail)
    {
        if(mail.IsClaimed || !mail.HasReward) return false;
        mail.IsClaimed = true;

        string userId = FirebaseAuthService.Instance.UserId;
        bool saved = await _repository.SaveMailAsync(userId, mail);

        if(saved == false)
        {
            mail.IsClaimed = false;
            return false;
        }

        ApplyReward(mail);

        return true;
    }

    private void ApplyReward(MailData mail)
    {
        // 점수 지급
        GameManager.Instance.AddScore(mail.RewardScore);
    }
}
