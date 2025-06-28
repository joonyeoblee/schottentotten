using System.Collections.Generic;
using CardGame;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class UI_RoomList : MonoBehaviourPunCallbacks
{
    public GameObject RoomPrefab;
    public GameObject RoomContainer;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log($"OnRoomListUpdate 호출됨. 방 개수: {roomList.Count}");

        foreach (RoomInfo info in roomList)
        {
            Debug.Log($"{info.Name} | RemovedFromList: {info.RemovedFromList}, Visible: {info.IsVisible}, Open: {info.IsOpen}");
        }
    }
    private void Awake()
    {
        Debug.Log("UI_RoomList: Awake 호출됨");
    }

    private void OnEnable()
    {
        Debug.Log("UI_RoomList: OnEnable 호출됨");
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PhotonNetwork.JoinLobby(); // 방 리스트를 강제로 갱신
        }
    }
}
