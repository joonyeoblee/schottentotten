using Photon.Pun;
using UnityEngine;
public class CardSlot : MonoBehaviourPunCallbacks
{
    private Card _card;
    public Card Card => _card;

    public void Refresh(Card card)
    {
        _card = card;
    }
}
