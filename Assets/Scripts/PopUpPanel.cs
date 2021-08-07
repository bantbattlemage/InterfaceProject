using UnityEngine;
using UnityEngine.UI;


public class PopUpPanel : MonoBehaviour
{
	public GameObject ButtonPrefab;
	public Transform Body;
	public Text PanelName;
	public Text PanelText;
	public Button CloseButton;
	public Button[] PopUpButtons;

	public delegate void PopUpPanelEvent(PopUpPanel sender);
	public PopUpPanelEvent PanelDetroyed;

	public void InitializePopUp(string popUpName = "", string popUpText = "", PopUpButtonProperties[] buttons = null)
	{
		PanelName.text = popUpName;
		PanelText.text = popUpText;

		CloseButton.onClick.AddListener(ClosePopUp);

		if (buttons != null)
		{
			foreach (PopUpButtonProperties newButton in buttons)
			{
				AddButton(newButton.ButtonText, newButton.Callback);
			}
		}
		else
		{
			AddButton();
		}
	}

	public void AddButton(string buttonText = "", UnityEngine.Events.UnityAction onClickCallback = null)
	{
		if (onClickCallback == null)
		{
			onClickCallback = ClosePopUp;
		}

		if (buttonText == "")
		{
			buttonText = "OK";
		}

		GameObject newButton = Instantiate(ButtonPrefab, Body);
		newButton.GetComponent<Button>().onClick.AddListener(onClickCallback);
		newButton.GetComponentInChildren<Text>().text = buttonText;
	}

	public void ClosePopUp()
	{
		if (PanelDetroyed != null)
		{
			PanelDetroyed(this);
		}

		Destroy(gameObject);
	}
}

public class PopUpButtonProperties
{
	public string ButtonText;
	public UnityEngine.Events.UnityAction Callback;
}
