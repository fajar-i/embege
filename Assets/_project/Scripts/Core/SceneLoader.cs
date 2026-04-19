// File: SceneLoader.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image progressBar; // Pastikan Image Type di Unity diset ke "Filled"

    private void Awake()
    {
        // Pola Singleton & Persistent
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Objek ini tidak akan hancur saat pindah scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Sembunyikan loading screen saat game baru mulai
        if (loadingScreen != null) loadingScreen.SetActive(false);
        LoadScene(SceneList.MainMenu);
    }

    // Panggil fungsi ini dari skrip manapun: SceneLoader.Instance.LoadScene(SceneList.MainMenu);
    public void LoadScene(SceneList sceneToLoad)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneToLoad.ToString()));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        // 1. Tampilkan Layar Loading
        if (loadingScreen != null) loadingScreen.SetActive(true);
        if (progressBar != null) progressBar.fillAmount = 0f;

        // Beri jeda sedikit agar UI sempat render (mencegah kedip jika loading terlalu cepat)
        yield return new WaitForSeconds(0.2f); 

        // 2. Mulai proses loading di background
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Mencegah scene langsung aktif sebelum loading selesai 100%
        operation.allowSceneActivation = false;

        // 3. Update Progress Bar
        while (!operation.isDone)
        {
            // Unity loading progress berhenti di 0.9. Kita normalize agar bar bisa penuh 100%
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (progressBar != null) progressBar.fillAmount = progress;

            // Jika loading sudah mencapai 90% (tahap akhir unity)
            if (operation.progress >= 0.9f)
            {
                // Opsional: Anda bisa tambahkan "Press any key to continue" di sini nanti
                
                // Izinkan scene berpindah
                operation.allowSceneActivation = true;
            }

            yield return null; // Tunggu frame berikutnya
        }

        // 4. Sembunyikan Layar Loading setelah scene baru terbuka
        if (loadingScreen != null) loadingScreen.SetActive(false);
    }
}