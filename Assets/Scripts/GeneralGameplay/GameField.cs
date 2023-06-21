using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameField : MonoBehaviour
{
    [SerializeField] private PrefabsOfGameElements PrefabsOfGameElements;
    [SerializeField] private int Height;
    [SerializeField] private int Width;
    [SerializeField] private float WidthOfOneGameElement = 1.28f;
    [SerializeField] private int QuantityOfEnemyTanksWhichShouldBeDestroyed = 5;
    [SerializeField] private Victory Victory;
    [SerializeField] private float IntervalInSecondsBetweenSpawningNewTanks = 2;
    [SerializeField] private TextMeshPro TextWithQuantityOfLeftEnemies;
    [SerializeField] private List<BonusBlock> PrefabsOfBonusBlocks = new();
    [SerializeField] private float IntervalInSecondsBetweenSpawningNewBonuses;

    private int StartingQuantityOfEnemyTanksWhichShouldBeDestroyed;
    private List<GameObject> AllShowedObjects = new();
    private int QuantityOfEnemyTanksOnTheField = 0;
    private Vector2 LeftUpperCornerPosition;
    private Vector2 LeftBottomCornerPosition;
    private Vector2 RightTopCornerPosition;
    private Vector2 SizeOfEnemyTank;
    private int CounterForCreatingNewTank = 0;
    private int NeededValueOfCounterForCreatingNewTank;
    private int CounterForCreatingNewBonus = 0;
    private int NeededValueOfCounterForCreatingNewBonus;

    public Vector2 SizeOfTheWholeMap { get => new (Width * WidthOfOneGameElement, Height * WidthOfOneGameElement); }
    public float AllPoints { get; set; } = 0;
    public List<List<KeyValuePair<GameElement, Direction>>> StartingPositionOnTheGameField { get; private set; } = new();

    private void Awake()
    {
        UpdateTextWithQuantityOfLeftEnemies();
        NeededValueOfCounterForCreatingNewTank = (int)(IntervalInSecondsBetweenSpawningNewTanks / Time.fixedDeltaTime);
        NeededValueOfCounterForCreatingNewBonus = (int)(IntervalInSecondsBetweenSpawningNewBonuses / Time.fixedDeltaTime);
        GameObject PrefabOfEnemyTank = PrefabsOfGameElements.GetPrefabForGameElementOrReturnNullIfNotFound(GameElement.EnemyTank);
        SizeOfEnemyTank = PrefabOfEnemyTank.GetComponent<BoxCollider2D>().size;
        SizeOfEnemyTank.Scale(PrefabOfEnemyTank.transform.localScale);
        StartingQuantityOfEnemyTanksWhichShouldBeDestroyed = QuantityOfEnemyTanksWhichShouldBeDestroyed;
        Time.timeScale = 1;
        InitializeArray();
        GenerateStartingPosition();
        DisplayStartingPositionOnTheBoard();
    }

    private void InitializeArray()
    {
        for (int i = 0; i < Height; i++)
        {
            StartingPositionOnTheGameField.Add(new());
            for (int j = 0; j < Width; j++)
            {
                StartingPositionOnTheGameField[i].Add(new());
            }
        }
    }

    private void GenerateStartingPosition()
    {
        Direction ChooseDirectionRandomly()
        {
            int RandomNumber = new System.Random().Next(0, 4);
            if(RandomNumber == 0)
            {
                return Direction.Up;
            }
            else if(RandomNumber == 1)
            {
                return Direction.Left;
            }
            else if(RandomNumber == 2)
            {
                return Direction.Right;
            }
            else
            {
                return Direction.Down;
            }
        }

        GameElement ChooseRandomGameElementFromTwoOptions(GameElement gameElement1, GameElement gameElement2)
        {
            return new System.Random().Next(0, 2) == 0 ? gameElement1 : gameElement2;
        }

        bool[,] MazeScheme = new MazeGenerator().Generate(Width - 2, Height - 2);
        List<KeyValuePair<int, int>> EmptyFields = new();

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if(i == 0 || i + 1 == Height || j == 0 || j + 1 == Width)
                {
                    StartingPositionOnTheGameField[i][j] = new(GameElement.IndestructibleWall, Direction.Up);
                }
                else
                {
                    try
                    {
                        if (MazeScheme[i - 1, j - 1])
                        {
                            StartingPositionOnTheGameField[i][j] = new(GameElement.Empty, Direction.Up);
                            EmptyFields.Add(new(i, j));
                        }
                        else
                        {
                            StartingPositionOnTheGameField[i][j] = new(ChooseRandomGameElementFromTwoOptions(GameElement.DestructibleWall, GameElement.IndestructibleWall), Direction.Up);
                        }
                    }
                    catch(System.IndexOutOfRangeException)
                    {
                        Debug.Log($"{i}, {j}");
                    }
                    catch(System.ArgumentOutOfRangeException)
                    {
                        Debug.Log($"{i}, {j}");
                    }
                }
            }
        }

        void PlaceGameElementAtEmptyField(KeyValuePair<int, int> coordinatesOfThisField, GameElement elementWhichWillnePlacedAtRandomEmptyField, Direction direction)
        {
            StartingPositionOnTheGameField[coordinatesOfThisField.Key][coordinatesOfThisField.Value] = new(elementWhichWillnePlacedAtRandomEmptyField, direction);
        }

        for(int i = 0; i < EmptyFields.Count; i++)
        {
            for(int j = 0; j < EmptyFields.Count - i - 1; j++)
            {
                if (EmptyFields[j].Value > EmptyFields[j+1].Value)
                {
                    int key = EmptyFields[j].Key;
                    int value = EmptyFields[j].Value;
                    EmptyFields[j] = new(EmptyFields[j + 1].Key, EmptyFields[j + 1].Value);
                    EmptyFields[j + 1] = new(key, value);
                }
            }
        }

        PlaceGameElementAtEmptyField(EmptyFields[0], GameElement.PlayerTank, ChooseDirectionRandomly());
        for (int i = 1; i < 3; i++)
        {
            PlaceGameElementAtEmptyField(EmptyFields[i], GameElement.EnemyTank, ChooseDirectionRandomly());
        }
        PlaceGameElementAtEmptyField(EmptyFields[EmptyFields.Count - 1], GameElement.PlayerBase, Direction.Up);
    }

    private void DisplayStartingPositionOnTheBoard()
    {
        LeftUpperCornerPosition = new Vector2((- Width + 1) * WidthOfOneGameElement / 2, (Height - 1) * WidthOfOneGameElement / 2);
        Vector2 PositionOfCurrentElement;
        for(int i = 0; i < Height; i++)
        {
            PositionOfCurrentElement = new Vector2(LeftUpperCornerPosition.x, LeftUpperCornerPosition.y - i * WidthOfOneGameElement);
            for (int j = 0; j < Width; j++)
            {
                if(i == 0 && j + 1 == Width)
                {
                    RightTopCornerPosition = PositionOfCurrentElement;
                }
                else if(i + 1 == Height && j == 0)
                {
                    LeftBottomCornerPosition = PositionOfCurrentElement;
                }
                if(StartingPositionOnTheGameField[i][j].Key == GameElement.EnemyTank)
                {
                    QuantityOfEnemyTanksOnTheField++;
                }
                GameObject PrefabToBeInstantiated = PrefabsOfGameElements.GetPrefabForGameElementOrReturnNullIfNotFound(StartingPositionOnTheGameField[i][j].Key);
                if (PrefabToBeInstantiated != null)
                {
                    GameObject ThisGameElementAsGameObject = Instantiate(PrefabToBeInstantiated);
                    AllShowedObjects.Add(ThisGameElementAsGameObject);
                    ThisGameElementAsGameObject.transform.position = PositionOfCurrentElement;
                    ThisGameElementAsGameObject.transform.rotation = Quaternion.AngleAxis((int)StartingPositionOnTheGameField[i][j].Value, Vector3.forward);
                }
                PositionOfCurrentElement += WidthOfOneGameElement * Vector2.right;
            }
        }
    }

    private void ClearAllShowedObjects()
    {
        for(int i = 0; i < AllShowedObjects.Count; i++)
        {
            Destroy(AllShowedObjects[i]);
        }
        Bullet[] AllBullets = FindObjectsOfType<Bullet>();
        for(int i = 0; i < AllBullets.Length; i++)
        {
            Destroy(AllBullets[i].gameObject);
        }
        AllShowedObjects.Clear();
    }

    public void RestartThisLevel()
    {
        AllPoints = 0;
        CounterForCreatingNewTank = 0;
        CounterForCreatingNewBonus = 0;
        QuantityOfEnemyTanksWhichShouldBeDestroyed = StartingQuantityOfEnemyTanksWhichShouldBeDestroyed;
        QuantityOfEnemyTanksOnTheField = 0;
        ClearAllShowedObjects();
        DisplayStartingPositionOnTheBoard();
        UpdateTextWithQuantityOfLeftEnemies();
        Time.timeScale = 1;
    }

    public void GenerateNewLevel()
    {
        AllPoints = 0;
        CounterForCreatingNewTank = 0;
        CounterForCreatingNewBonus = 0;
        QuantityOfEnemyTanksWhichShouldBeDestroyed = StartingQuantityOfEnemyTanksWhichShouldBeDestroyed;
        QuantityOfEnemyTanksOnTheField = 0;
        InitializeArray();
        GenerateStartingPosition();
        ClearAllShowedObjects();
        DisplayStartingPositionOnTheBoard();
        UpdateTextWithQuantityOfLeftEnemies();
        Time.timeScale = 1;
    }

    public void WhenOneEnemyTankWasDestroyed(float pointsForThisTank = 0)
    {
        AllPoints += pointsForThisTank;
        QuantityOfEnemyTanksWhichShouldBeDestroyed--;
        QuantityOfEnemyTanksOnTheField--;
        UpdateTextWithQuantityOfLeftEnemies();
        if (QuantityOfEnemyTanksWhichShouldBeDestroyed == 0)
        {
            Victory.Win();
        }
    }

    private void SpawnNewEnemyTank()
    {
        if (QuantityOfEnemyTanksOnTheField + 1 <= QuantityOfEnemyTanksWhichShouldBeDestroyed)
        {
            bool CanNewEnemyTankBePlacedHere = false;
            Vector2 PositionOfNewTank;
            while (CanNewEnemyTankBePlacedHere == false)
            {
                PositionOfNewTank = new(UnityEngine.Random.Range(LeftBottomCornerPosition.x, RightTopCornerPosition.x), UnityEngine.Random.Range(LeftBottomCornerPosition.y, RightTopCornerPosition.y));
                if (Physics2D.OverlapBox(PositionOfNewTank, SizeOfEnemyTank, 0) == null)
                {
                    CanNewEnemyTankBePlacedHere = true;
                    QuantityOfEnemyTanksOnTheField++;
                    GameObject SpawnedTank = Instantiate(PrefabsOfGameElements.GetPrefabForGameElementOrReturnNullIfNotFound(GameElement.EnemyTank));
                    AllShowedObjects.Add(SpawnedTank);
                    SpawnedTank.transform.position = PositionOfNewTank;
                }
            }
        }
    }

    private void SpawnNewBonusBlock()
    {
        bool CanNewBlockBePlacedHere = false;
        Vector2 PositionOfNewBlock;
        GameObject PrefabOfTheBlockWhichWillBeSpawned = PrefabsOfBonusBlocks[new System.Random().Next(PrefabsOfBonusBlocks.Count)].gameObject;
        Vector2 SizeOfNewBlock = Vector2.Scale(PrefabOfTheBlockWhichWillBeSpawned.transform.localScale, PrefabOfTheBlockWhichWillBeSpawned.GetComponent<BoxCollider2D>().size);
        while (CanNewBlockBePlacedHere == false)
        {
            PositionOfNewBlock = new(UnityEngine.Random.Range(LeftBottomCornerPosition.x, RightTopCornerPosition.x), UnityEngine.Random.Range(LeftBottomCornerPosition.y, RightTopCornerPosition.y));
            if (Physics2D.OverlapBox(PositionOfNewBlock, SizeOfNewBlock, 0) == null)
            {
                CanNewBlockBePlacedHere = true;
                GameObject SpawnedBlock = Instantiate(PrefabOfTheBlockWhichWillBeSpawned);
                AllShowedObjects.Add(SpawnedBlock);
                SpawnedBlock.transform.position = PositionOfNewBlock;
            }
        }
    }

    private void FixedUpdate()
    {
        if(CounterForCreatingNewTank >= NeededValueOfCounterForCreatingNewTank)
        {
            SpawnNewEnemyTank();
            CounterForCreatingNewTank = 0;
        }
        CounterForCreatingNewTank++;

        if(CounterForCreatingNewBonus >= NeededValueOfCounterForCreatingNewBonus)
        {
            SpawnNewBonusBlock();
            CounterForCreatingNewBonus = 0;
        }
        CounterForCreatingNewBonus++;
    }

    private void UpdateTextWithQuantityOfLeftEnemies()
    {
        TextWithQuantityOfLeftEnemies.text = $"Enemies left: {QuantityOfEnemyTanksWhichShouldBeDestroyed}";
    }
}