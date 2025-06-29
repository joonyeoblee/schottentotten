using EPOOutline;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Transform))]
public class TargetSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Outlinable _outlinable;

    private void Start()
    {
        _outlinable = GetComponent<Outlinable>();
        _outlinable.enabled = false;
    }

    public void ActivateOutlinable()
    {
        _outlinable.enabled = true;
    }

    public void DeactivateOutlinable()
    {
        _outlinable.enabled = false;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        var cardSlot = GetComponent<CardSlot>();
        if (cardSlot != null)
        {
            Debug.Log($"카드 클릭됨.{cardSlot.Card?.CardNumber}");
        }
        // if (cardSlot.Card == null)
        //     Debug.LogWarning("이 슬롯에 카드가 없음!");
    }
    
    public void ChangeOutlineColor(Color color)
    {
        _outlinable.OutlineParameters.Color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _outlinable.enabled = false;
    }
    
    private void Reset()
    {
        if (GetComponent<Collider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();

        if (GetComponent<Outlinable>() == null)
            gameObject.AddComponent<Outlinable>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _outlinable.enabled = true;
    }
}