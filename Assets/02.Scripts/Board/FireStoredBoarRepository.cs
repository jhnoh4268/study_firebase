using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStoreBoardRepository
{
    private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    private const string CollectionName = "posts";
    public async UniTask<bool> CreatePostAsync(PostData data)
    {
        try
        {
            await _db.Collection("posts").AddAsync(data).AsUniTask();
            Debug.Log("게시글 작성완료");
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError("게시글 작성실패: " + e.Message);
            return false;
        }
    }

    public async UniTask<List<PostData>> GetLatestAsync(int count)
    {
        CollectionReference collectionReference = _db.Collection(CollectionName);
        List<PostData> postDatas = new List<PostData>();

        try
        {
            // BestScore, UpdatedAt 기준으로 정렬된 상위 10개의 데이터 읽어오기
            Query query = collectionReference
                .OrderByDescending("CreatedAt")
                .Limit(count);

            QuerySnapshot snapshot = await query.GetSnapshotAsync().AsUniTask();

            foreach(DocumentSnapshot document in snapshot)
            {
                postDatas.Add(document.ConvertTo<PostData>());
            }

            return postDatas;
        }
        catch( Exception e )
        {
            Debug.LogError("게시글 조회실패: " + e.Message);
            return postDatas;
        }
    }

    public async UniTask<bool> UpdatedPostAsync(string postId, string comment)
    {
        try
        {
            DocumentReference documentReference = _db.Collection(CollectionName).Document(postId);
            await documentReference.UpdateAsync("Comment", comment).AsUniTask();
            Debug.Log("게시글 수정완료");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("게시글 수정실패: " + e.Message);
            return false;
        }
    }

    public async UniTask<bool> DeletePostAsync(string postId)
    {
        try
        {
            DocumentReference documentReference = _db.Collection(CollectionName).Document(postId);
            await documentReference.DeleteAsync().AsUniTask();
            Debug.Log("게시글 삭제완료");
            return true;
        } catch(Exception e)
        {
            Debug.LogError("게시글 삭제실패: " + e.Message);
            return false;
        }
    }
}
