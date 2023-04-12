using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoSingleTon<MenuUIManager>
{
    public Stack<MenuUIElement> uiStack = new();

    public void Close()
    {
        if (uiStack.Count > 0)
        {
            uiStack.Pop();
        }
    }
}
