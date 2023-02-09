using CommandPatterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommandManager : MonoBehaviour
{
    public Stack<Command> totalComandHistory = new Stack<Command>();

    private Stack<Command> commandHistory = new Stack<Command>();
    private Stack<Command> undoHistory = new Stack<Command>();

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    Undo();
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    Redo();
        //}

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
           // ExcuteCommand(keyCreate);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Reverse());
        }
    }
    //private static void ExcuteNewCommand(Command commandButton)
    //{
    //    commandButton.Execute();
    //    commandHistory.Push(commandButton);
    //    //totalComandHistory.Push(commandButton.Execute);
    //}
    private IEnumerator Reverse()
    {
        while (totalComandHistory.Count > 0)
        {
            Debug.Log(totalComandHistory.Count);
            Debug.Log("ㄴㄷㅌ");
            Command cmd = totalComandHistory.Pop();
            cmd.Undo();
            yield return new WaitForSeconds(2);
        }
    }
    public  void ExcuteCommand(Command command)
    {
        command.Execute();
        totalComandHistory.Push(command);
    }

  
    //private void Redo()
    //{
    //    if (undoHistory.Count == 0) { print("REDO�� ������ ���� : ������"); return; }
    //    Command cmd = undoHistory.Pop();
    //    cmd.Execute();
    //   // totalComandHistory.Push(cmd.Execute);
    //}

    //private void Undo()
    //{
    //    if (commandHistory.Count == 0) { print("UNDO�� ������ ���� : �ʱⰪ"); return; }
    //    Command cmd = commandHistory.Pop();
    //    cmd.Undo();
    //    undoHistory.Push(cmd);
    //    //totalComandHistory.Push(cmd.Undo);
    //}


}
