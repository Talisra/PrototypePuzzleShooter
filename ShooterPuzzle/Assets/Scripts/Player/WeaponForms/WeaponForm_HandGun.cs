using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponForm_HandGun : WeaponForm
{
    public override void InitForm()
    {
        player.OnWeaponStartFire += ShootBullet;
    }

    public override void Reset()
    {
        
    }

    private void ShootBullet(object sender, Player.OnShootEventArgs e)
    {
        Bullet bullet = PrefabPooler.Instance.Get("Bullet_prototype", e.firePointPos, Quaternion.identity).GetComponent<Bullet>();
        bullet.SetDirection(e.aimPointPos);
    }
}
