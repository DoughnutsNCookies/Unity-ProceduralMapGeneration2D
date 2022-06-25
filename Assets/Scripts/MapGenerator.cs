using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GridVisualizer gridVisualizer;
    public MapVisualizer mapVisualizer;
    public Direction startEdge;
    public Direction exitEdge;
    public bool randomPlacement;
    public int numberOfPieces;
    private Vector3 startPosition;
    private Vector3 exitPosition;

    public int width;
    public int length;
    private MapGrid grid;

    void Start()
    {
        gridVisualizer.VisualizeGrid(width, length);
        GenerateNewMap();
    }

    public void GenerateNewMap()
    {
        mapVisualizer.ClearMap();

        grid = new MapGrid(width, length);

        MapHelper.RandomlyChooseAndSetStartAndExit(grid, ref startPosition, ref exitPosition, randomPlacement, startEdge, exitEdge);

        CandidateMap map = new CandidateMap(grid, numberOfPieces);
        map.CreateMap(startPosition, exitPosition);
        mapVisualizer.VisualizeMap(grid, map.ReturnMapData(), false);
    }
}
