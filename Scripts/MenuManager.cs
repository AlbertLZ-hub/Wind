using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Referencias de Botones")]
    public Button jugarButton;      // Botón JUGAR
    public Button opcionesButton;   // Botón OPCIONES  
    public Button salirButton;      // Botón SALIR

    // Start se llama antes del primer frame
    void Start()
    {
        // Configurar listeners para los botones
        if (jugarButton != null)
            jugarButton.onClick.AddListener(Jugar);        // Al hacer click en JUGAR, llama a función Jugar

        if (opcionesButton != null)
            opcionesButton.onClick.AddListener(Opciones);  // Al hacer click en OPCIONES, llama a función Opciones

        if (salirButton != null)
            salirButton.onClick.AddListener(Salir);        // Al hacer click en SALIR, llama a función Salir

        // Asegurar que el tiempo corre normal (por si venimos de pausa)
        Time.timeScale = 1f;

        // Mostrar cursor en menús
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Función para botón JUGAR
    public void Jugar()
    {
        Debug.Log("Cargando Nivel 1...");      // Mensaje en consola para debug
        SceneManager.LoadScene("01_Nivel1");   // Cargar escena del Nivel 1
    }

    // Función para botón OPCIONES  
    public void Opciones()
    {
        Debug.Log("Abriendo opciones...");     // Mensaje en consola
        // Aquí podrías cargar escena de opciones o mostrar panel
    }

    // Función para botón SALIR
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");    // Mensaje en consola
        Application.Quit();                    // Cerrar aplicación

        // Para testing en Editor de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Detener play mode en editor
#endif
    }
}
