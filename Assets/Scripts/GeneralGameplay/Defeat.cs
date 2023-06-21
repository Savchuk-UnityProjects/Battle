using TMPro;
using UnityEngine;

public class Defeat : MonoBehaviour
{
    [SerializeField] private GameObject CanvasToBeShowedOnDefeat;
    [SerializeField] private TextMeshProUGUI TextWithScore;
    [SerializeField] private GameField GameField;

    public void BeDefeated()
    {
        Time.timeScale = 0;
        TextWithScore.text = $"Your score: {GameField.AllPoints} points";
        CanvasToBeShowedOnDefeat.SetActive(true);
    }
}