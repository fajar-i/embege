using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width = 20;
    [SerializeField] private int height = 20;
    [SerializeField] private float cellSize = 1f;

    private PlacedObject[,] gridArray; // Aktifkan array ini
    public static GridManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        gridArray = new PlacedObject[width, height];
    }

    public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, 0, y) * cellSize;
    public float GetCellSize() => cellSize;

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.z / cellSize);
    }

    // Cek apakah koordinat ada di dalam batas grid
    private bool IsValidGridPosition(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

    // Cek apakah area kosong dan valid untuk ditempati
    public bool CanPlaceObject(int x, int y, KitchenUtilityData utilityData, UtilityDirection dir)
    {
        Vector2Int size = utilityData.GetRotatedSize(dir);
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (!IsValidGridPosition(x + i, y + j) || gridArray[x + i, y + j] != null)
                    return false;
            }
        }
        return true;
    }

    // Daftarkan objek ke dalam array logika
    public void OccupyGrid(int x, int y, PlacedObject placedObject, KitchenUtilityData data, UtilityDirection dir)
    {
        Vector2Int size = data.GetRotatedSize(dir);
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                gridArray[x + i, y + j] = placedObject;
            }
        }
    }
}