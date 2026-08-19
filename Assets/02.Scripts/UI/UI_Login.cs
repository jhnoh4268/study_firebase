using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Login : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private Button _emailLoginButton;

    private void Start()
    {
        _messageText.text = "";

        _emailLoginButton.onClick.AddListener(EmailLogin);
    }

    private async void EmailLogin()
    {
        string email = _emailInputField.text;
        string password = _passwordInputField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            _messageText.text = "이메일과 비밀번호를 확인해주세요!";
            return;
        }

        // 파이어 베이스 초기화, 로그인
        bool loginResult = await FirebaseAuthService.Instance.LoginOrRigisterAsync(email, password);

        if(loginResult)
        {
            if(string.IsNullOrEmpty(FirebaseAuthService.Instance.NickName))
            {
                await FirebaseAuthService.Instance.SetNickAsync(MakeRandomNickname());
                Debug.Log("유저 닉네임: " + FirebaseAuthService.Instance.NickName);
            }
            SceneManager.LoadScene(1);

        }
        else
        {
            _messageText.text = "이메일과 비밀번호를 확인해주세요!";
        }
    }

    private string MakeRandomNickname()
    {
        string[] adjectives = {
        "달콤한", "상큼한", "푸른", "말랑한", "용감한",
        "빛나는", "귀여운", "행복한", "즐거운", "시원한"
        };

        string[] nouns = {
        "수박", "딸기", "고양이", "강아지", "호랑이",
        "바람", "구름", "별빛", "사과", "토끼"
        };

        string randomAdjective = adjectives[Random.Range(0, adjectives.Length)];
        string randomNoun = nouns[Random.Range(0, nouns.Length)];
        int randomNumber = Random.Range(1000, 10000);

        return $"{randomAdjective}{randomNoun}{randomNumber}";
    }
}
