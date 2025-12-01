using UnityEngine;  // Importa el namespace básico de Unity

public class AudioManager : MonoBehaviour  // Define la clase AudioManager que hereda de MonoBehaviour
{
    // INSTANCIA ESTÁTICA - Permite acceso global al AudioManager desde cualquier script
    public static AudioManager instance;

    // HEADER para organizar variables en el Inspector de Unity
    [Header("Música de Fondo")]
    public AudioClip menuMusic;      // Clip de audio para música del menú principal
    public AudioClip nivel1Music;    // Clip de audio para música del nivel 1
    public AudioClip nivel2Music;    // Clip de audio para música del nivel 2

    [Header("Efectos de Sonido")]
    public AudioClip collectSound;   // Sonido al recoger objetos
    public AudioClip doorSound;      // Sonido al abrir puertas
    public AudioClip jumpSound;      // Sonido al saltar (opcional)

    // FUENTES DE AUDIO PRIVADAS - Componentes que reproducen el sonido
    private AudioSource musicSource; // Fuente para música de fondo (se repite en bucle)
    private AudioSource sfxSource;   // Fuente para efectos de sonido (one-shot)

    // AWAKE - Se ejecuta antes de Start, cuando el objeto se crea
    void Awake()
    {
        // PATRÓN SINGLETON - Garantiza que solo hay una instancia del AudioManager
        if (instance == null)  // Si no existe instancia...
        {
            instance = this;                    // Esta es la primera instancia
            DontDestroyOnLoad(gameObject);      // No destruir al cambiar de escena
        }
        else  // Si ya existe una instancia...
        {
            Destroy(gameObject);  // Destruir este objeto duplicado
            return;               // Salir para evitar ejecutar código innecesario
        }

        // CREAR COMPONENTES DE AUDIO DINÁMICAMENTE
        musicSource = gameObject.AddComponent<AudioSource>();  // Añadir AudioSource para música
        sfxSource = gameObject.AddComponent<AudioSource>();    // Añadir AudioSource para efectos

        // CONFIGURAR MÚSICA DE FONDO
        musicSource.loop = true;      // Repetir en bloop infinito
        musicSource.volume = 0.3f;    // Volumen al 30% (evita que sea muy molesto)

        // CONFIGURAR EFECTOS DE SONIDO
        sfxSource.volume = 0.5f;      // Volumen al 50% para efectos
    }

    // START - Se ejecuta en el primer frame después de Awake
    void Start()
    {
        // Reproducir música del menú al iniciar el juego
        PlayMenuMusic();
    }

    // ========== MÉTODOS PARA MÚSICA DE FONDO ==========

    // Reproduce la música del menú principal
    public void PlayMenuMusic()
    {
        // Verificar que existe el clip y la fuente de audio
        if (menuMusic != null && musicSource != null)
        {
            musicSource.clip = menuMusic;  // Asignar clip de música
            musicSource.Play();            // Reproducir
            Debug.Log("Reproduciendo música menú");  // Mensaje para depuración
        }
    }

    // Reproduce la música del nivel 1
    public void PlayNivel1Music()
    {
        if (nivel1Music != null && musicSource != null)
        {
            musicSource.clip = nivel1Music;  // Cambiar clip a música nivel 1
            musicSource.Play();              // Reproducir
            Debug.Log("Reproduciendo música nivel 1");
        }
    }

    // Reproduce la música del nivel 2
    public void PlayNivel2Music()
    {
        if (nivel2Music != null && musicSource != null)
        {
            musicSource.clip = nivel2Music;  // Cambiar clip a música nivel 2
            musicSource.Play();              // Reproducir
            Debug.Log("Reproduciendo música nivel 2");
        }
    }

    // ========== MÉTODOS PARA EFECTOS DE SONIDO ==========

    // Reproduce sonido al recoger objetos (llamado desde Collectable.cs)
    public void PlayCollectSound()
    {
        if (collectSound != null && sfxSource != null)  // Verificar recursos
        {
            sfxSource.PlayOneShot(collectSound);  // Reproducir efecto una vez
            Debug.Log("Sonido collect reproducido");
        }
    }

    // Reproduce sonido al abrir puertas (llamado desde Door.cs)
    public void PlayDoorSound()
    {
        if (doorSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(doorSound);  // Reproducir efecto una vez
            Debug.Log("Sonido door reproducido");
        }
    }

    // Reproduce sonido al saltar (opcional, no implementado en este proyecto)
    public void PlayJumpSound()
    {
        if (jumpSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(jumpSound);  // Reproducir efecto una vez
        }
    }

    // ========== MÉTODOS PARA CONTROL DE VOLUMEN ==========

    // Cambia el volumen de la música (usado desde OptionsMenu.cs)
    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)  // Verificar que existe la fuente
            musicSource.volume = volume;  // Asignar nuevo volumen (0.0 a 1.0)
    }

    // Cambia el volumen de los efectos de sonido
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)  // Verificar que existe la fuente
            sfxSource.volume = volume;  // Asignar nuevo volumen (0.0 a 1.0)
    }
}