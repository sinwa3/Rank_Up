using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>플레이어가 가진 아이템 목록 보관</summary>
public class Inventory : MonoBehaviour
{
    #region 인스펙터
    [Header("슬롯")]
    [SerializeField] private List<InventorySlot> _slots = new List<InventorySlot>();
    #endregion

    #region 이벤트
    public event Action OnSlotsChanged;
    #endregion

    #region 프로퍼티
    public IReadOnlyList<InventorySlot> Slots => _slots;
    #endregion

    /// <summary>아이템을 count개 추가 (이미 있으면 수량만 증가)</summary>
    public void Add(ItemData item, int count = 1)
    {
        if (item == null || count <= 0)
        {
            Debug.LogWarning("잘못된 아이템 또는 개수 (Inventory)");

            return;
        }

        InventorySlot slot = FindSlot(item);

        if (slot != null)
        {
            slot.AddCount(count);
        }

        else
        {
            _slots.Add(new InventorySlot(item, count));
        }

        OnSlotsChanged?.Invoke();
    }

    /// <summary>아이템을 count개 제거 (0개가 되면 칸 삭제), 성공 여부 반환</summary>
    public bool Remove(ItemData item, int count = 1)
    {
        if (item == null || count <= 0)
        {
            Debug.LogWarning("잘못된 아이템 또는 개수 (Inventory)");

            return false;
        }

        InventorySlot slot = FindSlot(item);

        if (slot == null || slot.ItemCount < count)
        {
            return false;
        }

        slot.RemoveCount(count);

        if (slot.ItemCount == 0)
        {
            _slots.Remove(slot);
        }

        OnSlotsChanged?.Invoke();

        return true;
    }

    /// <summary>해당 아이템 칸 검색 (없으면 null)</summary>
    private InventorySlot FindSlot(ItemData item)
    {
        foreach (var slot in _slots)
        {
            if (slot.ItemData == item)
            {
                return slot;
            }
        }

        return null;
    }
}
