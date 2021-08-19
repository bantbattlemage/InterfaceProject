using System;
using System.Collections.Generic;
using System.Linq;
using GameComms;
using UnityEngine;
using UnityEngine.UI;

public class PanelContentChat : PanelContent
{
	public RectTransform MessagesRoot;
	public GameObject ChatMessagePrefab;
	public InputField TextField;
	public ScrollRect ScrollArea;

	public List<ChatMessageContainer> ChatMessages;

	public int RoomId { get { return _roomId; } }

	public static int CharacterLimit { get { return 1000; } }

	private ChatUser _user;
	private int _roomId;

	void Update()
	{
		AdjustSize();
	}

	void OnDestroy()
	{
		if (!ChatCommunicator.Quitting)
		{
			ChatCommunicator.Instance.RemoveChatPanel(this);
			ChatCommunicator.Instance.NewChatInformationRecieved -= OnNewChatInformationRecieved;
		}
	}

	private void OnNewChatInformationRecieved(ChatMessageResponse messageResponse)
	{
		DisplayChatMessages(messageResponse);
	}

	private void OnScrollTopHit()
	{
		Debug.Log("Nothing implemented on scroll top hit");
	}

	public override void Initialize(int roomId = 1)
	{
		if (ChatMessages != null && ChatMessages.Count != 0)
		{
			throw new System.Exception("tried to initialize chat window already initialized");
		}

		TextField.characterLimit = CharacterLimit;
		TextField.onEndEdit.AddListener((message) =>
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				SubmitMessage(message);
				TextField.text = "";
			}
		});

		ScrollArea.onValueChanged.AddListener((vector) =>
		{
			if (vector.y >= 0.98f)
			{
				OnScrollTopHit();
			}
		});

		ChatMessages = new List<ChatMessageContainer>();
		MessagesRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

		_roomId = roomId;
		PlayerController player = GameController.Instance.Player;

		ChatCommunicator.Instance.NewChatInformationRecieved += OnNewChatInformationRecieved;
		ChatCommunicator.Instance.SendChatJoinRequest(player.UserId, _roomId, player.Username, (response) =>
		{
			if (_user != null)
			{
				Debug.LogWarning("Already initialized chat room.");
				return;
			}

			_user = response.AssignedChatUser;
			ChatCommunicator.Instance.RegisterChatPanel(this);

			DisplayChatMessages(new ChatMessageResponse() { ChatMessages = response.ChatMessages });
		});
	}

	public void SubmitMessage(string message)
	{
		ChatMessage chatMessage = new ChatMessage();
		chatMessage.UserId = _user.UserId;
		chatMessage.RoomId = _user.RoomId;
		chatMessage.Username = _user.Username;
		chatMessage.Message = message;

		ChatCommunicator.Instance.SendChatPostRequest(chatMessage, (response) =>
		{
			Debug.Log("posted message successfully");
		});
	}

	public void DisplayChatMessages(ChatMessageResponse chatData)
	{
		if (chatData == null || chatData.ChatMessages == null || chatData.ChatMessages.Length == 0)
		{
			return;
		}

		List<ChatMessage> orderedData = chatData.ChatMessages.ToList().OrderBy(x => x.TimeStamp).ToList();

		for (int i = 0; i < orderedData.Count; i++)
		{
			AddMessage(orderedData[i]);
		}
	}

	public void AddMessage(ChatMessage data)
	{
		if (data.Message.Length > CharacterLimit)
		{
			InterfaceController.Instance.LogWarning($"Tried to add message longer than character count of {CharacterLimit} ({data.Message.Length})");
			return;
		}

		if (ChatMessages.Any(x => x.Data.TimeStamp == data.TimeStamp && x.Data.Message == data.Message))
		{
			return;
		}

		//string message = $"{data.Username} ({data.TimeStamp.ToLocalTime()}): {data.Message}";

		GameObject newChatMessageObject = Instantiate(ChatMessagePrefab);
		ChatMessageContainer newChatMessage = newChatMessageObject.GetComponent<ChatMessageContainer>();
		newChatMessageObject.transform.SetParent(MessagesRoot);
		newChatMessage.Initialize(data, 60);

		if (ChatMessages.Count % 2 != 0)
		{
			newChatMessageObject.GetComponent<Image>().enabled = false;
		}

		ChatMessages.Add(newChatMessage);
		ChatMessages.ForEach(x => x.AdjustSize());
	}

	public void AdjustSize()
	{
		float size = 0;
		foreach (ChatMessageContainer mesage in ChatMessages) { size += mesage.Rect.rect.height; }

		if (size != MessagesRoot.rect.size.y)
		{
			MessagesRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
		}
	}
}