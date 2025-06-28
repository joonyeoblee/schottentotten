using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DebugRoomCreator : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            Debug.Log("!!!!!!!!!!!!!!!");
            string name = "AutoRoom_" + Random.Range(0, 1000);
            PhotonNetwork.CreateRoom(name, new RoomOptions
            {
                MaxPlayers = 4,
                IsOpen = true,
                IsVisible = true
            });
        }
    }

}