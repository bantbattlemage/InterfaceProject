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

	public static int CharacterLimit { get { return 1000; } }

	public RectTransform Rect { get { if (_rect == null) { _rect = GetComponent<RectTransform>(); } return _rect; } }
	private RectTransform _rect;

	private Coroutine _echo;

	void Update()
	{
		AdjustSize();
	}

	void OnDestroy()
	{
		if (_echo != null)
		{
			StopCoroutine(_echo);
		}
	}

	public override void Initialize(int numberOfChatMessages = 20)
	{
		if (ChatMessages != null && ChatMessages.Count != 0)
		{
			throw new System.Exception("tried to initialize chat window already initialized");
		}

		TextField.characterLimit = CharacterLimit;
		TextField.onEndEdit.AddListener((x) =>
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				SubmitMessage(x);
			}

			TextField.text = "";
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

		// ChatController.Instance.SendChatReadRequest(1, (response) =>
		// {
		// 	DisplayChatMessages(response);
		// 	StartCoroutine(StartEcho());
		// });

		StartCoroutine(Echo());
	}

	private IEnumerator<WaitForSeconds> StartEcho()
	{
		if (_echo != null)
		{
			throw new System.Exception("tried to start echo already running");
		}

		yield return new WaitForSeconds(2f);

		_echo = StartCoroutine(Echo());
	}

	private IEnumerator<WaitForSeconds> Echo()
	{
		ChatMessage lastReadMessage;

		if (ChatMessages.Count > 0)
		{
			lastReadMessage = ChatMessages[ChatMessages.Count - 1].Data;
		}
		else
		{
			lastReadMessage = new ChatMessage();
			lastReadMessage.Message = "";
			lastReadMessage.RoomId = 1;
			lastReadMessage.UserId = -1;
			DateTime fakeTime = DateTime.Today.AddYears(-1);
			// DateTime oneYear = DateTime.MinValue.AddYears(1);
			// TimeSpan s = fakeTime.Subtract(oneYear);
			lastReadMessage.TimeStamp = fakeTime;
		}

		ChatController.Instance.SendChatUpdateRequest(lastReadMessage, (response) =>
		{
			DisplayChatMessages(response);
		});

		yield return new WaitForSeconds(2f);

		_echo = StartCoroutine(Echo());
	}

	public void SubmitMessage(string message)
	{
		ChatMessage chatMessage = new ChatMessage();
		chatMessage.UserId = 1;
		chatMessage.RoomId = 1;
		chatMessage.Message = message;

		ChatController.Instance.SendChatPostRequest(chatMessage, (response) =>
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

		if (ChatMessages.Any(x => x.Data == data))
		{
			return;
		}

		string message = data.UserId.ToString("000");
		message += ": " + data.Message;

		GameObject newChatMessageObject = Instantiate(ChatMessagePrefab);
		ChatMessageContainer newChatMessage = newChatMessageObject.GetComponent<ChatMessageContainer>();
		newChatMessage.Initialize(data);
		newChatMessage.SetText(message);
		newChatMessageObject.transform.SetParent(MessagesRoot);
		ChatMessages.Add(newChatMessage);
		ChatMessages.ForEach(x => x.AdjustSize());

		// ChatController.Instance.SendReadDisplayNameRequest(data.UserId, (response) =>
		// {
		// 	string message = response.User.Username;
		// 	message += ": " + data.Message;

		// 	GameObject newChatMessageObject = Instantiate(ChatMessagePrefab);
		// 	ChatMessageContainer newChatMessage = newChatMessageObject.GetComponent<ChatMessageContainer>();
		// 	newChatMessage.Initialize(data);
		// 	newChatMessage.SetText(message);
		// 	newChatMessageObject.transform.SetParent(MessagesRoot);
		// 	ChatMessages.Add(newChatMessage);
		// 	ChatMessages.ForEach(x => x.AdjustSize());

		// 	AdjustSize();
		// });
	}

	private void OnScrollTopHit()
	{

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


// private void AddTestMessages(int numberOFMessages = 20)
// {
// 	for (int i = 0; i < numberOFMessages; i++)
// 	{
// 		int random = Random.Range(1, 5);
// 		string s = $"{i} ({random}): ";

// 		for (int k = 0; k < random; k++)
// 		{
// 			s += SelectableText.LoremIpsum + " ";
// 		}

// 		AddMessage(s);
// 	}
// }
