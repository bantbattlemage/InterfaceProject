using System.Collections.Generic;
using Newtonsoft.Json;
using RestClient.Core;
using UnityEngine;

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
		string userName = request.Username;
		string password = request.Password;

		if (userName == null || password == null || userName == "" || password == "")
		{
			return;
		}

		string json = JsonConvert.SerializeObject(request);
		string appendedURL = $"{GameController.Instance.ServerURL}{"login/"}";

		StartCoroutine(RestWebClient.Instance.HttpPost(appendedURL, json, (r) =>
		{
			if (r.Error != null)
			{
				Debug.LogWarning(r.Error);
			}
			else
			{
				Debug.Log(r.Data);

				LogInResponse response = JsonConvert.DeserializeObject<LogInResponse>(r.Data);

				if (response.Success)
				{
					LogInPopUp.ClosePopUp();
					OnSuccessfulLogin();
				}
				else
				{
					InterfaceController.Instance.CreateNewPopUp(popUpText: response.Message);
				}
			}
		}));
	}

	public void SubmitLogInRequest(string userName, string password)
	{
		LogInRequest request = new LogInRequest() { Username = userName, Password = password, NewRegistration = false };
		ProcessLogInRequest(request);
	}

	public void SubmitRegisterNewAccountRequest(string userName, string password)
	{
		LogInRequest request = new LogInRequest() { Username = userName, Password = password, NewRegistration = true };
		ProcessLogInRequest(request);
	}
}
