using UnityEngine;

/// <summary>사용하면 HP를 회복하는 아이템</summary>
[CreateAssetMenu(fileName = "HealPotion", menuName = "RankUp/Item Data")]
public class HealItemData : ItemData
{
    #region 인스펙터
    [Header("회복")]
    [SerializeField] private int _healAmount = 1;
    #endregion

    /// <summary>대상의 HP를 회복량만큼 회복</summary>
    public override void Use(PlayerStatus target)
    {
        if (target == null)
        {
            Debug.LogWarning("플레이어 스탯 없음 (HealItemData)");

            return;
        }

        target.Heal(_healAmount);
    }
}
