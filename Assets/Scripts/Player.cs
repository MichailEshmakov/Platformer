using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(GroundChecker))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _fallingLevel;

    private Mover _mover;
    private GroundChecker _groundChecker;
    private bool _isDead = false;
    private Rigidbody2D _rigidbody;

    public UnityEvent OnJump;
    public UnityEvent OnDie;
    public UnityEvent OnFall;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _groundChecker = GetComponent<GroundChecker>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _mover.IsWaitJump == false && _groundChecker.IsOnGround && _isDead == false)
        {
            OnJump?.Invoke();
        }

        if (transform.position.y < _fallingLevel && _isDead == false)
        {
            _isDead = true;
            OnFall?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (_isDead == false)
        {
            float moving = Input.GetAxis("Horizontal");
            _mover.Move(moving);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy) || collision.gameObject.TryGetComponent(out Bullet bullet))
        {
            if (_isDead == false)
            {
                _isDead = true;
                _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                OnDie?.Invoke();
            }
        }
    }
}
