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