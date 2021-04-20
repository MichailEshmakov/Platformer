using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(GroundChecker))]
public class AnimationSetter : MonoBehaviour
{
    private Mover _mover;
    private GroundChecker _groundChecker;
    private Animator _animator;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _groundChecker = GetComponent<GroundChecker>();
        _animator = GetComponentInChildren<Animator>();

        if (_animator == null)
        {
            Debug.LogError($"Animator of {gameObject.name} not found");
        }
    }

    private void Update()
    {
        _animator.SetBool("isRunning", _mover.Speed != 0);
        _animator.SetBool("isOnGround", _groundChecker.IsOnGround);
        if (_mover.Speed < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            if (_mover.Speed > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void Jump()
    {
        _animator.SetBool("isJump", true);
        _animator.SetBool("isLand", false);
    }

    public void Land()
    {
        _animator.SetBool("isJump", false);
        _animator.SetBool("isLand", true);
    }

    public void Die()
    {
        _animator.SetTrigger("die");
    }
}
