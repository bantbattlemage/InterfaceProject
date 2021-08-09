using UnityEngine;
using UnityEngine.EventSystems;

public class RenderTextureRaycast : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
 {
    public float MaxZoomOut = 50;
    public float MinZoomIn = 10;

    [SerializeField] protected Camera UICamera;    //  should be left null
     [SerializeField] public RectTransform RawImageRectTrans;
     [SerializeField] public Camera RenderToTextureCamera;

    private bool _focused = false;

    void Update()
    {
        if(!_focused)
        {
            return;
        }

        if (CastRay(out var raycastHit))
        {
            //Debug.Log("hovered: " + raycastHit.collider.gameObject.name);
        }

        CheckMouseScrollZoom();
    }

    private bool CastRay(out RaycastHit raycastHit)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(RawImageRectTrans, Input.mousePosition, UICamera, out localPoint);
        Vector2 normalizedPoint = Rect.PointToNormalized(RawImageRectTrans.rect, localPoint);
        var renderRay = RenderToTextureCamera.ViewportPointToRay(normalizedPoint);
        if (Physics.Raycast(renderRay, out raycastHit))
        {
            return true;
        }

        return false;
    }

    private void CheckMouseScrollZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            RenderToTextureCamera.orthographicSize--;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            RenderToTextureCamera.orthographicSize++;
        }

        if (RenderToTextureCamera.orthographicSize < MinZoomIn)
        {
            RenderToTextureCamera.orthographicSize = MinZoomIn;
        }
        if (RenderToTextureCamera.orthographicSize > MaxZoomOut)
        {
            RenderToTextureCamera.orthographicSize = MaxZoomOut;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
     {
         if (CastRay(out var raycastHit))
         {
             Debug.Log("Hit: " + raycastHit.collider.gameObject.name);
         }
         else
         {
             Debug.Log("No hit object");
         }
     }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _focused = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _focused = false;
    }
}