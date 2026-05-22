using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

public GameObject Panelinicio;
public GameObject Panelpartida;
public GameObject btnContinuar;
    public void BtnComenzar()
    {
        Panelinicio.SetActive(false);
        Panelpartida.SetActive(true);

        // Muestra u oculta Continuar según si hay partida guardada
        bool hayPartida = false;
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.GetInt("capitulo_" + i, 0) == 1)
            {
                hayPartida = true;
                break;
            }
        }
        if (btnContinuar != null)
            btnContinuar.SetActive(hayPartida);
    }

    public void BtnVolverInicio()
    {
        Panelpartida.SetActive(false);
        Panelinicio.SetActive(true);
    }

    // ── Selección de partida ──────────────────────────────────────────────────────

    public void BtnContinuar()
    {
        SceneManager.LoadScene("Desk");
    }

    public void BtnNuevaPartida()
    {
        GameManager.Instance.NuevaPartida();
        PlayerPrefs.DeleteKey("cinematicaVista");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Intro");
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



