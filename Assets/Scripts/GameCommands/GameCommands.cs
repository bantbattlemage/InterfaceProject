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

		if (ProcessHelpCommand(input, sender))
		{
			return true;
		}

		if (ProcessMarketCommand(input, sender))
		{
			return true;
		}

        if (ProcessMapCommand(input, sender))
        {
            return true;
        }

		return false;
	}

	public static bool ProcessNewPanelCommand(string input, GameCommandInput sender)
	{
		GameCommand command = new GameCommand(new string[] { "NEW" }, (Object t) =>
		{
			InterfaceController.Instance.CreateNewPanel((Transform)t);
		});

		if (command.ProcessCommand(input))
		{
			switch (sender.InputType)
			{
				case GameCommandInputType.CommandBar:
					command.CommandAction(null);
					break;
				case GameCommandInputType.InterfacePanel:
					InterfacePanelGroup group = sender.transform.parent.parent.GetComponent<InterfacePanel>().ParentPanelGroup;
					if (group != null)
					{
						command.CommandAction(group.transform);
					}
					else
					{
						command.CommandAction(null);
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
		GameCommand command = new GameCommand(new string[] { "LOGOUT", "OUT" }, (Object t) =>
		{
			InterfaceController.Instance.LogOut();
		});

		if (command.ProcessCommand(input))
		{
			command.CommandAction(null);
			return true;
		}

		return false;
	}

	public static bool ProcessHelpCommand(string input, GameCommandInput sender)
	{
		GameCommand command = new GameCommand(new string[] { "HELP", "HLP" }, (Object t) =>
		{
			string helpText = "Type a Command into a Command Input Field to open a new window or panel. \n"
			+ "Command List: \n" +
			"HELP: Open this window. \n" +
			"LOGOUT: Log out of the game. \n" +
			"NEW: Create a new panel. \n" +
			"MARKET, MKT: Open the market. \n" +
			"MAP: Open the map. \n";

            InterfaceController.Instance.CreateNewPopUp("Help", helpText, new PopUpButtonProperties[0]);
		});

		if (command.ProcessCommand(input))
		{
			command.CommandAction(null);
			return true;
		}

		return false;
	}

	public static bool ProcessMarketCommand(string input, GameCommandInput sender)
	{
		GameCommand command = new GameCommand(new string[] { "MARKET", "MKT" }, (Object t) =>
		{
			InterfaceController.Instance.CreateMarketPanel((Transform)t);
		});

		if (command.ProcessCommand(input))
		{
			if (sender.InputType == GameCommandInputType.InterfacePanel)
			{
				if (sender.transform.parent.parent.GetComponent<InterfacePanel>())
				{
					command.CommandAction(sender.transform.parent.parent);
				}
			}
			else
			{
				command.CommandAction(null);
			}

			return true;
		}

		return false;
	}

    public static bool ProcessMapCommand(string input, GameCommandInput sender)
    {
        GameCommand command = new GameCommand(new string[] { "MAP" }, (Object t) =>
        {
            InterfaceController.Instance.CreateMapPanel((Transform)t);
        });

        if (command.ProcessCommand(input))
        {
            if (sender.InputType == GameCommandInputType.InterfacePanel)
            {
                if (sender.transform.parent.parent.GetComponent<InterfacePanel>())
                {
                    command.CommandAction(sender.transform.parent.parent);
                }
            }
            else
            {
                command.CommandAction(null);
            }

            return true;
        }

        return false;
    }
}