using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_MailButton : MonoBehaviour
{
    [SerializeField] private UI_MailPopup popup;
    [SerializeField] private GameObject _redDotObject;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenPopup);
        MailManager.Instance.OnMailChanged += Refresh;
    }

    private void OnDestroy()
    {
        MailManager.Instance.OnMailChanged -= Refresh;
    }

    private void OpenPopup()
    {
        popup.Open();
    }

    private void Refresh()
    {
        IReadOnlyList<MailData> mails = MailManager.Instance.Mails;

        _redDotObject.SetActive(false);
        foreach (MailData mail in mails)
        {
            if(!mail.IsRead)
            {
                _redDotObject.SetActive(true);
                break;
            }
        }
    }
}
