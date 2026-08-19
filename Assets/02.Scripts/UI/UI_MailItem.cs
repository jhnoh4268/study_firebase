using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MailItem : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject _newTag;
    [SerializeField] private GameObject _readTag;
    [SerializeField] private GameObject _claimedTag;

    private MailData _mail = null;
    private Action<MailData> _onClick = null;

    private void Awake()
    {
        _button.onClick.AddListener(() => _onClick(_mail));
    }
    public void Bind(MailData mail, Action<MailData> onClick)
    {
        _mail = mail;
        _onClick = onClick;

        _titleText.text = mail.Title;
        _newTag.SetActive(!mail.IsRead);
        _readTag.SetActive(mail.IsRead);
        _claimedTag.SetActive(mail.IsClaimed);
    }
}
