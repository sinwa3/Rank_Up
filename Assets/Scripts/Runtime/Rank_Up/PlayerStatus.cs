using System;
using UnityEngine;

/// <summary>플레이어 HP 보관, 바뀌면 알림</summary>
public class PlayerStatus : MonoBehaviour
{
    #region 인스펙터
    [Header("HP")]
    [SerializeField] private int _maxHp = 100;
    [SerializeField] private int _startHp = 50;
    #endregion

    #region 이벤트
    public event Action OnHpChanged;
    #endregion

    #region 내부 변수
    private int _currentHp;
    #endregion

    #region 프로퍼티
    public int CurrentHp => _currentHp;
    public int MaxHp => _maxHp;
    #endregion

    /// <summary>현재 HP를 시작 HP로 초기화</summary>
    private void Awake()
    {
        _maxHp = Mathf.Max(_maxHp, 1);
        _currentHp = Mathf.Clamp(_startHp, 1, _maxHp);
    }

    /// <summary>HP를 amount만큼 회복</summary>
    public void Heal(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        _currentHp = Mathf.Min(_currentHp + amount, _maxHp);
        OnHpChanged?.Invoke();
    }
}
