using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>상점 패널 열기/닫기, 상품 목록 표시, 구매 처리와 결과 표시</summary>
public class ShopUI : MonoBehaviour
{
    #region 인스펙터
    [Header("데이터")]
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private ShopZone _shopZone;
    [SerializeField] private List<ItemData> _products = new List<ItemData>();

    [Header("UI")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Transform _slotParent;
    [SerializeField] private ShopSlotUI _slotPrefab;
    [SerializeField] private TMP_Text _feedbackText;

    [Header("입력")]
    [SerializeField] private InputActionReference _interactAction;
    #endregion

    /// <summary>인스펙터 연결 누락 확인</summary>
    private void Awake()
    {
        bool isValid = true;

        if (_inventory == null)
        {
            Debug.LogWarning("인벤토리 없음 (ShopUI)");
            isValid = false;
        }

        if (_wallet == null)
        {
            Debug.LogWarning("지갑 없음 (ShopUI)");
            isValid = false;
        }

        if (_shopZone == null)
        {
            Debug.LogWarning("상점 범위 없음 (ShopUI)");
            isValid = false;
        }

        if (_products.Count == 0)
        {
            Debug.LogWarning("상품 목록 비어 있음 (ShopUI)");
            isValid = false;
        }

        foreach (var product in _products)
        {
            if (product == null || product.Price <= 0)
            {
                Debug.LogWarning("빈 상품 또는 가격 0 이하 상품 있음 (ShopUI)");
                isValid = false;

                break;
            }
        }

        if (_panel == null)
        {
            Debug.LogWarning("패널 없음 (ShopUI)");
            isValid = false;
        }

        if (_slotParent == null || _slotPrefab == null)
        {
            Debug.LogWarning("슬롯 부모 또는 프리팹 없음 (ShopUI)");
            isValid = false;
        }

        if (_feedbackText == null)
        {
            Debug.LogWarning("피드백 텍스트 없음 (ShopUI)");
            isValid = false;
        }

        if (_interactAction == null)
        {
            Debug.LogWarning("상호작용 입력 없음 (ShopUI)");
            isValid = false;
        }

        if (!isValid)
        {
            enabled = false;
        }
    }

    /// <summary>범위 변경 알림, 상호작용 입력 구독</summary>
    private void OnEnable()
    {
        _shopZone.OnRangeChanged += HandleRangeChanged;

        _interactAction.action.performed += HandleInteract;
        _interactAction.action.Enable();
    }

    /// <summary>구독 해제</summary>
    private void OnDisable()
    {
        _shopZone.OnRangeChanged -= HandleRangeChanged;

        _interactAction.action.performed -= HandleInteract;
        _interactAction.action.Disable();
    }

    /// <summary>시작할 때 상품 목록을 한 번 그리고 패널을 닫음</summary>
    private void Start()
    {
        BuildSlots();
        _feedbackText.text = "";
        _panel.SetActive(false);
    }

    /// <summary>E 입력 시 열려 있으면 닫고, 닫혀 있으면 범위 안일 때만 열기</summary>
    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (_panel.activeSelf)
        {
            _panel.SetActive(false);

            return;
        }

        if (!_shopZone.IsPlayerInRange)
        {
            return;
        }

        _feedbackText.text = "";
        _panel.SetActive(true);
    }

    /// <summary>범위를 벗어나면 패널 닫기</summary>
    private void HandleRangeChanged()
    {
        if (!_shopZone.IsPlayerInRange)
        {
            _panel.SetActive(false);
        }
    }

    /// <summary>상품마다 UI를 하나씩 만듦</summary>
    private void BuildSlots()
    {
        foreach (var product in _products)
        {
            ShopSlotUI slotUI = Instantiate(_slotPrefab, _slotParent);
            slotUI.Setup(product, BuyItem);
        }
    }

    /// <summary>item 구매 시도 (상품 줄 구매 버튼 콜백)</summary>
    private void BuyItem(ItemData item)
    {
        if (!_wallet.TrySpend(item.Price))
        {
            _feedbackText.text = "골드 부족";

            return;
        }

        _inventory.Add(item);
        _feedbackText.text = $"{item.ItemName} 구매 성공";
    }
}
