using Fusion;
using UnityEngine;

public class Shot : NetworkBehaviour
{
    [SerializeField] Transform muzzlePoint;

    [SerializeField] int damage;
    [SerializeField] float range;

    public void Fire()
    {
        if (Runner.GetPhysicsScene().Raycast(muzzlePoint.position, muzzlePoint.forward, out RaycastHit info, range))
        {
            if (info.transform.tag != "player") return;

            PlayerController player = info.transform.GetComponent<PlayerController>();
            player.TakeHitRpc(damage);
        }
    }
}
