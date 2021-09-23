using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponForm_Cannon : WeaponForm
{
    public BombAimAssist aimAssist;
    private float bombMass = 5f; // manualy change if the bomb mass is changed
    private float bombGravityScale = 2f;

    private float power;
    public float minPower = 500; //500
    public float maxPower = 4000; // 4000
    public float maxChargeTime = 1;
    private float chargeMultiplier;
    private float unknownMultiplier = 1.41f; //No explanation why but this multiplier works with aim assist the best

    public float coolDown;
    private float cdCounter = 0;
    private bool canShoot = true;
    private bool isCharging = false;
    protected override void Awake()
    {
        base.Awake();
        GameObject aimAssistObj = Instantiate(Resources.Load("Prefabs/BombAimDisplay", typeof(GameObject)),
            transform.position, Quaternion.identity) as GameObject;
        
        aimAssist = aimAssistObj.GetComponent<BombAimAssist>();
        aimAssist.Init(bombGravityScale);
        aimAssist.gameObject.SetActive(false);
        chargeMultiplier = (maxPower - minPower) / maxChargeTime;
    }

    public override void InitForm()
    {
        player.OnWeaponContinuousFire += ChargeBomb;
        player.OnWeaponEndFire += FireBomb;
        power = minPower;
    }

    public override void Reset()
    {
        base.Reset();
        power = minPower;
        aimAssist.gameObject.SetActive(false);
    }

    private void ChargeBomb(object sender, Player.OnShootEventArgs e)
    {
        if (canShoot)
        {
            isCharging = true;
            if (!aimAssist.gameObject.activeSelf)
                aimAssist.gameObject.SetActive(true);
            aimAssist.RefreshParameters(e.firePointPos, -e.angle, (power / bombMass) * unknownMultiplier);
            power += Time.deltaTime * chargeMultiplier;
            if (power >= maxPower)
                power = maxPower;
        }
    }

    private void FireBomb(object sender, Player.OnShootEventArgs e)
    {
        if (canShoot && isCharging)
        {
            aimAssist.gameObject.SetActive(false);
            Bomb bomb = PrefabPooler.Instance.Get("Bomb_prototype", e.firePointPos, Quaternion.identity).GetComponent<Bomb>();
            bomb.SetDirectionAndPower(e.aimPointPos, power);
            power = minPower;
            canShoot = false;
            isCharging = false;
        }
    }
    private void Update()
    {
        if (!canShoot)
        {
            cdCounter += Time.deltaTime;
            if (cdCounter >= coolDown)
            {
                cdCounter = 0;
                canShoot = true;
            }
        }
    }
}
