using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    public TextMeshProUGUI txtDato;

    private string[] datos = {
        "¿Sabías que los seres humanos pasan aproximadamente 6 años de su vida soñando?",
        "¿Sabías que el 95% de los sueños se olvidan a los 5 minutos de despertar?",
        "¿Sabías que las personas ciegas de nacimiento sueñan con sonidos, olores y emociones en lugar de imágenes?",
        "¿Sabías que es imposible soñar con una cara que nunca hayas visto? Tu mente combina rostros reales.",
        "¿Sabías que los animales también sueñan? Los perros y gatos muestran movimientos oculares rápidos igual que los humanos.",
        "¿Sabías que soñar en blanco y negro era común antes de la televisión a color? El 12% de las personas aún sueña así.",
        "¿Sabías que durante el sueño REM tu cuerpo queda paralizado para que no actúes físicamente lo que sueñas?",
        "¿Sabías que los sueños lúcidos — donde sabes que estás soñando — pueden entrenarse con práctica?",
        "¿Sabías que las pesadillas son más comunes en niños entre 3 y 6 años que en adultos?",
        "¿Sabías que el cerebro durante el sueño es tan activo como cuando estás despierto?",
        "¿Sabías que algunas personas sufren parálisis del sueño — despiertan pero no pueden moverse durante varios minutos?",
        "¿Sabías que los sueños negativos superan a los positivos? El miedo es la emoción más común en los sueños.",
        "¿Sabías que puedes tener entre 4 y 7 sueños por noche aunque no los recuerdes?",
        "¿Sabías que los sonámbulos pueden caminar, cocinar e incluso conducir sin despertar ni recordarlo?",
        "¿Sabías que el déjà vu puede estar relacionado con fragmentos de sueños que el cerebro no archivó correctamente?"
    };

    void Start()
    {
        txtDato.text = datos[Random.Range(0, datos.Length)];

        string escena = PlayerPrefs.GetString("escenaPendiente", "");
        if (!string.IsNullOrEmpty(escena))
            StartCoroutine(CargarEscena(escena));
    }

    IEnumerator CargarEscena(string nombreEscena)
    {
        yield return null;

        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombreEscena);
        operacion.allowSceneActivation = false;

  
        float tiempoMinimo = 4f;
        float tiempoTranscurrido = 0f;

        while (!operacion.isDone)
        {
            tiempoTranscurrido += Time.deltaTime;

            // Solo pasa cuando la escena está lista Y pasaron los 2 segundos
            if (operacion.progress >= 0.9f && tiempoTranscurrido >= tiempoMinimo)
            {
                operacion.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
