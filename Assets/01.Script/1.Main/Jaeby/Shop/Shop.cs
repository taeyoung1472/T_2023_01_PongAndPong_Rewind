using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class Shop : MonoSingleTon<Shop>
{
    [SerializeField]
    private ItemSlot _slotPrefab = null;
    [SerializeField]
    private Transform _parent = null;

    private List<GameObject> _children = new List<GameObject>();

    [SerializeField]
    private RectTransform _content = null;
    private float _originWidth = 0f;

    private void Start()
    {
        _originWidth = _content.rect.width;
    }

    public void ShopInit(List<string> itemKeys)
    {
        for (int i = 0; i < _children.Count; i++)
        {
            Destroy(_children[i]);
        }
        _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _originWidth);
        for (int i = 0; i < itemKeys.Count; i++)
        {
            ItemData data = ItemDB.Instance.TryGetItem(itemKeys[i]);
            if (data != null)
            {
                ItemSlot slot = Instantiate(_slotPrefab);
                _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _content.rect.width + slot.GetComponent<RectTransform>().rect.width);
                slot.transform.SetParent(_parent);
                slot.Init(data);
                _children.Add(slot.gameObject);
            }
        }
    }
}
