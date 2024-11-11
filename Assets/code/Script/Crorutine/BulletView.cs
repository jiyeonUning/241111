using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    // �Ѿ� �� ����
    [SerializeField] BulletModel model;
    // ���� ���� �Ѿ� ���� UI ǥ��
    [SerializeField] TextMeshProUGUI curBulletText;
    // �ִ� ���� �Ѿ� ���� UI ǥ��
    [SerializeField] TextMeshProUGUI maxBulletText;

    private void OnEnable()
    {
        // ���� �Ѿ� ���� ������ ������ ó�� �־��� ��, �Ѿ��� �߰��ϴ� �Լ��� ������ ����Ѵ�.
        model.OnCurBulletChanged += UpdateCurBullet;
        model.OnMaxBulletChanged += UpdateMaxBullet;
    }

    private void OnDisable()
    {
        // ���� �Ѿ� ���� ������ ���� ������ ������ ��, �Ѿ��� �����ϴ� �Լ��� ������ ����Ѵ�.
        model.OnCurBulletChanged -= UpdateCurBullet;
        model.OnMaxBulletChanged -= UpdateMaxBullet;
    }

    private void Start()
    {
        // ���� �Ѿ� ���� ������ UI�� ǥ��
        UpdateCurBullet(model.CurBullet);
        // �ִ� �Ѿ� ���� ������ UI�� ǥ��
        UpdateMaxBullet(model.MaxBullet);
    }

    public void UpdateCurBullet(int curBullet) { curBulletText.text = $"{curBullet}"; }
    public void UpdateMaxBullet(int maxBullet) { maxBulletText.text = $"{maxBullet}"; }
}
