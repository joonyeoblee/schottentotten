public enum ERoomState
{
    Waiting,
    Joining,
    Playing
}


public class Room
{
    public string RoomTitle { get; private set; }
    public string Password { get; private set; }

    public ERoomState RoomState { get; private set; }

    public Room(string roomTitle, string password, ERoomState roomState)
    {
        RoomTitle = roomTitle;
        Password = password;
        RoomState = roomState;
    }
}