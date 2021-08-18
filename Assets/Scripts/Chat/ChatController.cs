using UnityEngine;
using GameComms;
using RestClient.Core;
using Newtonsoft.Json;
using System.Linq;

public class ChatController : MonoBehaviour
{
	private bool _waitingOnResponse = false;
	private float _counter = 0f;
	private float _timeOut = 5f;

	private static ChatController _instance;
	public static ChatController Instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}

			_instance = GameController.Instance.GetComponent<ChatController>();

			if (_instance == null)
			{
				_instance = GameController.Instance.gameObject.AddComponent<ChatController>();
			}

			return _instance;
		}
	}

	void Update()
	{
		if (_waitingOnResponse)
		{
			_counter += Time.deltaTime;
			if (_counter >= _timeOut)
			{
				_waitingOnResponse = false;
				_counter = 0;
			}
		}
	}

	public static void Initialize()
	{
		_instance = Instance;
	}

	public void SendReadDisplayNameRequest(int userId, System.Action<GetUserResponse> callBack)
	{
		string appendedURL = $"{GameController.Instance.ServerURL}login/{userId}";

		_waitingOnResponse = true;
		StartCoroutine(RestWebClient.Instance.HttpGet(appendedURL, (r) =>
		{
			GetUserResponse response = JsonConvert.DeserializeObject<GetUserResponse>(r.Data);
			callBack(response);
		}));
	}

	public void SendChatPostRequest(ChatMessage message, System.Action<ChatMessageResponse> callBack)
	{
		string appendedURL = $"{GameController.Instance.ServerURL}chat/";

		ChatMessagePostRequest request = new ChatMessagePostRequest();

		request.Message = message.Message;
		request.ChatRoomId = message.RoomId;
		request.SenderUserId = message.UserId;
		string json = JsonConvert.SerializeObject(request);

		_waitingOnResponse = true;
		StartCoroutine(RestWebClient.Instance.HttpPost(appendedURL, json, (r) =>
		{
			_waitingOnResponse = false;
			ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);
			callBack(response);
		}));
	}

	public void SendChatReadRequest(int chatRoomId, System.Action<ChatMessageResponse> callBack)
	{
		// if (_waitingOnResponse)
		// {
		// 	Debug.LogWarning("Waiting on response");
		// 	return;
		// }

		string appendedURL = $"{GameController.Instance.ServerURL}chat/{chatRoomId}";

		_waitingOnResponse = true;
		StartCoroutine(RestWebClient.Instance.HttpGet(appendedURL, (r) =>
		{
			_waitingOnResponse = false;
			ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);
			callBack(response);
		}));
	}

	public void SendChatUpdateRequest(ChatMessage lastReadMessage, System.Action<ChatMessageResponse> callBack)
	{
		// if (_waitingOnResponse)
		// {
		// 	Debug.LogWarning("Waiting on response");
		// 	return;
		// }

		ChatMessageReadRequest request = new ChatMessageReadRequest();
		request.ChatRoomId = lastReadMessage.RoomId;
		request.SenderUserId = 1;
		request.LastMessageRead = lastReadMessage.TimeStamp;

		string appendedURL = $"{GameController.Instance.ServerURL}chatupdate/";
		string json = JsonConvert.SerializeObject(request);

		Debug.LogWarning(json);

		_waitingOnResponse = true;
		StartCoroutine(RestWebClient.Instance.HttpPut(appendedURL, json, (r) =>
		{
			_waitingOnResponse = false;
			//Debug.LogWarning(r.Data);
			ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);

			// if (response != null && response.ChatMessages != null && response.ChatMessages.Contains(lastReadMessage))
			// {
			// 	response.ChatMessages.ToList().Remove(lastReadMessage);
			// }

			callBack(response);
		}));
	}
}