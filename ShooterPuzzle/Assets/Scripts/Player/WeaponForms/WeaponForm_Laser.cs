using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponForm_Laser : WeaponForm
{
    public LaserRay laser;
    protected override void Awake()
    {
        base.Awake();
        laser.gameObject.SetActive(false);
    }

    public override void InitForm()
    {
        player.OnWeaponContinuousFire += FireLaser;
        player.OnWeaponEndFire += StopLaser;
    }

    public override void Reset()
    {
        laser.gameObject.SetActive(false);
        slowAim = false;
    }

    private void FireLaser(object sender, Player.OnShootEventArgs e)
    {
        if (true)
        {
            laser.SetPositionAndDirection(e.firePointPos, (e.aimPointPos - e.firePointPos).normalized);
            if (!laser.gameObject.activeSelf)
            {
                laser.gameObject.SetActive(true);
            }
            slowAim = true;
        }
    }

    private void StopLaser(object sender, Player.OnShootEventArgs e)
    {
        slowAim = false;
        laser.gameObject.SetActive(false);
    }
}
