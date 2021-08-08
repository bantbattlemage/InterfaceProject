using System.Collections;
using UnityEngine;
using System.Linq;

public static class GameCommands
{
	public static bool ProcessCommand(string input, GameCommandInput sender)
	{
		input = input.ToUpper();

		//  NEW command to open a new panel
		GameCommand newPanel = new GameCommand(new string[] { "NEW" }, (Object t) =>
		{
			InterfaceController.Instance.CreateNewPanel((Transform)t);
		});

		if (newPanel.ProcessCommand(input))
		{
			switch (sender.InputType)
			{
				case GameCommandInputType.CommandBar:
					newPanel.GameAction(null);
					break;
				case GameCommandInputType.InterfacePanel:
					InterfacePanelGroup group = sender.transform.parent.parent.GetComponent<InterfacePanel>().ParentPanelGroup;
					if (group != null)
					{
						newPanel.GameAction(group.transform);
					}
					else
					{
						newPanel.GameAction(null);
					}

					break;
				default:
					break;
			}

			return true;
		}

		return false;
	}
}

public class GameCommand
{
	public string[] ValidInputs { get; private set; }
	public System.Action<Object> GameAction { get; private set; }

	public GameCommand(string[] validInputs, System.Action<Object> gameAction)
	{
		ValidInputs = validInputs;
		GameAction = gameAction;
	}

	public bool ProcessCommand(string input)
	{
		if (ValidInputs.Any(x => x == input))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
