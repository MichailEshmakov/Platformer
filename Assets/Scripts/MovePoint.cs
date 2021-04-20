using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    [SerializeField] private bool _isJump;
    [SerializeField] private bool _isInverseJump;

    public bool IsJump => _isJump;
    public bool IsInverseJump => _isInverseJump;
}
