using TMPro;
using UnityEngine;

public class UI_RankingItem : MonoBehaviour
{
    [SerializeField ] private TextMeshProUGUI _rankText;
    [SerializeField ] private TextMeshProUGUI _nicknameText;
    [SerializeField ] private TextMeshProUGUI _scoreText;

    public void Bind(int rank, RankingData data)
    {
        _rankText.text = rank.ToString();
        _nicknameText.text = data.Nickname;
        _scoreText.text = data.Bestscore.ToString("N0");
    }
}
