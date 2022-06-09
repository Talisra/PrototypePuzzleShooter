using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponForm_HandGun : WeaponForm
{
    public int totalAmmo;
    private int currentAmmo;

    private float ammoRegen = 0.55f;
    private float ammoRegenCounter = 0;

    public override void InitForm()
    {
        currentAmmo = totalAmmo;
        player.OnWeaponStartFire += ShootBullet;
    }

    public override void Reset()
    {
        
    }

    private void ShootBullet(object sender, Player.OnShootEventArgs e)
    {
        if (currentAmmo > 0)
        {
            AudioManager.Instance.Play("shoot_bullet");
            Bullet bullet = PrefabPooler.Instance.Get("Bullet_prototype", e.firePointPos, Quaternion.identity).GetComponent<Bullet>();
            bullet.SetDirection(e.aimPointPos);
            currentAmmo--;
        }

    }

    private void Update()
    {
        if (currentAmmo < totalAmmo)
        {
            ammoRegenCounter += Time.deltaTime;
            if (ammoRegenCounter >= ammoRegen)
            {
                currentAmmo++;
                ammoRegenCounter = 0;
            }
        }
    }
}
