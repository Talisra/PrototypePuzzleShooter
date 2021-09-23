using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnWeaponStartFire;
    public event EventHandler<OnShootEventArgs> OnWeaponEndFire;
    public event EventHandler<OnShootEventArgs> OnWeaponContinuousFire;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 firePointPos;
        public Vector3 aimPointPos;
        public float angle;
    }

    public SpriteRenderer playerGraphics;

    public float moveSpeed = 20f;
    public GameObject aimPointObject;
    public WeaponForm currentForm;

    private Transform aimPointRadiusObject;
    private Transform aimPointActual;
    private float aimPointRadius = 1;
    private Transform firePoint;
    private float aimingAngle;
    private float lockedAimAngle;

    private CharacterController2D cc;
    private float moveDirection = 0;
    private int rawDirection = 1;
    private bool jump = false;

    void Start()
    {
        cc = GetComponent<CharacterController2D>();
        firePoint = transform.Find("FirePoint");
        //aimPointObject = Instantiate(Resources.Load("Prefabs/AimPoint", typeof(GameObject)),
        //    transform.position, Quaternion.identity) as GameObject;
        //aimPointObject.transform.SetParent(transform);
        aimPointRadiusObject = aimPointObject.transform.Find("AimFeedback");
        aimPointActual = aimPointRadiusObject.Find("ActualPoint");
        aimPointRadiusObject.localPosition = new Vector3(0, aimPointRadius, 0);
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

    public void LockAim()
    {
        lockedAimAngle = aimingAngle;
    }

    void FixedUpdate()
    {
        cc.Move(moveDirection * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
