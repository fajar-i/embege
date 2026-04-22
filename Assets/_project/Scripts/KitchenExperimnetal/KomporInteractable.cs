using UnityEngine;

public class KomporInteractable : MonoBehaviour
{
    public string namaKompor;
    [TextArea] public string deskripsi;
    public Sprite gambar;

    public Transform objPosition;

    [HideInInspector]
    public Transform currentCharacter;

    public bool HasCharacter()
    {
        return currentCharacter != null;
    }

    private void OnMouseDown()
    {
        Debug.Log("Kompor diklik: " + namaKompor);

        if (UIKomporController.instance == null)
        {
            Debug.LogError("UIKomporController instance NULL!");
            return;
        }

        UIKomporController.instance.TampilkanData(namaKompor, deskripsi, gambar, this);
    }
}