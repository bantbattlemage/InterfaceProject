using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ScaleMode { None, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

public class ScalablePanel : MonoBehaviour, IDragHandler
{
	public bool FreeFloating = false;
	private float MaxSize = 5000;
	private float MarginSize = 10;

	private ScaleMode _scaleMode;
	private LayoutElement _element;
	private Vector2 _originalMousePosition;


	public RectTransform RectT { get { if (_rectT == null) { _rectT = GetComponent<RectTransform>(); } return _rectT; } }
	private RectTransform _rectT;
	private Vector2 _onDownPosition;

	void Awake()
	{
		_element = GetComponent<LayoutElement>();

		if (FreeFloating)
		{
			MaxSize = Screen.height * 0.75f;
		}
		else
		{
			MaxSize = Screen.width * 2;
			_element.flexibleHeight = MaxSize / 2;
			_element.flexibleWidth = MaxSize / 2;
			_element.preferredHeight = 0;
			_element.preferredWidth = 0;
		}
	}

	public void ResetScale()
	{
		_element.flexibleHeight = MaxSize / 2;
		_element.flexibleWidth = MaxSize / 2;
		_element.preferredHeight = 0;
		_element.preferredWidth = 0;
	}


	public void OnPointerDown(PointerEventData eventData)
	{
		_onDownPosition = GameInterface.GetDistanceFromAnchor(RectT, eventData.position);

		// float margin = 10;

		// if (position.x <= margin)
		// {
		// 	Debug.Log("left");
		// }

		// if (position.x >= RectT.rect.width - margin)
		// {
		// 	Debug.Log("right");
		// }

		// if (position.y <= margin)
		// {
		// 	Debug.Log("top");
		// }

		// if (position.y >= RectT.rect.height - margin)
		// {
		// 	Debug.Log("bottom");
		// }
	}

	public void OnDrag(PointerEventData eventData)
	{
		//--TODO: CALCULATE POSITION FROM CENTER

		Vector2 position = GameInterface.GetDistanceFromAnchor(RectT, eventData.pressPosition);

		float margin = 20;
		bool edgeClick = false;
		Vector2 scaleFactor = new Vector2(1, 1);
		bool[] corners = new bool[4];

		if (position.x <= margin)
		{
			Debug.Log("left");
			edgeClick = true;
			corners[0] = true;
		}
		else if (position.x >= RectT.rect.width - margin)
		{
			Debug.Log("right");
			edgeClick = true;
			corners[1] = true;
		}

		if (position.y <= margin)
		{
			Debug.Log("top");
			edgeClick = true;
			corners[2] = true;
		}
		else if (position.y >= RectT.rect.height - margin)
		{
			Debug.Log("bottom");
			edgeClick = true;
			corners[3] = true;
		}

		if (corners.Count(x => x) == 1)
		{
			//	left
			if (corners[0])
			{
				scaleFactor = new Vector2(-1, 0);
			}
			//	right
			else if (corners[1])
			{
				scaleFactor = new Vector2(1, 0);
			}
			//	top
			else if (corners[2])
			{
				scaleFactor = new Vector2(0, -1);
			}
			//	bottom
			else if (corners[3])
			{
				scaleFactor = new Vector2(0, -1);
			}
		}
		else if (corners.Length == 2)
		{
			//	top left
			if (corners[2] && corners[0])
			{
				scaleFactor = new Vector2(-1, -1);
			}
			//	top right
			else if (corners[2] && corners[1])
			{
				scaleFactor = new Vector2(1, 1);
			}
			//	bottom right
			else if (corners[3] && corners[1])
			{
				scaleFactor = new Vector2(1, 1);
			}
			//	bottom left
			else if (corners[3] && corners[0])
			{
				scaleFactor = new Vector2(-1, -1);
			}
		}

		if (edgeClick)
		{
			if (FreeFloating)
			{
				RectT.sizeDelta += new Vector2(eventData.delta.x * scaleFactor.x, eventData.delta.y * scaleFactor.y);
			}
			else
			{

			}
		}

		if (FreeFloating)
		{
			// if (mouseDelta.x != 0)
			// {
			// 	scaleFactor.x = rect.sizeDelta.x / MaxSize;
			// }
			// if (mouseDelta.y != 0)
			// {
			// 	scaleFactor.y = rect.sizeDelta.y / MaxSize;
			// }

			// mouseDelta.x *= 0.1f * scaleFactor.x;
			// mouseDelta.y *= 0.1f * scaleFactor.y;


			// if (rect.sizeDelta.x > MaxSize)
			// {
			// 	rect.sizeDelta = new Vector2(MaxSize, rect.sizeDelta.y);
			// }
			// if (rect.sizeDelta.y > MaxSize)
			// {
			// 	rect.sizeDelta = new Vector2(rect.sizeDelta.x, MaxSize);
			// }
			// if (rect.sizeDelta.x <= MaxSize / 10)
			// {
			// 	rect.sizeDelta = new Vector2(MaxSize / 10, rect.sizeDelta.y);
			// }
			// if (rect.sizeDelta.y <= MaxSize / 10)
			// {
			// 	rect.sizeDelta = new Vector2(rect.sizeDelta.x, MaxSize / 10);
			// }
		}
		// else
		// {
		// 	if (mouseDelta.x != 0)
		// 	{
		// 		scaleFactor.x = 0.5f * (_element.flexibleWidth / MaxSize);
		// 	}
		// 	if (mouseDelta.y != 0)
		// 	{
		// 		scaleFactor.y = 0.5f * (_element.flexibleHeight / MaxSize);
		// 	}

		// 	mouseDelta.x *= scaleFactor.x;
		// 	mouseDelta.y *= scaleFactor.y;

		// 	_element.flexibleWidth += mouseDelta.x;
		// 	_element.flexibleHeight += mouseDelta.y;

		// 	if (_element.flexibleHeight >= MaxSize)
		// 	{
		// 		_element.flexibleHeight = MaxSize;
		// 	}
		// 	else if (_element.flexibleHeight <= 1)
		// 	{
		// 		_element.flexibleHeight = 1;
		// 	}
		// 	if (_element.flexibleWidth >= MaxSize)
		// 	{
		// 		_element.flexibleWidth = MaxSize;
		// 	}
		// 	else if (_element.flexibleWidth <= 1)
		// 	{
		// 		_element.flexibleWidth = 1;
		// 	}
		// }
	}

	/*public void OnDrag(PointerEventData eventData)
	{
		if (!_buttons.Any(x => x.gameObject == eventData.selectedObject))
		{
			return;
		}

		Vector2 newMousePosition = eventData.position;
		Vector2 mouseDelta = new Vector2(0, 0);
		Vector2 scaleFactor = new Vector2(1, 1);

		switch (_scaleMode)
		{
			case ScaleMode.Top:
				mouseDelta.y = -(_originalMousePosition.y - newMousePosition.y);
				break;
			case ScaleMode.Bottom:
				mouseDelta.y = (_originalMousePosition.y - newMousePosition.y);
				break;
			case ScaleMode.Left:
				mouseDelta.x = (_originalMousePosition.x - newMousePosition.x);
				break;
			case ScaleMode.Right:
				mouseDelta.x = -(_originalMousePosition.x - newMousePosition.x);
				break;
			case ScaleMode.TopLeft:
				mouseDelta.y = -(_originalMousePosition.y - newMousePosition.y);
				mouseDelta.x = (_originalMousePosition.x - newMousePosition.x);
				break;
			case ScaleMode.TopRight:
				mouseDelta.y = -(_originalMousePosition.y - newMousePosition.y);
				mouseDelta.x = -(_originalMousePosition.x - newMousePosition.x);
				break;
			case ScaleMode.BottomLeft:
				mouseDelta.x = (_originalMousePosition.x - newMousePosition.x);
				mouseDelta.y = (_originalMousePosition.y - newMousePosition.y);
				break;
			case ScaleMode.BottomRight:
				mouseDelta.x = -(_originalMousePosition.x - newMousePosition.x);
				mouseDelta.y = (_originalMousePosition.y - newMousePosition.y);
				break;
			case ScaleMode.None:
				return;
		}

		if (FreeFloating)
		{
			RectTransform rect = GetComponent<RectTransform>();

			if (mouseDelta.x != 0)
			{
				scaleFactor.x = rect.sizeDelta.x / MaxSize;
			}
			if (mouseDelta.y != 0)
			{
				scaleFactor.y = rect.sizeDelta.y / MaxSize;
			}

			mouseDelta.x *= 0.1f * scaleFactor.x;
			mouseDelta.y *= 0.1f * scaleFactor.y;

			rect.sizeDelta += new Vector2(mouseDelta.x, mouseDelta.y);

			if (rect.sizeDelta.x > MaxSize)
			{
				rect.sizeDelta = new Vector2(MaxSize, rect.sizeDelta.y);
			}
			if (rect.sizeDelta.y > MaxSize)
			{
				rect.sizeDelta = new Vector2(rect.sizeDelta.x, MaxSize);
			}
			if (rect.sizeDelta.x <= MaxSize / 10)
			{
				rect.sizeDelta = new Vector2(MaxSize / 10, rect.sizeDelta.y);
			}
			if (rect.sizeDelta.y <= MaxSize / 10)
			{
				rect.sizeDelta = new Vector2(rect.sizeDelta.x, MaxSize / 10);
			}
		}
		else
		{
			if (mouseDelta.x != 0)
			{
				scaleFactor.x = 0.5f * (_element.flexibleWidth / MaxSize);
			}
			if (mouseDelta.y != 0)
			{
				scaleFactor.y = 0.5f * (_element.flexibleHeight / MaxSize);
			}

			mouseDelta.x *= scaleFactor.x;
			mouseDelta.y *= scaleFactor.y;

			_element.flexibleWidth += mouseDelta.x;
			_element.flexibleHeight += mouseDelta.y;

			if (_element.flexibleHeight >= MaxSize)
			{
				_element.flexibleHeight = MaxSize;
			}
			else if (_element.flexibleHeight <= 1)
			{
				_element.flexibleHeight = 1;
			}
			if (_element.flexibleWidth >= MaxSize)
			{
				_element.flexibleWidth = MaxSize;
			}
			else if (_element.flexibleWidth <= 1)
			{
				_element.flexibleWidth = 1;
			}
		}
	}*/

	// public void OnEndDrag(PointerEventData eventData)
	// {
	// 	if (FreeFloating)
	// 	{
	// 		return;
	// 	}

	// 	if (_element.flexibleHeight >= MaxSize)
	// 	{
	// 		_element.flexibleHeight = MaxSize - MarginSize;
	// 	}
	// 	else if (_element.flexibleHeight <= 1)
	// 	{
	// 		_element.flexibleHeight = MarginSize;
	// 	}
	// 	if (_element.flexibleWidth >= MaxSize)
	// 	{
	// 		_element.flexibleWidth = MaxSize - MarginSize;
	// 	}
	// 	else if (_element.flexibleWidth <= 1)
	// 	{
	// 		_element.flexibleWidth = MarginSize;
	// 	}
	// }
}
