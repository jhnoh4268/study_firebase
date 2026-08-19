using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BoardPopup : MonoBehaviour
{
    [SerializeField] private TMP_InputField _commentInput;
    [SerializeField] private Button _summitButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private UI_PostItem _itemPrefab;
    [SerializeField] private Transform _listRoot;

    private const int PageSize = 20;

    private PostData _editingPost = null;

    private void Awake()
    {
        _summitButton.onClick.AddListener(CreateOrUpdatePostAsync);
        _closeButton.onClick.AddListener(Close);
        _cancelButton.onClick.AddListener(ExitEdit);

        ExitEdit();
        gameObject.SetActive(false);
    }

    public async void Open()
    {
        gameObject.SetActive(true);
        await LoadPostAsync();
    }

    public void Close()
    {
        gameObject.SetActive(false);

    }
    private async UniTask LoadPostAsync()
    {

        List<PostData> posts = await BoardManager.Instance.GetLatestPostAsync(PageSize);

        foreach(UI_PostItem item in _listRoot.GetComponentsInChildren<UI_PostItem>())
        {
            Destroy(item.gameObject);
        }

        foreach(PostData data in posts)
        {
            UI_PostItem item = Instantiate(_itemPrefab, _listRoot);
            item.Bind(data, StartEdit, DeletePost);
        }
    }

    public async void CreateOrUpdatePostAsync()
    {

        string comment = _commentInput.text.Trim();
        if(string.IsNullOrEmpty(comment)) return;

        _summitButton.interactable = false;

        bool success;
        if(_editingPost == null)
        {
            success = await BoardManager.Instance.CreatePostAsync(comment);

        }
        else
        {
            success = await BoardManager.Instance.UpdatedPostAsync(_editingPost, comment);
        }
        if(success)
        {
            _commentInput.text = string.Empty;
            ExitEdit();
            await LoadPostAsync();

        }

        _summitButton.interactable = true;
    }

    private void StartEdit(PostData postData)
    {
        _editingPost = postData;
        _commentInput.text = postData.Comment;
        _cancelButton.gameObject.SetActive(true);
    }

    private void ExitEdit()
    {
        _editingPost = null;
        _commentInput.text = string.Empty;
        _cancelButton.gameObject.SetActive(false);
    }

    private async void DeletePost(PostData postData)
    {
        await BoardManager.Instance.DeletePostAsync(postData);
        await LoadPostAsync();
    }
}
