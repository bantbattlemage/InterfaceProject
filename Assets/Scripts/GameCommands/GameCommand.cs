using System.Linq;
using UnityEngine;

public class GameCommand
{
	public string[] ValidInputs { get; private set; }
	public System.Action<Object> CommandAction { get; private set; }

	public GameCommand(string[] validInputs, System.Action<Object> gameAction)
	{
		ValidInputs = validInputs;
		CommandAction = gameAction;
	}

	public bool ProcessCommand(string input, GameCommandInput sender)
	{
		if (ValidInputs.Any(x => x == input))
		{
			if (sender != null && sender.InputType == GameCommandInputType.InterfacePanel)
			{
				if (sender.transform.parent.parent.GetComponent<InterfacePanel>())
				{
					CommandAction(sender.transform.parent.parent);
				}
			}
			else
			{
				CommandAction(null);
			}

			return true;
		}
		else
		{
			return false;
		}
	}
}