using TMPro;
using UnityEngine;

/// <summary>플레이어 HP를 텍스트로 표시</summary>
public class PlayerHpUI : MonoBehaviour
{
    #region 인스펙터
    [Header("데이터")]
    [SerializeField] private PlayerStatus _playerStatus;

    [Header("UI")]
    [SerializeField] private TMP_Text _hpText;
    #endregion

    /// <summary>인스펙터 연결 누락 확인</summary>
    private void Awake()
    {
        bool isValid = true;

        if (_playerStatus == null)
        {
            Debug.LogWarning("플레이어 스탯 없음 (PlayerHpUI)");
            isValid = false;
        }

        if (_hpText == null)
        {
            Debug.LogWarning("HP 텍스트 없음 (PlayerHpUI)");
            isValid = false;
        }

        if (!isValid)
        {
            enabled = false;
        }
    }

    /// <summary>HP 변경 알림 구독</summary>
    private void OnEnable()
    {
        _playerStatus.OnHpChanged += Refresh;
    }

    /// <summary>구독 해제</summary>
    private void OnDisable()
    {
        _playerStatus.OnHpChanged -= Refresh;
    }

    /// <summary>시작 HP를 한 번 그림</summary>
    private void Start()
    {
        Refresh();
    }

    /// <summary>현재 HP / 최대 HP를 텍스트에 표시</summary>
    private void Refresh()
    {
        _hpText.text = $"HP {_playerStatus.CurrentHp} / {_playerStatus.MaxHp}";
    }
}
