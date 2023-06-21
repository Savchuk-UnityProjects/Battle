using UnityEngine;

public enum BulletType
{
    PlayerBullet,
    EnemyBullet
}

public class Bullet : MonoBehaviour
{
    public BulletType BulletType;
}