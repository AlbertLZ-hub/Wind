using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Cargar cualquier escena por nombre
    public void LoadScene(string sceneName)
    {
        Debug.Log("Cargando escena: " + sceneName);  // Mensaje debug
        SceneManager.LoadScene(sceneName);           // Cargar escena
        Time.timeScale = 1f;                         // Asegurar tiempo normal
    }

    // Cargar siguiente nivel (por índice)
    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;  // Índice escena actual
        SceneManager.LoadScene(currentSceneIndex + 1);                     // Cargar siguiente
    }

    // Reiniciar nivel actual
    public void RestartLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;  // Nombre escena actual
        Debug.Log("Reiniciando: " + currentSceneName);                 // Mensaje debug
        SceneManager.LoadScene(currentSceneName);                      // Recargar escena
        Time.timeScale = 1f;                                           // Tiempo normal
    }

    // Volver al menú principal
    public void BackToMenu()
    {
        Debug.Log("Volviendo al menú principal");  // Mensaje debug
        SceneManager.LoadScene("00_MenuPrincipal"); // Cargar menú
        Time.timeScale = 1f;                        // Tiempo normal
    }
}