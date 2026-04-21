using System; // Wajib untuk menggunakan Action

// Kelas statis tidak bisa ditaruh di GameObject, ia hidup di memori secara global
public static class GameEvents
{
    // -----------------------------------------------------
    // DAFTAR FREKUENSI RADIO (EVENTS)
    // -----------------------------------------------------

    // Event sederhana (tanpa data)
    public static Action OnDayEnded; 

    // Event dengan pengiriman data (mengirimkan tipe INT)
    // Contoh: Siaran "Uang Berubah" akan mengirimkan angka uang terbarunya
    public static Action<int> OnMoneyChanged; 

    // Event dengan banyak data (misal mengirim string nama makanan dan bool berhasil)
    public static Action<string, bool> OnFoodPlated; 

}