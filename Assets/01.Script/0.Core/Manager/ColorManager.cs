using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorManager
{
    public static Color GetColor(ColorCODEX codex)
    {
        Color color = new Color();
        color.a = 1;

        switch (codex)
        {
            case ColorCODEX.RED:
                color.r = 255;
                break;
            case ColorCODEX.BLUE:
                color.b = 255;
                break;
            case ColorCODEX.GREEN:
                color.g = 255;
                break;
            case ColorCODEX.YELLOW:
                color.r = 255;
                color.g = 255;
                break;
            case ColorCODEX.MINT:
                color.g = 255;
                color.b = 255;
                break;
            case ColorCODEX.PURPLE:
                color.r = 128;
                color.b = 255;
                break;
            case ColorCODEX.WHITE:
                color = Color.white;
                break;
            default:
                break;
        }

        return color;
    }
}
public enum ColorCODEX
{
    RED,
    BLUE,
    GREEN,
    YELLOW,
    MINT,
    PURPLE,
    WHITE,
}
