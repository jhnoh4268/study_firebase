using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PostItem : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _commentText;
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _deleteButton;


    public void Bind(PostData data, Action<PostData> onEdit, Action<PostData> onDelete)
    {
        _nicknameText.text = data.Nickname;
        _timeText.text = data.CreatedAt.ToDateTime().ToLocalTime().ToString("MM-dd HH:mm");
        _commentText.text = data.Comment;

        bool isAuthor = BoardManager.Instance.IsAuthor(data);
        _editButton.gameObject.SetActive(isAuthor);
        _deleteButton.gameObject.SetActive(isAuthor);

        _editButton.onClick.AddListener(() => onEdit?.Invoke(data));
        _deleteButton.onClick.AddListener(() => onDelete?.Invoke(data));

    }
}
