using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void CargarEscena(string nombreEscena)
    {
        // Guarda qué escena cargar
        PlayerPrefs.SetString("escenaPendiente", nombreEscena);
        PlayerPrefs.Save();

        // Carga la pantalla de carga primero
        SceneManager.LoadScene("LoadingScene");
    }
}