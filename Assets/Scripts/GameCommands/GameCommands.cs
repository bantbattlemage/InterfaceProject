using System.Collections;
using UnityEngine;
using System.Linq;

public static class GameCommands
{
	public static string[] ValidHelpInputs = new string[] { "HLP", "HELP" };
	public static string[] ValidNewPanelInputs = new string[] { "NEW", "PANEL" };
	public static string[] ValidChatInputs = new string[] { "CHAT" };
	public static string[] ValidMapInputs = new string[] { "MAP" };
	public static string[] ValidMarketInputs = new string[] { "MKT, MARKET" };
	public static string[] ValidLogOutInputs = new string[] { "OUT", "LOGOUT" };

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

		if (ProcessChatCommand(input, sender))
		{
			return true;
		}

		return false;
	}

	public static bool ProcessNewPanelCommand(string input, GameCommandInput sender)
	{
		GameCommand command = new GameCommand(ValidNewPanelInputs, (Object t) =>
		{
			switch (sender.InputType)
			{
				case GameCommandInputType.CommandBar:
					InterfaceController.Instance.CreateNewPanel();
					break;
				case GameCommandInputType.InterfacePanel:
					InterfacePanelGroup group = sender.transform.parent.parent.GetComponent<InterfacePanel>().ParentPanelGroup;
					if (group != null)
					{
						InterfaceController.Instance.CreateNewPanel(group.transform);
					}
					else
					{
						InterfaceController.Instance.CreateNewPanel();
					}
					break;
				default:
					InterfaceController.Instance.LogWarning("InterfacePanel has no type?");
					break;
			}
		});

		return command.ProcessCommand(input, sender);
	}

	public static bool ProcessLogOutCommand(string input, GameCommandInput sender)
	{
		GameCommand command = new GameCommand(ValidLogOutInputs, (Object t) =>
		{
			InterfaceController.Instance.LogOut();
		});

		return command.ProcessCommand(input, sender);
	}

	public static bool ProcessHelpCommand(string input, GameCommandInput sender)
	{
		GameCommand command = new GameCommand(ValidHelpInputs, (Object t) =>
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

		return command.ProcessCommand(input, sender);
	}

	public static bool ProcessMarketCommand(string input, GameCommandInput sender)
	{
		GameCommand command = new GameCommand(new string[] { "MARKET", "MKT" }, (Object t) =>
		{
			InterfaceController.Instance.LogWarning("NOT IMPLEMENTED YET");
			//InterfaceController.Instance.CreateMarketPanel((Transform)t);
		});

		return command.ProcessCommand(input, sender);
	}

	public static bool ProcessMapCommand(string input, GameCommandInput sender)
	{
		GameCommand command = new GameCommand(ValidMapInputs, (Object t) =>
		{
			InterfaceController.Instance.CreateNewPanel(InterfaceController.Instance.MapPrefab, (Transform)t);
		});

		return command.ProcessCommand(input, sender);
	}

	public static bool ProcessChatCommand(string input, GameCommandInput sender)
	{
		GameCommand command = new GameCommand(ValidChatInputs, (Object t) =>
		{
			InterfacePanel chatPanel = InterfaceController.Instance.CreateNewPanel(InterfaceController.Instance.ChatPrefab, (Transform)t);
			PanelContent chat = chatPanel.ContentObjects[0] as PanelContentChat;
			chat.Initialize(Globals.UNIVERSE_CHAT);
		});

		return command.ProcessCommand(input, sender);
	}
}