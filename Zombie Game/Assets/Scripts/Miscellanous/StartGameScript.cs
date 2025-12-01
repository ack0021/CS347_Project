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
    }
}
