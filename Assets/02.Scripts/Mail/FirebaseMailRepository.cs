using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseMailRepository
{
    private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    private const string UserCollection = "users";
    private const string MailCollection = "mails";

    // 우편조회
    public async UniTask<List<MailData>> GetMyMailsAsync(string userId, int count)
    {
        CollectionReference collectionReference = _db.Collection(UserCollection).Document(userId).Collection(MailCollection);
        List<MailData> mails = new();

        DateTime now = DateTime.UtcNow;
        try
        {
            // BestScore, UpdatedAt 기준으로 정렬된 상위 10개의 데이터 읽어오기
            Query query = collectionReference
                //.WhereGreaterThan("ExpireAt", now)
                .OrderByDescending("CreatedAt")
                .Limit(count);


            QuerySnapshot snapshot = await query.GetSnapshotAsync().AsUniTask();

            foreach(DocumentSnapshot document in snapshot)
            {
                MailData mail = document.ConvertTo<MailData>();
                if(mail.ExpiresAt.ToDateTime() > now)
                {
                    mails.Add(mail);
                }
            }

            Debug.Log("메일 조회성공");
            return mails;
        } catch(Exception e)
        {
            Debug.LogError("메일 조회실패" + e.Message);
            return mails;
        }
    }

    // 우편수정

    public async UniTask<bool> SaveMailAsync(string userId, MailData mail)
    {
        try
        {
            await _db.Collection(UserCollection).Document(userId)
                .Collection(MailCollection).Document(mail.Id)
                .SetAsync(mail).AsUniTask();

            Debug.Log("우편 수정 성공");
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError("우편 수정 실패: " + e.Message);
            return false;
        }
    }
}
