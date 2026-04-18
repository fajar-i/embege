using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // WAJIB TAMBAHKAN INI

public class BuildUIManager : MonoBehaviour
{
    [Header("References")]
    public Transform menuContainer; 
    public GameObject buttonPrefab; 

    [Header("Database")]
    public List<KitchenUtilityData> availableUtilities;

    private void Start()
    {
        GenerateMenu();
    }

    private void GenerateMenu()
    {
        foreach (var utilityData in availableUtilities)
        {
            // Cek jika ada slot kosong di List Inspector untuk mencegah error lain
            if (utilityData == null) continue; 

            GameObject btnObj = Instantiate(buttonPrefab, menuContainer);
            
            // --- BAGIAN YANG DIUBAH ---
            // Cari komponen TextMeshPro
            TMP_Text buttonText = btnObj.GetComponentInChildren<TMP_Text>();
            
            if (buttonText != null) {
                buttonText.text = utilityData.utilityName;
            } else {
                Debug.LogError("Tidak ada komponen TextMeshPro di dalam prefab tombol Anda!");
            }
            // --------------------------

            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() => 
            {
                BuildManager.Instance.SelectUtility(utilityData);
            });
        }
    }
}