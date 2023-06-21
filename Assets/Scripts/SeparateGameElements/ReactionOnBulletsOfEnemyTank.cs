public class ReactionOnBulletsOfEnemyTank : ObjectWhichReactsOnBullet
{
    private void Awake()
    {
        OnCollisionWithBullet += (bullet) =>
        {
            if(bullet.GetComponent<Bullet>().BulletType == BulletType.PlayerBullet)
            {
                EnemyTank ThisEnemyTank = GetComponent<EnemyTank>();
                if (ThisEnemyTank != null)
                {
                    ThisEnemyTank.OnCollisionWithBullet();
                }
            }
        };
    }
}