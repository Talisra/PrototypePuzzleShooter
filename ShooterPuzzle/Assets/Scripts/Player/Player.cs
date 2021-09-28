using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // events
    public event EventHandler<OnShootEventArgs> OnWeaponStartFire;
    public event EventHandler<OnShootEventArgs> OnWeaponEndFire;
    public event EventHandler<OnShootEventArgs> OnWeaponContinuousFire;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 firePointPos;
        public Vector3 aimPointPos;
        public float angle;
    }

    // graphics and colliders
    public SpriteRenderer playerGraphics;
    private CharacterController2D cc;
    private BoxCollider2D feet;


    // movement
    public float moveSpeed = 20f;
    public GameObject aimPointObject;
    public WeaponForm currentForm;
    private float moveDirection = 0;
    private int rawDirection = 1;
    private bool jump = false;

    // aim
    private Transform aimPointRadiusObject;
    private Transform aimPointActual;
    private float aimPointRadius = 1;
    private Transform firePoint;
    private float aimingAngle;
    private float lockedAimAngle;

    // hp and damage
    public int maxLives;
    private int currentLives;

    private bool isInvulnerable = false;
    private float invulnerableAfterDamageTime = 1;
    private float invulenrableDelay;
    private float invulnerableCounter = 0;

    public int GetCurrentLives()
    {
        return currentLives;
    }
    void Awake()
    {
        feet = GetComponent<BoxCollider2D>();
        cc = GetComponent<CharacterController2D>();
        firePoint = transform.Find("FirePoint");
        aimPointRadiusObject = aimPointObject.transform.Find("AimFeedback");
        aimPointActual = aimPointRadiusObject.Find("ActualPoint");
        aimPointRadiusObject.localPosition = new Vector3(0, aimPointRadius, 0);
        currentLives = maxLives;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Steel"))
        {
            transform.parent = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Steel"))
        {
            transform.parent = null;
        }
    }

    // Change the current weapon form to a new one, clear old events.
    public void ChangeForm(WeaponForm newForm)
    {
        if (currentForm) // reset the previous form if its not the first form
            currentForm.Reset();
        currentForm = newForm;
        OnWeaponStartFire = null;
        OnWeaponEndFire = null;
        OnWeaponContinuousFire = null;
        newForm.InitForm();
        playerGraphics.sprite = currentForm.formSprite;
    }

    private void Update()
    {
        HandleMovement();
        HandleAiming();
        HandleShooting();
        HandleInvulnerability();
    }

    void HandleMovement()
    {
        moveDirection = Input.GetAxisRaw("Horizontal") * moveSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        rawDirection = (int)((moveDirection != 0) ? Mathf.Sign(moveDirection) : rawDirection);
    }

    void HandleAiming()
    {
        // get aim angle by mouse position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(
           new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));
        Vector3 aimDirection = (mousePosition - firePoint.position).normalized;
        aimingAngle = Mathf.Atan2(-aimDirection.x, aimDirection.y) * Mathf.Rad2Deg;

        if (currentForm.capAim) // cap aiming if needed
        {
            if (aimingAngle > 0)
            {
                aimingAngle = Mathf.Clamp(aimingAngle, currentForm.minAngle, currentForm.maxAngle);
            }
            else
            {
                aimingAngle = Mathf.Clamp(aimingAngle, -currentForm.maxAngle, -currentForm.minAngle);
            }
        }
        if (currentForm.slowAim) // slow aim
        {
            float angleDistance = aimingAngle - lockedAimAngle;
            if (Mathf.Abs(angleDistance) > 0.1f)
            {
                float targetAngle = Mathf.MoveTowardsAngle(lockedAimAngle, aimingAngle, currentForm.slowAimSpeed); ;
                aimPointObject.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                lockedAimAngle = targetAngle;
            }
            else // adjust aim when close to target
            {
                aimPointObject.transform.rotation = Quaternion.Euler(0, 0, aimingAngle);
            }
        }
        else // fast aim
        {
            aimPointObject.transform.rotation = Quaternion.Euler(0, 0, aimingAngle);
        }


    }
    void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            OnWeaponStartFire?.Invoke(this, new OnShootEventArgs
            {
                firePointPos = firePoint.position,
                aimPointPos = aimPointActual.position,
                angle = aimingAngle
            });
        }
        if (Input.GetButtonUp("Fire1"))
        {
            OnWeaponEndFire?.Invoke(this, new OnShootEventArgs
            {
                firePointPos = firePoint.position,
                aimPointPos = aimPointActual.position,
                angle = aimingAngle
            });
        }
        if (Input.GetButton("Fire1"))
        {
            OnWeaponContinuousFire?.Invoke(this, new OnShootEventArgs
            {
                firePointPos = firePoint.position,
                aimPointPos = aimPointActual.position,
                angle = aimingAngle
            });
        }
    }

    private void HandleInvulnerability()
    {
        invulnerableCounter += Time.deltaTime;
        if (invulnerableCounter >= invulenrableDelay)
        {
            // End invulnerability
            invulnerableCounter = 0;
            isInvulnerable = false;
            Color temp = playerGraphics.color;
            playerGraphics.color = new Color(temp.r, temp.g, temp.b, 1f);
        }
    }

    public void LockAim()
    {
        lockedAimAngle = aimingAngle;
    }

    public void TakeDamage()
    {
        if (!isInvulnerable)
        {
            currentLives--;
            GainInvulnerability(invulnerableAfterDamageTime);
            if (currentLives <= 0)
            {
                Debug.Log("Game Over!");
            }
        }
    }

    public void GainInvulnerability(float amount)
    {
        isInvulnerable = true;
        invulenrableDelay = amount;
        invulnerableCounter = 0;
        Color temp = playerGraphics.color;
        playerGraphics.color = new Color(temp.r, temp.g, temp.b, 0.5f);
    }

    void FixedUpdate()
    {
        cc.Move(moveDirection * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
