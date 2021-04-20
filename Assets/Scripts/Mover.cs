using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundChecker))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _mayMoveNotOnGround;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpDelay;

    private Rigidbody2D _rigidbody2D;
    private GroundChecker _groundChecker;
    private float _jumpImpulse;

    public float Speed { get; private set; }
    public bool IsWaitJump { get; private set; }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponent<GroundChecker>();
        _jumpImpulse = Mathf.Sqrt(2 * (-Physics2D.gravity.y) * _jumpHeight) * _rigidbody2D.mass;
    }

    public void Move(float intensivity)
    {
        if (_mayMoveNotOnGround || (_mayMoveNotOnGround == false && _groundChecker.IsOnGround))
        {
            Speed = _speed * intensivity;
            _rigidbody2D.velocity = new Vector2(Speed, _rigidbody2D.velocity.y);
        }
    }

    public void Stop()
    {
        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
    }

    public void Jump()
    {
        StartCoroutine(JumpWithDelay());
    }

    public IEnumerator JumpWithDelay()
    {
        IsWaitJump = true;
        if (_jumpDelay > Time.fixedDeltaTime)
        {
            yield return new WaitForSeconds(_jumpDelay);
        }
        else
        {
            yield return new WaitForFixedUpdate();
        }

        if (_groundChecker.IsOnGround)
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpImpulse, ForceMode2D.Impulse);
        }

        IsWaitJump = false;
    }
}
