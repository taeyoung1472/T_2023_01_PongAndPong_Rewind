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
            case IconType.Bujang:
                return researcherIcon;
            case IconType.Sajang:
                return researcherIcon;
            case IconType.Secretary:
                return researcherIcon;
            case IconType.Investor:
                return researcherIcon;
            case IconType.Employee:
                return researcherIcon;
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
            case IconType.Bujang:
                return "부장";
            case IconType.Sajang:
                return "사장";
            case IconType.Secretary:
                return "비서";
            case IconType.Investor:
                return "투자자";
            case IconType.Employee:
                return "직원";
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
