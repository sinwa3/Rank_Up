using System;
using UnityEngine;

/// <summary>인벤토리 한 칸</summary>
[Serializable]
public class InventorySlot
{
    #region 인스펙터
    [Header("아이템")]
    [SerializeField] private ItemData _itemData;
    [SerializeField] private int _itemCount;
    #endregion

    #region 프로퍼티
    public ItemData ItemData => _itemData;
    public int ItemCount => _itemCount;
    #endregion

    /// <summary>아이템과 시작 수량으로 칸 생성</summary>
    public InventorySlot(ItemData item, int count)
    {
        _itemData = item;
        _itemCount = count;
    }

    /// <summary>수량을 amount만큼 증가</summary>
    public void AddCount(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        _itemCount += amount;
    }

    /// <summary>수량을 amount만큼 감소</summary>
    public void RemoveCount(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        _itemCount = Mathf.Max(0, _itemCount - amount);
    }
}
