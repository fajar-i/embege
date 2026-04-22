using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public KomporInteractable currentKompor;
    void Awake()
    {
        if (!CharacterManager.allCharacters.Contains(transform))
        {
            CharacterManager.allCharacters.Add(transform);
        }
    }
}