// File: PlacedObject.cs
using UnityEngine;
using System.Collections.Generic;

public class PlacedObject : MonoBehaviour
{
    // Data statis dari ScriptableObject.
    public KitchenUtilityData UtilityData { get; private set; }
    
    // Data dinamis spesifik untuk instance ini.
    private Vector2Int gridOrigin;
    private UtilityDirection direction;

    // Helper untuk mendapatkan semua sel grid yang ditempati objek ini.
    public List<Vector2Int> GetGridPositions()
    {
        Vector2Int size = UtilityData.GetRotatedSize(direction);
        List<Vector2Int> positions = new List<Vector2Int>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                positions.Add(gridOrigin + new Vector2Int(x, y));
            }
        }
        return positions;
    }

    // Fungsi setup yang akan dipanggil oleh BuildManager saat objek dibuat.
    public void Setup(KitchenUtilityData utilityData, Vector2Int origin, UtilityDirection dir)
    {
        this.UtilityData = utilityData;
        this.gridOrigin = origin;
        this.direction = dir;
    }

    // Fungsi untuk menghancurkan objek dan membersihkan referensi.
    public void Demolish()
    {
        // ... Logika untuk animasi hancur, pengembalian resource, dll.
        Destroy(gameObject);
    }
}