using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ScaleMode { None, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

public class ScalablePanel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
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
	private bool _trackChildren;

	void Awake()
	{
		MaxSize = Screen.width * 2;

		_element = GetComponent<LayoutElement>();
		_buttons = new Button[] { Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

		foreach (Button b in _buttons)
		{
			b.GetComponent<PanelScaleButton>().OnPanelScaleButtonMouseDown += OnPanelScaleButtonMousDown;
		}

		// _element.flexibleHeight = 0;
		// _element.flexibleWidth = 0;
		// _element.flexibleHeight = MaxSize / 2;
		// _element.flexibleWidth = MaxSize / 2;
		_element.flexibleHeight = MaxSize / 2;
		_element.flexibleWidth = MaxSize / 2;
		_element.preferredHeight = 0;
		_element.preferredWidth = 0;

		_trackChildren = true;
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

	private void OnPanelScaleButtonMousDown(ScaleMode scaleMode, PointerEventData eventData)
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
				break;
			case ScaleMode.TopRight:
				break;
			case ScaleMode.BottomLeft:
				break;
			case ScaleMode.BottomRight:
				break;
			case ScaleMode.None:
				return;
		}

		if (mouseDelta.x != 0)
		{
			scaleFactor.x = _element.flexibleWidth / MaxSize;
		}
		if (mouseDelta.y != 0)
		{
			scaleFactor.y = _element.flexibleHeight / MaxSize;
		}

		Debug.Log(mouseDelta);

		mouseDelta.x *= 0.1f * scaleFactor.x;
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

	public void OnEndDrag(PointerEventData eventData)
	{
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
