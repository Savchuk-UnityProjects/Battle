using System.Collections.Generic;

public class MazeGenerator
{
    public bool[,] Generate(int width, int height)
    {        
        bool[,] Maze = new bool[height, width];

        int Abscissa = UnityEngine.Random.Range(0, width / 2) * 2;
        int Ordinate = UnityEngine.Random.Range(0, height / 2) * 2;

        HashSet<KeyValuePair<int, int>> ConnectPoints = new HashSet<KeyValuePair<int, int>>
        {
            new KeyValuePair<int, int>(Abscissa, Ordinate)
        };

        while (ConnectPoints.Count > 0)
        {
            KeyValuePair<int, int> Point = ConnectPoints.RemoveRandomElement();

            Abscissa = Point.Key;
            Ordinate = Point.Value;

            Maze[Ordinate, Abscissa] = true;

            Connect(Maze, Abscissa, Ordinate);
            AddVisitedPoints(Maze, ConnectPoints, Abscissa, Ordinate);
        }

        return Maze;
    }

    private void Connect(bool[,] maze, int abscissa, int ordinate)
    {
        int[,] Directions = new int[,]
        {
            {-1, 0 },
            { 1, 0 },
            { 0,-1 },
            { 0, 1 }
        };

        Shuffling.Shuffle(Directions);

        for (int i = 0; i < Directions.GetLength(0); i++)
        {
            int NeighboringAbscissa = abscissa + Directions[i, 0] * 2;
            int NeighboringY = ordinate + Directions[i, 1] * 2;

            if (DoesRoadExist(maze, NeighboringAbscissa, NeighboringY))
            {
                int ConnectorAbscissa = abscissa + Directions[i, 0];
                int ConnectorY = ordinate + Directions[i, 1];
                maze[ConnectorY, ConnectorAbscissa] = true;

                return;
            }
        }
    }

    private void AddVisitedPoints(bool[,] maze, HashSet<KeyValuePair<int, int>> points, int abscissa, int ordinate)
    {
        void AddVisitedPointIfPoint(int abscissa, int Y)
        {
            if (IsPointInMaze(maze, abscissa, Y) && !DoesRoadExist(maze, abscissa, Y))
            {
                points.Add(new(abscissa, Y));
            }
        }

        AddVisitedPointIfPoint(abscissa - 2, ordinate);
        AddVisitedPointIfPoint(abscissa + 2, ordinate);
        AddVisitedPointIfPoint(abscissa, ordinate - 2);
        AddVisitedPointIfPoint(abscissa, ordinate + 2);
    }


    private bool DoesRoadExist(bool[,] maze, int abscissa, int ordinate)
    {
        return IsPointInMaze(maze, abscissa, ordinate) && maze[ordinate, abscissa] == true;
    }

    private bool IsPointInMaze(bool[,] maze, int abscissa, int ordinate)
    {
        int height = maze.GetLength(0);
        int width = maze.GetLength(1);
        return abscissa >= 0 && abscissa < width && ordinate >= 0 && ordinate < height;
    }
}