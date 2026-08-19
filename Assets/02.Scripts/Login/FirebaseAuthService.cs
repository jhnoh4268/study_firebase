using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Crashlytics;
using System;
using UnityEngine;

public class FirebaseAuthService : MonoBehaviour
{
    private static FirebaseAuthService _instance;
    public static FirebaseAuthService Instance => _instance;

    private FirebaseAuth _auth = null;
    public string UserId => _auth.CurrentUser.UserId;
    public string NickName => _auth.CurrentUser.DisplayName;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        Debug.Log("firebaseauth instance 할당");
        DontDestroyOnLoad(gameObject);
    }


    private async void Start()
    {
        await UniTask.WaitUntil(() => FirebaseInitialize.IsReady);
        _auth = FirebaseAuth.DefaultInstance;

        Debug.Log("firebase auth 준비완료");
    }

    private async UniTask<bool> RegisterAsync(string email, string password)
    {
        try
        {
            AuthResult result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
            Debug.LogFormat("회원가입 성공: {0} ({1})", result.User.DisplayName, result.User.UserId);

            return true;
        } catch(Exception e)
        {
            Debug.LogError("회원가입 실패 " + e.Message);
            return false;
        }
    }

    public async UniTask<bool> LoginAsync(string email, string password)
    {
        try
        {
            AuthResult result = await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            Debug.LogFormat("로그인 성공: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

            // 크레이틱스 유저설정
            Crashlytics.SetUserId(result.User.UserId);
            return true;
        } catch(Exception e)
        {
            Debug.LogError("로그인 실패: " + e.Message);
            return false;
        }
    }

    public async UniTask<bool> LoginOrRigisterAsync(string email, string password)
    {
        bool loginSuccess = await LoginAsync(email, password);
        if(loginSuccess)
            return true;

        bool register = await RegisterAsync(email, password);
        if(!register)
            return false;

        return await LoginAsync(email, password);
    }

    public void LogOut()
    {
        _auth.SignOut();
        Debug.Log("로그아웃");
    }

    public async UniTask SetNickAsync(string nickName)
    {
        try
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = nickName
            };
            await _auth.CurrentUser.UpdateUserProfileAsync(profile).AsUniTask();
            Debug.Log("유저 닉네임 설정완료");
        }
        catch(Exception e)
        {
            Debug.LogError("유저 닉네임 설정 실패: " + e);
        }
    }
}
