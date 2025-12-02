using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    // Start is called before the first frame update
  

  public void OnStartPress()
    {
        SceneManager.LoadScene("Scene_0");
    }

    public void OnNextPress()
    {
        SceneManager.LoadScene("Parker's - SampleScene");

        var mm = FindObjectOfType<MenuMusic>();
        if (mm != null)
        {
            Destroy(mm.gameObject);  // Destroy main menu music object
        }
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
