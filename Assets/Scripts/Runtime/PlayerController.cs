using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    [Header("Movement")]
    public float playerSpeed;

    public float zombieSpeed;

    [Header("Game")]
    [SerializeField] private int bodyCount = 0;
    [SerializeField] private GameObject bloodSplatter;
    public bool spriteOverride;
    [SerializeField] private  TextMeshProUGUI gameOverText;
    
    [Space(10)]
    [Header("Zombie Stage")]
    [SerializeField] private bool isZombie;
    [SerializeField] private int currentZombieStage = 0;
    public List<ZombieStageSO> zombieStages = new List<ZombieStageSO>();
    private float _timePercentage;
    
    public UnityEvent onPauseGameEvent;

    public bool IsZombie { get => isZombie; set => isZombie = value; }
    public int BodyCount { get => bodyCount; set => bodyCount = value; }
    private bool IsGameStarting { get; set; }
    private Rigidbody2D _playerRb;
    private bool _gameOver;
    private readonly Collider2D[] _targetColliders = new Collider2D[1];
    private Animator _animator;
    private bool _isPaused;
    private bool _isGameOver;
    private static readonly int Velocity = Animator.StringToHash("Velocity");
    private SpriteRenderer _spriteRenderer;
    private static readonly int NextStage = Animator.StringToHash("NextStage");

    private void Start()
    {
        // Assign Rigidbody to player
        _playerRb = GetComponent<Rigidbody2D>();
        // _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        // Reset bodyCount
        bodyCount = 0;
        _isGameOver = false;
        IsGameStarting = false;
    }

    private void LateUpdate()
    {
        if (!spriteOverride)
            HandleUpdateSprite();
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _isPaused = !_isPaused;
            onPauseGameEvent?.Invoke();
            return;
        }

        if (IsGameStarting && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            StartTitle();
        }

        if (_isPaused || _isGameOver || IsGameStarting)
        {
            _playerRb.velocity = Vector2.zero;
            return;
        };
        
        _animator.SetFloat(Velocity, _playerRb.velocity.magnitude);
        // Handle player movement on each Update
        HandlePlayerMovement();
        // Handle eatable checks
        if (!IsZombie) return;
        HandleEatableCheck();
        HandleTimeRemaining();
    }

    private void HandleUpdateSprite()
    {
        // Debug.Log($"HandleUpdateSprite {zombieStages[currentZombieStage].sprite.name}");
        _spriteRenderer.sprite = zombieStages[currentZombieStage].sprite;
    }

    private void StartTitle()
    {
        UIManager.Instance.DisplayTitleUI();
        IsGameStarting = false;
    }

    public void StartGame() => StartCoroutine(nameof(StartingGame));
    public void TriggerNextGameStage() => _animator.SetTrigger(NextStage);

    private IEnumerator StartingGame()
    {
        yield return new WaitForSeconds(1);
        IsGameStarting = true;
    }

    private void HandleEatableCheck()
    {
        Physics2D.OverlapCircleNonAlloc(transform.position, 0.5f, _targetColliders);
        foreach (var target in _targetColliders)
        {
            if (target.gameObject.TryGetComponent(out IEatable eatable))
            {
                eatable.KillMe();
            }
        }
    }

    private void HandlePlayerMovement()
    {
        // Get player inputs
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");
        if (horizontalMovement != 0 && IsZombie)
            HandleSpriteFlip(horizontalMovement <= 0);
        // Create a direction vector based on player inputs
        var direction = new Vector2(horizontalMovement, verticalMovement);
        // Adjust distance for diagonal movement
        if (horizontalMovement != 0f && verticalMovement != 0f)
            direction *= 0.7f;
        // Add velocity to rigidbody to move character
        _playerRb.velocity = direction * playerSpeed;
    }

    public void AddBodyCount() => bodyCount++;

    public void HandleGameOver()
    {
        gameOverText.SetText($"You ate {bodyCount}");
        Instantiate(bloodSplatter, transform.position, Quaternion.identity);
        _isGameOver = true;
        _playerRb.velocity = Vector2.zero;
    }

    public void TogglePlayerFreeze(bool freeze)
    {
        _playerRb.velocity = Vector2.zero;
        _playerRb.drag = freeze ? 9999999 : 0;
        GetComponent<Collider2D>().enabled = !freeze;
        if (freeze)
        {
            _playerRb.Sleep();
        }
        else
        {
            _playerRb.WakeUp();
        }
    }
    
    private void HandleTimeRemaining()
    {
        _timePercentage = CountdownTimer.GetTimeRemaining() / CountdownTimer.GetStartingTime();
        if (_timePercentage < 0.1f) return;
        if (_timePercentage < zombieStages[currentZombieStage].triggerValue)
        {
            HandleZombieStageChange();
        }
    }

    private void HandleZombieStageChange()
    {
        // Debug.Log($"HandleZombieStageChange currentStage: {currentZombieStage} new stage: {currentZombieStage + 1}");
        currentZombieStage++;
        // if (currentZombieStage > zombieStages.Count - 1) return;
        // _spriteRenderer.sprite = zombieStages[currentZombieStage].sprite;
    }

    private void HandleSpriteFlip(bool flip)
    {
        _spriteRenderer.flipX = flip;
    }

    public void TriggerZombieMode()
    {
        IsZombie = true;
    }
}
