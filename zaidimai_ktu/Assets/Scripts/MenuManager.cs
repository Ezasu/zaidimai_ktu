using System.Diagnostics;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown Dropdown;

    public Button setOver;

    bool gameEnded = false;

    public void StartGame()
    {
        if (Dropdown == null)
            SceneManager.LoadScene("MainMenu");
        else
        {
            if ((Dropdown.options[Dropdown.value].text) == "KAROLIS LEVEL")
                SceneManager.LoadScene("KarolisScene");
            if ((Dropdown.options[Dropdown.value].text) == "KAJUS LEVEL")
                SceneManager.LoadScene("KajusScene");
        }
    }

    public void GameOver()
    {
        if (!gameEnded)
        {
            UnityEngine.Debug.Log("Death");
            gameEnded = true;
            setOver.onClick.Invoke();
            //otherObject.GetComponent<GrayScale>().enabled = false;
            //Invoke("RestartGame", 2f);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}