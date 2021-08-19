using GameComms;
using UnityEngine.UI;

public class ChatMessageContainer : SelectableText
{
	public Text UsernameDisplay;
	public Text TimestampDisplay;
	public ChatMessage Data;

	public void Initialize(ChatMessage data, float padding)
	{
		Data = data;
		TextField.characterLimit = PanelContentChat.CharacterLimit;
		Padding = padding;

		UsernameDisplay.text = data.Username;
		TimestampDisplay.text = data.TimeStamp.ToLocalTime().ToString("MM/dd (HH:mm)");
		SetText(data.Message);
	}
}
