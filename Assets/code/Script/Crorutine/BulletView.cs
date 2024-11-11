using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    // 총알 모델 참조
    [SerializeField] BulletModel model;
    // 현재 소지 총알 갯수 UI 표시
    [SerializeField] TextMeshProUGUI curBulletText;
    // 최대 소지 총알 갯수 UI 표시
    [SerializeField] TextMeshProUGUI maxBulletText;

    private void OnEnable()
    {
        // 현재 총알 소지 갯수에 변동이 처음 있었을 때, 총알을 추가하는 함수를 가져와 사용한다.
        model.OnCurBulletChanged += UpdateCurBullet;
        model.OnMaxBulletChanged += UpdateMaxBullet;
    }

    private void OnDisable()
    {
        // 현재 총알 소지 갯수에 생긴 변동이 끝났을 때, 총알을 차감하는 함수를 가져와 사용한다.
        model.OnCurBulletChanged -= UpdateCurBullet;
        model.OnMaxBulletChanged -= UpdateMaxBullet;
    }

    private void Start()
    {
        // 현재 총알 소지 갯수를 UI에 표시
        UpdateCurBullet(model.CurBullet);
        // 최대 총알 소지 갯수를 UI에 표시
        UpdateMaxBullet(model.MaxBullet);
    }

    public void UpdateCurBullet(int curBullet) { curBulletText.text = $"{curBullet}"; }
    public void UpdateMaxBullet(int maxBullet) { maxBulletText.text = $"{maxBullet}"; }
}
