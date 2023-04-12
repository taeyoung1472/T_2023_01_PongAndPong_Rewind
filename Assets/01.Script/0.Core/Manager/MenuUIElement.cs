using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIElement : MonoBehaviour
{
    public void Open()
    {
        MenuUIManager.Instance.uiStack.Push(this);
    }
    
    public void Close()
    {
        MenuUIManager.Instance.Close();
    }
}
