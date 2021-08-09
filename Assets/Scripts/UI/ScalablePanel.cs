using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ScaleMode { None, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

public class ScalablePanel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public bool FreeFloating = false;
	private float MaxSize = 5000;
	private float MarginSize = 10;

	public Button Top;
	public Button Bottom;
	public Button Left;
	public Button Right;
	public Button TopLeft;
	public Button TopRight;
	public Button BottomLeft;
	public Button BottomRight;

	private Button[] _buttons;
	private ScaleMode _scaleMode;
	private LayoutElement _element;
	private Vector2 _originalMousePosition;

	private int _numberOfChildren = 0;
	private bool _trackChildren = true;

	void Awake()
	{
		_element = GetComponent<LayoutElement>();
		_buttons = new Button[] { Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

		foreach (Button b in _buttons)
		{
			b.GetComponent<PanelScaleButton>().OnPanelScaleButtonMouseDown += OnPanelScaleButtonMouseDown;
		}

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

	void Update()
	{
		if (_trackChildren)
		{
			if (transform.childCount != _numberOfChildren)
			{
				_numberOfChildren = transform.childCount;
				Top.transform.parent.SetSiblingIndex(_numberOfChildren);
			}
		}
	}

	private void OnPanelScaleButtonMouseDown(ScaleMode scaleMode, PointerEventData eventData)
	{
		_scaleMode = scaleMode;
		_originalMousePosition = eventData.position;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!_buttons.Any(x => x.gameObject == eventData.selectedObject))
		{
			return;
		}
	}

	public void OnDrag(PointerEventData eventData)
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

			mouseDelta.x *= 0.05f * scaleFactor.x;
			mouseDelta.y *= 0.05f * scaleFactor.y;

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
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (FreeFloating)
		{
			return;
		}

		if (_element.flexibleHeight >= MaxSize)
		{
			_element.flexibleHeight = MaxSize - MarginSize;
		}
		else if (_element.flexibleHeight <= 1)
		{
			_element.flexibleHeight = MarginSize;
		}
		if (_element.flexibleWidth >= MaxSize)
		{
			_element.flexibleWidth = MaxSize - MarginSize;
		}
		else if (_element.flexibleWidth <= 1)
		{
			_element.flexibleWidth = MarginSize;
		}
	}
}
