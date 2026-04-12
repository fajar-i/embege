using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlatingManager : MonoBehaviour
{
    [Header("Referensi Sistem")]
    public DatabaseManager dbManager;
    public GameObject foodUIPrefab;

    [Header("Referensi UI Panel")]
    public Transform panelKiriInventaris;
    public Transform panelTengahPiring;

    [Header("Referensi UI Teks Statistik")]
    public TextMeshProUGUI txtGizi;
    public TextMeshProUGUI txtSelera;
    public TextMeshProUGUI txtProbabilitas;

    // List makanan yang sedang ada di atas piring
    private List<FoodData> platedFoods = new List<FoodData>();

    void Start()
    {
        // 1. Munculkan semua makanan dari CSV ke Panel Kiri
        PopulateInventoryUI();
        
        // 2. Set statistik awal ke angka 0
        UpdateStatsUI(); 
    }

    void PopulateInventoryUI()
    {
        // Looping semua isi kamus (Dictionary) di DatabaseManager
        foreach (var item in dbManager.FoodDatabase.Values)
        {
            // Buat tombol baru dari prefab
            GameObject newBtn = Instantiate(foodUIPrefab, panelKiriInventaris);
            
            // Set datanya
            FoodUIItem uiItem = newBtn.GetComponent<FoodUIItem>();
            uiItem.Setup(item, this, false); // false = ada di inventaris, bukan di piring
        }
    }

    // Fungsi dipanggil saat tombol di Panel Kiri diklik
    public void AddToPlate(FoodData foodToAdd)
    {
        platedFoods.Add(foodToAdd);

        // Buat visual tombol di piring
        GameObject platedBtn = Instantiate(foodUIPrefab, panelTengahPiring);
        FoodUIItem uiItem = platedBtn.GetComponent<FoodUIItem>();
        uiItem.Setup(foodToAdd, this, true); // true = ada di piring

        UpdateStatsUI(); // Hitung ulang!
    }

    // Fungsi dipanggil saat tombol di Panel Tengah diklik
    public void RemoveFromPlate(FoodUIItem itemToRemove)
    {
        platedFoods.Remove(itemToRemove.myFoodData);
        Destroy(itemToRemove.gameObject); // Hapus tombol dari layar
        UpdateStatsUI(); // Hitung ulang!
    }

    // RUMUS RAHASIA ANDA ADA DI SINI
    void UpdateStatsUI()
    {
        if (platedFoods.Count == 0)
        {
            txtGizi.text = "Gizi: 0";
            txtSelera.text = "Selera: 0";
            txtProbabilitas.text = "Peluang Habis: 0%";
            return;
        }

        float totalGizi = 0;
        float totalSelera = 0;

        foreach (FoodData food in platedFoods)
        {
            totalGizi += food.nutrition;
            totalSelera += food.taste;
        }

        // Karena satu piring bisa punya banyak makanan, kita gunakan Rata-rata agar persentase masuk akal
        float avgGizi = totalGizi / platedFoods.Count;
        float avgSelera = totalSelera / platedFoods.Count;

        // FORMULA ANDA: (Selera * 0.8) + (Gizi * 0.2)
        float probabilitas = (avgSelera * 0.8f) + (avgGizi * 0.2f);

        // Tampilkan ke layar (F1 berarti tampilkan 1 angka di belakang koma)
        txtGizi.text = $"Gizi: {totalGizi}"; 
        txtSelera.text = $"Selera: {totalSelera}";
        txtProbabilitas.text = $"Peluang Habis: {probabilitas:F1}%";
    }
}