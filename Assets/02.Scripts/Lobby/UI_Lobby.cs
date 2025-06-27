using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Lobby : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI NickNameTextUI;
    public TMP_InputField RoomNameInputField;
    public TMP_InputField PasswordInputField;

    public GameObject RoomPanel;

    public GameObject RoomPrefab;

    private void Start()
    {
        NickNameTextUI.text = AccountManager.Instance.CurrentAccount.Nickname;
    }

    public void OpenRoomPanel()
    {
        RoomPanel.SetActive(true);
    }

    public void CloseRoomPanel()
    {
        RoomPanel.SetActive(false);
    }

    public void CreateRoom()
    {
        Room room = new Room(RoomNameInputField.text + Time.deltaTime, PasswordInputField.text, ERoomState.Waiting);
        PhotonNetwork.CreateRoom(RoomNameInputField.text + Time.deltaTime,
            new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
        CloseRoomPanel();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        SceneManager.LoadScene(2);
    }
}