using TMPro;
using UnityEngine;

public class UI_Scores : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bestScoreTextUI;
    [SerializeField] private TextMeshProUGUI _currentScoreTextUI;

    private void Start()
    {
        Refresh();
        GameManager.Instance.OnScoreChanged += Refresh;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnScoreChanged -= Refresh;
    }

    private void Refresh()
    {
        _bestScoreTextUI.text = $"최고 점수: {GameManager.Instance.BestScore}";
        _currentScoreTextUI.text = $"현재 점수: {GameManager.Instance.Score}";
    }
}
