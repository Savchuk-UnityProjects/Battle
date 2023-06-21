using UnityEngine;

public class Tank : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] protected float Speed = 5;

    [Header("Shooting")]
    [SerializeField] protected GameObject BulletPrefab;
    [SerializeField] protected GameObject StartPlaceOfBullet;
    [SerializeField] protected float StartSpeedOfBullet = 10;

    protected GameObject Shoot()
    {
        GameObject NewBullet = Instantiate(BulletPrefab);
        NewBullet.transform.position = StartPlaceOfBullet.transform.position;
        NewBullet.transform.rotation = transform.rotation;
        NewBullet.GetComponent<Rigidbody2D>().velocity = transform.rotation * (StartSpeedOfBullet * Vector2.up);
        return NewBullet;
    }
}