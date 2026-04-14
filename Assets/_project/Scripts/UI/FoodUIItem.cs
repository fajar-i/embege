using UnityEngine;
using UnityEngine.UI;
using TMPro; // Pastikan ini tetap ada

public class FoodUIItem : MonoBehaviour
{
    // KITA UBAH MENJADI PRIVATE (Agar tidak muncul di Inspector Unity)
    private TextMeshProUGUI foodNameText;
    
    [HideInInspector] 
    public FoodData myFoodData; 
    [HideInInspector] 
    public PlatingManager manager; 
    [HideInInspector] 
    public bool isOnPlate = false; 

    // Dipanggil oleh PlatingManager saat tombol ini dibuat
    public void Setup(FoodData data, PlatingManager pManager, bool isPlated)
    {
        // === INI KUNCI ANTI-GAGALNYA ===
        // Script akan secara otomatis mencari komponen TextMeshProUGUI
        // di dalam dirinya sendiri atau anak objeknya.
        foodNameText = GetComponentInChildren<TextMeshProUGUI>();

        // Cek apakah teks berhasil ditemukan
        if (foodNameText == null)
        {
            Debug.LogError("Gagal menemukan TextMeshProUGUI di dalam prefab tombol ini!");
            return;
        }
        // ===============================

        myFoodData = data;
        manager = pManager;
        isOnPlate = isPlated;
        
        // Ubah teks layar
        foodNameText.text = data.foodName;

        // Menambahkan fungsi klik pada tombol secara otomatis via script
        GetComponent<Button>().onClick.AddListener(OnFoodClicked);
    }

    void OnFoodClicked()
    {
        if (isOnPlate)
        {
        }
        else
        {
        }
    }
}