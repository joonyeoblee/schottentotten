using UnityEngine;

public class Account
{
    public string Nickname { get; private set; }

    public Account(string nickname)
    {
        Nickname = nickname;
    }
}
