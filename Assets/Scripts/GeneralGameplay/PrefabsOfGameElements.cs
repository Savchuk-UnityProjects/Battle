using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsOfGameElements", menuName = "ScriptableObjects/PrefabsOfGameElements")]
public class PrefabsOfGameElements : ScriptableObject
{
    [SerializeField] private List<GameElement> GameElements;
    [SerializeField] private List<GameObject> PrefabsOfTheseGameElements;

    public GameObject GetPrefabForGameElementOrReturnNullIfNotFound(GameElement gameElement)
    {
        for(int i = 0; i < GameElements.Count; i++)
        {
            if (GameElements[i].Equals(gameElement))
            {
                return PrefabsOfTheseGameElements[i];
            }
        }
        return null;
    }
}