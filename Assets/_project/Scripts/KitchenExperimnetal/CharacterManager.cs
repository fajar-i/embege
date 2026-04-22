using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static List<Transform> allCharacters = new List<Transform>();

    void Awake()
    {
        if (allCharacters == null)
            allCharacters = new List<Transform>();
    }
}