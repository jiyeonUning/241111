using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletModel : MonoBehaviour
{
    // 현재 소지한 총알의 갯수
    [SerializeField] int curBullet;
    public int CurBullet
    {
        get { return curBullet; }
        // 현재 소지한 총알의 갯수 = 유니티 이벤트 함수를 실행해 총알 추가&차감을 관리
        set { curBullet = value; OnCurBulletChanged?.Invoke(curBullet); }
    }
    public UnityAction<int> OnCurBulletChanged;

    //=================================================================================

    // 최대 소지할 수 있는 총알의 갯수
    [SerializeField] int maxBullet;
    public int MaxBullet
    {
        get { return maxBullet; }
        set { maxBullet = value; OnMaxBulletChanged?.Invoke(maxBullet); }
    }
    public UnityAction<int> OnMaxBulletChanged;
}
