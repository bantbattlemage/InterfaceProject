using UnityEngine;
using UnityEngine.UI;

public enum GameCommandInputType { CommandBar, InterfacePanel }

public class GameCommandInput : MonoBehaviour
{
	public InputField TextInputField;
	public GameCommandInputType InputType;

	void Start()
	{
		if (TextInputField == null)
		{
			TextInputField = GetComponent<InputField>();
		}

		TextInputField.onEndEdit.AddListener((text) =>
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				ProcessInput(text);
				TextInputField.text = "";
			}
		});
	}

	private void ProcessInput(string input)
	{
		Debug.Log(string.Format("Command entered: {0}", input));

		if (GameCommands.ProcessCommand(input, this))
		{
			Debug.Log(string.Format("{0} command successfully processed", input));
		}
	}
}