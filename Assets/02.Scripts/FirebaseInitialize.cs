using Cysharp.Threading.Tasks;
using Firebase.Crashlytics;
using System;
using UnityEngine;

public class FirebaseInitialize : MonoBehaviour
{
    private static bool _isReady = false;
    public static bool IsReady => _isReady;


    private async void Start()
    {

        await CheckFirebaseDependenciesAsync();
    }
    private async UniTask CheckFirebaseDependenciesAsync()
    {
        Debug.Log("Firebase 초기화 시작");

        var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
        if(dependencyStatus == Firebase.DependencyStatus.Available)
        {
            Debug.Log("Firebase 초기화 성공");
            _isReady = true;

            // 크레이틱스는 기본적으로 앱이 종류되는 Fatal한 에러만 보고
            // 개발할때 실수하는 예외(outOfRange, nullRef...)도 에러로 처리;
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;

            throw new Exception("테스트용 예외");
        } else
        {
            UnityEngine.Debug.LogError(System.String.Format(
              "Firebase 초기화 실패: {0}", dependencyStatus));
        }
    }
}
