using EPOOutline;
using UnityEngine;
using UnityEngine.UI;

public class CardDragger : MonoBehaviour
{
    private bool _isDragging = false;
    private Vector3 _dragOffset;
    private Vector3 _originPosition;
    private Camera _mainCamera;

    private void Start()
    {
        _originPosition = transform.position;
        _mainCamera = Camera.main;
    }
    
    // 카드를 처음 클릭했을 때 호출
    private void OnMouseDown()
    {
        _isDragging = true;
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _dragOffset = transform.position - mousePos;
    }
    
    // 마우스 누르고 있는 동안에 호출
    private void OnMouseDrag()
    {
        if (!_isDragging) return;
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos + _dragOffset;
    }
    
    // 마우스 뗄 때 호출
    private void OnMouseUp()
    {
        _isDragging = false;
        
        var hits = Physics2D.OverlapPointAll(transform.position);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("PlayerSlot"))
            {
                var slotCollider = hit.GetComponent<Collider2D>();

                if (slotCollider != null && slotCollider.OverlapPoint(transform.position))
                {
                    transform.position = hit.transform.position;
                    return;
                }
            }
        }
        
        transform.position = _originPosition;
    }
}
