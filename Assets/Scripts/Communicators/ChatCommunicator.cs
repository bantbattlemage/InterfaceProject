using UnityEngine;
using GameComms;
using RestClient.Core;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ChatCommunicator : Singleton<ChatCommunicator>, ICommunicator
{
	private static List<PanelContentChat> _activeChats = new List<PanelContentChat>();

	private int updateMessageCount = 5;
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
			SendChatUpdateRequest(chat.RoomId, updateMessageCount, (response) =>
			{
				if (response != null && response.ChatMessages != null && response.ChatMessages.Length > 0)
				{
					chat.DisplayChatMessages(response);
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
		if (!PulseCommunicator.Pulse)
		{
			Debug.LogWarning("Waiting for server connection...");
			return;
		}

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
		if (!PulseCommunicator.Pulse)
		{
			Debug.LogWarning("Waiting for server connection...");
			return;
		}

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

	/// <summary>
	///	Request the entire chat history.
	/// </summary>
	public void SendChatReadRequest(int chatRoomId, System.Action<ChatMessageResponse> callBack)
	{
		if (!PulseCommunicator.Pulse)
		{
			Debug.LogWarning("Waiting for server connection...");
			return;
		}

		string appendedURL = $"{GameController.Instance.ServerURL}chat/{chatRoomId}";

		StartCoroutine(RestWebClient.Instance.HttpGet(appendedURL, (r) =>
		{
			ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);
			callBack(response);
		}));
	}

	/// <summary>
	///	Request the most recent chat history. Returns up to {searchDepth}+1 messages. (Zero-indexed, sending 0 returns 1 message)
	/// </summary>
	public void SendChatUpdateRequest(int chatRoomId, int searchDepth, System.Action<ChatMessageResponse> callBack)
	{
		if (!PulseCommunicator.Pulse)
		{
			Debug.LogWarning("Waiting for server connection...");
			return;
		}

		string appendedURL = $"{GameController.Instance.ServerURL}chat/{chatRoomId}?depth={searchDepth}";

		StartCoroutine(RestWebClient.Instance.HttpGet(appendedURL, (r) =>
		{
			ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);
			callBack(response);
		}));
	}
}