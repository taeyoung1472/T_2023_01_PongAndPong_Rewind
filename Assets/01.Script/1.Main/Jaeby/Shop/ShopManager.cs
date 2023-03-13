using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class ShopManager : MonoSingleTon<ShopManager>
{
    [SerializeField]
    private TextMeshProUGUI _moneyText = null;

    private List<ItemSlot> _damgis = new List<ItemSlot>();
    private List<DamgiItem> _damgiItems = new List<DamgiItem>();

    [SerializeField]
    private DamgiItem _damgiItemPrefab = null;
    [SerializeField]
    private GameObject _jangbaguniObj = null;
    [SerializeField]
    private GameObject _itemListObj = null;
    [SerializeField]
    private Transform _damgiParent = null;

    [SerializeField]
    private RectTransform _content = null;
    private float _originHeight = 0f;

    private void Start()
    {
        _originHeight = _content.rect.height;
        _moneyText.SetText(player.playerJsonData.money.ToString());
        GoItemList();
    }

    public void GoItemList()
    {
        _itemListObj.SetActive(true);
        _jangbaguniObj.SetActive(false);
    }

    public void GoJangbaguni()
    {
        for(int i = 0; i < _damgiItems.Count; i++)
        {
            Destroy(_damgiItems[i].gameObject);
        }
        _damgiItems.Clear();

        _itemListObj.SetActive(false);
        _jangbaguniObj.SetActive(true);
        _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _originHeight);

        for (int i = 0; i < _damgis.Count; i++)
        {
            DamgiItem damgi = Instantiate(_damgiItemPrefab, _damgiParent);
            damgi.UISet(_damgis[i].Data);
            damgi.transform.SetSiblingIndex(0);
            _damgiItems.Add(damgi);
            _content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _content.rect.height + damgi.GetComponent<RectTransform>().rect.height);
        }
    }

    public bool TryDamgi(ItemSlot slot)
    {
        int allPrice = 0;
        for (int i = 0; i < _damgis.Count; i++)
            allPrice += _damgis[i].Data.price;
        allPrice += slot.Data.price;
        if (player.playerJsonData.money < allPrice)
            return false;

        _damgis.Add(slot);
        return true;
    }

    public void Buy()
    {
        for (int i = 0; i < _damgis.Count; i++)
            _damgis[i].TryBuy();
        _damgis.Clear();
        _moneyText.SetText(player.playerJsonData.money.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            Buy();
    }
}
