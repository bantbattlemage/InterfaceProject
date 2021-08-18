using UnityEngine;
using GameComms;
using RestClient.Core;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

public class ChatCommunicator : Singleton<ChatCommunicator>, ICommunicator
{
	public delegate void ChatInformationEvent(ChatMessageResponse messageResponse);
	public ChatInformationEvent NewChatInformationRecieved;

	private static List<PanelContentChat> _activeChats = new List<PanelContentChat>();

	private float _echoInterval = 1f;
	private Coroutine _echo;

	void Awake()
	{
		StartCoroutine(Echo());
	}

	void OnDestroy()
	{
		if (_echo != null)
		{
			StopCoroutine(_echo);
		}
	}

	private IEnumerator<WaitForSeconds> Echo()
	{
		foreach (PanelContentChat chat in _activeChats)
		{
			SendChatReadRequest(chat.RoomId, (response) =>
			{
				if (response.ChatMessages != null && response.ChatMessages.Length > 0)
				{
					NewChatInformationRecieved(response);
				}
			});
		}

		yield return new WaitForSeconds(_echoInterval);

		_echo = StartCoroutine(Echo());
	}

	public void RegisterChatPanel(PanelContentChat chat)
	{
		if (_activeChats.Contains(chat))
		{
			InterfaceController.Instance.LogWarning("Tried to register chat panel already registered");
			return;
		}

		_activeChats.Add(chat);
	}

	public void RemoveChatPanel(PanelContentChat chat)
	{
		if (!_activeChats.Contains(chat))
		{
			InterfaceController.Instance.LogWarning("Tried to register chat panel already registered");
			return;
		}

		_activeChats.Remove(chat);
	}

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
		request.Username = message.Username;

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

			if (NewChatInformationRecieved != null)
			{
				NewChatInformationRecieved(response);
			}

			callBack(response);
		}));
	}

	// public void SendChatUpdateRequest(ChatMessage lastReadMessage, System.Action<ChatMessageResponse> callBack)
	// {
	// 	ChatMessageReadRequest request = new ChatMessageReadRequest();
	// 	request.ChatRoomId = lastReadMessage.RoomId;
	// 	request.SenderUserId = 1;
	// 	request.LastMessageRead = lastReadMessage.TimeStamp;

	// 	string appendedURL = $"{GameController.Instance.ServerURL}chatupdate/";
	// 	string json = JsonConvert.SerializeObject(request);

	// 	StartCoroutine(RestWebClient.Instance.HttpPut(appendedURL, json, (r) =>
	// 	{
	// 		ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);

	// 		callBack(response);
	// 	}));
	// }

	// public void SendGetAllChatMessagesRequest(ChatMessage lastReadMessage, System.Action<ChatMessageResponse> callBack)
	// {
	// 	ChatMessageReadRequest request = new ChatMessageReadRequest();
	// 	request.ChatRoomId = lastReadMessage.RoomId;
	// 	request.SenderUserId = 1;
	// 	request.LastMessageRead = lastReadMessage.TimeStamp;

	// 	string appendedURL = $"{GameController.Instance.ServerURL}chatupdate/";
	// 	string json = JsonConvert.SerializeObject(request);

	// 	StartCoroutine(RestWebClient.Instance.HttpPut(appendedURL, json, (r) =>
	// 	{
	// 		ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);

	// 		callBack(response);
	// 	}));
	// }
}