using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlatingManager : MonoBehaviour
{
    [Header("Referensi Sistem")]
    public DatabaseManager dbManager;
    public OmprengGrid omprengGrid; // Referensi ke Grid Piring
    public GameObject draggableFoodPrefab; // Cetak biru UI Makanan

    [Header("Referensi UI Panel")]
    public Transform panelInventaris; // Tempat menuangkan semua makanan dari CSV

    [Header("UI Teks Statistik")]
    public TextMeshProUGUI txtGizi;
    public TextMeshProUGUI txtProbabilitas;

    [Header("Simulasi Kebutuhan Klien")]
    public TipeGizi targetGizi = TipeGizi.Stamina;
    public float targetJumlahGizi = 80f;
    public ProfilRasa rasaDisukai = ProfilRasa.Gurih;
    public ProfilRasa rasaDibenci = ProfilRasa.Manis;

    void Start()
    {
        // Beri waktu sejenak agar DatabaseManager membaca CSV sebelum kita memanggilnya
        Invoke("PopulateInventoryUI", 0.1f);
    }

    void PopulateInventoryUI()
    {
        // Looping semua data CSV
        foreach (var item in dbManager.FoodDatabase.Values)
        {
            // Spawn Prefab Makanan ke Panel Inventaris
            GameObject newBtn = Instantiate(draggableFoodPrefab, panelInventaris);
            
            DraggableFood dragLogic = newBtn.GetComponent<DraggableFood>();
            
            // Suntikkan data dan referensi Grid ke dalamnya
            dragLogic.Setup(item, omprengGrid, omprengGrid.cellSize);
        }
        
        UpdateStatsUI(); // Kalkulasi awal (0)
    }

    // FUNGSI INI DIPANGGIL OLEH OMPRENGGRID SAAT MAKANAN BERHASIL DITARUH/DIAMBIL
    public void UpdateStatsUI()
    {
        // Ambil list makanan yang ada di piring langsung dari OmprengGrid
        List<FoodData> makananDiPiring = omprengGrid.GetFoodsOnPlate();

        if (makananDiPiring.Count == 0)
        {
            txtGizi.text = $"Gizi: 0 / {targetJumlahGizi}";
            txtProbabilitas.text = "Peluang Habis: 0%";
            return;
        }

        // --- PASTE RUMUS BALANCING ANDA DI SINI ---
        // (Rumus perhitungan Mood, Kecocokan Gizi, Utama vs Lauk dari jawaban kita yang lalu)
        // Saya persingkat untuk menghemat ruang, tapi konsepnya sama:
        
        float totalGiziCocok = 0;
        float dayaTarikUtama = 0;
        float totalDayaTarikLauk = 0;
        bool adaUtama = false;
        int jumlahLauk = 0;

        foreach (FoodData food in makananDiPiring)
        {
            if (food.tipeGizi == targetGizi) totalGiziCocok += food.nutrition;

            float dayaTarikFinal = food.baseAppeal;
            if (food.profilRasa == rasaDisukai) dayaTarikFinal *= 1.5f;
            else if (food.profilRasa == rasaDibenci) dayaTarikFinal *= 0.5f;
            dayaTarikFinal = Mathf.Clamp(dayaTarikFinal, 0, 100);

            if (food.type == FoodType.Utama && !adaUtama)
            {
                dayaTarikUtama = dayaTarikFinal;
                adaUtama = true;
            }
            else 
            {
                totalDayaTarikLauk += dayaTarikFinal;
                jumlahLauk++;
            }
        }

        float bobotGizi = Mathf.Clamp01(totalGiziCocok / targetJumlahGizi) * 40f;
        float bobotUtama = (dayaTarikUtama / 100f) * 40f;
        float bobotLauk = jumlahLauk > 0 ? ((totalDayaTarikLauk / jumlahLauk) / 100f) * 20f : 0f;

        float probabilitasAkhir = adaUtama ? (bobotGizi + bobotUtama + bobotLauk) : bobotLauk;

        txtGizi.text = $"{targetGizi}: {totalGiziCocok} / {targetJumlahGizi}"; 
        txtProbabilitas.text = $"Peluang Habis: {probabilitasAkhir:F1}%";
    }
}