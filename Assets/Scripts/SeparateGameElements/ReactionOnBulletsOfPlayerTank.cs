using UnityEngine;
using TMPro;

[RequireComponent(typeof(PlayerTank))]
public class ReactionOnBulletsOfPlayerTank : ObjectWhichReactsOnBullet
{
    [SerializeField] private int Lives = 3;
    [SerializeField] private string NameOfObjectWithTextWithQuantityOfLives = "TextWithQuantityOfLives";

    private TextMeshPro TextWithQuantityOfLives;
    private Defeat Defeat;
    private PlayerTank ThisPlayerTank;

    private void Awake()
    {
        if(Lives <= 0)
        {
            Debug.LogError($"Quantity of lives should be greater than 0");
        }
        TextWithQuantityOfLives = GameObject.Find(NameOfObjectWithTextWithQuantityOfLives).GetComponent<TextMeshPro>();
        UpdateTextWithQuantityOfLives();
        Defeat = FindObjectOfType<Defeat>();
        ThisPlayerTank = GetComponent<PlayerTank>();
        OnCollisionWithBullet += (bullet) => CollisionWithBullet();
    }

    private void UpdateTextWithQuantityOfLives()
    {
        TextWithQuantityOfLives.text = $"Lives: {Lives}";
    }

    private void CollisionWithBullet()
    {
        Lives--;
        if (Lives == 0)
        {
            Defeat.BeDefeated();
        }
        UpdateTextWithQuantityOfLives();
        ThisPlayerTank.transform.position = ThisPlayerTank.StartPosition;
    }
}