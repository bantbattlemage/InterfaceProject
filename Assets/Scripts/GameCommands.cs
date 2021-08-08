using System.Collections;
using UnityEngine;
using System.Linq;

public static class GameCommands
{
	public static bool ProcessCommand(string input, GameCommandInput sender)
	{
		input = input.ToUpper();

		if (ProcessNewPanelCommand(input, sender))
		{
			return true;
		}

		if (ProcessLogOutCommand(input, sender))
		{
			return true;
		}

		return false;
	}

	public static bool ProcessNewPanelCommand(string input, GameCommandInput sender)
	{
		GameCommand newPanelCommand = new GameCommand(new string[] { "NEW" }, (Object t) =>
		{
			InterfaceController.Instance.CreateNewPanel((Transform)t);
		});

		if (newPanelCommand.ProcessCommand(input))
		{
			switch (sender.InputType)
			{
				case GameCommandInputType.CommandBar:
					newPanelCommand.GameAction(null);
					break;
				case GameCommandInputType.InterfacePanel:
					InterfacePanelGroup group = sender.transform.parent.parent.GetComponent<InterfacePanel>().ParentPanelGroup;
					if (group != null)
					{
						newPanelCommand.GameAction(group.transform);
					}
					else
					{
						newPanelCommand.GameAction(null);
					}

					break;
				default:
					break;
			}

			return true;
		}

		return false;
	}

	public static bool ProcessLogOutCommand(string input, GameCommandInput sender)
	{
		GameCommand logOutCommand = new GameCommand(new string[] { "LOGOUT" }, (Object t) =>
		{
			InterfaceController.Instance.LogInScreen.gameObject.SetActive(true);
			InterfaceController.Instance.LogInScreen.InitializeLogInScreen();
		});

		if (logOutCommand.ProcessCommand(input))
		{
			logOutCommand.GameAction(null);
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
