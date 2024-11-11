using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // �Ѿ� �ڷ�ƾ ����� ���� ����
    [SerializeField] BulletModel model;
    // ���ظ�� �� true�� �Ǵ� UI �̹���
    [SerializeField] Image scope;

    // ī�޶� ����
    [SerializeField] CinemachineVirtualCamera tpsCam;
    [SerializeField] CinemachineFreeLook freeLookCam;

    // ���� ��� Ȱ��ȭ ����
    [SerializeField] bool tpsMode;

    [SerializeField] float moveSpeed; // �̵��ӵ�
    [SerializeField] float Sensitivity; // ���콺 �ΰ���
    [SerializeField] float reloadTime; // �Ѿ� ������ �ð�

    // ȸ�� ����
    private float xAngle;
    private float yAngle;

    private void Start()
    {
        // ���콺 Ŀ�� ����
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // ���콺 ��Ŭ�� ��, ���� ��� Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Mouse1)) { ChangeTPSmode(true); }
        // ���콺 ��Ŭ�� ���� ��, ���� ��� ��Ȱ��ȭ
        else if (Input.GetKeyUp(KeyCode.Mouse1)) { ChangeTPSmode(false); }

        // ���ظ�� Ȱ��ȭ �� ������
        if (tpsMode) { TPSmove(); }
        // ���ظ�� ��Ȱ��ȭ ��, = ��� ������ ���� ������
        else { FreeLookMove(); }

        // ���콺 ��Ŭ�� ��, �Ѿ� �߻�
        if (Input.GetKeyDown(KeyCode.Mouse0)) { Fire(); }
        // R Ű �Է� ��, �Ѿ� ����
        if (Input.GetKeyDown(KeyCode.R)) { Reload(); }
    }

    // �Ϲ� ��忡�� ���� ���� ���� ���¸� �ٲ��ִ� �Լ��� �ۼ�
    void ChangeTPSmode(bool tpsMode)
    {
        this.tpsMode = tpsMode;

        // ���� ��尡 Ȱ��ȭ �Ǿ��� ��,
        if (tpsMode)
        {
            // ���� ��� ī�޶��� �켱���� ����
            tpsCam.Priority = 20;
            // ���� ��� ī�޶��� ��ġ�� ���� ī�޶��� ������ ����
            Vector3 lookDir = Camera.main.transform.forward;
            // ���� ��� ī�޶��� ����(y)�� 0���� ����
            lookDir.y = 0;

            // ���� ��� ī�޶��� �������� ������ ������ lookDir, �� ���� ī�޶��� ȸ�������� ����
            transform.rotation = Quaternion.LookRotation(lookDir);
            xAngle = transform.eulerAngles.x;
            yAngle = transform.eulerAngles.y;

            // ���� ��� �̹����� ��Ȱ��ȭ�� ��, �ڷ�ƾ�� ���� �ش� ��� �̹����� ����� �����.
            if (scopeRoutine != null) { StopCoroutine(scopeRoutine); }
            // ���� ��� �̹����� ���� ��� �ڷ�ƾ�� Ȱ��ȭ �Ǿ��� �� ���ȴ�.
            scopeRoutine = StartCoroutine(showScopeRoutine());
        }
        // �� ���� ��� (=���ظ�� ��Ȱ��ȭ)
        else
        {
            // ���� ����� �켱������ ����
            tpsCam.Priority = 5;
            // ���� ��� �̹����� ��Ȱ��ȭ�� ��, �ڷ�ƾ�� ���� �ش� ��� �̹����� ����� �����.
            if (scopeRoutine != null) { StopCoroutine(scopeRoutine); }
            // ���� ��� �̹����� �ڷ�ƾ�� ���۵Ǿ��� �� �̹����� �����.
            scopeRoutine = StartCoroutine(HideScopeRoutine());
        }
    }

    // �ڷ�ƾ �̺�Ʈ ����
    Coroutine scopeRoutine;

    // ���� ��� �̹����� �� �� �ְ� ���ִ� �ڷ�ƾ �̺�Ʈ �Լ� ����
    IEnumerator showScopeRoutine()
    {
        while (true)
        {
            yield return null;
            scope.fillAmount += 5 * Time.deltaTime;
            if (scope.fillAmount >= 1f) yield break;
        }
    }
    // ���� ��� �̹����� ���� �� �ְ� ���ִ� �ڷ�ƾ �̺�Ʈ �Լ� ����
    IEnumerator HideScopeRoutine()
    {
        while (true)
        {
            yield return null;
            scope.fillAmount -= 5 * Time.deltaTime;
            if (scope.fillAmount <= 0f) yield break;
        }
    }

    // ���� ��� ������ ����
    void TPSmove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // ī�޶��� ��ġ�� ����ī�޶��� ��ġ�� �����ϰ� ���ش�.
        Transform camTransform = Camera.main.transform;

        // �̵� ����
        Vector3 forwardDir = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;
        Vector3 rightDir = new Vector3(camTransform.right.x, 0, camTransform.right.z).normalized;
        Vector3 dir = forwardDir * z + rightDir * x;
        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);

        // ȸ�� ����
        yAngle += Input.GetAxis("Mouse X") * Sensitivity;
        xAngle -= Input.GetAxis("Mouse Y") * Sensitivity;
        transform.rotation = Quaternion.Euler(xAngle, yAngle, 0);
    }

    // �Ϲ� ��� ������ ����
    void FreeLookMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Transform camTransform = Camera.main.transform;

        Vector3 forwardDir = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;
        Vector3 rightDir = new Vector3(camTransform.right.x, 0, camTransform.right.z).normalized;
        Vector3 dir = forwardDir * z + rightDir * x;
        if (dir == Vector3.zero) return;

        // �̵�����
        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
        // ȸ������
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
    }

    // �Ѿ� �߻� �Լ� ����
    void Fire()
    {
        // �Ѿ��� ���� �� ��� ��� X
        if (model.CurBullet <= 0) return;
        // ���� ��尡 ��Ȱ��ȭ ������ �� ��� ��� X
        if (tpsMode == false) return;
        // �Ѿ��� �������ϰ� ���� �� ��� ��� X 
        if (reloadRoutine != null) return;

        // �Ѿ� ��� ��, ���� �Ѿ��� ������ �ϳ� �����Ѵ�.
        model.CurBullet--;
    }

    // ������ �ڷ�ƾ �̺�Ʈ �Լ� ����
    Coroutine reloadRoutine;
    void Reload()
    {
        // ���� �Ѿ��� ������ �ִ� �Ѿ��� ������ ���ٸ�, �Լ� ���� X
        if (model.CurBullet >= model.MaxBullet) return;
        // ������ �ڷ�ƾ �̺�Ʈ�� ��Ȱ��ȭ ���¶��, �Լ� ���� X
        if (reloadRoutine != null) return;
        // �������� �������� �����ϴ� �ڷ�ƾ �̺�Ʈ �Լ��� ����Ǿ��� �� ���ȴ�.
        reloadRoutine = StartCoroutine(ReloadRoutine());
    }

    // ������ �ڷ�ƾ �̺�Ʈ �Լ� �ۼ�
    IEnumerator ReloadRoutine()
    {
        // �Լ� ���� ��, ������ �ҿ� �ð� ��ŭ ��ٸ���.
        yield return new WaitForSeconds(reloadTime);
        // �Լ� ���� ��, ���� ���� �Ѿ��� ������ �ִ� ���� ���� �Ѿ˷� ���� �����Ѵ�.
        model.CurBullet = model.MaxBullet;
        // �Լ� ���� ��, �ڷ�ƾ �̺�Ʈ�� ��Ȱ��ȭ �Ѵ�.
        reloadRoutine = null;
    }
}
