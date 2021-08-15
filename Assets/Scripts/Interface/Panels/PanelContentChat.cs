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

	public static int CharacterLimit { get { return 1000; } }

	public RectTransform Rect { get { if (_rect == null) { _rect = GetComponent<RectTransform>(); } return _rect; } }
	private RectTransform _rect;

	void Update()
	{
		AdjustSize();
	}

	public override void Initialize(int numberOfChatMessages = 20)
	{
		if (ChatMessages != null && ChatMessages.Count != 0)
		{
			throw new System.Exception("tried to initialize chat window already initialized");
		}

		TextField.characterLimit = CharacterLimit;
		TextField.onEndEdit.AddListener((x) => { if (Input.GetKeyDown(KeyCode.Return)) SubmitMessage(x); });

		ScrollArea.onValueChanged.AddListener((vector) =>
		{
			if (vector.y >= 0.98f)
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
		if (message.Length > CharacterLimit)
		{
			Debug.LogWarning($"Tried to add message longer than character count of {CharacterLimit} ({message.Length})");
			return;
		}

		GameObject newChatMessageObject = Instantiate(ChatMessagePrefab);
		ChatMessage newChatMessage = newChatMessageObject.GetComponent<ChatMessage>();
		newChatMessage.Initialize();
		newChatMessage.SetText(message);
		newChatMessageObject.transform.SetParent(MessagesRoot);
		ChatMessages.Add(newChatMessage);
		ChatMessages.ForEach(x => x.AdjustSize());

		AdjustSize();
	}

	private void OnScrollTopHit()
	{
		AddTestMessages(5);
	}

	public void AdjustSize()
	{
		float size = 0;
		foreach (ChatMessage mesage in ChatMessages) { size += mesage.Rect.rect.height; }

		if (size != MessagesRoot.rect.size.y)
		{
			MessagesRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
		}
	}

	private void AddTestMessages(int numberOFMessages = 20)
	{
		for (int i = 0; i < numberOFMessages; i++)
		{
			int random = Random.Range(1, 5);
			string s = $"{i} ({random}): ";

			for (int k = 0; k < random; k++)
			{
				s += SelectableText.LoremIpsum + " ";
			}

			AddMessage(s);
		}
	}
}
