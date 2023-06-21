using System;
using UnityEngine;

public class ObjectWhichReactsOnBullet : MonoBehaviour
{
    protected Action<GameObject> OnCollisionWithBullet = null;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == Layers.BulletLayer)
        {
            OnCollisionWithBullet?.Invoke(collision.collider.gameObject);
        }
    }
}