using Fusion;
using UnityEngine.Events;

public class PlayerModel : NetworkBehaviour
{
    public UnityAction<int> OnChangedBulletEvent;
    public void OnChangedBullet() => OnChangedBulletEvent?.Invoke(curBullet);
    [Networked, OnChangedRender(nameof(OnChangedBullet))] public int curBullet { get; set; }
                                                          public int maxBullet;


    public UnityAction<int> OnChangedHpEvent;
    public void OnChangedHP() => OnChangedHpEvent?.Invoke(hp);
    [Networked, OnChangedRender(nameof(OnChangedHP))] public int hp { get; set; }
                                                      public int maxHp;

    // === === ===

    public float moveSpeed;
}
