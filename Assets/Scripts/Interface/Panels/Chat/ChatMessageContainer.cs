using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameComms;

public class ChatMessageContainer : SelectableText
{
	public ChatMessage Data;

	public void Initialize(ChatMessage data)
	{
		Data = data;
		TextField.characterLimit = PanelContentChat.CharacterLimit;
	}
}
