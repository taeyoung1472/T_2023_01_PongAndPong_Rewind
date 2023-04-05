using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCData _npcData = null;
    public NPCData npcData => _npcData;

    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _titleText = null;
    [SerializeField]
    private Image _iconImage = null;
    [SerializeField]
    private DesignDataSO _designDataSO = null;

    private void OnValidate()
    {
        if (_npcData == null || _designDataSO == null)
            return;

        if (_nameText != null)
            _nameText.SetText(_npcData.npcName);
        if (_titleText != null)
        {
            _titleText.color = _designDataSO.GetColor(_npcData.npcType);
            _titleText.SetText("< " + _designDataSO.GetTitle(_npcData.iconType) + " >");
        }
        if (_iconImage != null)
            _iconImage.sprite = _designDataSO.GetIcon(_npcData.iconType);
    }

    public void CameraTargetAdd()
    {
        CamManager.Instance.AddTargetGroup(transform, 3f, 2.5f);
    }

    public void CameraTargetRemove()
    {
        CamManager.Instance.RemoveTargetGroup(transform);
    }
}