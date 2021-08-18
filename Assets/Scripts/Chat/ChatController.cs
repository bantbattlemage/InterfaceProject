using UnityEngine;
using GameComms;
using RestClient.Core;
using Newtonsoft.Json;
using System.Linq;

public class ChatController : MonoBehaviour
{

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

	public static void Initialize()
	{
		_instance = Instance;
	}

	// public void SendReadDisplayNameRequest(int userId, System.Action<GetUserResponse> callBack)
	// {
	// 	string appendedURL = $"{GameController.Instance.ServerURL}login/{userId}";

	// 	_waitingOnResponse = true;
	// 	StartCoroutine(RestWebClient.Instance.HttpGet(appendedURL, (r) =>
	// 	{
	// 		GetUserResponse response = JsonConvert.DeserializeObject<GetUserResponse>(r.Data);
	// 		callBack(response);
	// 	}));
	// }

	public void SendChatJoinRequest(int userId, int roomId, string username, System.Action<ChatJoinResponse> callBack)
	{
		string appendedURL = $"{GameController.Instance.ServerURL}chat/";

		JoinChatRoomRequest request = new JoinChatRoomRequest();
		request.Username = username;
		request.ChatRoomId = roomId;
		request.SenderUserId = userId;

		string json = JsonConvert.SerializeObject(request);

		StartCoroutine(RestWebClient.Instance.HttpPut(appendedURL, json, (r) =>
		{
			ChatJoinResponse response = JsonConvert.DeserializeObject<ChatJoinResponse>(r.Data);
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
		request.Username = "test";

		string json = JsonConvert.SerializeObject(request);

		StartCoroutine(RestWebClient.Instance.HttpPost(appendedURL, json, (r) =>
		{
			ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);
			callBack(response);
		}));
	}

	public void SendChatReadRequest(int chatRoomId, System.Action<ChatMessageResponse> callBack)
	{
		string appendedURL = $"{GameController.Instance.ServerURL}chat/{chatRoomId}";

		StartCoroutine(RestWebClient.Instance.HttpGet(appendedURL, (r) =>
		{
			ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);
			callBack(response);
		}));
	}

	public void SendChatUpdateRequest(ChatMessage lastReadMessage, System.Action<ChatMessageResponse> callBack)
	{
		ChatMessageReadRequest request = new ChatMessageReadRequest();
		request.ChatRoomId = lastReadMessage.RoomId;
		request.SenderUserId = 1;
		request.LastMessageRead = lastReadMessage.TimeStamp;

		string appendedURL = $"{GameController.Instance.ServerURL}chatupdate/";
		string json = JsonConvert.SerializeObject(request);

		StartCoroutine(RestWebClient.Instance.HttpPut(appendedURL, json, (r) =>
		{
			ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);

			callBack(response);
		}));
	}

	public void SendGetAllChatMessagesRequest(ChatMessage lastReadMessage, System.Action<ChatMessageResponse> callBack)
	{
		ChatMessageReadRequest request = new ChatMessageReadRequest();
		request.ChatRoomId = lastReadMessage.RoomId;
		request.SenderUserId = 1;
		request.LastMessageRead = lastReadMessage.TimeStamp;

		string appendedURL = $"{GameController.Instance.ServerURL}chatupdate/";
		string json = JsonConvert.SerializeObject(request);

		StartCoroutine(RestWebClient.Instance.HttpPut(appendedURL, json, (r) =>
		{
			ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);

			callBack(response);
		}));
	}
}