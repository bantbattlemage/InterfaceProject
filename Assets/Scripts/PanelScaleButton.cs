using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelScaleButton : Button
{
	public delegate void PanelScaleButtonPressed(ScaleMode scaleMode, PointerEventData eventData);
	public PanelScaleButtonPressed OnPanelScaleButtonMouseDown;

	public override void OnPointerDown(PointerEventData eventData)
	{
		ScaleMode scaleMode;

		switch (gameObject.name)
		{
			case "Top":
				scaleMode = ScaleMode.Top;
				break;
			case "Bottom":
				scaleMode = ScaleMode.Bottom;
				break;
			case "Left":
				scaleMode = ScaleMode.Left;
				break;
			case "Right":
				scaleMode = ScaleMode.Right;
				break;
			case "TopLeft":
				scaleMode = ScaleMode.TopLeft;
				break;
			case "TopRight":
				scaleMode = ScaleMode.TopRight;
				break;
			case "BottomLeft":
				scaleMode = ScaleMode.BottomLeft;
				break;
			case "BottomRight":
				scaleMode = ScaleMode.BottomRight;
				break;
			default:
				scaleMode = ScaleMode.Top;
				throw new System.Exception("???");
		}

		OnPanelScaleButtonMouseDown(scaleMode, eventData);
		base.OnPointerDown(eventData);
	}
}
