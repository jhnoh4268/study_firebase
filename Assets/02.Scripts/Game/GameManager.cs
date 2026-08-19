using Cysharp.Threading.Tasks;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance => _instance;

    private GameConfig _config = new GameConfig();
    public GameConfig Config => _config;

    private FirebaseGameDetaRepository _repository = null;
    private FirebaseRemoteConfigService _remoteConfigService = null;

    private async void Awake()
    {
        if(_instance != null)
        {
            Destroy(_instance);
            return;
        }

        _instance = this;
        _repository = new FirebaseGameDetaRepository();

        GameData data = await _repository.LoadAsync(FirebaseAuthService.Instance.UserId);

        if(data != null)
        {
            _bestScore = data.BestScore;
            OnScoreChanged?.Invoke();
        }
        else
        {
            Debug.LogError("데이터 로드 실패");
        }

    }

    private int _score;
    public int Score => _score;

    private int _bestScore;
    public int BestScore => _bestScore;

    public Action OnScoreChanged = null;

    private async void Start()
    {
        _remoteConfigService = new FirebaseRemoteConfigService();
        _config = await _remoteConfigService.FetchDataAsync();
        _remoteConfigService.OnCofigUpdated += ReadRemoteData;
        _remoteConfigService.StartListenering();

        Debug.Log("ScoreMultiplier" + _config.ScoreMultiplier);
        Debug.Log("DropCooldown" + _config.DropCooldown);
    }

    // 데이터가 변경될때마다 자동으로 호출
    private async void ReadRemoteData()
    {
        _config = await _remoteConfigService.ReadDataAsync();

        Debug.Log("ScoreMultiplier" + _config.ScoreMultiplier);
        Debug.Log("DropCooldown" + _config.DropCooldown);
    }

    public void AddScore(int value)
    {
        _score += value;
        if(_score > BestScore)
        {
            _bestScore = _score;

            _repository.SaveAsync(FirebaseAuthService.Instance.UserId, new GameData() { BestScore = _bestScore }).Forget();
            RankingManager.Instance.SaveRankingAysnc(_bestScore).Forget();
        }

        Debug.Log($"현재 점수 {_score} 최고 점수 {_bestScore}");
        OnScoreChanged?.Invoke();
    }


    

    private void OnDestroy()
    {
        _remoteConfigService.DestroyListenering();
    }
}
