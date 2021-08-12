using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PopUpPanel : MonoBehaviour, IPointerDownHandler
{
	public GameObject ButtonPrefab;
	public GameObject InputFieldPrefab;
	public Transform Body;
	public Text PanelName;
	public Text PanelText;
	public Button CloseButton;
	public Button[] PopUpButtons;
	public InputField[] InputFields;

	public delegate void PopUpPanelEvent(PopUpPanel sender);
	public PopUpPanelEvent PanelDetroyed;

	public RectTransform RectT { get { if (_rectT == null) { _rectT = GetComponent<RectTransform>(); } return _rectT; } }
	private RectTransform _rectT;

	void Start()
	{
		if (CloseButton)
		{
			CloseButton.onClick.AddListener(ClosePopUp);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		// Vector2 position = GetDistanceFromAnchor(eventData.position);

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

	public void InitializePopUp(string popUpName = "", string popUpText = "", PopUpButtonProperties[] buttons = null, PopUpInputFieldProperties[] inputFields = null)
	{
		PanelName.text = popUpName;
		PanelText.text = popUpText;


		if (inputFields != null)
		{
			foreach (PopUpInputFieldProperties newInputField in inputFields)
			{
				AddInputField(newInputField.InactiveText, newInputField.Callback);
			}
		}

		if (buttons != null)
		{
			foreach (PopUpButtonProperties newButton in buttons)
			{
				AddButton(newButton.ButtonText, newButton.Callback);
			}
		}
		else
		{
			if (inputFields == null)
			{
				AddButton();
			}
			else if (inputFields.Length == 1)
			{
				AddButton("Submit", () => { inputFields[0].Callback(GetComponentInChildren<InputField>().text); });
			}
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

	public void AddInputField(string inactiveText, UnityEngine.Events.UnityAction<string> callback)
	{
		GameObject newInputField = Instantiate(InputFieldPrefab, Body);
		newInputField.GetComponent<InputField>().onEndEdit.AddListener((text) =>
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				callback(text);
			}
		});

		newInputField.GetComponent<InputField>().placeholder.GetComponent<Text>().text = inactiveText;
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
	public UnityAction Callback;

	public PopUpButtonProperties(string buttonText, UnityAction action)
	{
		ButtonText = buttonText;
		Callback = action;
	}
}

public class PopUpInputFieldProperties
{
	public string InactiveText;
	public UnityAction<string> Callback;

	public PopUpInputFieldProperties(string inactiveText, UnityAction<string> callback)
	{
		InactiveText = inactiveText;
		Callback = callback;
	}
}
