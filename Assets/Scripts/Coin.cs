using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class Coin : MonoBehaviour
{
    [SerializeField] private float _respawnPeriod;
    [SerializeField] private float _jumpPeriod;
    [SerializeField] private float _jumpHeight;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        transform.DOMoveY(_jumpHeight, _jumpPeriod).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetRelative(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player) && _spriteRenderer.enabled)
        {
            _spriteRenderer.enabled = false;
            StartCoroutine(Respawn());
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawnPeriod);
        _spriteRenderer.enabled = true;
    }
}
