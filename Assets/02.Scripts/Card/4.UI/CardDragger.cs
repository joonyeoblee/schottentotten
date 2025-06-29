using Photon.Pun;
using UnityEngine;

public class CardDragger : MonoBehaviourPun
{
    private bool _isDragging = false;
    private Vector3 _dragOffset;
    private Vector3 _originPosition;
    private Camera _mainCamera;
    private CardController _cardController;

    private void Start()
    {
        _originPosition = transform.position;
        _mainCamera = Camera.main;
        _cardController = GetComponent<CardController>();
    }

    // 카드 드래그 시작
    private void OnMouseDown()
    {
        _isDragging = true;
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _dragOffset = transform.position - mousePos;
    }

    // 카드 드래그 중
    private void OnMouseDrag()
    {
        if (!_isDragging) return;
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos + _dragOffset;
    }

    // 마우스 떼었을 때
    private void OnMouseUp()
    {
        _isDragging = false;

        var hits = Physics2D.OverlapPointAll(transform.position);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("PlayerSlot"))
            {
                var slotCollider = hit.GetComponent<Collider2D>();
                var cardSlot = hit.GetComponent<CardSlot>();
                var slotview = hit.GetComponent<PhotonView>();
                // 다른 플레이어에게 카드 상태를 알려주는 RPC 호출
                
                slotview.RPC("RPC_UpdateCardInSlot", RpcTarget.Others, 
                    _cardController.Card.CardNumber, 
                    (int)_cardController.Card.Color, 
                    _cardController.Card.CardImageAddress);

                // 카드 UI 갱신
                cardSlot.Refresh(_cardController.Card);

                // if (slotCollider != null && slotCollider.OverlapPoint(transform.position))
                // {
                //     transform.position = hit.transform.position;
                //     return;
                // }
            }
        }

        // 원위치로 되돌리기
        transform.position = _originPosition;
    }
}
