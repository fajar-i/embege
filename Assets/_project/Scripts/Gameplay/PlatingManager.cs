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

    [Header("Simulasi Order Harian (Dari Sekolah)")]
    public int maxSlotPiring = 3; // Batasan Slot!
    public TipeGizi targetGiziYangDibutuhkan = TipeGizi.Stamina;
    public float targetJumlahGizi = 80f;

    [Header("Simulasi Mood Siswa")]
    public ProfilRasa rasaYangLagiDisukai = ProfilRasa.Gurih;
    public ProfilRasa rasaYangLagiDibenci = ProfilRasa.Manis;
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
        if (platedFoods.Count >= maxSlotPiring)
        {
            Debug.Log("Piring Sudah Penuh!");
            return; // Batalkan penambahan
        }
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
        if (platedFoods.Count == 0) return; // (Sama seperti sebelumnya)

        float totalGiziYangCocok = 0;
        float dayaTarikUtama = 0;
        float totalDayaTarikLauk = 0;
        bool adaMakananUtama = false;
        int jumlahLauk = 0;

        foreach (FoodData food in platedFoods)
        {
            // --- 1. MENGHITUNG KECOCOKAN GIZI ---
            // Hanya ditambahkan JIKA tipe gizinya sesuai dengan permintaan sekolah
            if (food.tipeGizi == targetGiziYangDibutuhkan)
            {
                totalGiziYangCocok += food.nutrition;
            }

            // --- 2. MENGHITUNG PENGARUH MOOD PADA DAYA TARIK ---
            float dayaTarikFinal = food.baseAppeal; // Mulai dari nilai dasar

            // Aplikasikan Multiplier berdasarkan Mood
            if (food.profilRasa == rasaYangLagiDisukai)
            {
                dayaTarikFinal *= 1.5f; // Naik 50% jika disukai
            }
            else if (food.profilRasa == rasaYangLagiDibenci)
            {
                dayaTarikFinal *= 0.5f; // Turun separuh jika dibenci
            }

            // Mentokkan agar daya tarik tidak tembus lebih dari 100
            dayaTarikFinal = Mathf.Clamp(dayaTarikFinal, 0, 100);

            // --- 3. MEMISAHKAN UTAMA DAN LAUK ---
            if (food.type == FoodType.Utama && !adaMakananUtama)
            {
                dayaTarikUtama = dayaTarikFinal;
                adaMakananUtama = true;
            }
            else 
            {
                totalDayaTarikLauk += dayaTarikFinal;
                jumlahLauk++;
            }
        }

        // --- 4. PEMBOBOTAN AKHIR (Seperti Rumus Sebelumnya) ---
        // A. Bobot Gizi (Maks 40%) - Berdasarkan gizi yang COCOK saja
        float persentaseGizi = Mathf.Clamp01(totalGiziYangCocok / targetJumlahGizi); 
        float bobotGizi = persentaseGizi * 40f;

        // B. Bobot Utama (Maks 40%) - Sudah dipengaruhi Mood
        float bobotUtama = (dayaTarikUtama / 100f) * 40f;

        // C. Bobot Lauk (Maks 20%) - Sudah dipengaruhi Mood
        float rataRataLauk = jumlahLauk > 0 ? (totalDayaTarikLauk / jumlahLauk) : 0f;
        float bobotLauk = (rataRataLauk / 100f) * 20f;

        float probabilitasAkhir = adaMakananUtama ? (bobotGizi + bobotUtama + bobotLauk) : bobotLauk;

        // Tampilkan ke UI
        txtGizi.text = $"{targetGiziYangDibutuhkan}: {totalGiziYangCocok} / {targetJumlahGizi}"; 
        txtProbabilitas.text = $"Peluang Habis: {probabilitasAkhir:F1}%";
    }
}