using System.Collections.Generic;
using Newtonsoft.Json;
using RestClient.Core;
using UnityEngine;

public class PulseCommunicator : Singleton<PulseCommunicator>, ICommunicator
{
	public static bool Pulse { get; private set; }

	private float _echoInterval = 1f;
	private Coroutine _echo;
	private Coroutine _retry;

	void OnDestroy()
	{
		if (_echo != null)
		{
			StopCoroutine(_echo);
		}

		if (_retry != null)
		{
			StopCoroutine(_retry);
		}
	}

	public void Initialize()
	{
		GetServerPulse((response) =>
		{
			Pulse = response;

			if (Pulse)
			{
				_echo = StartCoroutine(Echo());
			}
			else
			{
				InterfaceController.Instance.LogWarning("Couldn't connect to server");

				_retry = StartCoroutine(RetryConnection());
			}
		});
	}

	private IEnumerator<WaitForSeconds> RetryConnection()
	{
		yield return new WaitForSeconds(_echoInterval);

		GetServerPulse((response) =>
		{
			Pulse = response;

			if (Pulse)
			{
				InterfaceController.Instance.LogWarning("Successfully established server connection");
				_echo = StartCoroutine(Echo());
			}
			else
			{
				_retry = StartCoroutine(RetryConnection());
			}
		});
	}

	private IEnumerator<WaitForSeconds> Echo()
	{
		GetServerPulse((response) =>
		{
			bool previousPulse = Pulse;
			Pulse = response;

			if (!Pulse && previousPulse)
			{
				InterfaceController.Instance.LogWarning("Lost connection to server");
			}
			else if (Pulse && !previousPulse)
			{
				InterfaceController.Instance.LogWarning("Regained connection");
			}

			if (Pulse)
			{
				if (_retry != null)
				{
					StopCoroutine(_retry);
					_retry = null;
				}
			}
		});

		yield return new WaitForSeconds(_echoInterval);

		_echo = StartCoroutine(Echo());
	}

	private void GetServerPulse(System.Action<bool> callBack)
	{
		string appendedURL = $"{GameController.Instance.ServerURL}pulse/";

		StartCoroutine(RestWebClient.Instance.HttpGet(appendedURL, (r) =>
		{
			bool response = false;

			try
			{
				response = JsonConvert.DeserializeObject<bool>(r.Data);
			}
			catch
			{
				response = false;
			}

			callBack(response);
		}));
	}
}
