using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public Toggle tSmall;
    public Toggle tMedium;
    public Toggle tLarge;
    public Toggle tExtraLarge;

    public int WorldSize = 0;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleSmall()
    {
        WorldSize = 0;

    }

    public void ToggleMedium()
    {
        WorldSize = 1;

    }

    public void ToggleLarge()
    {
        WorldSize = 2;

    }
    public void ToggleExtraLarge()
    {
        WorldSize = 3;

    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
