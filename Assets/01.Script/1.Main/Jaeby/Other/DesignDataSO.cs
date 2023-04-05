using UnityEngine;

[CreateAssetMenu(menuName = "SO/Data/DesignData")]
public class DesignDataSO : ScriptableObject
{
    [Header("아이콘")]
    public Sprite researcherIcon = null;
    public Sprite portalIcon = null;
    public Sprite traderIcon = null;
    [Header("색깔")]
    public Color nomalColor = Color.white;
    public Color portalColor = Color.white;
    public Color specialColor = Color.white;

    public Sprite GetIcon(IconType type)
    {
        switch (type)
        {
            case IconType.None:
                return null;
            case IconType.Researcher:
                return researcherIcon;
            case IconType.Portal:
                return portalIcon;
            case IconType.Trader:
                return traderIcon;
            default:
                break;
        }
        return null;
    }

    public string GetTitle(IconType type)
    {
        switch (type)
        {
            case IconType.None:
                return null;
            case IconType.Researcher:
                return "연구원";
            case IconType.Portal:
                return "포탈 관리인";
            case IconType.Trader:
                return "상인";
            default:
                break;
        }
        return null;
    }

    public Color GetColor(NPCType type)
    {
        switch (type)
        {
            case NPCType.None:
                break;
            case NPCType.Nomal:
                return nomalColor;
            case NPCType.Portal:
                return portalColor;
            case NPCType.Special:
                return specialColor;
            default:
                break;
        }
        return Color.white;
    }
}
