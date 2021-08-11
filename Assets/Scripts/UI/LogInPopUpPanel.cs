using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInPopUpPanel : PopUpPanel
{
	string LogInPromptText;

	public InputField LogInField { get { return InputFields[0]; } }
	public InputField PasswordField { get { return InputFields[1]; } }
	public Button LogInSubmitButton { get { return PopUpButtons[0]; } }
	public Button RegisterSubmitButton { get { return PopUpButtons[1]; } }

	public delegate void LogInRequestEvent(string userName, string password);
	public LogInRequestEvent LogInSubmit;
	public LogInRequestEvent RegisterSubmit;

	void Start()
	{
		LogInField.onEndEdit.AddListener((text) =>
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				OnLogInSubmit(LogInField.text, PasswordField.text);
			}
		});

		PasswordField.onEndEdit.AddListener((text) =>
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				OnLogInSubmit(LogInField.text, PasswordField.text);
			}
		});

		CloseButton.onClick.AddListener(ClosePopUp);
		LogInSubmitButton.onClick.AddListener(() => { OnLogInSubmit(LogInField.text, PasswordField.text); });
		RegisterSubmitButton.onClick.AddListener(() => { OnRegisterSubmit(LogInField.text, PasswordField.text); });
	}

	public void OnLogInSubmit(string userName, string password)
	{
		LogInSubmit(userName, password);
	}

	public void OnRegisterSubmit(string userName, string password)
	{
		RegisterSubmit(userName, password);
	}
}
