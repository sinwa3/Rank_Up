using TMPro;
using UnityEngine;

/// <summary>플레이어 재화를 텍스트로 표시</summary>
public class GoldUI : MonoBehaviour
{
    #region 인스펙터
    [Header("데이터")]
    [SerializeField] private Wallet _wallet;

    [Header("UI")]
    [SerializeField] private TMP_Text _goldText;
    #endregion

    /// <summary>인스펙터 연결 누락 확인</summary>
    private void Awake()
    {
        bool isValid = true;

        if (_wallet == null)
        {
            Debug.LogWarning("지갑 없음 (GoldUI)");
            isValid = false;
        }

        if (_goldText == null)
        {
            Debug.LogWarning("골드 텍스트 없음 (GoldUI)");
            isValid = false;
        }

        if (!isValid)
        {
            enabled = false;
        }
    }

    /// <summary>재화 변경 알림 구독</summary>
    private void OnEnable()
    {
        _wallet.OnGoldChanged += Refresh;
    }

    /// <summary>구독 해제</summary>
    private void OnDisable()
    {
        _wallet.OnGoldChanged -= Refresh;
    }

    /// <summary>시작 재화를 한 번 그림</summary>
    private void Start()
    {
        Refresh();
    }

    /// <summary>현재 재화를 텍스트에 표시</summary>
    private void Refresh()
    {
        _goldText.text = $"GOLD {_wallet.CurrentGold}";
    }
}
