using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>상점 목록의 상품 한 줄 표시, 구매 버튼이 눌리면 ShopUI에 알림</summary>
public class ShopSlotUI : MonoBehaviour
{
    #region 인스펙터
    [Header("UI 연결")]
    [SerializeField] private Button _buyButton;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    #endregion

    #region 내부 변수
    private ItemData _itemData;
    private Action<ItemData> _onBuyClicked;
    #endregion

    /// <summary>인스펙터 연결 누락 확인</summary>
    private void Awake()
    {
        bool isValid = true;

        if (_buyButton == null)
        {
            Debug.LogWarning("구매 버튼 없음 (ShopSlotUI)");
            isValid = false;
        }

        if (_iconImage == null)
        {
            Debug.LogWarning("아이콘 이미지 없음 (ShopSlotUI)");
            isValid = false;
        }

        if (_nameText == null || _priceText == null)
        {
            Debug.LogWarning("텍스트 없음 (ShopSlotUI)");
            isValid = false;
        }

        if (!isValid)
        {
            enabled = false;
        }
    }

    /// <summary>구매 버튼 클릭 구독</summary>
    private void OnEnable()
    {
        _buyButton.onClick.AddListener(HandleBuyClick);
    }

    /// <summary>구독 해제</summary>
    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(HandleBuyClick);
    }

    /// <summary>줄을 item 정보로 채우고 구매 콜백 저장</summary>
    public void Setup(ItemData item, Action<ItemData> onBuyClicked)
    {
        if (item == null || !enabled)
        {
            Debug.LogWarning("상품 없음 또는 UI 연결 누락 (ShopSlotUI)");

            return;
        }

        _itemData = item;
        _onBuyClicked = onBuyClicked;

        _nameText.text = item.ItemName;
        _priceText.text = $"{item.Price} G";

        Sprite icon = item.Icon;
        _iconImage.sprite = icon;
        _iconImage.enabled = icon != null;
    }

    /// <summary>구매 버튼이 눌리면 이 줄의 아이템을 콜백으로 전달</summary>
    private void HandleBuyClick()
    {
        _onBuyClicked?.Invoke(_itemData);
    }
}
