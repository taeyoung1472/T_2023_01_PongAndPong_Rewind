using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTex;
    [SerializeField] private Vector2 cursotHotspot;

#if !UNITY_EDITOR
    private void Start()
    {
        Cursor.SetCursor(cursorTex, new Vector2(cursorTex.width * cursotHotspot.x, cursorTex.height * cursotHotspot.y), CursorMode.Auto);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
#endif
}
