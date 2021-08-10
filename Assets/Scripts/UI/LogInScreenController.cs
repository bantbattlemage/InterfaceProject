using System.Collections.Generic;
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
	}

	public void SubmitLogInRequest(string userName, string password)
	{
		LogInRequest request = new LogInRequest() { Username = userName };
		string jsonRequest = JsonUtility.ToJson(request);
		string appendedURL = $"{GameController.Instance.ServerURL}{"login/"}";

		StartCoroutine(RestWebClient.Instance.HttpPost(appendedURL, jsonRequest, (r) =>
		{
			if(r.Error != null)
			{
				Debug.LogWarning(r.Error);
			}
			else if ( r.Data.Equals("true"))
			{
				LogInPopUp.ClosePopUp();
				OnSuccessfulLogin();
			}
		}));
	}
}
