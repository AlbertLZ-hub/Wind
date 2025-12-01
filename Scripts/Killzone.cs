using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{
    // Se ejecuta cuando otro collider entra en el trigger
    void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entró es el jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Jugador cayó en killzone!");  // Mensaje debug
            ReiniciarNivel();                         // Llamar función reinicio
        }
    }

    // Función para reiniciar el nivel
    void ReiniciarNivel()
    {
        // Obtener nombre de la escena actual
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Reiniciando nivel: " + currentSceneName);  // Mensaje debug
        SceneManager.LoadScene(currentSceneName);             // Recargar escena
    }
}