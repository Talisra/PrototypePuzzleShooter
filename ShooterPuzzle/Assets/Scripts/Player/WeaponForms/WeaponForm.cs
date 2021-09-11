using UnityEngine;

public class WeaponForm : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]protected Player player;
    public Sprite formSprite;

    public bool capAim; // use if the weapon is capped in aiming
    public float minAngle;
    public float maxAngle;

    [HideInInspector]
    public bool slowAim;
    public float slowAimSpeed;

    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public virtual void Reset() // resets the form, used when changing forms in the player
    // so the form change will be smoother
    {

    }

    public virtual void InitForm() // Change the player's form. 
    //In inheritage: add the correct event of the weapon's shoot
    {

    }
}
