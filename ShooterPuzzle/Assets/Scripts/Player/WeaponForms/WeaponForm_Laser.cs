using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponForm_Laser : WeaponForm
{
    public LaserRay laser;
    private bool startedFire = false;
    public GameObject prefireParticlePrefab;
    private ParticleSystem prefireParticle;
    public float coolDown;
    private float cdCounter = 0;
    private bool canShoot = false;

    protected override void Awake()
    {
        base.Awake();
        prefireParticle = Instantiate(prefireParticlePrefab, Vector2.zero, Quaternion.identity).GetComponent<ParticleSystem>();
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
        prefireParticle.gameObject.SetActive(false);
        slowAim = false;
    }

    private void FireLaser(object sender, Player.OnShootEventArgs e)
    {
        prefireParticle.gameObject.SetActive(true);
        prefireParticle.transform.position = player.transform.position;
        if (canShoot)
        {
            if (!startedFire)
            {
                player.LockAim();
                startedFire = true;
            }
            laser.SetPositionAndDirection(e.firePointPos, (e.aimPointPos - e.firePointPos).normalized);
            if (!laser.gameObject.activeSelf)
            {
                laser.gameObject.SetActive(true);
            }
            slowAim = true;
        }else
        {
            cdCounter += Time.deltaTime;
            if (cdCounter >= coolDown)
            {
                cdCounter = 0;
                canShoot = true;
            }
        }
    }

    private void StopLaser(object sender, Player.OnShootEventArgs e)
    {
        prefireParticle.gameObject.SetActive(false);
        startedFire = false;
        slowAim = false;
        laser.gameObject.SetActive(false);
        canShoot = false;
    }

}
