using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _targetOffset;
    [SerializeField] private Transform _barrel;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private float _shootReactionDelay;
    [SerializeField] private float _shootingPeriod;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootingDistance;
    [SerializeField] private LayerMask _layerMask;

    private bool _isLookingAtTarget;
    private bool _isTagretCaptured = false;

    private void OnValidate()
    {
        _maxAngle = Mathf.Clamp(_maxAngle, 0, 180);
        _rotationSpeed = LimitWithZero(_rotationSpeed);
        _shootReactionDelay = LimitWithZero(_shootReactionDelay);
        _shootingPeriod = LimitWithZero(_shootingPeriod);
        _shootingDistance = LimitWithZero(_shootingDistance);
    }

    private void Update()
    {
        _barrel.rotation = Quaternion.RotateTowards(_barrel.rotation, Quaternion.LookRotation(Vector3.forward, _barrel.position - (_target.transform.position + _targetOffset)), _rotationSpeed * Time.deltaTime);
        _barrel.rotation = ClampRotation(_barrel.rotation);
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(_barrel.position, _muzzle.position - _barrel.position, _shootingDistance, _layerMask);
        _isLookingAtTarget = hit.collider != null && hit.collider.gameObject == _target;
        if (_isLookingAtTarget && _isTagretCaptured == false)
        {
            _isTagretCaptured = true;
            StartCoroutine(ShootWithDelay());
        }
    }

    private IEnumerator ShootWithDelay()
    {
        float delay = 0;
        while (delay < _shootReactionDelay && _isLookingAtTarget)
        {
            delay += Time.deltaTime;
            yield return null;
        }

        if (_isLookingAtTarget)
        {
            StartCoroutine(Shoot());
        }
        else
        {
            _isTagretCaptured = false;
        }
    }

    private IEnumerator Shoot()
    {
        while (_isLookingAtTarget)
        {
            Instantiate(_bulletPrefab, _muzzle.position, _muzzle.rotation);
            yield return new WaitForSeconds(_shootingPeriod);
        }

        _isTagretCaptured = false;
    }

    private Quaternion ClampRotation(Quaternion quaternion)
    {
        quaternion.z /= quaternion.w;
        quaternion.w = 1.0f;

        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(quaternion.z);
        angleZ = Mathf.Clamp(angleZ, -_maxAngle, _maxAngle);
        quaternion.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);

        return quaternion.normalized;
    }

    private float LimitWithZero(float value)
    {
        return value < 0 ? 0 : value;
    }
}
