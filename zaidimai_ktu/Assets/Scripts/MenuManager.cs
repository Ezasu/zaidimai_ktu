using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown Dropdown;

    [Range(0f, 10f)]
    public float strenght = 1f;

    public void StartGame()
    {
        if (Dropdown == null)
            SceneManager.LoadScene("MainMenu");
        if ((Dropdown.options[Dropdown.value].text) == "KAROLIS LEVEL")
            SceneManager.LoadScene("KarolisScene");
        if ((Dropdown.options[Dropdown.value].text) == "KAJUS LEVEL")
            SceneManager.LoadScene("KajusScene");
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}