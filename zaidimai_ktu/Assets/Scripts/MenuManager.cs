using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject Dropdown;

    [Range(0f, 10f)]
    public float strenght = 1f;

    public void StartGame ()
    {
        Debug.Log("Start");
        SceneManager.LoadScene("KarolisScene");
    }

    public void QuitGame()
    {
        Debug.Log(Dropdown.options[Dropdown.value].text);
        Debug.Log("Exit");
        Application.Quit();
    }
}
