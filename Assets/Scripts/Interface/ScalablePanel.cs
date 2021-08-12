using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ScaleMode { None, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

public class ScalablePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
{
	public bool FreeFloating = false;
	private float _maxSize = 5000;
	private float _minSize = 100;
	private float _edgeMargin = 20;
	private ScaleMode _scaleMode;
	private LayoutElement _element;
	private RectTransform _rectT;
	private Vector2 _scaleFactor;

	public RectTransform RectT { get { if (_rectT == null) { _rectT = GetComponent<RectTransform>(); } return _rectT; } }
	public LayoutElement LElement { get { if (_element == null) { _element = GetComponent<LayoutElement>(); } return _element; } }
	public bool Dragging { get; private set; }

	void Awake()
	{
		Dragging = false;

		if (FreeFloating)
		{
			_minSize = (_edgeMargin * 2) + 100;
		}
		else
		{
			_maxSize = Screen.width * 2;
			LElement.flexibleHeight = _maxSize / 2;
			LElement.flexibleWidth = _maxSize / 2;
			LElement.preferredHeight = 0;
			LElement.preferredWidth = 0;
		}
	}

	public void ResetScale()
	{
		LElement.flexibleHeight = _maxSize / 2;
		LElement.flexibleWidth = _maxSize / 2;
		LElement.preferredHeight = 0;
		LElement.preferredWidth = 0;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		// Vector2 position;
		// RectTransformUtility.ScreenPointToLocalPointInRectangle(RectT, eventData.pressPosition, null, out position);
		// Debug.Log(position);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Dragging = false;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		bool[] sides = new bool[4];
		Vector2 position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(RectT, eventData.pressPosition, null, out position);

		//	y value is inverted
		position.y = -position.y;

		if (position.x <= _edgeMargin)
		{
			Debug.Log("left");
			sides[0] = true;
			Dragging = true;
		}
		else if (position.x >= RectT.rect.width - _edgeMargin)
		{
			Debug.Log("right");
			sides[1] = true;
			Dragging = true;
		}

		if (position.y <= _edgeMargin)
		{
			Debug.Log("top");
			sides[2] = true;
			Dragging = true;
		}
		else if (position.y >= RectT.rect.height - _edgeMargin)
		{
			Debug.Log("bottom");
			sides[3] = true;
			Dragging = true;
		}

		if (sides.Count(x => x) == 1)
		{
			//	left
			if (sides[0])
			{
				_scaleFactor = new Vector2(-1, 0);
			}
			//	right
			else if (sides[1])
			{
				_scaleFactor = new Vector2(1, 0);
			}
			//	top
			else if (sides[2])
			{
				_scaleFactor = new Vector2(0, 1);
			}
			//	bottom
			else if (sides[3])
			{
				_scaleFactor = new Vector2(0, -1);
			}
		}
		else if (sides.Count(x => x) == 2)
		{
			//	top left
			if (sides[2] && sides[0])
			{
				//Debug.Log("top left");
				_scaleFactor = new Vector2(-1, 1);
			}
			//	top right
			else if (sides[2] && sides[1])
			{
				//Debug.Log("top right");
				_scaleFactor = new Vector2(1, 1);
			}
			//	bottom right
			else if (sides[3] && sides[1])
			{
				//Debug.Log("bottom right");
				_scaleFactor = new Vector2(1, -1);
			}
			//	bottom left
			else if (sides[3] && sides[0])
			{
				//Debug.Log("bottom left");
				_scaleFactor = new Vector2(-1, -1);
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		bool[] sides = new bool[4];
		Vector2 position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(RectT, eventData.pressPosition, null, out position);

		//	i don't know why the y value is inverted but it is
		position.y = -position.y;

		if (Dragging)
		{
			if (FreeFloating)
			{
				Vector2 newSize = RectT.sizeDelta + new Vector2(eventData.delta.x * _scaleFactor.x, eventData.delta.y * _scaleFactor.y);

				if ((newSize.x >= _maxSize || newSize.x <= _minSize) || (newSize.y >= _maxSize || newSize.y <= _minSize))
				{
					return;
				}

				if (_scaleFactor.x == -1)
				{
					RectT.anchoredPosition += new Vector2(eventData.delta.x, 0);
				}

				if (_scaleFactor.y == 1)
				{
					RectT.anchoredPosition += new Vector2(0, eventData.delta.y);
				}

				RectT.sizeDelta = newSize;
			}
			else
			{
				Vector2 size = new Vector2(_element.flexibleWidth, _element.flexibleHeight);
				Vector2 newSize = size + new Vector2(eventData.delta.x * _scaleFactor.x, eventData.delta.y * _scaleFactor.y);

				_element.flexibleWidth = newSize.x;
				_element.flexibleHeight = newSize.y;
			}
		}
	}
}
