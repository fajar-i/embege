using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("Ghost Colors")]
    public Color validColor = new Color(0, 1, 0, 0.5f);
    public Color invalidColor = new Color(1, 0, 0, 0.5f);
    private MeshRenderer[] ghostRenderers;
    private bool isDeleteMode = false;
    private KitchenUtilityData currentSelectedData;
    private UtilityDirection currentDir = UtilityDirection.Down;

    private GameObject ghostInstance;
    private Camera mainCam;
    private Plane groundPlane;

    private void Awake() => Instance = this;
    private void Start()
    {
        mainCam = Camera.main;
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (Keyboard.current.xKey.wasPressedThisFrame) ToggleDeleteMode();

        if (isDeleteMode)
        {
            HandleDeletion();
            return;
        }

        if (currentSelectedData == null) return;

        HandleGhostAndPlacement();
        HandleRotation();
    }

    public void SelectUtility(KitchenUtilityData data)
    {
        isDeleteMode = false;
        currentSelectedData = data;
        currentDir = UtilityDirection.Down;

        if (ghostInstance != null) Destroy(ghostInstance);
        ghostInstance = Instantiate(data.ghostPrefab);
        ghostRenderers = ghostInstance.GetComponentsInChildren<MeshRenderer>();
    }

    private void HandleGhostAndPlacement()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        GridManager.Instance.GetXY(mousePos, out int x, out int y);

        // --- BAGIAN YANG DIPERBAIKI ---
        // 1. Dapatkan posisi awal grid
        Vector3 snapPos = GridManager.Instance.GetWorldPosition(x, y);
        // 2. Tambahkan offset berdasarkan rotasi agar visual tetap di dalam batas logika
        Vector3 offsetPos = snapPos + GetRotationOffset(currentSelectedData, currentDir);

        ghostInstance.transform.position = offsetPos;
        ghostInstance.transform.rotation = GetRotationFromDirection(currentDir);
        // ------------------------------

        bool canPlace = GridManager.Instance.CanPlaceObject(x, y, currentSelectedData, currentDir);

        Color targetColor = canPlace ? validColor : invalidColor;
        foreach (var r in ghostRenderers) r.material.color = targetColor;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Klik Kiri (Kirim offsetPos, bukan snapPos biasa)
        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace) 
        {
            PlaceUtility(x, y, offsetPos);
        }

        // Klik Kanan
        if (Mouse.current.rightButton.wasPressedThisFrame) DeselectUtility();
    }

    private void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode;
        DeselectUtility();
        Debug.Log("Delete Mode: " + isDeleteMode);
    }

    private void HandleDeletion()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePos = GetMouseWorldPosition();
            GridManager.Instance.GetXY(mousePos, out int x, out int y);

            PlacedObject objToDestroy = GridManager.Instance.GetPlacedObjectAt(x, y);
            if (objToDestroy != null)
            {
                GridManager.Instance.ClearGrid(objToDestroy);
                objToDestroy.Demolish();
            }
        }
    }

    private void PlaceUtility(int x, int y, Vector3 spawnPosition)
    {
        GameObject newObj = Instantiate(currentSelectedData.prefab, spawnPosition, GetRotationFromDirection(currentDir));
        PlacedObject placedObj = newObj.GetComponent<PlacedObject>();

        placedObj.Setup(currentSelectedData, new Vector2Int(x, y), currentDir);
        GridManager.Instance.OccupyGrid(x, y, placedObj, currentSelectedData, currentDir);
    }

    private void HandleRotation()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            currentDir = (UtilityDirection)(((int)currentDir + 1) % 4);
        }
    }

    private void DeselectUtility()
    {
        currentSelectedData = null;
        if (ghostInstance != null) Destroy(ghostInstance);
    }

    private Quaternion GetRotationFromDirection(UtilityDirection dir)
    {
        return dir switch
        {
            UtilityDirection.Down => Quaternion.Euler(0, 0, 0),
            UtilityDirection.Left => Quaternion.Euler(0, 90, 0),
            UtilityDirection.Up => Quaternion.Euler(0, 180, 0),
            UtilityDirection.Right => Quaternion.Euler(0, 270, 0),
            _ => Quaternion.identity
        };
    }

   // --- FUNGSI YANG DIPERBAIKI ---
    private Vector3 GetRotationOffset(KitchenUtilityData data, UtilityDirection dir)
    {
        float cellSize = GridManager.Instance.GetCellSize(); 

        return dir switch
        {
            UtilityDirection.Down => new Vector3(0, 0, 0),
            // Didorong ke atas (sumbu Z) sebesar Width
            UtilityDirection.Left => new Vector3(0, 0, data.width * cellSize), 
            // Didorong ke kanan dan ke atas sebesar Width & Height
            UtilityDirection.Up => new Vector3(data.width * cellSize, 0, data.height * cellSize),
            // Didorong ke kanan (sumbu X) sebesar Height
            UtilityDirection.Right => new Vector3(data.height * cellSize, 0, 0), 
            _ => Vector3.zero
        };
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }
        return Vector3.zero;
    }
}