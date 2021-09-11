
using UnityEngine;

public class bullet_test : MonoBehaviour
{
    [SerializeField] protected Player player;

    private void Start()
    {
        player.OnWeaponStartFire += ShootProjectile;
    }

    private void ShootProjectile(object sender, Player.OnShootEventArgs e) {
        Bullet bullet = PrefabPooler.Instance.Get("Bullet_prototype", e.firePointPos, Quaternion.identity).GetComponent<Bullet>();
        bullet.SetDirection(e.aimPointPos);
    }
}
