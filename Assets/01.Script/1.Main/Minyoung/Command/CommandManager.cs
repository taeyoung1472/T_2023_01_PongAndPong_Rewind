using CommandPatterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    Stack<Command> totalCommand;

    Command keyCreate;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ExcuteCommand(keyCreate);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Reverse());
        }
    }
    private void ExcuteCommand(Command command)
    {
        command.Execute();
        totalCommand.Push(command);
    }

    private IEnumerator Reverse()
    {
        while (totalCommand.Count > 0)
        {
            Command cmd = totalCommand.Pop();
            cmd.Undo();
            yield return new WaitForSeconds(2);
        }
    }
}
