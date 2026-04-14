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
        TextAsset csvFile = Resources.Load<TextAsset>("Data/FoodData");

        if (csvFile == null)
        {
            Debug.LogError("File FoodData.csv tidak ditemukan di folder Resources/Data!");
            return;
        }

        string[] lines = csvFile.text.Split(new char[] { '\n' });

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] columns = lines[i].Split(',');

            if (columns.Length >= 10) // Sekarang ada 10 kolom!
            {
                FoodData newFood = new FoodData();

                // 1. Ambil data Teks (String)
                newFood.id = columns[0].Trim();
                newFood.foodName = columns[1].Trim();

                // 2. Konversi Teks menjadi ENUM (Abaikan besar/kecil huruf dengan nilai 'true')
                System.Enum.TryParse(columns[2].Trim(), true, out newFood.type);
                System.Enum.TryParse(columns[3].Trim(), true, out newFood.tipeGizi);
                System.Enum.TryParse(columns[4].Trim(), true, out newFood.profilRasa);

                // 3. Konversi Teks menjadi Angka (Float)
                float.TryParse(columns[5].Trim(), out newFood.nutrition);
                float.TryParse(columns[6].Trim(), out newFood.baseAppeal);

                // 4. Ambil Deskripsi
                newFood.description = columns[7].Trim();

                // 5. Ukuran grid di piring
                int.TryParse(columns[8].Trim(), out newFood.gridWidth);
                int.TryParse(columns[9].Trim(), out newFood.gridHeight);

                // Masukkan ke dalam kamus (Dictionary)
                FoodDatabase.Add(newFood.id, newFood);
            }
            else
            {
                Debug.LogWarning($"Baris ke-{i} di CSV memiliki format yang salah atau kolom kurang!");
            }
        }

        Debug.Log($"Database Sukses Dimuat! Total Makanan: {FoodDatabase.Count}");

        // (Opsional) Cek di Console apakah data masuk dengan benar
        // Debug.Log("Contoh: " + FoodDatabase["itm_dragon"].foodName + " punya gizi " + FoodDatabase["itm_dragon"].tipeGizi);
    }

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