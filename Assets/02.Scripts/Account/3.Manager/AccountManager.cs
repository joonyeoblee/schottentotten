using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountManager : Singleton<AccountManager>
{
    public Account CurrentAccount { get; private set; }

    public void TryLogin(Account account)
    {
        CurrentAccount = account;
        Debug.Log(CurrentAccount.Nickname);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("네임 서버 접속 완료");
        Debug.Log(PhotonNetwork.CloudRegion);

        SceneManager.LoadScene(1);
    }
}