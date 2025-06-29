using Photon.Pun;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class CardSlot : MonoBehaviourPunCallbacks
{
    private Card _card;
    public Card Card => _card;
    public SpriteRenderer CardSprite;
    
    public bool IsOccupied => _card != null;
    
    public void Refresh(Card card)
    {
        _card = card;
        // Addressables.LoadAssetAsync<Sprite>(_card.CardImageAddress).Completed += handle =>
        // {
        //      CardSprite.sprite = handle.Result;
        // };
    }

    public void Clear()
    {
        _card = null;
    }
    
    // RPC를 통해 카드 상태를 업데이트
    [PunRPC]
    public void RPC_UpdateCardInSlot(int cardNumber, int color, string cardImageAddress)
    {
        // 카드 정보를 업데이트
        Card newCard = new Card(cardNumber, (ECardColor)color);
        Refresh(newCard); // 카드 UI 업데이트
    }
    
}
