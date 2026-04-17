// File: KitchenUtilityData.cs
using UnityEngine;

// Enum untuk rotasi, membuat logika lebih jelas daripada angka 0-3.
public enum UtilityDirection { Down, Left, Up, Right }

[CreateAssetMenu(fileName = "NewUtilityData", menuName = "Arcane Kitchen/Kitchen Utility Data")]
public class KitchenUtilityData : ScriptableObject
{
    [Tooltip("ID unik untuk referensi, misal 'UTL_COMP_01'")]
    public string id;
    public string utilityName;
    public GameObject prefab;
    
    // Gunakan versi transparan/berwarna untuk visualisasi penempatan.
    public GameObject ghostPrefab; 

    public int width = 1;
    public int height = 1;

    // Fungsi helper untuk mendapatkan ukuran yang sudah dirotasi.
    public Vector2Int GetRotatedSize(UtilityDirection dir)
    {
        // Jika rotasi horizontal (Left/Right), tukar width dan height.
        return (dir == UtilityDirection.Left || dir == UtilityDirection.Right) 
            ? new Vector2Int(height, width) 
            : new Vector2Int(width, height);
    }
}