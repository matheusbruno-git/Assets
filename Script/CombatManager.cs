using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CombatManager : MonoBehaviour
{
    [Header("Aim")]
    [SerializeField] private CinemachineFreeLook aimVirtualCamera;
    [SerializeField] private CinemachineFreeLook virtualCamera;
    [SerializeField] private float normalSensitivity = 1f;
    [SerializeField] private float aimSensitivity = 0.5f;
    [SerializeField] private LayerMask aimColliderLayerMask;
    bool isAiming;
    public PlayerData playerData;

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
    public float damage;
    public float weaponMultiplier;

    private PlayerInput playerInput;
    public bool stealthMode = false;

    private InputAction aimAction;
    private InputAction attack1Action;
    private InputAction attack2Action;
    private InputAction parryAction;

    // Combo system
    private int comboStep = 0;
    private float lastAttackTime;
    public float comboResetTime = 1f;

    void Start()
    {
        isAttacking = false;
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        damage = playerData.Strength * weaponMultiplier;

        aimAction = playerInput.actions["Aim"];
        attack1Action = playerInput.actions["Attack1"];
        attack2Action = playerInput.actions["Attack2"];
        parryAction = playerInput.actions["Parry"];
    }

    void Update()
    {
        HandleAiming();
        HandleComboAttack();

        if (parryAction.triggered)
        {
            StartCoroutine(Parry());
        }
    }

    private IEnumerator Parry()
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

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
            {
                Vector3 aimTarget = hit.point;
                aimTarget.y = transform.position.y;
                Vector3 aimDirection = (aimTarget - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            }
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            virtualCamera.gameObject.SetActive(true);
        }
    }

    private void HandleComboAttack()
    {
        if (isAiming || stealthMode || isAttacking) return;

        if (attack1Action.triggered && Time.time - lastAttackTime > 0.2f)
        {
            lastAttackTime = Time.time;
            comboStep++;
            if (comboStep > 3) comboStep = 1;

            Transform target = GetNearestEnemy();
            if (target != null)
            {
                FaceTarget(target);
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance > approachDistance)
                {
                    Vector3 dir = (target.position - transform.position).normalized;
                    controller.Move(dir * moveSpeed * Time.deltaTime);
                }
            }

            anim.SetTrigger("Attack" + comboStep);
            isAttacking = true;
        }

        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }
    }

    private Transform GetNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayerMask);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
        if (!isBlocking)
        {
            anim.Play("Impact");
            playerData.Health -= amount;
        }
    }

    public void ShootArrow()
    {
        if (isAiming && attack1Action.triggered && arrowCount > 0)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
            arrow.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * arrowSpeed);
            arrowCount--;
        }
    }

    public bool IsInCombat()
    {
        return isAttacking || isDodging || isAiming || isBlocking;
    }
}
