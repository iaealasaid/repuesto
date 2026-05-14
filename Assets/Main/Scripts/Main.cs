using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public void BtnPlay()
    {
        SceneManager.LoadScene("Desk");
    }

    public void BtnExit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void BtnMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void BtnCam()
    {
        SceneManager.LoadScene("Camera");
    }
    public void BtnInicio()
    {
        SceneManager.LoadScene("Start");
    }
}

