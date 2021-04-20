using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private MovePoint[] _movePoints;
    [SerializeField] private int _currentMovePointIndex = 0;

    private Mover _mover;
    private bool _isInverseDirection = false;

    private void OnValidate()
    {
        if (_currentMovePointIndex < 0 || _currentMovePointIndex >= _movePoints.Length)
        {
            _currentMovePointIndex = 0;
        }
    }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void FixedUpdate()
    {
        float _movingDirection = transform.position.x < _movePoints[_currentMovePointIndex].transform.position.x ? 1 : -1;
        _mover.Move(_movingDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out MovePoint movePoint) && _movePoints[_currentMovePointIndex] == movePoint)
        {
            SetNextMovePointIndex();
            JumpIfNeed(movePoint);
        }
    }

    private void SetNextMovePointIndex()
    {
        int indexShift = _isInverseDirection ? -1 : 1;
        _currentMovePointIndex += indexShift;
        if (_currentMovePointIndex >= _movePoints.Length || _currentMovePointIndex < 0)
        {
            _currentMovePointIndex -= indexShift * 2;
            _isInverseDirection = !_isInverseDirection;
        }
    }

    private void JumpIfNeed(MovePoint movePoint)
    {
        if (_isInverseDirection == false && movePoint.IsJump || _isInverseDirection && movePoint.IsInverseJump)
        {
            _mover.Jump();
        }
    }

}
