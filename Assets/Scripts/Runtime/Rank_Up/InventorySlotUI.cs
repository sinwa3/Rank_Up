using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>인벤토리 목록의 한 칸 표시, 클릭되면 InventoryUI에 알림</summary>
public class InventorySlotUI : MonoBehaviour
{
    #region 인스펙터
    [Header("UI 연결")]
    [SerializeField] private Button _button;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private GameObject _highlight;
    #endregion

    #region 내부 변수
    private ItemData _itemData;
    private Action<ItemData> _onClicked;
    #endregion

    #region 프로퍼티
    public ItemData ItemData => _itemData;
    #endregion

    /// <summary>인스펙터 연결 누락 확인</summary>
    private void Awake()
    {
        bool isValid = true;

        if (_button == null)
        {
            Debug.LogWarning("버튼 없음 (InventorySlotUI)");
            isValid = false;
        }

        if (_iconImage == null)
        {
            Debug.LogWarning("아이콘 이미지 없음 (InventorySlotUI)");
            isValid = false;
        }

        if (_nameText == null || _countText == null)
        {
            Debug.LogWarning("텍스트 없음 (InventorySlotUI)");
            isValid = false;
        }

        if (_highlight == null)
        {
            Debug.LogWarning("하이라이트 오브젝트 없음 (InventorySlotUI)");
            isValid = false;
        }

        if (!isValid)
        {
            enabled = false;
        }
    }

    /// <summary>클릭 구독</summary>
    private void OnEnable()
    {
        _button.onClick.AddListener(HandleClick);
    }

    /// <summary>구독 해제</summary>
    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    /// <summary>칸을 slot 정보로 채우고 클릭 콜백 저장</summary>
    public void Setup(InventorySlot slot, Action<ItemData> onClicked)
    {
        if (slot == null || slot.ItemData == null || !enabled)
        {
            Debug.LogWarning("아이템 없는 칸 또는 UI 연결 누락 (InventorySlotUI)");

            return;
        }

        _itemData = slot.ItemData;
        _onClicked = onClicked;
        _nameText.text = _itemData.ItemName;
        _countText.text = slot.ItemCount.ToString();

        Sprite icon = _itemData.Icon;
        _iconImage.sprite = icon;
        _iconImage.enabled = icon != null;
    }

    /// <summary>선택 하이라이트 켜기/끄기</summary>
    public void SetSelected(bool isSelected)
    {
        if (!enabled)
        {
            return;
        }

        _highlight.SetActive(isSelected);
    }

    /// <summary>클릭되면 이 칸의 아이템을 콜백으로 전달</summary>
    private void HandleClick()
    {
        _onClicked?.Invoke(_itemData);
    }
}
