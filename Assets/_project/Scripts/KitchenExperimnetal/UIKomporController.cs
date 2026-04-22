using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIKomporController : MonoBehaviour
{
    public static UIKomporController instance;

    public Transform characterContainer;
    public Transform restPosition;

    public GameObject panelUI;

    public TextMeshProUGUI judulText;
    public TextMeshProUGUI deskripsiText;
    public Image gambar;

    public GameObject characterAssignPanel;
    public GameObject characterListPanel;

    public CharacterListUI characterListUI;

    private KomporInteractable currentKompor;

    void Awake()
    {
        instance = this;
    }

    public void TampilkanData(string nama, string deskripsi, Sprite img, KomporInteractable kompor)
    {
        panelUI.SetActive(true);

        judulText.text = nama;
        deskripsiText.text = deskripsi;
        gambar.sprite = img;

        currentKompor = kompor;

        characterAssignPanel.SetActive(true);
        characterListPanel.SetActive(false);
    }

    public void BukaCharacterList()
    {
        characterAssignPanel.SetActive(false);
        characterListPanel.SetActive(true);

        characterListUI.RefreshList();
    }

    public void TutupCharacterList()
    {
        characterListPanel.SetActive(false);
        characterAssignPanel.SetActive(true);
    }

    public void TutupUI()
    {
        panelUI.SetActive(false);
    }

    public KomporInteractable GetCurrentKompor()
    {
        return currentKompor;
    }
}