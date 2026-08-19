using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MailAdminTool : MonoBehaviour
{
    [SerializeField] private TMP_InputField _titleInput;
    [SerializeField] private TMP_InputField _messageInput;
    [SerializeField] private TMP_InputField _rewardInput;
    [SerializeField] private TMP_InputField _targetUserInput;
    [SerializeField] private TMP_InputField _expiresDaysInput;

    [SerializeField] private Button _createMailButton;
    [SerializeField] private TextMeshProUGUI _resultText;

    private void Awake()
    {
        _createMailButton.onClick.AddListener(CreateMailAsync);
        ClearInputs();
    }

    private async void CreateMailAsync()
    {
        string title = _titleInput.text.Trim();
        string message = _messageInput.text.Trim();
        string targetUserId = _targetUserInput.text.Trim();

        int.TryParse(_rewardInput.text, out int rewardScore);
        int.TryParse(_expiresDaysInput.text, out int expiresDays);

        if(string.IsNullOrEmpty(title) || string.IsNullOrEmpty(message))
        {
            _resultText.text = "제목과 내용을 입력해주세요!";
            return;
        }

        if(string.IsNullOrEmpty(targetUserId))
        {
            _resultText.text = "받는 유저의 UID를 입력해주세요.";
            return;
        }

        bool success = await MailAdminManager.Instance.SendMailAsync(targetUserId, title, message, rewardScore, expiresDays);

        if(success)
        {
            _resultText.text = "우편을 보냈습니다!";
            ClearInputs();
        }
        else
        {
            _resultText.text = "우편을 보내기에 실패했습니다.";
        }
    }

    private void ClearInputs()
    {
        _titleInput.text = "";
        _messageInput.text = "";
        _targetUserInput.text = "";
        _rewardInput.text = "0";
        _resultText.text = "0";
    }
}
