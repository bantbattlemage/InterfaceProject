using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelContentChat : PanelContent
{
	public RectTransform MessagesRoot;
	public GameObject ChatMessagePrefab;

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

		ChatMessages = new List<ChatMessage>();
		MessagesRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

		for (int i = 0; i < numberOfChatMessages; i++)
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

	public void AddMessage(string message = "")
	{
		GameObject newChatMessageObject = Instantiate(ChatMessagePrefab);
		ChatMessage newChatMessage = newChatMessageObject.GetComponent<ChatMessage>();
		ChatMessages.Add(newChatMessage);
		newChatMessage.Initialize();
		newChatMessage.SetText(message);
		newChatMessageObject.transform.SetParent(MessagesRoot);
		newChatMessageObject.transform.SetSiblingIndex(0);

		RectTransform newMessageTransform = newChatMessageObject.GetComponent<RectTransform>();

		float size = MessagesRoot.rect.height + newMessageTransform.rect.height;
		MessagesRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
	}
}
