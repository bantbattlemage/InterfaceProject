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

	private bool _waitingOnResponse = false;

	public void InitializeLogInScreen()
	{
		GameObject logInPopUpObject = Instantiate(LogInPopUpPrefab, InterfaceController.Instance.PopUpLayer);
		LogInPopUp = logInPopUpObject.GetComponent<LogInPopUpPanel>();
		LogInPopUp.LogInSubmit += SubmitLogInRequest;
		LogInPopUp.RegisterSubmit += SubmitRegisterNewAccountRequest;
	}

	public void ProcessLogInRequest(LogInRequest request)
	{
		if (_waitingOnResponse)
		{
			Debug.LogWarning("LogIn request ignored, waiting on existing request response");
			return;
		}
		_waitingOnResponse = true;

		string json = JsonConvert.SerializeObject(request);
		string appendedURL = $"{GameController.Instance.ServerURL}{"login/"}";

		StartCoroutine(RestWebClient.Instance.HttpPost(appendedURL, json, (r) =>
		{
			_waitingOnResponse = false;

			if (r.Error != null)
			{
				InterfaceController.Instance.LogWarning(r.Error);
			}
			else
			{
				Debug.Log(r.Data);

				LogInResponse response = JsonConvert.DeserializeObject<LogInResponse>(r.Data);

				if (response.Success)
				{
					GameController.Instance.SessionToken = request.Password;
					LogInPopUp.ClosePopUp();
					OnSuccessfulLogin();
				}
				else
				{
					InterfaceController.Instance.LogWarning(response.Message);
				}
			}
		}));
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
