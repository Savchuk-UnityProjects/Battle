using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Tank
{
    [Header("MovingRandomly")]
    [SerializeField] private float IntervalInSecondsForChangingDirection = 0.3f;
    [SerializeField] private float IntervalInSecondsForShooting = 1f;

    [Header("Quantity of points")]
    [SerializeField] private float PointsForThisTank = 100;

    [Header("Strength of this tank")]
    [SerializeField] private int QuantityOfBulletsForDestroyingThisTank = 1;

    [Header("Possibility of stronger tank")]
    [SerializeField] private Sprite SpriteForStrongTank;
    [SerializeField, Range(0, 1)] private float PossibilityOfStrongTank;

    private GameField GameField;
    private Rigidbody2D ThisRigidbody2D;
    private int CounterForChangingDirection = 0;
    private int NeededValueOfCounterForChangingDirection;
    private int CounterForShooting = 0;
    private int NeededValueOfCounterForShooting;
    private Vector2 PositionOfThePlayerBase;

    private void Awake()
    {
        bool WillThisTankBeStrong = (new System.Random().Next(0, Mathf.RoundToInt(1f / PossibilityOfStrongTank)) == 0);
        if(WillThisTankBeStrong)
        {
            GetComponent<SpriteRenderer>().sprite = SpriteForStrongTank;
            Speed *= 1.5f;
            IntervalInSecondsForShooting *= 1.5f;
            QuantityOfBulletsForDestroyingThisTank *= 2;
            PointsForThisTank *= 1.5f;
        }

        GameField = FindObjectOfType<GameField>();
        ThisRigidbody2D = GetComponent<Rigidbody2D>();
        NeededValueOfCounterForChangingDirection = (int)(IntervalInSecondsForChangingDirection / Time.fixedDeltaTime);
        CounterForChangingDirection = NeededValueOfCounterForChangingDirection -1;
        NeededValueOfCounterForShooting = (int)(IntervalInSecondsForShooting / Time.fixedDeltaTime);
        CounterForShooting = NeededValueOfCounterForShooting - 1;
    }

    private void Start()
    {
        PositionOfThePlayerBase = FindObjectOfType<PlayerBase>().transform.position;
    }

    private void MoveInDirection(Vector2 direction)
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.up, direction);
        ThisRigidbody2D.velocity = direction.normalized * Speed;
    }

    private Vector2 ChooseRandomlyBetweenTwoVector2(Vector2 v1, Vector2 v2)
    {
        return new System.Random().Next(0, 2) == 0 ? v1 : v2;
    }

    private Vector2 ChooseDirection()
    {
        Vector2 DifferenceBetweenPositions = PositionOfThePlayerBase - (Vector2)transform.position;
        if(DifferenceBetweenPositions.x <= 0 && DifferenceBetweenPositions.y <= 0)
        {
            return ChooseRandomlyBetweenTwoVector2(Vector2.left, Vector2.down);
        }
        else if(DifferenceBetweenPositions.x > 0 && DifferenceBetweenPositions.y <= 0)
        {
            return ChooseRandomlyBetweenTwoVector2(Vector2.right, Vector2.down);
        }
        else if (DifferenceBetweenPositions.x <= 0 && DifferenceBetweenPositions.y > 0)
        {
            return ChooseRandomlyBetweenTwoVector2(Vector2.left, Vector2.up);
        }
        else if (DifferenceBetweenPositions.x > 0 && DifferenceBetweenPositions.y > 0)
        {
            return ChooseRandomlyBetweenTwoVector2(Vector2.right, Vector2.up);
        }
        return Vector2.down;
    }

    private void FixedUpdate()
    {
        CounterForChangingDirection++;
        if(CounterForChangingDirection == NeededValueOfCounterForChangingDirection)
        {
            CounterForChangingDirection = 0;
            MoveInDirection(ChooseDirection());
        }
        CounterForShooting++;
        if(CounterForShooting == NeededValueOfCounterForShooting)
        {
            CounterForShooting = 0;
            Shoot();
        }
    }

    private new void Shoot()
    {
        base.Shoot().GetComponent<Bullet>().BulletType = BulletType.EnemyBullet;
    }

    public void OnCollisionWithBullet()
    {
        QuantityOfBulletsForDestroyingThisTank--;
        if(QuantityOfBulletsForDestroyingThisTank <= 0)
        {
            OnDestroyByBullet();
        }
    }

    public void OnDestroyByBullet()
    {
        GameField.WhenOneEnemyTankWasDestroyed(PointsForThisTank);
        Destroy(gameObject);
    }
}