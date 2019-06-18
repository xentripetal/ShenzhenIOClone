using UnityEngine;
using UnityEngine.EventSystems;

public class ChipDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public RectTransform ParentTransform;
    public float xOffset = 75f;
    public float yOffset = 85f;
    public float multiplier = 77f;
   
    private Vector2 offset;
    private Matrix4x4 localMatrix;
    
    
    public void OnBeginDrag(PointerEventData eventData) {
        localMatrix = ParentTransform.localToWorldMatrix;
        offset = eventData.position - (Vector2) ParentTransform.position;
        Debug.Log(ParentTransform.localPosition);
        Debug.Log(ParentTransform.position);
        Debug.Log(ParentTransform.anchoredPosition);
    }

    public void OnDrag(PointerEventData eventData) {
        var pos = eventData.position - offset;
        var x = pos.x - xOffset;
        x = NearestMultiple(pos.x) + xOffset;
        var y = pos.y - yOffset;
        y = NearestMultiple(y) + yOffset;
        ParentTransform.position = new Vector2(x, y);

    }

    private float NearestMultiple(float val) {
        return Mathf.Round(val / multiplier) * multiplier;
    } 

    public void OnEndDrag(PointerEventData eventData) {
    }
}