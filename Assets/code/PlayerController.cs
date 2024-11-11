using Cinemachine;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] Image Scope;
    [SerializeField] PlayerModel model;
    [SerializeField] Shot gun;

    private CinemachineVirtualCamera followCam;
    private Vector3 PlayerDir;
    private float curMoveSpeed;

    // === === === 


    public override void Spawned()
    {
        if (HasStateAuthority == true)
        {
            followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.Follow = transform;
        }
        Scope.fillAmount = 0;
        model.hp = model.maxHp;
        curMoveSpeed = model.moveSpeed;
        model.curBullet = model.maxBullet;
    }

    private void Update()
    {
        PlayerDir.x = Input.GetAxisRaw("Horizontal");
        PlayerDir.z = Input.GetAxisRaw("Vertical");

        curMoveSpeed = model.moveSpeed / 2;

        // �޸��� ����Ʈ
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (HasStateAuthority == false) return;
            curMoveSpeed = model.moveSpeed;
        }

        // ���� ���
        if (Input.GetMouseButton(1))
        {
            if (HasStateAuthority == false) return;
            Debug.Log("���� ��� Ȱ��ȭ");

            // �ó׸ӽ� ī�޶� Ȯ�� �Ǵ� �ڵ� ���� ����
            Scope.fillAmount = 1;
        }
        else { Scope.fillAmount = 0; }

        // ���� ���
        if (Input.GetMouseButtonDown(0))
        {
            if (HasStateAuthority == false) return;

            if (model.curBullet > 0)
            {
                gun.Fire();
                model.curBullet--;
            }
            else return;
        }

        // ������ ���
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (HasStateAuthority == false) return;

            Debug.Log("������ ����");
            model.curBullet = model.maxBullet;
        }
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void TakeHitRpc(int damage)
    {
        model.hp -= damage;
    }


    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false) return;
        if (PlayerDir == Vector3.zero) return;

        transform.forward = followCam.transform.forward;
        PlayerDir = new Vector3(PlayerDir.x, 0, PlayerDir.z).normalized;
        transform.Translate(PlayerDir * curMoveSpeed * Runner.DeltaTime);
    }
}
