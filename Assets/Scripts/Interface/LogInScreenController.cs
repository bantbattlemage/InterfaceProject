using UnityEngine;
using GameComms;

public class LogInScreenController : MonoBehaviour
{
	public GameObject LogInPopUpPrefab;
	public LogInPopUpPanel LogInPopUp { get; private set; }

	public static bool SaveLogIn { get { return PlayerPrefs.GetInt(Globals.SAVE_LOG_IN) == 1 ? true : false; } }

	public delegate void SuccessfulLogInEvent();
	public SuccessfulLogInEvent OnSuccessfulLogin;

	public void InitializeLogInScreen()
	{
		GameObject logInPopUpObject = Instantiate(LogInPopUpPrefab, InterfaceController.Instance.PopUpLayer);
		LogInPopUp = logInPopUpObject.GetComponent<LogInPopUpPanel>();
		LogInPopUp.LogInSubmit += SubmitLogInRequest;
		LogInPopUp.RegisterSubmit += SubmitRegisterNewAccountRequest;

		if (SaveLogIn && PlayerPrefs.GetString(Globals.USERNAME) != null)
		{
			LogInPopUp.LogInField.text = PlayerPrefs.GetString(Globals.USERNAME);
			LogInPopUp.PasswordField.text = PlayerPrefs.GetString(Globals.PASSWORD);
		}
		else if (!SaveLogIn)
		{
			PlayerPrefs.SetString(Globals.USERNAME, "");
			PlayerPrefs.SetString(Globals.PASSWORD, "");
		}
	}

	public void ProcessLogInRequest(LogInRequest request)
	{
		LoginCommunicator.Instance.ProcessLogInRequest(request, (response) =>
		{
			if (response.Success)
			{
				GameController.Instance.Player.SetPlayerCredentials(response.UserId, response.Username, response.AccessKey);

				if (SaveLogIn)
				{
					PlayerPrefs.SetString(Globals.USERNAME, request.Username);
					PlayerPrefs.SetString(Globals.PASSWORD, request.Password);
					PlayerPrefs.SetString(Globals.ACCESS_KEY, response.AccessKey);
				}

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
