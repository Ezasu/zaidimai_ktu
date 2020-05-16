using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Dropdown Dropdown;

    [Range(0f, 10f)]
    public float strenght = 1f;

    public void StartGame()
    {
        Debug.Log("Start");
        if ((Dropdown.options[Dropdown.value].text) == "Karolis Level")
            SceneManager.LoadScene("KarolisScene");
        if ((Dropdown.options[Dropdown.value].text) == "Kajus Level")
            SceneManager.LoadScene("KajusScene");
    }

    public void QuitGame()
    {
        Debug.Log(Dropdown.options[Dropdown.value].text);
        Debug.Log("Exit");
        Application.Quit();
    }
}