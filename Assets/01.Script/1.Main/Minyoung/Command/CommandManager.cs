using CommandPatterns;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoSingleTon<CommandManager>
{
    private Stack<Command> commandStack = new();
    private Stack<Command> undoStack = new();
    private void Update()
    {
        if (Utility.ComboKeyCheck(KeyCode.LeftControl, KeyCode.Z))
        {
            Undo();
        }
        if (Utility.ComboKeyCheck(KeyCode.LeftControl, KeyCode.Y))
        {
            Redo();
        }
    }
    public void ExcuteCommand(Command cmd)
    {
        cmd.Execute();
        commandStack.Push(cmd);
    }
    private void Redo()     //컨트롤Y
    {
        if (undoStack.Count == 0)
                return;
            Command cmd = undoStack.Pop();
        cmd.Execute();
        commandStack.Push(cmd);
    }
    private void Undo()    //컨트롤Z
    {
        if (commandStack.Count == 0)
            return;
        Command cmd = commandStack.Pop();
        cmd.Undo();
        undoStack.Push(cmd);
    }
}
