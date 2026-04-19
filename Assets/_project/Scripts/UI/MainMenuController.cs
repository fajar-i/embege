using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject panelUtama;
    [SerializeField] private GameObject panelSettings;

    private void Start()
    {
        // Pastikan saat masuk scene, panel utama menyala dan settings mati
        ShowPanelUtama();
    }

    // --- FUNGSI NAVIGASI PANEL ---

    public void ShowPanelUtama()
    {
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
}