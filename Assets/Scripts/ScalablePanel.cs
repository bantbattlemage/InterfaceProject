using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ScaleMode { Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

public class ScalablePanel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public float MaxSize = 500;
	public float MarginSize = 10;

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

	void Start()
	{
		_element = GetComponent<LayoutElement>();
		_buttons = new Button[] { Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

		foreach (Button b in _buttons)
		{
			b.GetComponent<PanelScaleButton>().OnPanelScaleButtonMouseDown += OnPanelScaleButtonMousDown;
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

		Debug.Log("Begin drag " + _scaleMode.ToString());
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
				mouseDelta.y = _originalMousePosition.y - newMousePosition.y;
				break;
			case ScaleMode.Left:
				mouseDelta.x = _originalMousePosition.x - newMousePosition.x;
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
		}

		//Debug.Log(string.Format("({0}, {1})", mouseDelta.x, mouseDelta.y));
		if (mouseDelta.x != 0)
		{
			scaleFactor.x = _element.flexibleWidth / MaxSize;  //(float)Math.Log((double)mouseDelta.x);
		}
		if (mouseDelta.y != 0)
		{
			scaleFactor.y = _element.flexibleHeight / MaxSize; //(float)Math.Log((double)mouseDelta.y);
		}

		mouseDelta.x *= 0.01f * scaleFactor.x;
		mouseDelta.y *= 0.01f * scaleFactor.y;

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

		if (_element.preferredHeight >= MaxSize)
		{
			_element.preferredHeight = MaxSize;
		}
		else if (_element.preferredHeight <= 1)
		{
			_element.preferredHeight = 1;
		}
		if (_element.preferredWidth >= MaxSize)
		{
			_element.preferredWidth = MaxSize;
		}
		else if (_element.preferredWidth <= 1)
		{
			_element.preferredWidth = 1;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		float height = _element.flexibleHeight / MaxSize;
		float width = _element.flexibleWidth / MaxSize;

		// _element.flexibleWidth = 10 * width;
		// _element.flexibleHeight = 10 * height;

		// if (_element.flexibleHeight >= MaxSize)
		// {
		// 	_element.flexibleHeight = MaxSize - MarginSize;
		// }
		// else if (_element.flexibleHeight <= 1)
		// {
		// 	_element.flexibleHeight = MarginSize;
		// }
		// if (_element.flexibleWidth >= MaxSize)
		// {
		// 	_element.flexibleWidth = MaxSize - MarginSize;
		// }
		// else if (_element.flexibleWidth <= 1)
		// {
		// 	_element.flexibleWidth = MarginSize;
		// }
	}
}
