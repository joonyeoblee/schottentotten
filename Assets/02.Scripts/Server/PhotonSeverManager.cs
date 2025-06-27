using Photon.Pun;

// Photon API 네임스페이

// 역할: 포톤 서버 관리자(서버 연결, 로비 입장, 방 입장, 게임 입장)
public class PhotonSeverManager : MonoBehaviourPunCallbacks
{
    // MonoBehaviourPunCallbacks : 유니티 이벤트 말고도 Pun 서버 이벤트를 받을 수 있다.
    private readonly string _gameVersion = "0.0.1";
    private readonly string _nickName = "danme";

    private void Start()
    {
        // 설정

        // 0. 데이터 송수신 빈도를 매 초당 60회로 설정한다. (기본은 10)
        PhotonNetwork.SendRate = 60; // 선호하는 값이지 보장 X
        PhotonNetwork.SerializationRate = 60;
        // 1. 버전 : 버전이 다르면 다른 서버로 접속이 된다.
        PhotonNetwork.GameVersion = _gameVersion;

        // 2. 닉네임 : 게임에서 사용할 사용자의 별명(중복가능) -> 판별을 위해서는 ActorID를 쓴다
        PhotonNetwork.NickName = _nickName;

        //방장이 로드한 씬으로 다른 참여자가 똑같이 이동하게끔 동기화 해주는 옵션
        //방장 : 방을 만든 소유자이자 "마스터 클라이언트" (방마다 한명의 마스터 클라이언트가 존재)
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // public void EnterGame()
    // {
    //     // 설정값들을 이용해 서버 접속 시도
    //     PhotonNetwork.ConnectUsingSettings();
    // }
    //
    // // 포톤 서버에 접속 후 호출되는 콜백 함수
    // public override void OnConnected()
    // {
    //     Debug.Log("네임 서버 접속 완료");
    //     Debug.Log(PhotonNetwork.CloudRegion);
    // }
    //
    // // 포톤 마스터 서버에 접속 후 호출되는 콜백 함수
    // public override void OnConnectedToMaster()
    // {
    //     Debug.Log("Connected to Master!");
    //     Debug.Log(PhotonNetwork.CloudRegion);
    //     Debug.Log($"Is in Lobby: {PhotonNetwork.InLobby}"); // 로비 입장 유무
    //
    //     PhotonNetwork.JoinLobby();
    //     //PhotonNetwork.JoinLobby(TypedLobby.Default);
    // }
    // private readonly TypedLobby _lobbyA = new TypedLobby("A", LobbyType.Default);
    // private TypedLobby _lobbyB = new TypedLobby("B", LobbyType.Default);
    //
    // public override void OnJoinedLobby()
    // {
    //     Debug.Log("로비 (채널) 입장 완료!");
    //     Debug.Log($"Is in Lobby: {PhotonNetwork.InLobby}"); // 로비 입장 유무
    //
    //     // 랜덤 방에 들어간다.
    //     PhotonNetwork.JoinRandomRoom();
    // }
    //
    // // 랜덤 룸 입장에 실패했을 경우 호출되는 콜백 함수
    // public override void OnJoinRandomFailed(short returnCode, string message)
    // {
    //     Debug.Log($"랜덤방 입장에 실패 했습니다 {returnCode}:{message}");
    //
    //     // 룸 속성 정의
    //     RoomOptions roomOptions = new RoomOptions();
    //     roomOptions.MaxPlayers = 20; // 룸에 입장할 수 있는 최대 접속자 수
    //     roomOptions.IsOpen = true; // 룸의 오픈 여부
    //     roomOptions.IsVisible = true; // 로비에서 룸 목록에 노출시킬지 여부
    //
    //     // 룸 생성
    //     // PhotonNetwork.CreateRoom("test", roomOptions);
    //     // 룸 입장 또는 생성
    //     // PhotonNetwork.JoinOrCreateRoom("test", roomOptions, TypedLobby.Default);
    // }
    //
    // // 룸에 입장한 후 호출되는 콜백 함수
    // public override void OnJoinedRoom()
    // {
    //     Debug.Log("룸 입장 완료!");
    //     Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
    //     Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");
    //
    //     // 룸에 접속한 사용자 정보
    //     Dictionary<int, Photon.Realtime.Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
    //     foreach (KeyValuePair<int, Photon.Realtime.Player> player in roomPlayers)
    //     {
    //         Debug.Log($"{player.Value.NickName} : {player.Value.ActorNumber}");
    //
    //         // 진짜 고유 아이디
    //         Debug.Log(player.Value.UserId); // 친구 기능, 귓속말 등등에 쓰이지만... 알아서...
    //     }
    //
    //     // 방에 입장 완료가 되면 플레이어를 생성한다.
    //     // 포톤에서는 게임 오브젝트 생성후 포톤 서버에 등록까지 해야 한다.
    //     // PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    //
    //
    // }
    // // 룸 생성에 실패하면 호출되는 콜백 함수
    // public override void OnCreateRoomFailed(short returnCode, string message)
    // {
    //     Debug.Log($"CreatRoom Failed {returnCode}:{message}");
    // }
    //
    // // 룸 생성이 성공했을 때 호출되는 콜백 함수
    // public override void OnCreatedRoom()
    // {
    //     Debug.Log("Created Room");
    //     // 생성된 룸 이름 확인
    //     Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    // }


}
