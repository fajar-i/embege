using UnityEngine;
using UnityEngine.InputSystem;

public class TestRouter : MonoBehaviour
{
    void Update()
    {
        // Tekan 1 untuk ke Main Menu
        if (Keyboard.current.digit1Key.wasPressedThisFrame) 
            SceneLoader.Instance.LoadScene(SceneList.MainMenu);
            
        // Tekan 2 untuk ke Gameplay
        if (Keyboard.current.digit2Key.wasPressedThisFrame) 
            SceneLoader.Instance.LoadScene(SceneList.Gameplay);
    }
}