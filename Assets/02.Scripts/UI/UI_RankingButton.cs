using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_RankingButton : MonoBehaviour
{
    [SerializeField] private UI_RankingPopUp _rankingPopup;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickRankingPopup);
    }
    private void OnClickRankingPopup()
    {
        _rankingPopup.Open();
    }
}
