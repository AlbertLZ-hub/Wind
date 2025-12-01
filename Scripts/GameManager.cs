// Importar namespaces necesarios de Unity
using UnityEngine;           // Funcionalidades básicas de Unity
using UnityEngine.UI;        // Para componentes de UI (Text)
using UnityEngine.SceneManagement;  // Para cambiar entre escenas

// Clase principal que gestiona el estado global del juego
public class GameManager : MonoBehaviour  // Hereda de MonoBehaviour para ser componente
{
    // INSTANCIA ESTÁTICA - Patrón Singleton para acceso global
    public static GameManager instance;

    // HEADER para organizar variables en el Inspector de Unity
    [Header("Referencias del HUD")]
    public Text objectsText;  // Referencia al elemento de texto que muestra objetos recogidos
    public Text timeText;     // Referencia al elemento de texto que muestra tiempo restante

    [Header("Configuración del Juego")]
    public int totalObjects = 5;    // Número total de objetos a recoger en cada nivel
    public float gameTime = 120f;   // Tiempo total en segundos para completar el nivel (2 minutos)

    [Header("Menú Pausa")]
    public GameObject pausePanel;   // Referencia al GameObject del panel de pausa UI

    // VARIABLES PÚBLICAS para acceso desde otros scripts como VictoryScreen
    public int objectsCollected = 0;  // Contador de objetos recogidos (accesible públicamente)
    public float currentTime;         // Tiempo actual restante (accesible públicamente)

    // VARIABLES PRIVADAS para estado interno del juego
    private bool gameRunning = true;  // Controla si el juego está activo
    private bool isPaused = false;    // Controla si el juego está pausado

    // AWAKE - Se ejecuta cuando el objeto se crea, antes de Start
    void Awake()
    {
        // PATRÓN SINGLETON - Garantiza que solo hay una instancia del GameManager
        if (instance == null)  // Si no existe instancia...
        {
            instance = this;                    // Esta es la primera instancia
            DontDestroyOnLoad(gameObject);      // No destruir al cambiar de escena
        }
        else  // Si ya existe una instancia...
        {
            Destroy(gameObject);  // Destruir este objeto duplicado
        }
    }

    // START - Se ejecuta en el primer frame después de Awake
    void Start()
    {
        // Inicializar variables para nuevo nivel
        ResetForNewLevel();

        // Reproducir música adecuada para la escena actual
        PlaySceneMusic();

        // Ocultar panel de pausa al inicio (si está asignado)
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    // UPDATE - Se ejecuta cada frame (60 veces por segundo aprox.)
    void Update()
    {
        // Solo actualizar lógica del juego si está corriendo y no está pausado
        if (gameRunning && !isPaused)
        {
            currentTime -= Time.deltaTime;  // Restar tiempo transcurrido desde el último frame
            UpdateHUD();                    // Actualizar interfaz de usuario

            // Verificar si se acabó el tiempo
            if (currentTime <= 0)
            {
                GameOver();  // Llamar función de fin de juego
            }
        }

        // Detectar tecla ESC para pausar/reanudar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();  // Alternar estado de pausa
        }
    }

    // Reproduce la música correspondiente a la escena actual
    void PlaySceneMusic()
    {
        // Verificar que existe AudioManager
        if (AudioManager.instance != null)
        {
            // Obtener nombre de la escena actual
            string sceneName = SceneManager.GetActiveScene().name;

            // Reproducir música según la escena
            if (sceneName == "00_MenuPrincipal")
            {
                AudioManager.instance.PlayMenuMusic();  // Música para menú principal
            }
            else if (sceneName == "01_Nivel1")
            {
                AudioManager.instance.PlayNivel1Music();  // Música para nivel 1
            }
            else if (sceneName == "02_Nivel2")
            {
                AudioManager.instance.PlayNivel2Music();  // Música para nivel 2
            }
        }
    }

    // Función llamada cuando el jugador recoge un objeto
    public void CollectObject()
    {
        // No permitir recoger objetos si el juego está pausado
        if (isPaused) return;

        objectsCollected++;  // Incrementar contador de objetos recogidos
        Debug.Log("Objeto recogido: " + objectsCollected + "/" + totalObjects);  // Mensaje debug
        UpdateHUD();  // Actualizar interfaz

        // Verificar si se recogieron todos los objetos del nivel
        if (objectsCollected >= totalObjects)
        {
            CompleteLevel();  // Llamar función de nivel completado
        }
    }

    // Gestiona la finalización exitosa de un nivel
    void CompleteLevel()
    {
        gameRunning = false;  // Detener lógica del juego
        string currentScene = SceneManager.GetActiveScene().name;  // Obtener nombre escena actual

        Debug.Log("¡Nivel completado! Escena: " + currentScene);  // Mensaje debug

        // Esperar 1 segundo y cambiar a siguiente escena según nivel actual
        if (currentScene == "01_Nivel1")
        {
            Invoke("LoadNivel2", 1f);  // Nivel1 completado → cargar Nivel2
        }
        else if (currentScene == "02_Nivel2")
        {
            Invoke("LoadVictory", 1f);  // Nivel2 completado → cargar pantalla Victoria
        }
        else
        {
            Invoke("LoadMenu", 1f);  // Cualquier otra escena → volver al menú
        }
    }

    // Carga el Nivel 2
    void LoadNivel2()
    {
        ResetForNewLevel();               // Resetear variables para nuevo nivel
        SceneManager.LoadScene("02_Nivel2");  // Cargar escena del nivel 2
    }

    // Carga la pantalla de victoria
    void LoadVictory()
    {
        SceneManager.LoadScene("03_Victoria");  // Cargar escena de victoria
    }

    // Carga el menú principal
    void LoadMenu()
    {
        SceneManager.LoadScene("00_MenuPrincipal");  // Cargar escena del menú
    }

    // Actualiza todos los elementos de la interfaz (HUD)
    void UpdateHUD()
    {
        // Actualizar texto de objetos recogidos
        if (objectsText != null)
        {
            objectsText.text = "Objetos: " + objectsCollected + "/" + totalObjects;
        }

        // Actualizar texto de tiempo restante
        if (timeText != null)
        {
            // Convertir tiempo de segundos a minutos y segundos
            int minutes = Mathf.FloorToInt(currentTime / 60);  // Parte entera de minutos
            int seconds = Mathf.FloorToInt(currentTime % 60);  // Segundos restantes
            // Formatear como "MM:SS" con dos dígitos cada uno
            timeText.text = string.Format("Tiempo: {0:00}:{1:00}", minutes, seconds);
        }
    }

    // Gestiona la finalización del juego por tiempo agotado
    void GameOver()
    {
        gameRunning = false;  // Detener juego
        Debug.Log("¡Se acabó el tiempo! Game Over");  // Mensaje debug
        SceneManager.LoadScene("00_MenuPrincipal");  // Volver al menú principal
    }

    // Alterna entre estado de pausa y juego normal
    void TogglePause()
    {
        // Verificar que existe referencia al panel de pausa
        if (pausePanel == null)
        {
            Debug.LogWarning("No hay panel de pausa asignado");  // Advertencia si falta
            return;  // Salir si no hay panel
        }

        // Cambiar estado de pausa (si estaba pausado → reanudar, si no → pausar)
        isPaused = !isPaused;

        // Mostrar u ocultar panel de pausa según estado
        pausePanel.SetActive(isPaused);

        // Controlar velocidad del juego: 0 = pausado, 1 = normal
        Time.timeScale = isPaused ? 0 : 1;

        // Controlar cursor según estado de pausa
        if (isPaused)
        {
            // En pausa: cursor libre y visible para usar menú
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Juego pausado");
        }
        else
        {
            // En juego: cursor bloqueado e invisible (para juegos 3D)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Juego reanudado");
        }
    }

    // ========== FUNCIONES PARA BOTONES DEL MENÚ DE PAUSA ==========

    // Llamada desde botón "CONTINUAR" en menú de pausa
    public void ResumeGame()
    {
        TogglePause();  // Simplemente alterna estado (reanuda)
    }

    // Llamada desde botón "MENÚ PRINCIPAL" en menú de pausa
    public void PauseToMainMenu()
    {
        Time.timeScale = 1;     // Asegurar tiempo normal
        isPaused = false;       // Salir de estado pausado
        SceneManager.LoadScene("00_MenuPrincipal");  // Cargar menú
    }

    // Llamada desde botón "SALIR" en menú de pausa
    public void PauseQuitGame()
    {
        Application.Quit();  // Cerrar aplicación (solo funciona en build)

        // Directiva de preprocesador: código solo para Editor de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Detener Play Mode en editor
#endif
    }

    // Resetea todas las variables para comenzar un nuevo nivel
    void ResetForNewLevel()
    {
        objectsCollected = 0;  // Reiniciar contador de objetos
        currentTime = gameTime; // Restablecer tiempo inicial
        gameRunning = true;    // Activar lógica del juego
        isPaused = false;      // Asegurar que no está pausado
        Time.timeScale = 1;    // Velocidad de juego normal

        Debug.Log("Juego reseteado para nuevo nivel");  // Mensaje debug
    }

    // Función pública para reiniciar juego desde menús externos
    public void ResetGame()
    {
        ResetForNewLevel();  // Reutiliza la lógica de reset
    }

    // Devuelve el tiempo actual formateado como string "MM:SS"
    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);  // Calcular minutos
        int seconds = Mathf.FloorToInt(currentTime % 60);  // Calcular segundos
        return string.Format("{0:00}:{1:00}", minutes, seconds);  // Formatear como "00:00"
    }
}