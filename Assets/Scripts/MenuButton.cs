using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public GameObject activate;
    public GameObject deactivate;
    public string scene;
    public bool quit;
    public void Click()
    {
        if (quit)
            Application.Quit();
        else if (!string.IsNullOrEmpty(scene))
            SceneManager.LoadScene(scene);
        else
        {
            activate.SetActive(true);
            deactivate.SetActive(false);
        }
    }
}
