using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterItemUI : MonoBehaviour
{
    public TextMeshProUGUI namaText;
    public Button addRemoveBtn;

    private TextMeshProUGUI buttonText;
    private Transform characterTransform;

    void Awake()
    {
        if (addRemoveBtn == null)
            addRemoveBtn = GetComponentInChildren<Button>();

        if (addRemoveBtn != null)
            buttonText = addRemoveBtn.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetCharacter(string nama, Transform character)
    {
        namaText.text = nama;
        characterTransform = character;

        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>();

        CharacterData data = character.GetComponent<CharacterData>();

        if (data == null)
            data = character.gameObject.AddComponent<CharacterData>();

        KomporInteractable currentKompor = UIKomporController.instance.GetCurrentKompor();

        if (data.currentKompor == currentKompor)
        {
            buttonText.text = "Unassign";
        }
        else if (data.currentKompor != null)
        {
            buttonText.text = "Assigned";
        }
        else
        {
            buttonText.text = "Assign";
        }

        addRemoveBtn.onClick.RemoveAllListeners();
        addRemoveBtn.onClick.AddListener(OnClickAssign);
    }

    void OnClickAssign()
    {
        KomporInteractable targetKompor = UIKomporController.instance.GetCurrentKompor();
        if (targetKompor == null || characterTransform == null) return;

        Transform rest = UIKomporController.instance.restPosition;

        CharacterData data = characterTransform.GetComponent<CharacterData>();

        if (data == null)
        {
            data = characterTransform.gameObject.AddComponent<CharacterData>();
        }

        KomporInteractable asalKompor = data.currentKompor;

        if (data.currentKompor == targetKompor)
        {
            ReturnToRest(characterTransform, rest);

            targetKompor.currentCharacter = null;
            data.currentKompor = null;

            UIKomporController.instance.characterListUI.RefreshList();
            return;
        }

        // 🔥 SWAP
        if (asalKompor != null && targetKompor.currentCharacter != null)
        {
            Transform charTarget = targetKompor.currentCharacter;

            SetCharacterToKompor(characterTransform, targetKompor);
            SetCharacterToKompor(charTarget, asalKompor);
        }
        else
        {
            // 🔥 pindah dari kompor lama
            if (asalKompor != null)
            {
                asalKompor.currentCharacter = null;
            }

            // 🔥 replace di kompor baru
            if (targetKompor.currentCharacter != null)
            {
                Transform oldChar = targetKompor.currentCharacter;
                CharacterData oldData = oldChar.GetComponent<CharacterData>();

                ReturnToRest(oldChar, rest);

                if (oldData != null)
                    oldData.currentKompor = null;
            }

            // 🔥 assign baru
            SetCharacterToKompor(characterTransform, targetKompor);
        }

        // 🔥 UPDATE UI
        UIKomporController.instance.characterListUI.RefreshList();
    }

    void ReturnToRest(Transform character, Transform rest)
    {
        CharacterData data = character.GetComponent<CharacterData>();

        character.SetParent(rest);

        Vector3 pos = character.position;
        pos.x = rest.position.x;
        pos.z = rest.position.z;

        character.position = pos;
        character.rotation = rest.rotation;

        if (data != null)
            data.currentKompor = null;
    }

    void SetCharacterToKompor(Transform character, KomporInteractable kompor)
    {
        CharacterData data = character.GetComponent<CharacterData>();

        kompor.currentCharacter = character;

        if (data != null)
            data.currentKompor = kompor;

        Transform target = kompor.objPosition;

        Vector3 pos = character.position;
        pos.x = target.position.x;
        pos.z = target.position.z;

        character.position = pos;
        character.rotation = target.rotation;

        character.SetParent(target);
    }
}