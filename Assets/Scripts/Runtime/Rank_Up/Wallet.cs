using System;
using UnityEngine;

/// <summary>플레이어 재화 보관</summary>
public class Wallet : MonoBehaviour
{
    #region 인스펙터
    [Header("재화")]
    [SerializeField] private int _startGold = 500;
    #endregion

    #region 이벤트
    public event Action OnGoldChanged;
    #endregion

    #region 내부 변수
    private int _currentGold;
    #endregion

    #region 프로퍼티
    public int CurrentGold => _currentGold;
    #endregion

    /// <summary>현재 재화를 시작 재화로 초기화</summary>
    private void Awake()
    {
        _currentGold = Mathf.Max(_startGold, 0);
    }

    /// <summary>amount만큼 차감 시도, 성공하면 true</summary>
    public bool TrySpend(int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (_currentGold < amount)
        {
            return false;
        }

        _currentGold -= amount;
        OnGoldChanged?.Invoke();

        return true;
    }
}
