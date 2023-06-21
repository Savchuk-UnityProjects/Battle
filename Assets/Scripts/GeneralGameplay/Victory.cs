using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Victory : MonoBehaviour
{
    [SerializeField] private GameObject CanvasToBeShowedOnVictory;
    [SerializeField] private TextMeshProUGUI TextWithScore;
    [SerializeField] private GameField GameField;

    public void Win()
    {
        Time.timeScale = 0;
        TextWithScore.text = $"Your score: {GameField.AllPoints} points";
        CanvasToBeShowedOnVictory.SetActive(true);
    }
}