using UnityEngine;
using System.Collections.Generic;

// Struct untuk mendaftarkan suara di Inspector
[System.Serializable]
public class SoundData
{
    public string soundName;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM Source (Musik Latar)")]
    [SerializeField] private AudioSource bgmSource; // AudioSource khusus musik

    [Header("Audio Database")]
    public SoundData[] bgmList;
    public SoundData[] sfxList;

    [Header("Pitch Randomizer Settings")]
    [SerializeField] private float minPitch = 0.85f;
    [SerializeField] private float maxPitch = 1.15f;

    private void Awake()
    {
        // Pola Singleton
        if (Instance == null)
        {
            Instance = this;
            // Kita biarkan SceneLoader yang menempel di [SYSTEM_PERSISTENT] menangani DontDestroyOnLoad.
            // AudioManager cukup menempel di objek yang sama.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- FUNGSI SFX (EFEK SUARA) ---

    // Panggil ini untuk suara UI biasa: AudioManager.Instance.PlaySFX("Click", true);
    public void PlaySFX(string soundName, bool randomizePitch = false)
    {
        Debug.Log("SFX Played");
        SoundData sfx = System.Array.Find(sfxList, s => s.soundName == soundName);
        
        if (sfx == null || sfx.clip == null)
        {
            Debug.LogWarning("SFX: " + soundName + " tidak ditemukan!");
            return;
        }

        // Bikin GameObject sementara untuk memutar suara (aman untuk spam klik)
        GameObject tempAudioObj = new GameObject("TempSFX_" + soundName);
        AudioSource tempSource = tempAudioObj.AddComponent<AudioSource>();

        tempSource.clip = sfx.clip;
        tempSource.volume = sfx.volume;

        // FITUR UTAMA ANDA: Pitch Randomizer
        if (randomizePitch)
        {
            tempSource.pitch = Random.Range(minPitch, maxPitch);
        }

        tempSource.Play();

        // Hancurkan GameObject otomatis setelah suara selesai
        Destroy(tempAudioObj, sfx.clip.length + 0.1f);
    }


    // --- FUNGSI BGM (MUSIK) ---

    public void PlayBGM(string soundName)
    {
        SoundData bgm = System.Array.Find(bgmList, s => s.soundName == soundName);

        if (bgm == null || bgm.clip == null) return;

        // Jika musik yang sama sudah diputar, jangan diputar ulang dari awal
        if (bgmSource.clip == bgm.clip) return; 

        bgmSource.clip = bgm.clip;
        bgmSource.volume = bgm.volume;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}