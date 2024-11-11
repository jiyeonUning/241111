using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] PlayerModel bulletModel;

    [SerializeField] TextMeshProUGUI curBulletText;
    [SerializeField] TextMeshProUGUI maxBulletText;
    [SerializeField] TextMeshProUGUI hpText;

    private void OnEnable()
    {
        bulletModel.OnChangedHpEvent += ChangedHP;
        bulletModel.OnChangedBulletEvent += ChangedCurBullet;
    }

    private void OnDisable()
    {
        bulletModel.OnChangedHpEvent -= ChangedHP;
        bulletModel.OnChangedBulletEvent -= ChangedCurBullet;
    }

    private void Start()
    {
        hpText.text = bulletModel.hp.ToString();
        curBulletText.text = bulletModel.curBullet.ToString();
        maxBulletText.text = bulletModel.maxBullet.ToString();
    }


    public void ChangedHP(int hp) { hpText.text = hp.ToString(); }
    public void ChangedCurBullet(int curBullet) { curBulletText.text = curBullet.ToString(); }
}
