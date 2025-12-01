// Importar namespaces necesarios de Unity
using UnityEngine;      // Funcionalidades básicas de Unity
using UnityEngine.UI;   // Para componentes de UI (Text, Button, etc.)

// Clase que gestiona la Interfaz de Usuario (HUD) durante el juego
public class HUDManager : MonoBehaviour  // Hereda de MonoBehaviour para ser componente
{
    // HEADER para organizar variables en el Inspector de Unity
    [Header("Elementos HUD")]
    public Text objectsText;          // Referencia al elemento de texto que muestra objetos recogidos
    public Text timeText;             // Referencia al elemento de texto que muestra tiempo restante
    public Text healthText;           // Referencia opcional para mostrar vida del jugador (no implementado)
    public GameObject pausePanel;     // Referencia al panel UI del menú de pausa

    // UPDATE - Se ejecuta una vez por frame (60 veces por segundo aprox.)
    void Update()
    {
        UpdateHUD();          // Llamar a función que actualiza la información en pantalla
        CheckPauseInput();    // Llamar a función que verifica si se presionó ESC para pausa
    }

    // Función que actualiza todos los elementos visibles del HUD
    void UpdateHUD()
    {
        // Actualizar texto de objetos recogidos si existe GameManager y el texto está asignado
        if (GameManager.instance != null && objectsText != null)
        {
            // Usa interpolación de strings ($"texto {variable}") para formatear el texto
            objectsText.text = $"Objetos: {GetObjectsText()}";
        }

        // Actualizar texto de tiempo restante si está asignado
        if (timeText != null)
        {
            timeText.text = $"Tiempo: {GetTimeText()}";
        }

        // NOTA: healthText no se actualiza aquí ya que no está implementado el sistema de vida
    }

    // Función que obtiene el texto formateado para los objetos recogidos
    string GetObjectsText()
    {
        // EN ESTA VERSIÓN: Devuelve un valor estático (placeholder)
        // EN UNA VERSIÓN COMPLETA: Debería acceder a GameManager.instance.objectsCollected
        return "0/5";  // Texto temporal - debería ser reemplazado por datos reales
    }

    // Función que obtiene el texto formateado para el tiempo restante
    string GetTimeText()
    {
        // EN ESTA VERSIÓN: Devuelve un valor estático (placeholder)  
        // EN UNA VERSIÓN COMPLETA: Debería acceder a GameManager.instance.currentTime
        return "02:00";  // Texto temporal - debería ser reemplazado por datos reales
    }

    // Verifica si se presionó la tecla ESC para activar/desactivar pausa
    void CheckPauseInput()
    {
        // Input.GetKeyDown detecta cuando se presiona la tecla (solo en el frame que se presiona)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();  // Llamar a función que alterna estado de pausa
        }
    }

    // Alterna entre estado de pausa y juego normal
    void TogglePause()
    {
        // Verificar que existe referencia al panel de pausa
        if (pausePanel != null)
        {
            // Determinar nuevo estado: si está activo → desactivar, si está inactivo → activar
            bool isPaused = !pausePanel.activeSelf;

            // Activar/desactivar visualmente el panel
            pausePanel.SetActive(isPaused);

            // Time.timeScale controla la velocidad del juego:
            // 0 = juego pausado, 1 = velocidad normal, valores entre 0 y 1 = cámara lenta
            Time.timeScale = isPaused ? 0 : 1;

            // Controlar visibilidad y bloqueo del cursor
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;  // Cursor libre para usar menú
                Cursor.visible = true;                   // Cursor visible
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;  // Cursor bloqueado en centro (para juegos 3D)
                Cursor.visible = false;                    // Cursor invisible
            }
        }
    }

    // ========== FUNCIONES PARA BOTONES DEL MENÚ DE PAUSA ==========

    // Se llama cuando se presiona el botón "CONTINUAR" en el menú de pausa
    public void ResumeGame()
    {
        // Verificar que existe panel de pausa
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);       // Ocultar panel
            Time.timeScale = 1;                // Reanudar tiempo del juego
            Cursor.lockState = CursorLockMode.Locked;  // Bloquear cursor para juego
            Cursor.visible = false;                    // Ocultar cursor
        }
    }

    // Se llama cuando se presiona el botón "MENÚ PRINCIPAL" en el menú de pausa
    public void GoToMainMenu()
    {
        Time.timeScale = 1;  // Asegurar que el tiempo está normalizado

        // Cargar escena del menú principal
        // Se usa el nombre completo del namespace porque hay conflicto con SceneManager propio
        UnityEngine.SceneManagement.SceneManager.LoadScene("00_MenuPrincipal");
    }
}