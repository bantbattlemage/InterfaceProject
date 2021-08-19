using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ScaleMode { None, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };

public class ScalablePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool IsFreeFloating = false;
	private Vector2 _maxSize;
	private Vector2 _minSize;
	private float _edgeMargin = 40;
	private ScaleMode _scaleMode;
	private LayoutElement _element;
	private RectTransform _rectT;
	private Vector2 _scaleFactor;

	public RectTransform RectT { get { if (_rectT == null) { _rectT = GetComponent<RectTransform>(); } return _rectT; } }
	public LayoutElement LElement { get { if (_element == null) { _element = GetComponent<LayoutElement>(); } return _element; } }
	public bool IsDragScaling { get; private set; }
	public Vector2 DefaultSize { get { return new Vector2(_maxSize.x / 2, _maxSize.y / 2); } }

	public void Initialize()
	{
		IsDragScaling = false;
		_maxSize = new Vector2(Screen.width, Screen.height);
		_minSize = new Vector2((_edgeMargin * 2), (_edgeMargin * 2));

		if (IsFreeFloating)
		{
			_minSize = new Vector2((_edgeMargin * 2), (_edgeMargin * 2));
		}
		else
		{
			_minSize = new Vector2(10, 10);

			LElement.flexibleWidth = DefaultSize.x;
			LElement.flexibleHeight = DefaultSize.y;
			LElement.preferredHeight = 0;
			LElement.preferredWidth = 0;
		}

		InterfacePanelGroup parent = transform.parent.GetComponent<InterfacePanelGroup>();
		if (parent != null)
		{
			parent.NormalizePanelSizes();
		}
	}

	public void ResetScale()
	{
		LElement.flexibleWidth = DefaultSize.x;
		LElement.flexibleHeight = DefaultSize.y;
		LElement.preferredWidth = 0;
		LElement.preferredHeight = 0;
	}

	public bool IsEdgeClick(PointerEventData eventData)
	{
		Vector2 position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(RectT, eventData.pressPosition, null, out position);

		//	y value is inverted
		position.y = -position.y;

		if (position.x <= _edgeMargin)
		{
			//Debug.Log("left");
			return true;
		}
		else if (position.x >= RectT.rect.width - _edgeMargin)
		{
			//Debug.Log("right");
			return true;
		}

		if (position.y <= _edgeMargin)
		{
			//Debug.Log("top");
			return true;

		}
		else if (position.y >= RectT.rect.height - _edgeMargin)
		{
			//Debug.Log("bottom");
			return true;
		}

		return false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!IsFreeFloating)
		{
			return;
		}

		IsDragScaling = false;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		InterfacePanelGroup parentGroup = transform.parent.GetComponent<InterfacePanelGroup>();
		ScalablePanel parentPanel;

		if (parentGroup)
		{
			while (parentGroup != null)
			{
				parentPanel = parentGroup.Scale;

				if (parentPanel.IsEdgeClick(eventData))
				{
					//Debug.Log("parent click");
					parentPanel.OnBeginDrag(eventData);
					return;
				}
				else if (parentGroup != null)
				{
					parentGroup = parentGroup.transform.parent.GetComponent<InterfacePanelGroup>();
				}
				else
				{
					break;
				}
			}
		}

		bool[] sides = new bool[4];
		Vector2 position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(RectT, eventData.pressPosition, null, out position);

		//	y value is inverted
		position.y = -position.y;

		if (position.x <= _edgeMargin)
		{
			sides[0] = true;
			IsDragScaling = true;
		}
		else if (position.x >= RectT.rect.width - _edgeMargin)
		{
			sides[1] = true;
			IsDragScaling = true;
		}

		if (position.y <= _edgeMargin)
		{
			sides[2] = true;
			IsDragScaling = true;
		}
		else if (position.y >= RectT.rect.height - _edgeMargin)
		{
			sides[3] = true;
			IsDragScaling = true;
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
		InterfacePanelGroup parentGroup = transform.parent.GetComponent<InterfacePanelGroup>();
		ScalablePanel parentPanel;

		if (parentGroup)
		{
			while (parentGroup != null)
			{
				parentPanel = parentGroup.Scale;

				if (parentPanel.IsDragScaling)
				{
					parentPanel.OnDrag(eventData);
					return;
				}
				else if (parentGroup != null)
				{
					parentGroup = parentGroup.transform.parent.GetComponent<InterfacePanelGroup>();
				}
				else
				{
					break;
				}
			}
		}

		bool[] sides = new bool[4];
		Vector2 position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(RectT, eventData.pressPosition, null, out position);

		//	i don't know why the y value is inverted but it is
		position.y = -position.y;

		if (IsDragScaling)
		{
			if (IsFreeFloating && !RectT.IsRectTransformInsideSreen())
			{
				IsDragScaling = false;
				return;
			}

			if (IsFreeFloating)
			{
				Vector2 newSize = RectT.sizeDelta + new Vector2(eventData.delta.x * _scaleFactor.x, eventData.delta.y * _scaleFactor.y);

				if ((newSize.x >= _maxSize.x || newSize.x <= _minSize.x) || (newSize.y >= _maxSize.y || newSize.y <= _minSize.y))
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
				float horizontalModifier = 2f;
				float verticalModifier = 2f;

				Vector2 size = new Vector2(_element.flexibleWidth, _element.flexibleHeight);
				Vector2 newSize = size + new Vector2(eventData.delta.x * _scaleFactor.x * horizontalModifier, eventData.delta.y * _scaleFactor.y * verticalModifier);

				if ((newSize.x >= _maxSize.x || newSize.x <= _minSize.x) || (newSize.y >= _maxSize.y || newSize.y <= _minSize.y))
				{
					return;
				}

				_element.flexibleWidth = newSize.x;
				_element.flexibleHeight = newSize.y;
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (IsFreeFloating)
		{
			return;
		}

		InterfacePanelGroup parentGroup = transform.parent.GetComponent<InterfacePanelGroup>();
		ScalablePanel parentPanel;

		if (parentGroup)
		{
			while (parentGroup != null)
			{
				parentPanel = parentGroup.Scale;

				if (parentPanel)
				{
					parentPanel.OnEndDrag(eventData);
				}

				parentGroup = parentGroup.transform.parent.GetComponent<InterfacePanelGroup>();

				if (parentGroup == null)
				{
					break;
				}
			}
		}

		IsDragScaling = false;
	}
}