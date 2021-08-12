using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabNavigatableInputField : InputField
{
	public delegate void OnPointerClickedEvent(PointerEventData eventData, TabNavigatableInputField sender);
	public OnPointerClickedEvent OnPointerClicked;

	public delegate void OnPointerDeselectedEvent(BaseEventData eventData, TabNavigatableInputField sender);
	public OnPointerDeselectedEvent OnPointerDeselected;

	public override void OnPointerClick(PointerEventData eventData)
	{
		OnPointerClicked(eventData, this);
		base.OnPointerClick(eventData);
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		OnPointerDeselected(eventData, this);
		base.OnDeselect(eventData);
	}
}
