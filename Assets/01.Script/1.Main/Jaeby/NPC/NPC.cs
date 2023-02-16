using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCType _npcType = NPCType.None;
    [SerializeField]
    private IconType _iconType = IconType.None;
    [SerializeField]
    private string _name = "";
    [SerializeField]
    private string _title = "";

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
        if (_nameText != null)
            _nameText.SetText(_name);
        if (_titleText != null)
        {
            _titleText.color = _designDataSO.GetColor(_npcType);
            _titleText.SetText("< " + _title + " >");
        }
        if (_iconImage != null)
            _iconImage.sprite = _designDataSO.GetIcon(_iconType);
    }
}