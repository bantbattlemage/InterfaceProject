using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private ScalablePanel _panel;

	void Start()
	{
		_panel = GetComponent<ScalablePanel>();
	}

	private Vector2 lastMousePosition;

	/// <summary>
	/// This method will be called on the start of the mouse drag
	/// </summary>
	/// <param name="eventData">mouse pointer event data</param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		//Debug.Log("Begin Drag");
		lastMousePosition = eventData.position;
	}

	/// <summary>
	/// This method will be called during the mouse drag
	/// </summary>
	/// <param name="eventData">mouse pointer event data</param>
	public void OnDrag(PointerEventData eventData)
	{
		if(_panel != null && _panel.IsDragScaling)
		{
			return;
		}

		//	do not drag if clicking something
		if (eventData.selectedObject != null)
		{
			return;
		}

		Vector2 currentMousePosition = eventData.position;
		Vector2 diff = currentMousePosition - lastMousePosition;
		RectTransform rect = GetComponent<RectTransform>();

		Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
		Vector3 oldPos = rect.position;
		rect.position = newPosition;
		if (!GetComponent<RectTransform>().IsRectTransformInsideSreen())
		{
			rect.position = oldPos;
		}
		lastMousePosition = currentMousePosition;
	}

	/// <summary>
	/// This method will be called at the end of mouse drag
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		//Debug.Log("End Drag");
		//Implement your funtionlity here
	}
}