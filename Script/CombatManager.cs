using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CombatManager : MonoBehaviour
{
    public PlayerData playerData;

    [Header("Aim")]
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float normalSensitivity = 1f;
    [SerializeField] private float aimSensitivity = 0.5f;
    [SerializeField] private LayerMask aimColliderLayerMask;
    bool isAiming;


    [Header("Arrow")]
    public GameObject arrowPrefab;
    public float arrowSpeed;
    public int arrowCount;
    [Header("Dodge")]
    private CharacterController controller;
    private bool isDodging;
    private float dodgeTimer;
    public float dodgeDistance = 10f;
    public float dodgeCooldown = 2f;

    private Vector3 moveDirection;

    [Header("Attack")]
    private Animator anim;
    public bool isAttacking;
    public float detectionRadius = 5f;
    public LayerMask enemyLayerMask;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float approachDistance = 1f;
    bool isBlocking;
    public float parryTime;

    private PlayerInput playerInput;

    public bool stealthMode = false;

    private InputAction aimAction;
    private InputAction attack1Action;
    private InputAction attack2Action;
    private InputAction parryAction;

    void Start()
    {
        isAttacking = false;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        playerInput = GetComponent<PlayerInput>();

        aimAction = playerInput.actions["Aim"];
        attack1Action = playerInput.actions["Attack1"];
        attack2Action = playerInput.actions["Attack2"];
        parryAction = playerInput.actions["Parry"];
    }

    void Update()
    {
        HandleAiming();
        HandleAttacks();
        if (parryAction.triggered)
        {
            Attack1();
        }
    }

    IEnumerator Parry()
    {
        isBlocking = true;
        yield return new WaitForSeconds(parryTime);
        isBlocking = false;
    }

    private void HandleAiming()
    {
        isAiming = aimAction.IsPressed();
        anim.SetBool("Aiming", isAiming);

        if (isAiming)
        {
            virtualCamera.gameObject.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(true);
            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                mouseWorldPosition = raycastHit.point;
            }

            Vector3 aimTarget = mouseWorldPosition;
            aimTarget.y = transform.position.y;
            Vector3 aimDirection = (aimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            virtualCamera.gameObject.SetActive(true);
        }
    }

    private void HandleAttacks()
    {
        if (attack1Action.triggered && !isAiming && !stealthMode)
        {
            Attack1();
        }
        else if (attack1Action.triggered && stealthMode)
        {
            StealthTakedown();
        }

        if (attack2Action.triggered && !isAttacking)
        {
            Attack2();
        }
    }

    private void Attack1()
    {
        isAttacking = true;
        anim.SetTrigger("Attack1");
    }

    private void Attack2()
    {
        isAttacking = true;
        anim.SetTrigger("Attack2");
    }

    private void StealthTakedown()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayerMask);

        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = collider.transform;
            }
        }

        if (nearestEnemy != null)
        {
            Target target = nearestEnemy.GetComponent<Target>();
            if (target != null)
            {
                target.Takedown();
            }
        }
    }

    public void ResetAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(float amount)
    {
        if(!isBlocking) 
        {
            anim.Play("Impact");
            health -= amount;
        }
    }

    

    
    public void ShootArrow()
    {
        if (isAiming && attack1Action.triggered && arrowCount != 0)
        {
            arrow.Shoot(arrowSpeed);
            GameObject arrow = Instantiate(arrowPrefab, transform.position,  transform.rotation);
            arrow.GetComponent<Rigidbody>().AddRelativeForce(new Vector3 (0, arrowSpeed, 0));
            arrowCount--;
        }
    }

    public bool IsInCombat()
    {
        return isAttacking || isDodging || isAiming || isBlocking;
    }
}
