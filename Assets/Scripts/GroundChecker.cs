using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float _bottomDepth;

    private List<Collider2D> _groundColliders = new List<Collider2D>();

    public bool IsOnGround => _groundColliders.Count > 0;
    public UnityEvent OnLanding;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_groundColliders.Contains(collision.collider) == false)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.point.y < transform.position.y - _bottomDepth)
                {
                    bool isOnLanding = false;
                    if (_groundColliders.Count == 0)
                    {
                        isOnLanding = true;
                    }

                    _groundColliders.Add(collision.collider);
                    if (isOnLanding)
                    {
                        OnLanding?.Invoke();
                    }

                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_groundColliders.Contains(collision.collider))
        {
            _groundColliders.Remove(collision.collider);
        }
    }
}
