using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private static BoardManager _instance;
    public static BoardManager Instance => _instance;

    private FireStoreBoardRepository _repository;
    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(_instance);
            return;
        }

        _instance = this;

        _repository = new FireStoreBoardRepository();
    }

    public UniTask<bool> CreatePostAsync(string comment)
    {

        PostData postData = new PostData()
        {
            Comment = comment,
            UserId = FirebaseAuthService.Instance.UserId,
            Nickname = FirebaseAuthService.Instance.NickName,
            CreatedAt = Timestamp.GetCurrentTimestamp()
        };

        return _repository.CreatePostAsync(postData);
    }

    public UniTask<List<PostData>> GetLatestPostAsync(int count)
    {
        return _repository.GetLatestAsync(count);
    }

    public bool IsAuthor(PostData data)
    {
        return data.UserId == FirebaseAuthService.Instance.UserId;
    }

    public async UniTask<bool> UpdatedPostAsync(PostData post, string comment)
    {
        if(!IsAuthor(post))
        {
            Debug.LogWarning("본인 게시글만 수정 할수있습니다!");
            return false;
        }

        return await _repository.UpdatedPostAsync(post.postId, comment);
    }

    public async UniTask<bool> DeletePostAsync(PostData post)
    {
        if(!IsAuthor(post))
        {
            Debug.LogWarning("본인 게시글만 삭제 할수있습니다!");
            return false;
        }

        return await _repository.DeletePostAsync(post.postId);
    }

}
