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
        Debug.Log("Start");
        if ((Dropdown.options[Dropdown.value].text) == "KAROLIS LEVEL")
            SceneManager.LoadScene("KarolisScene");
        if ((Dropdown.options[Dropdown.value].text) == "KAJUS LEVEL")
            SceneManager.LoadScene("KajusScene");
    }

    public void QuitGame()
    {
        Debug.Log(Dropdown.options[Dropdown.value].text);
        Debug.Log("Exit");
        Application.Quit();
    }
}