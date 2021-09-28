using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public Enemy enemy;
    public Slider slider;

    private float currentHp;
    private float maxHp;

    public void AttatchToEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        transform.parent = enemy.transform;
        transform.position = new Vector3(transform.position.x, transform.position.y + enemy.sr.sprite.bounds.size.y/2, 0);
    }


    // Update is called once per frame
    void Update()
    {
        if (enemy)
        {
            currentHp = enemy.GetCurrentHp();
            maxHp = enemy.maxHP;
            slider.value = currentHp / maxHp;
        }

    }
}