using System.Collections.Generic;
using Newtonsoft.Json;
using RestClient.Core;
using UnityEngine;
using GameComms;

public class LogInScreenController : MonoBehaviour
{
	public GameObject LogInPopUpPrefab;

	public LogInPopUpPanel LogInPopUp { get; private set; }

	public delegate void SuccessfulLogInEvent();
	public SuccessfulLogInEvent OnSuccessfulLogin;

	public void InitializeLogInScreen()
	{
		GameObject logInPopUpObject = Instantiate(LogInPopUpPrefab, InterfaceController.Instance.PopUpLayer);
		LogInPopUp = logInPopUpObject.GetComponent<LogInPopUpPanel>();
		LogInPopUp.LogInSubmit += SubmitLogInRequest;
		LogInPopUp.RegisterSubmit += SubmitRegisterNewAccountRequest;
	}

	public void ProcessLogInRequest(LogInRequest request)
	{
		LoginCommunicator.Instance.ProcessLogInRequest(request, (response) =>
		{
			if (response.Success)
			{
				GameController.Instance.Player.SetPlayerCredentials(response.UserId, response.Username, response.AccessKey);
				LogInPopUp.ClosePopUp();
				OnSuccessfulLogin();
			}
			else
			{
				InterfaceController.Instance.LogWarning(response.Message);
			}
		});
	}

	public void SubmitLogInRequest(string userName, string password, string email)
	{
		LogInRequest request = new LogInRequest() { Username = userName, Password = password, Email = email, NewRegistration = false };
		ProcessLogInRequest(request);
	}

	public void SubmitRegisterNewAccountRequest(string userName, string password, string email)
	{
		LogInRequest request = new LogInRequest() { Username = userName, Password = password, Email = email, NewRegistration = true };
		ProcessLogInRequest(request);
	}
}
