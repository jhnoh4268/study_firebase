using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseGameDetaRepository
{
    private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

    public async UniTask SaveAsync(string userID, GameData gameData)
    {
        try
        {
            // 저장할위치
            DocumentReference docRef = _db.Collection("users").Document(userID);

            await docRef.SetAsync(gameData).AsUniTask();
        } catch(Exception e)
        {
            Debug.Log("저장 실패");

        }
    }

    public async UniTask<GameData> LoadAsync(string userID)
    {
        try
        {
            DocumentReference docRef = _db.Collection("users").Document(userID);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync().AsUniTask();

            if(!snapshot.Exists)
            {
                Debug.Log("문서가 없습니다.");
                return new GameData();
            }

            return snapshot.ConvertTo<GameData>();
        } catch(Exception e)
        {
            Debug.LogError("조회실패" + e.Message);
            return null;
        }
    }
}
