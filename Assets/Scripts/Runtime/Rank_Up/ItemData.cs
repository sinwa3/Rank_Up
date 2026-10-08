using UnityEngine;

/// <summary>모든 아이템의 공통 정보가 있는 부모 에셋</summary>
public abstract class ItemData : ScriptableObject
{
    #region 인스펙터
    [Header("식별 정보")]
    [SerializeField] private string _itemName = "";
    [SerializeField] private Sprite _icon;
    [SerializeField, TextArea] private string _itemDescription = "";

    [Header("상점")]
    [SerializeField] private int _price = 0;
    #endregion

    #region 프로퍼티
    public string ItemName => _itemName;
    public Sprite Icon => _icon;
    public string ItemDescription => _itemDescription;
    public int Price => _price;
    #endregion

    /// <summary>아이템 효과를 대상에게 적용</summary>
    public abstract void Use(PlayerStatus target);
}
