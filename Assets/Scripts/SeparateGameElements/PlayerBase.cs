public class PlayerBase : ObjectWhichReactsOnBullet
{
    private Defeat Defeat;

    private void Awake()
    {
        Defeat = FindObjectOfType<Defeat>();
        OnCollisionWithBullet += (bullet) =>
        {
            Defeat.BeDefeated();
        };
    }
}