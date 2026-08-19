using UnityEngine;
using UnityEngine.UI;

public class UI_BoardButton : MonoBehaviour
{
    [SerializeField] private UI_BoardPopup _boardPopup;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickBoardPopup);
    }
    private void OnClickBoardPopup()
    {
        _boardPopup.Open();
    }
}
