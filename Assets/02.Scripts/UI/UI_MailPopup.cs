using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MailPopup : MonoBehaviour
{
    [SerializeField] private UI_MailItem _itemPrefab;
    [SerializeField] private Transform _content;

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private TextMeshProUGUI _rewardText;

    [SerializeField] private Button _claimeButton;
    [SerializeField] private GameObject _claimedObject;
    [SerializeField] private Button _closeButton;

    private List<MailData> _mails = new List<MailData>();
    private MailData _selectedMail = null;
    private void Awake()
    {
        _closeButton.onClick.AddListener(Close);
        _claimeButton.onClick.AddListener(ClaimAsync);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        LoadAsync().Forget();
    }
    public void Close()
    {
        gameObject?.SetActive(false);
    }

    private async UniTask LoadAsync()
    {
        _mails = await MailManager.Instance.GetMyMailsAsync(10);

        RefreshList();

        if(_mails.Count > 0 && _selectedMail == null)
        {
            SelectAsync(_mails[0]);
        }
    }

    private void RefreshList()
    {
        foreach(Transform child in _content)
        {
            Destroy(child.gameObject);
        }

        foreach(MailData mail in _mails)
        {
            UI_MailItem item = Instantiate(_itemPrefab, _content);
            item.Bind(mail, SelectAsync);
        }
    }

    private async void SelectAsync(MailData mail)
    {
        _selectedMail = mail;
        await MailManager.Instance.MarkReadAsync(mail);

        RefreshList();
        RefreshDetail();
    }

    private void RefreshDetail()
    {
        if(_selectedMail == null) return;

        _titleText.text = _selectedMail.Title;
        _messageText.text = _selectedMail.Message;
        _rewardText.text = _selectedMail.HasReward ? "보너스 점수 +" + _selectedMail.RewardScore : "보상 없음";
        
        bool canClaim = _selectedMail.HasReward && !_selectedMail.IsClaimed;
        _claimeButton.gameObject.SetActive(canClaim);
        _claimedObject.gameObject.SetActive(!canClaim);
    }

    private async void ClaimAsync()
    {
        if(_selectedMail == null || _selectedMail.IsClaimed) return;
        Debug.Log(_selectedMail.IsClaimed);

        _claimeButton.gameObject.SetActive(false);
        await MailManager.Instance.ClaimMailAsync(_selectedMail);

        RefreshDetail();
        RefreshList();
    }
}
