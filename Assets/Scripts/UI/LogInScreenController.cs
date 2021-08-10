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
		// StartCoroutine(RestWebClient.Instance.HttpGet(GameController.Instance.ServerURL, (r) =>
		// {
		// 	PlayerData p = JsonUtility.FromJson<PlayerData>(r.Data);

		// 	LogInPopUp.ClosePopUp();
		// 	OnSuccessfulLogin();
		// }));

		LogInRequest request = new LogInRequest() { Username = userName };
		string jsonRequest = JsonUtility.ToJson(request);
		string appendedURL = GameController.Instance.ServerURL + "login/";
		Debug.Log(jsonRequest);
		Debug.Log(appendedURL);

		RequestHeader header = new RequestHeader
		{
			Key = "Content-Type",
			Value = "application/json"
		};

		StartCoroutine(RestWebClient.Instance.HttpPost(appendedURL, jsonRequest, (r) =>
		{
			Debug.Log(r.Data);

			if (r.Data.Equals("true"))
			{
				LogInPopUp.ClosePopUp();
				OnSuccessfulLogin();
			}

		}, new List<RequestHeader> { header }));
	}
}
