using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelContentChat : PanelContent
{
	public RectTransform MessagesRoot;
	public GameObject ChatMessagePrefab;
	public InputField TextField;
	public ScrollRect ScrollArea;

	public List<ChatMessage> ChatMessages;

	void OnGUI()
	{
		float size = 0;
		ChatMessages.ForEach(x => size += x.GetComponent<RectTransform>().rect.height);
		MessagesRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
	}

	public override void Initialize(int numberOfChatMessages = 20)
	{
		if (ChatMessages != null && ChatMessages.Count != 0)
		{
			throw new System.Exception("tried to initialize chat window already initialized");
		}

		TextField.onEndEdit.AddListener((x) => { if (Input.GetKeyDown(KeyCode.Return)) SubmitMessage(x); });

		ScrollArea.onValueChanged.AddListener((vector) =>
		{
			if (vector.y == 1)
			{
				OnScrollTopHit();
			}
		});

		ChatMessages = new List<ChatMessage>();
		MessagesRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

		AddTestMessages();
	}

	public void SubmitMessage(string message)
	{
		AddMessage(message);
	}

	public void AddMessage(string message = "")
	{
		GameObject newChatMessageObject = Instantiate(ChatMessagePrefab);
		ChatMessage newChatMessage = newChatMessageObject.GetComponent<ChatMessage>();
		ChatMessages.Add(newChatMessage);
		newChatMessage.Initialize();
		newChatMessage.SetText(message);
		newChatMessageObject.transform.SetParent(MessagesRoot);
		// newChatMessageObject.transform.SetSiblingIndex(0);

		RectTransform newMessageTransform = newChatMessageObject.GetComponent<RectTransform>();

		float size = MessagesRoot.rect.height + newMessageTransform.rect.height;
		MessagesRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
	}

	private void OnScrollTopHit()
	{
		AddTestMessages();
	}

	private void AddTestMessages(int numberOFMessages = 20)
	{
		for (int i = 0; i < 20; i++)
		{
			int random = Random.Range(1, 10);
			string s = $"{i} ({random}): ";

			for (int k = 0; k < random; k++)
			{
				s += SelectableText.LoremIpsum + " ";
			}

			AddMessage(s);
		}
	}
}
