using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    private static RankingManager _instance;
    public static RankingManager Instance => _instance;

    // 캐싱: 자주 사용하는 데이터를 가까운곳에두고 일정시간동안 재사용하는 기법
    private float _cacheSeconds = 30f;
    private float _nextFetchTime = 0f;

    private FirebaseRankingDataStored _rankingDataRepository = null;

    private List<RankingData> _topRankings = new List<RankingData>();
    public IReadOnlyList<RankingData> TopRankings => _topRankings;

    private RankingData _myRanking = null;
    public RankingData MyRanking => _myRanking;

    private int _myRankingPosition = -1;
    public int MyRankingPosition => _myRankingPosition;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        _rankingDataRepository = new FirebaseRankingDataStored();
    }

    public async UniTask RefreshAsync(int topCount)
    {
        if(Time.realtimeSinceStartup < _nextFetchTime) return;

        _topRankings = await _rankingDataRepository.GetTopRankingAsync(topCount);
        _myRanking = await _rankingDataRepository.GetMyRankingAsync(FirebaseAuthService.Instance.UserId);

        if(_myRanking != null)
        {
            _myRankingPosition = await _rankingDataRepository.GetMyRankPositionAsync(_myRanking.Bestscore);
        }

        _nextFetchTime = Time.realtimeSinceStartup + _cacheSeconds;
    }

    public async UniTask SaveRankingAysnc(int bestScore)
    {
        RankingData rankingData = new RankingData()
        {
            Nickname = FirebaseAuthService.Instance.NickName,
            Bestscore = bestScore,
            UpdatedAt = Timestamp.GetCurrentTimestamp()
        };

        await _rankingDataRepository.SaveAsync(FirebaseAuthService.Instance.UserId, rankingData);

        _nextFetchTime = 0;
    }
}
