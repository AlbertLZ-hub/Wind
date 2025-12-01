// Importar namespaces necesarios de Unity
using UnityEngine;      // Funcionalidades básicas de Unity
using UnityEngine.UI;   // Para componentes de UI (Toggle, Dropdown, etc.)

// Clase que gestiona el menú de opciones del juego
public class OptionsMenu : MonoBehaviour  // Hereda de MonoBehaviour para ser componente
{
    // Referencias públicas que se asignan en el Inspector de Unity
    public GameObject panelOpciones;  // Referencia al panel que contiene todas las opciones
    public Toggle toggleMusica;       // Referencia al toggle para activar/desactivar música
    public Dropdown dropdownNivel;    // Referencia al dropdown para seleccionar nivel

    // Variable privada para guardar el estado de la música internamente
    private bool musicaActivada = true;  // Por defecto, música está activada

    // START - Se ejecuta cuando el objeto se activa en la escena
    void Start()
    {
        // Buscar automáticamente el panel si no se asignó en el Inspector
        if (panelOpciones == null)
        {
            // GameObject.Find busca un objeto por nombre en la escena actual
            panelOpciones = GameObject.Find("PanelOpciones");
        }

        // Ocultar el panel de opciones al inicio del juego
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);  // SetActive(false) hace invisible e inactivo el GameObject
        }

        // Configurar el toggle de música con valores por defecto
        if (toggleMusica != null)
        {
            // Por defecto, el toggle está marcado (música activada)
            toggleMusica.isOn = true;    // Marcar visualmente el toggle
            musicaActivada = true;       // Guardar estado interno

            // NOTA: No accedemos directamente a AudioManager.musicSource porque es privado
            // En una implementación completa, se leería el estado actual del AudioManager
        }
    }

    // ========== FUNCIONES PARA CONTROL DEL PANEL ==========

    // Alterna entre mostrar y ocultar el panel de opciones
    public void MostrarOcultarOpciones()
    {
        if (panelOpciones != null)
        {
            // Obtener estado actual del panel (true = visible, false = oculto)
            bool estadoActual = panelOpciones.activeSelf;

            // Cambiar al estado opuesto (mostrar si estaba oculto, ocultar si estaba visible)
            panelOpciones.SetActive(!estadoActual);

            // Mensaje de depuración con estado actual
            Debug.Log("Panel opciones: " + (!estadoActual ? "mostrado" : "ocultado"));
        }
    }

    // ========== FUNCIONES PARA OPCIONES DE CONFIGURACIÓN ==========

    // Se llama cuando cambia el estado del toggle de música
    // Parámetro musicaOn: true = música activada, false = música desactivada
    public void CambiarMusica(bool musicaOn)
    {
        // Verificar que existe el AudioManager en la escena
        if (AudioManager.instance != null)
        {
            if (musicaOn)
            {
                // Activar música: establecer volumen a 30% (0.3f)
                AudioManager.instance.SetMusicVolume(0.3f);
                Debug.Log("Música ACTIVADA");
                musicaActivada = true;  // Actualizar estado interno
            }
            else
            {
                // Desactivar música: establecer volumen a 0% (0f = silencio)
                AudioManager.instance.SetMusicVolume(0f);
                Debug.Log("Música DESACTIVADA");
                musicaActivada = false;  // Actualizar estado interno
            }
        }
        else
        {
            // Caso de fallback: si no hay AudioManager, solo guardar preferencia
            musicaActivada = musicaOn;
            Debug.Log("AudioManager no encontrado, guardando preferencia: " + musicaOn);
        }
    }

    // Se llama cuando se selecciona un nivel en el dropdown
    // Parámetro indice: 0 = Nivel 1, 1 = Nivel 2
    public void SeleccionarNivel(int indice)
    {
        // Mostrar mensaje de depuración con la selección
        Debug.Log("Nivel seleccionado: " + (indice == 0 ? "Nivel 1" : "Nivel 2"));

        // Guardar la selección usando PlayerPrefs (sistema de preferencias de Unity)
        // PlayerPrefs guarda datos simple entre sesiones del juego
        PlayerPrefs.SetInt("NivelSeleccionado", indice);  // Guardar valor
        PlayerPrefs.Save();  // Asegurar que se escribe en disco (importante!)

        // NOTA: En este proyecto no se usa esta selección para cargar niveles directamente
        // Podría usarse para: cargar nivel específico, recordar último nivel jugado, etc.
    }

    // ========== FUNCIÓN PARA BOTÓN VOLVER ==========

    // Se llama cuando se presiona el botón "VOLVER" en el panel de opciones
    public void Volver()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);  // Ocultar panel
            Debug.Log("Volviendo al menú principal");  // Mensaje debug
        }
    }
}