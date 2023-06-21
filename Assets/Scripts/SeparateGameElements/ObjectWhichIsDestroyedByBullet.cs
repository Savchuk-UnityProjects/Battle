public class ObjectWhichIsDestroyedByBullet : ObjectWhichReactsOnBullet
{
    private void Awake()
    {
        OnCollisionWithBullet += (bullet) => Destroy(gameObject);
    }
}