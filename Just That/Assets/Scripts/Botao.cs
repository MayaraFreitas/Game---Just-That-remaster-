using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botao : MonoBehaviour
{

    public void ChangeScene(string scene)
    {
        Debug.Log("SCENE: " + scene);
        SceneManager.LoadScene("Cena Principal");
        //Application.LoadLevel(scene);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
