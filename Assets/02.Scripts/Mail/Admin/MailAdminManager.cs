using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System;
using UnityEngine;

public class MailAdminManager : MonoBehaviour
{
    private static MailAdminManager _instance;
    public static MailAdminManager Instance => _instance;

    private FireStoreAdminRepository _repository;
    private string _adminEmail = "admin@subak.com";
    private string _adminPassword = "123456";
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _repository = new FireStoreAdminRepository();
    }


    private async void Start()
    {
        await UniTask.WaitUntil(() => FirebaseInitialize.IsReady);
        await UniTask.Delay(1000);

        bool result = await FirebaseAuthService.Instance.LoginOrRigisterAsync(_adminEmail, _adminPassword);

        Debug.Log(result ? "관리자 로그인 성공" : "관리자 로그인 실패");
        
    }

    public async UniTask<bool> SendMailAsync(string targetUserId, string title, string message,
        int rewardScore, int expiresDays)
    {
        DateTime expiresAt = expiresDays > 0 ? DateTime.UtcNow.AddDays(expiresDays) : DateTime.UtcNow.AddYears(100);

        MailData mail = new MailData()
        {
            Title = title,
            Message = message,
            RewardScore = rewardScore,
            CreatedAt = Timestamp.GetCurrentTimestamp(),
            ExpiresAt = Timestamp.FromDateTime(expiresAt),
        };

        return await _repository.CreateMailAsync(targetUserId, mail);
    }
}
