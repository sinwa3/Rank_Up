using UnityEngine;

/// <summary>월드에 놓인 아이템, 플레이어가 닿으면 인벤토리에 추가되고 사라짐</summary>
public class WorldItem : MonoBehaviour
{
    #region 인스펙터
    [Header("플레이어")]
    [SerializeField] private string _playerTag = "Player";

    [Header("아이템")]
    [SerializeField] private ItemData _itemData;
    [SerializeField] private int _addCount = 1;
    #endregion

    #region 내부 변수
    private bool _isCollected = false;
    #endregion

    /// <summary>아이템 연결 누락 확인</summary>
    private void Awake()
    {
        if (_itemData == null)
        {
            Debug.LogWarning("아이템 데이터 없음 (WorldItem)");
            enabled = false;
        }
    }

    /// <summary>닿은 대상이 인벤토리를 가졌으면 획득 처리</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected || !enabled)
        {
            return;
        }

        if (!string.IsNullOrEmpty(_playerTag) && other.CompareTag(_playerTag))
        {
            Inventory inventory = other.GetComponent<Inventory>();

            if (inventory == null)
            {
                Debug.LogWarning("플레이어에게 인벤토리 없음 (WorldItem)");

                return;
            }

            inventory.Add(_itemData, _addCount);
            _isCollected = true;
            gameObject.SetActive(false);
        }
    }
}
