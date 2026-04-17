using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    private KitchenUtilityData currentSelectedData;
    private UtilityDirection currentDir = UtilityDirection.Down;

    private GameObject ghostInstance;
    private Camera mainCam;
    private Plane groundPlane; // Virtual plane di Y=0 untuk raycast mouse

    private void Start()
    {
        mainCam = Camera.main;
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }
    // HAPUS INI NANTI, hanya untuk test tanpa UI
    [SerializeField] private KitchenUtilityData testUtility;
    private void LateUpdate()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) SelectUtility(testUtility);
    }

    private void Update()
    {
        LateUpdate();
        if (currentSelectedData == null) return;

        HandleGhostAndPlacement();
        HandleRotation();
    }

    // Dipanggil oleh UI Button (Nanti)
    public void SelectUtility(KitchenUtilityData data)
    {
        currentSelectedData = data;
        currentDir = UtilityDirection.Down;

        if (ghostInstance != null) Destroy(ghostInstance);
        ghostInstance = Instantiate(data.ghostPrefab);
    }

    private void HandleGhostAndPlacement()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        GridManager.Instance.GetXY(mouseWorldPosition, out int x, out int y);

        // Snap Ghost ke Grid
        Vector3 snapPos = GridManager.Instance.GetWorldPosition(x, y);
        ghostInstance.transform.position = snapPos;
        ghostInstance.transform.rotation = GetRotationFromDirection(currentDir);

        // Visual Feedback (Opsional: Ubah warna material ghost jika tidak valid)
        bool canPlace = GridManager.Instance.CanPlaceObject(x, y, currentSelectedData, currentDir);

        // Klik Kiri untuk Membangun
        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace)
        {
            PlaceUtility(x, y, snapPos);
        }

        // Klik Kanan untuk Batal Build
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            DeselectUtility();
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
            // Putar enumerator searah jarum jam
            currentDir = (UtilityDirection)(((int)currentDir + 1) % 4);
        }
    }

    private void DeselectUtility()
    {
        currentSelectedData = null;
        if (ghostInstance != null) Destroy(ghostInstance);
    }

    // Helper: Hitung rotasi visual (Pivot Bottom-Left)
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

    // Helper: Cari posisi mouse presisi di lantai Y=0
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