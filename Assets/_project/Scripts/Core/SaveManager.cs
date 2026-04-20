// File: SaveManager.cs
using UnityEngine;
using System.IO; // Wajib untuk baca/tulis file ke Hard Disk

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    // Ini adalah data yang sedang aktif (dimainkan) saat ini
    public SaveData currentData; 

    // Lokasi aman untuk menyimpan file di OS apapun (Windows, Android, iOS)
    private string saveFilePath;

    private void Awake()
    {
        // Setup Singleton
        if (Instance == null)
        {
            Instance = this;
            // Tentukan jalur file-nya (namanya bebas, misal: arcanesave.json)
            saveFilePath = Path.Combine(Application.persistentDataPath, "arcanesave.json");
            Debug.Log(saveFilePath);
            
            // Langsung muat data saat game baru dibuka
            LoadGame(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- FUNGSI MENYIMPAN ---
    public void SaveGame()
    {
        // 1. Ubah objek C# (currentData) menjadi teks JSON
        // Parameter 'true' agar format teksnya rapi dan mudah dibaca manusia
        string json = JsonUtility.ToJson(currentData, true);

        // 2. Tulis teks tersebut ke hard disk
        File.WriteAllText(saveFilePath, json);
        
        Debug.Log("Game Disimpan di: " + saveFilePath);
    }

    // --- FUNGSI MEMUAT ---
    public void LoadGame()
    {
        // Cek apakah file save sudah pernah dibuat sebelumnya
        if (File.Exists(saveFilePath))
        {
            // 1. Baca teks JSON dari hard disk
            string json = File.ReadAllText(saveFilePath);

            // 2. Ubah teks JSON kembali menjadi objek C#
            currentData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Save Data Berhasil Dimuat!");
        }
        else
        {
            // Jika tidak ada file (Pemain baru main pertama kali)
            currentData = new SaveData(); // Buat kotak data baru dengan nilai default
            Debug.Log("Tidak ada file save, membuat Save Data baru.");
        }
    }
}