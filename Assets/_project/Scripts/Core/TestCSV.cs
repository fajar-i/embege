using UnityEngine;

public class TestCSV : MonoBehaviour
{
    void Start()
    {
        // Ambil komponen Database
        DatabaseManager db = GetComponent<DatabaseManager>();
        
        // Panggil spesifik ID
        FoodData steak = db.GetFoodByID("itm_dragon");
        
        if(steak != null)
        {
            Debug.Log($"BERHASIL MEMBACA CSV: Nama: {steak.foodName}, Gizi: {steak.nutrition}");
        }
    }
}