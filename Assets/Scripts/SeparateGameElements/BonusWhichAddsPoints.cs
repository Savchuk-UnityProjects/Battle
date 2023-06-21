using UnityEngine;
using TMPro;

public class BonusWhichAddsPoints : BonusBlock
{
    [SerializeField] private float QuantityOfPoints;
    [SerializeField] private TextMeshPro TextWithPoints;
    
    private GameField GameField;

    private void Awake()
    {
        GameField = FindObjectOfType<GameField>();
        TextWithPoints.text = QuantityOfPoints.ToString();
        OnGettingThisBonusByPlayer += () => GameField.AllPoints += QuantityOfPoints;
    }

    private void OnDestroy()
    {
        OnGettingThisBonusByPlayer = null;
    }
}