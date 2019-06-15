using UnityEngine;
using UnityEngine.EventSystems;

public class ChipDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public RectTransform ParentTransform;
    public float xOffset = -756.6f;
    public float yOffset = -117.6f;
    public float multiplier = 76.8f;
   
    private Vector2 offset;
    private Matrix4x4 localMatrix;
    
    
    public void OnBeginDrag(PointerEventData eventData) {
        localMatrix = ParentTransform.localToWorldMatrix;
        offset = eventData.position - (Vector2) ParentTransform.localPosition;
        
    }

    public void OnDrag(PointerEventData eventData) {
        Debug.Log(localMatrix.MultiplyPoint3x4(eventData.position));
        Vector2 pos = eventData.position - offset;
        var x = pos.x - xOffset;
        x = NearestMultiple(pos.x) + xOffset;
        var y = pos.y - yOffset;
        y = NearestMultiple(y) + yOffset;
        ParentTransform.localPosition= new Vector2(x, y);

    }

    private float NearestMultiple(float val) {
        return Mathf.Round(val / multiplier) * multiplier;
    } 

    public void OnEndDrag(PointerEventData eventData) {
    }
}