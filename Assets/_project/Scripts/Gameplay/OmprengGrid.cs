using UnityEngine;
using System.Collections.Generic;

public class OmprengGrid : MonoBehaviour
{
    public PlatingManager manager; // Referensi balik ke manager utama
    private List<FoodData> foodsOnPlate = new List<FoodData>(); // Menyimpan daftar makanan
    // Ukuran nampan kita (3x3)
    public const int GRID_WIDTH = 3;
    public const int GRID_HEIGHT = 3;
    public float cellSize = 100f; // Ukuran pixel 1 kotak (1x1)
    // Tabel memori piring kita. 
    // Jika isinya 'null', berarti kotak itu kosong.
    // Jika ada isinya, berarti ada makanan di koordinat itu.
    private FoodData[,] gridArray = new FoodData[GRID_WIDTH, GRID_HEIGHT];
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); // Ambil kotak UI Piring
    }
    // Fungsi penting 1: Mengecek apakah makanan BISA diletakkan
    public bool CheckCanPlace(FoodData food, int startX, int startY)
    {
        // 1. Cek Batas Luar (Out of Bounds)
        // Jika ditaruh di titik X, lalu ditambah lebarnya melebihi 3, berarti tumpah.
        if (startX < 0 || startY < 0 || startX + food.gridWidth > GRID_WIDTH || startY + food.gridHeight > GRID_HEIGHT)
        {
            return false; // Gagal, keluar piring!
        }

        // 2. Cek Tabrakan dengan makanan lain
        // Kita scan semua kotak yang akan dimakan oleh makanan ini
        for (int x = startX; x < startX + food.gridWidth; x++)
        {
            for (int y = startY; y < startY + food.gridHeight; y++)
            {
                if (gridArray[x, y] != null) // Jika kotak ini TIDAK kosong
                {
                    return false; // Gagal, nabrak makanan lain!
                }
            }
        }

        return true; // Lulus semua ujian, tempat ini aman!
    }
    public bool TryPlaceFoodUI(DraggableFood foodUI, Vector2 dropPosition)
    {
        // 1. Ubah posisi Mouse di layar global menjadi posisi lokal di dalam kotak Piring UI
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            dropPosition,
            null,
            out localMousePos
        );

        // 2. Rumus Matematika mengubah pixel ke Index Array (0, 1, 2)
        // localMousePos.x itu positif ke kanan. localMousePos.y itu negatif ke bawah.
        int x = Mathf.FloorToInt(localMousePos.x / cellSize);
        int y = Mathf.FloorToInt(-localMousePos.y / cellSize); // Perhatikan tanda minus!

        // 3. Cek logikanya
        if (CheckCanPlace(foodUI.foodData, x, y))
        {
            PlaceFood(foodUI.foodData, x, y);
            foodsOnPlate.Add(foodUI.foodData); // Tambahkan ke List

            foodUI.transform.SetParent(this.transform);
            foodUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * cellSize, -y * cellSize);

            // Minta Manager untuk menghitung ulang STAT GIZI & MOOD!
            if (manager != null) manager.UpdateStatsUI();

            return true;
        }
        return false;
    }
    // Fungsi penting 2: Meletakkan makanan secara resmi ke dalam memori
    public void PlaceFood(FoodData food, int startX, int startY)
    {
        // Ingat, kita asumsikan CheckCanPlace sudah dipanggil sebelumnya dan hasilnya 'true'
        for (int x = startX; x < startX + food.gridWidth; x++)
        {
            for (int y = startY; y < startY + food.gridHeight; y++)
            {
                gridArray[x, y] = food; // Tandai kotak ini dengan data makanan tersebut
            }
        }

        Debug.Log($"{food.foodName} diletakkan di koordinat [{startX},{startY}]");

        // (Nanti di sini kita panggil fungsi UpdateStatsUI() dari PlatingManager)
    }

    // Fungsi tambahan: Mengambil makanan dari piring (Menghapus dari memori)
    public void RemoveFood(FoodData food)
    {
        // Scan seluruh piring, cari kotak yang diisi makanan ini, lalu kosongkan
        for (int x = 0; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                if (gridArray[x, y] == food)
                {
                    gridArray[x, y] = null; // Kosongkan
                }
            }
        }
        Debug.Log($"{food.foodName} diambil dari piring.");
    }
    public List<FoodData> GetFoodsOnPlate()
    {
        return foodsOnPlate;
    }
}