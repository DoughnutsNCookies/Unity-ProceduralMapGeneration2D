using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CandidateMap
{
    public MapGrid grid;
    private int numberOfPieces = 0;
    private bool[] obstaclesArray = null;
    private Vector3 startPoint;
    private Vector3 exitPoint;
    private List<KnightPiece> knightPiecesList;

    public MapGrid Grid { get => grid; }
    public bool[] ObstaclesArray { get => obstaclesArray; }

    public CandidateMap(MapGrid grid, int numberOfPieces)
    {
        this.numberOfPieces = numberOfPieces;
        this.grid = grid;
    }

    public void CreateMap(Vector3 startPosition, Vector3 exitPosition, bool autoRepair = false)
    {
        this.startPoint = startPosition;
        this.exitPoint = exitPosition;
        obstaclesArray = new bool[grid.Width * grid.Length];
        this.knightPiecesList = new List<KnightPiece>();
        RandomlyPlacedKnightPieces(this.numberOfPieces);

        PlaceObstacles();
    }

    private bool CheckIfPositionCanBeObstacle(Vector3 position)
    {
        if (position == startPoint || position == exitPoint)
            return false;
        int index = grid.CalculateIndexFromCoordinates(position.x, position.z);

        return obstaclesArray[index] == false;
    }

    private void RandomlyPlacedKnightPieces(int numberOfPieces)
    {
        var count = numberOfPieces;
        var knightPlacementTryLimit = 100;

        while (count > 0 && knightPlacementTryLimit > 0)
        {
            var randomIndex = Random.Range(0, obstaclesArray.Length);
            if (obstaclesArray[randomIndex] == false)
            {
                var coordinates = grid.CalculateCoordinatesFromIndex(randomIndex);
                if (coordinates == startPoint || coordinates == exitPoint)
                    continue;
                obstaclesArray[randomIndex] = true;
                knightPiecesList.Add(new KnightPiece(coordinates));
                count--;
            }
            knightPlacementTryLimit--;
        }
    }

    private void PlaceObstacleForThisKnight(KnightPiece knight)
    {
        foreach (var position in KnightPiece.listOfPossibleMoves)
        {
            var newPosition = knight.Position + position;
            if (grid.IsCellValid(newPosition.x, newPosition.z) && CheckIfPositionCanBeObstacle(newPosition))
            {
                obstaclesArray[grid.CalculateIndexFromCoordinates(newPosition.x, newPosition.z)] = true;
            }
        }
    }

    private void PlaceObstacles()
    {
        foreach (var knight in knightPiecesList)
        {
            PlaceObstacleForThisKnight(knight);
        }
    }

    public MapData ReturnMapData()
    {
        return new MapData
        {
            obstacleArray = this.obstaclesArray,
            knightPiecesList = knightPiecesList,
            startPosition = startPoint,
            exitPosition = exitPoint,
        };
    }
}