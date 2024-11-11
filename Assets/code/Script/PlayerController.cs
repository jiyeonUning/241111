using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // 총알 코루틴 사용을 위한 참조
    [SerializeField] BulletModel model;
    // 조준모드 시 true가 되는 UI 이미지
    [SerializeField] Image scope;

    // 카메라 참조
    [SerializeField] CinemachineVirtualCamera tpsCam;
    [SerializeField] CinemachineFreeLook freeLookCam;

    // 조준 모드 활성화 여부
    [SerializeField] bool tpsMode;

    [SerializeField] float moveSpeed; // 이동속도
    [SerializeField] float Sensitivity; // 마우스 민감도
    [SerializeField] float reloadTime; // 총알 재장전 시간

    // 회전 각도
    private float xAngle;
    private float yAngle;

    private void Start()
    {
        // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // 마우스 우클릭 시, 조준 모드 활성화
        if (Input.GetKeyDown(KeyCode.Mouse1)) { ChangeTPSmode(true); }
        // 마우스 우클릭 해제 시, 조준 모드 비활성화
        else if (Input.GetKeyUp(KeyCode.Mouse1)) { ChangeTPSmode(false); }

        // 조준모드 활성화 시 움직임
        if (tpsMode) { TPSmove(); }
        // 조준모드 비활성화 시, = 평소 상태일 때의 움직임
        else { FreeLookMove(); }

        // 마우스 좌클릭 시, 총알 발사
        if (Input.GetKeyDown(KeyCode.Mouse0)) { Fire(); }
        // R 키 입력 시, 총알 장전
        if (Input.GetKeyDown(KeyCode.R)) { Reload(); }
    }

    // 일반 모드에서 조준 모드로 현재 상태를 바꿔주는 함수를 작성
    void ChangeTPSmode(bool tpsMode)
    {
        this.tpsMode = tpsMode;

        // 조준 모드가 활성화 되었을 때,
        if (tpsMode)
        {
            // 조준 모드 카메라의 우선순위 설정
            tpsCam.Priority = 20;
            // 조준 모드 카메라의 위치를 메인 카메라의 앞으로 설정
            Vector3 lookDir = Camera.main.transform.forward;
            // 조준 모드 카메라의 높이(y)를 0으로 설정
            lookDir.y = 0;

            // 조준 모드 카메라의 각도값을 위에서 설정한 lookDir, 즉 메인 카메라의 회전값으로 설정
            transform.rotation = Quaternion.LookRotation(lookDir);
            xAngle = transform.eulerAngles.x;
            yAngle = transform.eulerAngles.y;

            // 조준 사격 이미지가 비활성화일 때, 코루틴을 통해 해당 사격 이미지의 사용을 멈춘다.
            if (scopeRoutine != null) { StopCoroutine(scopeRoutine); }
            // 조준 사격 이미지는 조준 모드 코루틴이 활성화 되었을 때 사용된다.
            scopeRoutine = StartCoroutine(showScopeRoutine());
        }
        // 그 외의 경우 (=조준모드 비활성화)
        else
        {
            // 조준 모드의 우선순위를 설정
            tpsCam.Priority = 5;
            // 조준 사격 이미지가 비활성화일 때, 코루틴을 통해 해당 사격 이미지의 사용을 멈춘다.
            if (scopeRoutine != null) { StopCoroutine(scopeRoutine); }
            // 조준 사격 이미지는 코루틴이 시작되었을 때 이미지를 숨긴다.
            scopeRoutine = StartCoroutine(HideScopeRoutine());
        }
    }

    // 코루틴 이벤트 생성
    Coroutine scopeRoutine;

    // 조준 사격 이미지를 볼 수 있게 해주는 코루틴 이벤트 함수 생성
    IEnumerator showScopeRoutine()
    {
        while (true)
        {
            yield return null;
            scope.fillAmount += 5 * Time.deltaTime;
            if (scope.fillAmount >= 1f) yield break;
        }
    }
    // 조준 사격 이미지를 숨길 수 있게 해주는 코루틴 이벤트 함수 생성
    IEnumerator HideScopeRoutine()
    {
        while (true)
        {
            yield return null;
            scope.fillAmount -= 5 * Time.deltaTime;
            if (scope.fillAmount <= 0f) yield break;
        }
    }

    // 조준 모드 움직임 구현
    void TPSmove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 카메라의 위치를 메인카메라의 위치와 동일하게 해준다.
        Transform camTransform = Camera.main.transform;

        // 이동 구현
        Vector3 forwardDir = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;
        Vector3 rightDir = new Vector3(camTransform.right.x, 0, camTransform.right.z).normalized;
        Vector3 dir = forwardDir * z + rightDir * x;
        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);

        // 회전 구현
        yAngle += Input.GetAxis("Mouse X") * Sensitivity;
        xAngle -= Input.GetAxis("Mouse Y") * Sensitivity;
        transform.rotation = Quaternion.Euler(xAngle, yAngle, 0);
    }

    // 일반 모드 움직임 구현
    void FreeLookMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Transform camTransform = Camera.main.transform;

        Vector3 forwardDir = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;
        Vector3 rightDir = new Vector3(camTransform.right.x, 0, camTransform.right.z).normalized;
        Vector3 dir = forwardDir * z + rightDir * x;
        if (dir == Vector3.zero) return;

        // 이동구현
        transform.Translate(dir.normalized * moveSpeed * Time.deltaTime, Space.World);
        // 회전구현
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
    }

    // 총알 발사 함수 구현
    void Fire()
    {
        // 총알이 없을 땐 기능 사용 X
        if (model.CurBullet <= 0) return;
        // 조준 모드가 비활성화 상태일 땐 기능 사용 X
        if (tpsMode == false) return;
        // 총알을 재장전하고 있을 땐 기능 사용 X 
        if (reloadRoutine != null) return;

        // 총알 사용 시, 현재 총알의 갯수를 하나 차감한다.
        model.CurBullet--;
    }

    // 재장전 코루틴 이벤트 함수 구현
    Coroutine reloadRoutine;
    void Reload()
    {
        // 현재 총알의 갯수가 최대 총알의 갯수와 같다면, 함수 실행 X
        if (model.CurBullet >= model.MaxBullet) return;
        // 재장전 코루틴 이벤트가 비활성화 상태라면, 함수 실행 X
        if (reloadRoutine != null) return;
        // 재장전은 재장전을 실행하는 코루틴 이벤트 함수가 실행되었을 때 사용된다.
        reloadRoutine = StartCoroutine(ReloadRoutine());
    }

    // 재장전 코루틴 이벤트 함수 작성
    IEnumerator ReloadRoutine()
    {
        // 함수 실행 시, 재장전 소요 시간 만큼 기다린다.
        yield return new WaitForSeconds(reloadTime);
        // 함수 실행 시, 현재 소지 총알의 갯수는 최대 소지 갯수 총알로 값을 변경한다.
        model.CurBullet = model.MaxBullet;
        // 함수 실행 후, 코루틴 이벤트를 비활성화 한다.
        reloadRoutine = null;
    }
}
