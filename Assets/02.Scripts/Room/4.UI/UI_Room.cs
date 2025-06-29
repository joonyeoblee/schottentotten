using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Room : MonoBehaviour
{
    public TextMeshProUGUI RoomTitleTextUI;
    public TextMeshProUGUI RoomPersonTextUI;
    public TextMeshProUGUI RoomStatusTextUI;

    private Room _myRoom;

    public void Refresh(Room myRoom)
    {
        _myRoom = myRoom;
        string[] RoomTitleTexts = _myRoom.RoomTitle.Split("_");
        RoomTitleTextUI.text = RoomTitleTexts[0];
        RoomPersonTextUI.text = $"{_myRoom.CurrentPlayers} / {_myRoom.MaxPlayers}";

        RoomStatusTextUI.text = ChangeStateToKR(_myRoom.RoomState);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_myRoom.RoomTitle);
    }

    private string ChangeStateToKR(ERoomState roomState)
    {
        switch (roomState)
        {
            case ERoomState.Waiting:
                return "대기 중";
            case ERoomState.Playing:
                return "게임 중";
            default:
                return "잘못된 상태";
        }
    }
}
