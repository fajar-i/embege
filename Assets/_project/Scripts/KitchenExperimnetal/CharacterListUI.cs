using UnityEngine;

public class CharacterListUI : MonoBehaviour
{
    public Transform content;
    public GameObject itemPrefab;
    public Transform characterContainer;

    public void RefreshList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        GenerateCharacterList();
    }

    void GenerateCharacterList()
    {
        foreach (Transform character in CharacterManager.allCharacters)
        {
            GameObject item = Instantiate(itemPrefab, content);

            CharacterItemUI itemUI = item.GetComponent<CharacterItemUI>();

            if (itemUI != null)
            {
                itemUI.SetCharacter(character.name, character);
            }
        }
    }
}