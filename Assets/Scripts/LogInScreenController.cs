using System.Collections;
using System.Collections.Generic;
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
		LogInPopUp.ClosePopUp();
		OnSuccessfulLogin();
	}
}
