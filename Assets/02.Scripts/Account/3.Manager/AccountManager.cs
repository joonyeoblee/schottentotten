using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountManager : Singleton<AccountManager>
{
    public Account CurrentAccount { get; private set; }

    public void TryLogin(Account account)
    {
        CurrentAccount = account;
        Debug.Log($"로그인 시도: {CurrentAccount.Nickname}");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("네임 서버 접속 완료");
        Debug.Log($"클라우드 리전: {PhotonNetwork.CloudRegion}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 연결되었습니다.");
        Debug.Log($"현재 상태: {PhotonNetwork.NetworkClientState}");
        Debug.Log($"로비 참가 여부: {PhotonNetwork.InLobby}");

        // 로비에 자동으로 참가
        bool joinResult = PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log($"로비 참가 요청 결과: {joinResult}");
    }

    public override void OnJoinedLobby()
    {
        // 씬 로드
        SceneManager.LoadScene(1);
        Debug.Log("로비에 참가했습니다.");
        Debug.Log($"현재 상태: {PhotonNetwork.NetworkClientState}");
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"로비 참가 실패: {returnCode} - {message}");
    }
}