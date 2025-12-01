// Importar namespaces necesarios de Unity
using UnityEngine;           // Funcionalidades básicas de Unity
using UnityEngine.UI;        // Para componentes de UI (Text, Button)
using UnityEngine.SceneManagement;  // Para cambiar entre escenas

// Clase que maneja la pantalla de victoria del juego
public class VictoryScreen : MonoBehaviour  // Hereda de MonoBehaviour para ser componente
{
    // HEADER organiza variables en el Inspector de Unity
    [Header("Referencias UI")]
    public Text textoVictoria;      // Referencia al texto que muestra "¡VICTORIA!"
    public Text textoObjetos;       // Referencia al texto que muestra cantidad de objetos recogidos
    public Text textoTiempo;        // Referencia al texto que muestra tiempo restante

    [Header("Botones")]
    public Button botonJugarNuevo;  // Referencia al botón "JUGAR DE NUEVO"
    public Button botonMenu;        // Referencia al botón "MENÚ PRINCIPAL"

    // START - Se ejecuta cuando la escena de victoria se carga
    void Start()
    {
        // Configurar cursor para menú (en juegos 3D suele estar bloqueado)
        Cursor.lockState = CursorLockMode.None;  // Desbloquear cursor
        Cursor.visible = true;                   // Hacer cursor visible

        // Llamar función para mostrar información del juego completado
        MostrarInformacion();

        // Configurar funcionalidad de botones programáticamente
        ConfigurarBotones();

        // Mensaje de depuración en consola
        Debug.Log("Pantalla de victoria cargada correctamente");
    }

    // Función que muestra la información del juego completado
    void MostrarInformacion()
    {
        // Verificar si existe el GameManager (viene del nivel anterior)
        if (GameManager.instance != null)
        {
            // Mostrar cantidad de objetos recogidos si existe la referencia
            if (textoObjetos != null)
            {
                // Acceder a la variable pública objectsCollected del GameManager
                textoObjetos.text = "Objetos recogidos: " + GameManager.instance.objectsCollected;
            }

            // Mostrar tiempo restante (en este caso mensaje genérico)
            if (textoTiempo != null)
            {
                // NOTA: currentTime es privado en GameManager en este diseño
                // En una versión mejorada se podría acceder con un método público
                textoTiempo.text = "¡Nivel completado con éxito!";
            }

            // Mostrar texto de victoria
            if (textoVictoria != null)
            {
                textoVictoria.text = "¡VICTORIA!";
            }
        }
        else
        {
            // Caso de fallback: si no hay GameManager (no debería pasar normalmente)
            Debug.LogWarning("GameManager no encontrado en pantalla victoria");

            // Mostrar valores por defecto
            if (textoObjetos != null)
                textoObjetos.text = "Objetos recogidos: 5";  // Valor ejemplo

            if (textoTiempo != null)
                textoTiempo.text = "Tiempo: --:--";          // Tiempo desconocido

            if (textoVictoria != null)
                textoVictoria.text = "¡VICTORIA!";           // Texto estático
        }
    }

    // Configura los listeners (oyentes) de los botones programáticamente
    void ConfigurarBotones()
    {
        // Configurar botón JUGAR DE NUEVO
        if (botonJugarNuevo != null)
        {
            // Eliminar listeners anteriores (por si ya tenía alguno)
            botonJugarNuevo.onClick.RemoveAllListeners();
            // Añadir nuevo listener que llama a JugarDeNuevo cuando se hace click
            botonJugarNuevo.onClick.AddListener(JugarDeNuevo);
        }

        // Configurar botón MENÚ PRINCIPAL
        if (botonMenu != null)
        {
            botonMenu.onClick.RemoveAllListeners();
            botonMenu.onClick.AddListener(IrAlMenu);
        }
    }

    // ========== FUNCIONES PARA BOTONES ==========

    // Se ejecuta cuando se presiona el botón "JUGAR DE NUEVO"
    public void JugarDeNuevo()
    {
        Debug.Log("Iniciando nuevo juego...");

        // Si existe el GameManager, resetear sus variables
        if (GameManager.instance != null)
        {
            GameManager.instance.ResetGame();  // Llamar función de reset
        }

        // Cargar la escena del primer nivel (reiniciar juego)
        SceneManager.LoadScene("01_Nivel1");
    }

    // Se ejecuta cuando se presiona el botón "MENÚ PRINCIPAL"
    public void IrAlMenu()
    {
        Debug.Log("Yendo al menú principal...");

        // Asegurar que el tiempo de juego está normalizado (por si venía de pausa)
        Time.timeScale = 1;

        // Cargar la escena del menú principal
        SceneManager.LoadScene("00_MenuPrincipal");
    }

    // Función opcional para botón SALIR (no implementado en UI pero disponible)
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        // Cerrar la aplicación (solo funciona en build, no en editor)
        Application.Quit();

        // Directiva de preprocesador: solo se compila en el Editor de Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Detener Play Mode en editor
#endif
    }
}