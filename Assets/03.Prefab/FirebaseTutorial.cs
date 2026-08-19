using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseTutorial : MonoBehaviour
{
    private bool _isFirebaseReady = false;
    private FirebaseAuth _auth = null;
    private FirebaseFirestore _db = null;


    private async void Start()
    {
        // 동기: 작업이 완료된후 다음작업실행
        // 비동기: 작업을 시켜놓고 다른작업실행

        await CheckFirebaseDependenciesAsync();

        LogOut();
        await RegisterAsync("test2@test.com", "123456");
        await LoginAsync("test2@test.com", "123456");
        await SaveAsync();
        await LoadAsync();
    }


    private void Update()
    {
        if(!_isFirebaseReady)
            return;

    }

    private async UniTask CheckFirebaseDependenciesAsync()
    {
        Debug.Log("Firebase 초기화 시작");
        
        try
        {
            var dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();
            if(dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Firebase 초기화 성공");
                _isFirebaseReady = true;

                _auth = FirebaseAuth.DefaultInstance;
                _db = FirebaseFirestore.DefaultInstance;
            } else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Firebase 초기화 실패: {0}", dependencyStatus));
            }
        }
        catch(Exception e)
        {
            Debug.LogError("초기화 실패: " + e.Message);
        }
    }

    private async UniTask RegisterAsync(string email, string password)
    {
        try
        {
            AuthResult result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
            Debug.LogFormat("회원가입 성공: {0} ({1})", result.User.DisplayName, result.User.UserId);
        }
        catch(Exception e)
        {
            Debug.LogError("회원가입 실패 " + e.Message);
        }
    }

    private async UniTask LoginAsync(string email, string password)
    {
        try
        {
            AuthResult result = await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            Debug.LogFormat("로그인 성공: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
        }
        catch (Exception e)
        {
            Debug.LogError("로그인 실패: " + e.Message);
        }
    }

    private void LogOut()
    {
        _auth.SignOut();
        Debug.Log("로그아웃");
    }

    private async UniTask SaveAsync()
    {
        try
        {
            // 저장할위치
            DocumentReference docRef = _db.Collection("students").Document("test");

            // 저장할데이터
            Dictionary<string, object> student = new Dictionary<string, object>
            {
                { "Age", "22" },
                { "Grade", "2" },
                { "Score", "100" }
            };

            await docRef.SetAsync(student).AsUniTask();
        }
        catch(Exception e)
        {
            Debug.Log("저장 실패");

        }
    }

    private async UniTask LoadAsync()
    {
        try
        {
            DocumentReference docRef = _db.Collection("students").Document("test");
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync().AsUniTask();

            if(!snapshot.Exists)
            {
                Debug.Log("문서가 없습니다.");
                return;
            }

            string age = snapshot.GetValue<string>("Age");
            string grade = snapshot.GetValue<string>("Grade");
            string score = snapshot.GetValue<string>("Score");
            Debug.Log($"Age: {age}, Grade: {grade}, Score: {score}");
        }
        catch(Exception e)
        {
            Debug.LogError("조회실패" + e.Message);
        }
    }
}
