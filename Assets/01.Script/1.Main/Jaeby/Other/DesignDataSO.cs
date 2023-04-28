using UnityEngine;

[CreateAssetMenu(menuName = "SO/Data/DesignData")]
public class DesignDataSO : ScriptableObject
{
    [Header("������")]
    public Sprite researcherIcon = null;
    public Sprite portalIcon = null;
    public Sprite traderIcon = null;
    [Header("����")]
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
                return "������";
            case IconType.Portal:
                return "��Ż ������";
            case IconType.Trader:
                return "����";
            case IconType.Bujang:
                return "����";
            case IconType.Sajang:
                return "����";
            case IconType.Secretary:
                return "��";
            case IconType.Investor:
                return "������";
            case IconType.Employee:
                return "����";
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
