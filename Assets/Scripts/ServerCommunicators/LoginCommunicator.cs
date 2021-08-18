using System.Collections;
using System.Collections.Generic;
using GameComms;
using Newtonsoft.Json;
using RestClient.Core;
using UnityEngine;

public class LoginCommunicator : Singleton<LoginCommunicator>, ICommunicator
{
	private bool _waitingOnResponse = false;

	public void ProcessLogInRequest(LogInRequest request, System.Action<LogInResponse> callBack)
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
				LogInResponse response = JsonConvert.DeserializeObject<LogInResponse>(r.Data);

				if (response.Success)
				{
					callBack(response);
				}
				else
				{
					InterfaceController.Instance.LogWarning(response.Message);
				}
			}
		}));
	}
}