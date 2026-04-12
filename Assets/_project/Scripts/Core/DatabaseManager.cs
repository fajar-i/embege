using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    // Dictionary untuk menyimpan data. Kunci-nya (Key) adalah ID (string), Isinya adalah tipe data FoodData.
    public Dictionary<string, FoodData> FoodDatabase = new Dictionary<string, FoodData>();

    void Awake()
    {
        LoadFoodDataFromCSV();
    }

    void LoadFoodDataFromCSV()
    {
        // 1. Membaca file CSV dari folder Resources
        TextAsset csvFile = Resources.Load<TextAsset>("Data/FoodData");
        
        if (csvFile == null)
        {
            Debug.LogError("File FoodData.csv tidak ditemukan di folder Resources/Data!");
            return;
        }

        // 2. Memecah teks menjadi baris-baris (split berdasarkan Enter / baris baru)
        string[] lines = csvFile.text.Split(new char[] { '\n' });

        // 3. Looping setiap baris (mulai dari i=1 untuk melewati baris Header)
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue; // Lewati baris kosong

            string[] columns = lines[i].Split(','); // Pecah baris berdasarkan koma

            if (columns.Length >= 5) // Pastikan ada 5 kolom sesuai data kita
            {
                FoodData newFood = new FoodData();
                newFood.id = columns[0].Trim();
                newFood.foodName = columns[1].Trim();
                newFood.description = columns[2].Trim();
                
                // Mengubah teks angka menjadi tipe float
                float.TryParse(columns[3], out newFood.nutrition);
                float.TryParse(columns[4], out newFood.taste);

                // Masukkan ke dalam kamus (Dictionary)
                FoodDatabase.Add(newFood.id, newFood);
            }
        }

        Debug.Log($"Database Loaded! Total Makanan: {FoodDatabase.Count}");
    }
    
    // Fungsi untuk memanggil data dari script lain dengan mudah
    public FoodData GetFoodByID(string foodID)
    {
        if (FoodDatabase.ContainsKey(foodID))
        {
            return FoodDatabase[foodID];
        }
        Debug.LogWarning($"Makanan dengan ID {foodID} tidak ditemukan!");
        return null;
    }
}