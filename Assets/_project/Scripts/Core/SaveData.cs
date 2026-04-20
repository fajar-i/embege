// File: SaveData.cs
[System.Serializable] // WAJIB ADA agar Unity bisa mengubahnya jadi JSON
public class SaveData
{
    // --- PENGATURAN (SETTINGS) ---
    public float bgmVolume = 1f; // Default 1 (100%)
    public float sfxVolume = 1f; // Default 1 (100%)

    // NANTI SAAT GDD SELESAI, ANDA TINGGAL NAMBAH DI SINI:
    // public int playerMoney = 1000;
    // public int kitchenLevel = 1;
    // public List<string> unlockedRecipes = new List<string>();
}