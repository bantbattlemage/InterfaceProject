using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessageContainer : SelectableText
{
	public void Initialize()
	{
		TextField.characterLimit = PanelContentChat.CharacterLimit;
	}
}
