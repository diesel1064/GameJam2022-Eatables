using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour, IEatable
{
    public int eatableID;
    public float moveSpeed = 2.75f;
    public UnityEvent onDeadEvent;
    public UnityEvent onScaredEvent;
    public UnityEvent onReturnToDefaultEvent;

    [SerializeField] private GameObject bloodSplatter;
    [SerializeField] private GameObject bloodPool;

    private Animator _animator;

    private bool _isScared;
    private bool _isDead;
    private Transform _zombie;
    private Vector2 _moveDir;
    private Rigidbody2D _rigidbody2D;
    private static readonly int Velocity = Animator.StringToHash("Velocity");
    private static readonly int IsScared = Animator.StringToHash("isScared");
    private static readonly int EatableID = Animator.StringToHash("EatableID");

    public GameObject BloodSplatter { get => bloodSplatter; set => bloodSplatter = value; }
    public GameObject BloodPool { get => bloodPool; set => bloodPool = value; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _moveDir = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        _animator.SetInteger(EatableID, eatableID);
        StartCoroutine(SetNewDirection());
    }

    private void Update()
    {
        if (_isDead)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }
        
        if (_isScared)
        {
            HandleRunAway();
            return;
        }

        HandleMovement(_moveDir);
        _animator.SetFloat(Velocity, _rigidbody2D.velocity.magnitude);
        HandleZombieCheck();
    }

    private void HandleRunAway()
    {
        _moveDir = transform.position - _zombie.position;
        HandleMovement((_moveDir));
    }

    private void HandleMovement(Vector2 moveDir)
    {
        if (moveDir.x != 0f && moveDir.y != 0f)
            moveDir *= 0.7f;
        _rigidbody2D.velocity = moveDir * moveSpeed;
    }

    private void HandleZombieCheck()
    {
        var playerCol = Physics2D.OverlapCircle(transform.position, 2f, 1 << LayerMask.NameToLayer($"Player"));
        if (!playerCol || !playerCol.TryGetComponent(out PlayerController player)) return;
        if (!player.IsZombie) return;
        _zombie = player.transform;
        OnScaredEventTrigger();
    }

    private void OnDeadEventTrigger()
    {
        TriggerBloodSplatter();
        TriggerBloodPool();
        PlayerController.Instance.AddBodyCount();
        if (_isDead) return;
        onDeadEvent?.Invoke();
        _isDead = true;
    }

    private void OnScaredEventTrigger()
    {
        _isScared = true;
        _animator.SetBool(IsScared, true);
        onScaredEvent?.Invoke();
    }

    private void OnReturnToDefaultEventTrigger()
    {
        _animator.SetBool(IsScared, false);
        onReturnToDefaultEvent?.Invoke();
    }

    public void KillMe()
    {
        moveSpeed = 0;
        OnDeadEventTrigger();
    }

    public void TriggerBloodSplatter()
    {
        Instantiate(BloodSplatter, transform.position, Quaternion.identity);
    }

    public void TriggerBloodPool()
    {
        Instantiate(BloodPool, transform.position, Quaternion.identity);
    }

    private IEnumerator SetNewDirection()
    {
        while (!_isDead && !_isScared)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            _moveDir = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        }
    }
}
