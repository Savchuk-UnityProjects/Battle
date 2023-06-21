using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class BonusBlock : MonoBehaviour
{
    [SerializeField] protected Action OnGettingThisBonusByPlayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet BulletOfTheOtherObject = collision.gameObject.GetComponent<Bullet>();
        bool IsTheOtherObjectPlayerBullet = BulletOfTheOtherObject != null && BulletOfTheOtherObject.BulletType == BulletType.PlayerBullet;
        PlayerTank PlayerTankOfTheOtherObject = collision.gameObject.GetComponent<PlayerTank>();
        bool IsTheOtherObjectPlayerTank = PlayerTankOfTheOtherObject != null;
        if(IsTheOtherObjectPlayerBullet || IsTheOtherObjectPlayerTank)
        {
            OnGettingThisBonusByPlayer.Invoke();
        }
        Destroy(gameObject);
    }
}