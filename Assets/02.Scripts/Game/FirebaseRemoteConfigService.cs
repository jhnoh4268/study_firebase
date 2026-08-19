using Cysharp.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseRemoteConfigService
{
    private readonly FirebaseRemoteConfig _remoteConfig = FirebaseRemoteConfig.DefaultInstance;
    public event Action OnCofigUpdated = null;

    // 매개변수 기본값 설정
    private async UniTask SetDefaultAsync()
    {
        GameConfig defaultConfig = new GameConfig();
        Dictionary<string, object> defaults = new Dictionary<string, object>();


        defaults.Add(nameof(defaultConfig.ScoreMultiplier), defaultConfig.ScoreMultiplier);
        defaults.Add(nameof(defaultConfig.DropCooldown), defaultConfig.DropCooldown);

        await Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).AsUniTask();
    }

    // 사용할 매개변수 값 가져오기
    private GameConfig ReadConfig()
    {
        float scoreMultiplier = (float)_remoteConfig.GetValue(nameof(GameConfig.ScoreMultiplier)).DoubleValue;
        float dropCooldown = (float)_remoteConfig.GetValue(nameof(GameConfig.DropCooldown)).DoubleValue;

        return new GameConfig(scoreMultiplier, dropCooldown);
    }

    // 값 가져오기 및 활성화

    public async UniTask<GameConfig> FetchDataAsync()
    {
        Debug.Log("RemoteConfig 값 가져오는중...");

        try
        {
            await SetDefaultAsync();
            await _remoteConfig.FetchAsync(TimeSpan.Zero);
            await _remoteConfig.ActivateAsync().AsUniTask();

            Debug.Log("RemoteConfig 값 가져오기 성공");
        } catch(Exception e)
        {
            Debug.LogError("RemoteConfig 값 가져오기 실패" + e.Message);
        }

        return ReadConfig();
    }

    public async UniTask<GameConfig> ReadDataAsync()
    {
        Debug.Log("RemoteConfig 값 가져오는중...");

        try
        {
            await _remoteConfig.ActivateAsync().AsUniTask();

            Debug.Log("RemoteConfig 값 가져오기 성공");
        } catch(Exception e)
        {
            Debug.LogError("RemoteConfig 값 가져오기 실패" + e.Message);
        }

        return ReadConfig();
    }

    //데이터가 변경될때 실시간으로 호출되는 이벤트/콜백 메서드
    void ConfigUpdateListenerEventHandler(object sender, ConfigUpdateEventArgs args) 
    {
        if(args.Error != RemoteConfigError.None)
        {
            Debug.LogError(String.Format("Remote config 업데이트 실패: {0}", args.Error));
            return;
        }

        Debug.Log("Remote config 업데이트 키: " + string.Join(", ", args.UpdatedKeys));

        // 데이터 다시 읽어오기
        OnCofigUpdated?.Invoke();
    }

    public void StartListenering()
    {
        FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener
          += ConfigUpdateListenerEventHandler;
    }

    public void DestroyListenering()
    {
        FirebaseRemoteConfig.DefaultInstance.OnConfigUpdateListener
          -= ConfigUpdateListenerEventHandler;
    }
}
