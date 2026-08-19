using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FireStoreAdminRepository
{
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

    public async UniTask<bool> CreateMailAsync(string targetUserId, MailData mailData)
    {
        try
        {
            // 어디에 저장할지
            // 서브콜렉션: 문서안에 또다른 콜렉션(중첩 구조)
            CollectionReference colRef = _db.Collection("users").Document(targetUserId).Collection("mails");

            await colRef.AddAsync(mailData).AsUniTask();
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError("우편 등록 실패: " + e.Message);

            return false;
        }
    }
}
