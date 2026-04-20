using UnityEngine;
using UnityEngine.UI; // WAJIB TAMBAH INI UNTUK MENGGUNAKAN SLIDER
public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject panelUtama;
    [SerializeField] private GameObject panelSettings;
    [Header("Settings UI")]
    [SerializeField] private Slider sfxSlider; // Slot untuk slider SFX
    private void Start()
    {
        // Pastikan saat masuk scene, panel utama menyala dan settings mati
        ShowPanelUtama();
        InitSettingsUI(); // Tarik data dari save file ke UI saat mulai
    }

    // --- FUNGSI NAVIGASI PANEL ---

    public void ShowPanelUtama()
    {
        if (panelSettings.activeSelf)
        {
            SaveManager.Instance.SaveGame();
        }
        panelUtama.SetActive(true);
        panelSettings.SetActive(false);
    }

    public void ShowPanelSettings()
    {
        AudioManager.Instance.PlaySFX("UIClick", true);
        panelUtama.SetActive(false);
        panelSettings.SetActive(true);
    }

    // --- FUNGSI AKSI TOMBOL ---

    public void OnButtonPlayClicked()
    {
        // Memanggil SceneLoader yang kita buat sebelumnya!
        // Pastikan Enum SceneList.Gameplay sudah ada
        AudioManager.Instance.PlaySFX("UIClick", true);
        SceneLoader.Instance.LoadScene(SceneList.Gameplay);
    }

    public void OnButtonQuitClicked()
    {
        Debug.Log("Quit Game Terpanggil! (Hanya terlihat di build asli, bukan di Editor)");

        // Keluar dari aplikasi
        Application.Quit();

        // Opsional: Untuk menghentikan play mode saat di dalam Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // --- FUNGSI SETTINGS ---
    private void InitSettingsUI()
    {
        if (sfxSlider != null)
        {
            // Set posisi knob slider sesuai dengan data yang di-load
            sfxSlider.value = SaveManager.Instance.currentData.sfxVolume;

            // Daftarkan event: Setiap kali slider digeser, jalankan fungsi UpdateSFXVolume
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        }
    }
    private void UpdateSFXVolume(float newValue)
    {
        // Update data di memori
        SaveManager.Instance.currentData.sfxVolume = newValue;
    }
}