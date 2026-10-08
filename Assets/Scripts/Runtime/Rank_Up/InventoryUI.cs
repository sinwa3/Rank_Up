using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>인벤토리 패널 열기/닫기, 목록 표시, 선택 관리</summary>
public class InventoryUI : MonoBehaviour
{
    #region 인스펙터
    [Header("데이터")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private PlayerStatus _playerStatus;

    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private InventorySlotUI _slotPrefab;
    [SerializeField] private Button _useButton;

    [Header("입력")]
    [SerializeField] private InputActionReference _toggleAction;
    #endregion

    #region 내부 변수
    private readonly List<InventorySlotUI> _slotUIs = new List<InventorySlotUI>();
    private ItemData _selectedItem;
    #endregion

    /// <summary>인스펙터 연결 누락 확인</summary>
    private void Awake()
    {
        bool isValid = true;

        if (_inventory == null)
        {
            Debug.LogWarning("인벤토리 없음 (InventoryUI)");
            isValid = false;
        }

        if (_panel == null)
        {
            Debug.LogWarning("패널 없음 (InventoryUI)");
            isValid = false;
        }

        if (_slotParent == null || _slotPrefab == null)
        {
            Debug.LogWarning("슬롯 부모 또는 프리팹 없음 (InventoryUI)");
            isValid = false;
        }

        if (_toggleAction == null)
        {
            Debug.LogWarning("토글 입력 없음 (InventoryUI)");
            isValid = false;
        }

        if (_useButton == null)
        {
            Debug.LogWarning("사용 버튼 없음 (InventoryUI)");
            isValid = false;
        }

        if (_playerStatus == null)
        {
            Debug.LogWarning("플레이어 스탯 없음 (InventoryUI)");
            isValid = false;
        }

        if (!isValid)
        {
            enabled = false;
        }
    }

    /// <summary>인벤토리 변경 알림, 토글 입력, 사용 버튼 구독</summary>
    private void OnEnable()
    {
        _inventory.OnSlotsChanged += Refresh;

        _toggleAction.action.performed += HandleToggle;
        _toggleAction.action.Enable();

        _useButton.onClick.AddListener(UseSelectedItem);
    }

    /// <summary>구독 해제</summary>
    private void OnDisable()
    {
        _inventory.OnSlotsChanged -= Refresh;

        _toggleAction.action.performed -= HandleToggle;
        _toggleAction.action.Disable();

        _useButton.onClick.RemoveListener(UseSelectedItem);
    }

    /// <summary>시작할 때 목록을 한 번 그리고 패널을 닫음</summary>
    private void Start()
    {
        Refresh();
        _panel.SetActive(false);
    }

    /// <summary>I 입력 시 패널 열기/닫기</summary>
    private void HandleToggle(InputAction.CallbackContext context)
    {
        _panel.SetActive(!_panel.activeSelf);
    }

    /// <summary>인벤토리 내용대로 슬롯 UI 그림</summary>
    private void Refresh()
    {
        IReadOnlyList<InventorySlot> slots = _inventory.Slots;

        while (_slotUIs.Count < slots.Count)
        {
            InventorySlotUI newSlotUI = Instantiate(_slotPrefab, _slotParent);
            _slotUIs.Add(newSlotUI);
        }

        bool isSelectedFound = false;

        for (int i = 0; i < _slotUIs.Count; i++)
        {
            if (i < slots.Count)
            {
                _slotUIs[i].gameObject.SetActive(true);
                _slotUIs[i].Setup(slots[i], SelectItem);

                if (slots[i].ItemData == _selectedItem)
                {
                    isSelectedFound = true;
                }
            }

            else
            {
                _slotUIs[i].gameObject.SetActive(false);
            }
        }

        if (!isSelectedFound)
        {
            _selectedItem = null;
        }

        UpdateHighlights();
    }

    /// <summary>아이템 선택 (슬롯 클릭 콜백)</summary>
    private void SelectItem(ItemData item)
    {
        _selectedItem = item;
        UpdateHighlights();
    }

    /// <summary>모든 슬롯 UI 중 선택된 아이템만 하이라이트</summary>
    private void UpdateHighlights()
    {
        foreach (var slotUI in _slotUIs)
        {
            slotUI.SetSelected(slotUI.ItemData == _selectedItem);
        }
    }

    /// <summary>선택한 아이템 1개 사용</summary>
    private void UseSelectedItem()
    {
        if (_selectedItem == null)
        {
            return;
        }

        ItemData itemData = _selectedItem;

        if (!_inventory.Remove(itemData))
        {
            return;
        }

        itemData.Use(_playerStatus);
    }
}
