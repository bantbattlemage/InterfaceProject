using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInPopUpPanel : PopUpPanel
{
	string LogInPromptText;

	public InputField LogInField { get { return InputFields[0]; } }
	public InputField PasswordField { get { return InputFields[1]; } }
	public InputField EmailField { get { return InputFields[2]; } }

	public Button LogInSubmitButton { get { return PopUpButtons[0]; } }
	public Button RegisterSubmitButton { get { return PopUpButtons[1]; } }

	public delegate void LogInRequestEvent(string userName, string password, string email);
	public LogInRequestEvent LogInSubmit;
	public LogInRequestEvent RegisterSubmit;

	public override void Initialize()
	{
		base.Initialize();

		LogInField.onEndEdit.AddListener((text) =>
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				OnLogInButtonPressed(LogInField.text, PasswordField.text);
			}
		});

		PasswordField.onEndEdit.AddListener((text) =>
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				OnLogInButtonPressed(LogInField.text, PasswordField.text);
			}
		});

		CloseButton.onClick.AddListener(ClosePopUp);
		LogInSubmitButton.onClick.AddListener(() => { OnLogInButtonPressed(LogInField.text, PasswordField.text); });
		RegisterSubmitButton.onClick.AddListener(() => { OnRegisterButtonPressed(LogInField.text, PasswordField.text, EmailField.text); });
	}

	public void OnLogInButtonPressed(string userName, string password)
	{
		LogInSubmit(userName, password, "");
	}

	public void OnRegisterButtonPressed(string userName, string password, string email)
	{
		RegisterSubmit(userName, password, email);
	}
}
