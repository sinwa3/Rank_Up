using System;
using UnityEngine;

/// <summary>상점 범위(Trigger), 플레이어가 안에 있는지 알려줌</summary>
public class ShopZone : MonoBehaviour
{
    #region 인스펙터
    [Header("플레이어")]
    [SerializeField] private string _playerTag = "Player";
    #endregion

    #region 이벤트
    public event Action OnRangeChanged;
    #endregion

    #region 내부 변수
    private bool _isPlayerInRange = false;
    #endregion

    #region 프로퍼티
    public bool IsPlayerInRange => _isPlayerInRange;
    #endregion

    /// <summary>플레이어가 들어오면 범위 안으로 표시</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(_playerTag) && other.CompareTag(_playerTag))
        {
            SetPlayerInRange(true);
        }
    }

    /// <summary>플레이어가 나가면 범위 밖으로 표시</summary>
    private void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(_playerTag) && other.CompareTag(_playerTag))
        {
            SetPlayerInRange(false);
        }
    }

    /// <summary>범위 상태를 바꾸고 알림</summary>
    private void SetPlayerInRange(bool isInRange)
    {
        if (_isPlayerInRange == isInRange)
        {
            return;
        }

        _isPlayerInRange = isInRange;
        OnRangeChanged?.Invoke();
    }
}
