using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseRankingDataStored
{
    private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    private const string CollectionName = "rankings";

    public async UniTask SaveAsync(string userID, RankingData gameData)
    {
        try
        {
            // 저장할위치
            DocumentReference docRef = _db.Collection(CollectionName).Document(userID);

            await docRef.SetAsync(gameData).AsUniTask();

            Debug.Log("랭킹 저장 성공");
        } catch(Exception)
        {
            Debug.Log("랭킹 저장 실패");

        }
    }

    public async UniTask<List<RankingData>> GetTopRankingAsync(int count)
    {
        CollectionReference collectionReference = _db.Collection(CollectionName);
        List<RankingData> rankingDatas = new List<RankingData>();

        try
        {
            // BestScore, UpdatedAt 기준으로 정렬된 상위 10개의 데이터 읽어오기
            Query query = collectionReference
                .OrderByDescending("Bestscore")
                .OrderBy("UpdatedAt")
                .Limit(count);


            QuerySnapshot snapshot = await query.GetSnapshotAsync().AsUniTask();
            
            foreach(DocumentSnapshot document in  snapshot)
            {
                rankingDatas.Add(document.ConvertTo<RankingData>());
            }

            return rankingDatas;
        } catch(Exception e)
        {
            Debug.LogError("조회실패" + e.Message);
            return rankingDatas;
        }
    }

    public async UniTask<RankingData> GetMyRankingAsync(string userID)
    {
        DocumentReference document = _db.Collection(CollectionName).Document(userID);

        try
        {
            DocumentSnapshot snapshot = await document.GetSnapshotAsync().AsUniTask();
            if(!snapshot.Exists)
            {
                return snapshot.ConvertTo<RankingData>();

            }

            return null;
        }
        catch(Exception e)
        {
            Debug.LogError("조회실패" + e.Message);
            return null;
        }
    }

    public async UniTask<int> GetMyRankPositionAsync(int bestScore)
    {
        CollectionReference collectionReference = _db.Collection(CollectionName);

        try
        {
            Query query = collectionReference
                .WhereGreaterThan("Bestscore", bestScore);

            AggregateQuerySnapshot snapshot = await query.Count.GetSnapshotAsync(AggregateSource.Server).AsUniTask();

            return (int)snapshot.Count + 1;
        } catch(Exception e)
        {
            Debug.LogError("내 등수계산 실패" + e.Message);
            return 0;
        }
    }

}
