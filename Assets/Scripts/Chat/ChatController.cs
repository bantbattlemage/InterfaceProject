using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameComms;
using RestClient.Core;
using Newtonsoft.Json;

public class ChatController : MonoBehaviour
{
	private static ChatController _instance;
	public static ChatController Instance
	{
		get
		{
			if(_instance != null)
			{
				return _instance;
			}

			_instance = GameController.Instance.GetComponent<ChatController>();

			if(_instance == null)
			{
				_instance = GameController.Instance.gameObject.AddComponent<ChatController>();
			}

			return _instance;
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.T))
		{
			SendChatReadRequest(null);
		}
	}

	public void SendChatPostRequest(ChatMessage message)
	{

	}

	public void SendChatReadRequest(ChatRoom room)
	{
		ChatMessageReadRequest request = new ChatMessageReadRequest();
		request.ChatRoomId = 1;
		request.SenderUserId = 1;
		request.LastMessageRead = System.DateTime.Now;

		string appendedURL = $"{GameController.Instance.ServerURL}chat/{request.ChatRoomId}";
		StartCoroutine(RestWebClient.Instance.HttpGet(appendedURL, (r) =>
		{
			Debug.LogWarning(r.Data);
			//ChatMessageResponse response = JsonConvert.DeserializeObject<ChatMessageResponse>(r.Data);
			//Debug.LogWarning(response);
		}));
	}
}