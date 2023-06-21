using UnityEngine;

public class ObjectWhichIsDestroyedOnCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}