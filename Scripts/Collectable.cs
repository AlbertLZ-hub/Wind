// Importar namespace básico de Unity
using UnityEngine;

// Clase que gestiona objetos recolectables en el juego (monedas, gemas, etc.)
public class Collectable : MonoBehaviour  // Hereda de MonoBehaviour para ser componente
{
    // HEADER organiza variables en el Inspector de Unity
    [Header("Configuración Objeto")]
    public int scoreValue = 1;         // Puntos que otorga este objeto al ser recogido
    public float rotationSpeed = 50f;  // Velocidad de rotación en grados por segundo

    [Header("Efectos")]
    public AudioClip collectSound;     // Sonido específico para este objeto (opcional, alternativo al general)

    // UPDATE - Se ejecuta cada frame (60 veces por segundo aprox.)
    void Update()
    {
        // Rotar el objeto continuamente alrededor del eje Y (eje vertical)
        // transform.Rotate gira el objeto en los ejes especificados
        // Parámetros: (eje X, eje Y, eje Z) en grados por frame
        // Time.deltaTime hace la rotación framerate-independent (igual velocidad en todos los PCs)
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Ejemplo: rotationSpeed = 50, Time.deltaTime ≈ 0.0167 (a 60 FPS)
        // Rotación por frame: 50 × 0.0167 ≈ 0.835 grados por frame
        // Rotación por segundo: 0.835 × 60 ≈ 50 grados/segundo (consistente)
    }

    // ONTRIGGERENTER - Se ejecuta cuando otro Collider entra en este trigger
    // Solo funciona si este GameObject tiene un Collider marcado como "Is Trigger"
    void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entró al trigger tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Mensaje de depuración en consola
            Debug.Log("Collectable recogido por jugador");

            // ========== SISTEMA DE SONIDO ==========

            // PRIMERA OPCIÓN: Usar AudioManager global (recomendado)
            if (AudioManager.instance != null)
            {
                // Reproducir sonido general de recolección desde AudioManager
                AudioManager.instance.PlayCollectSound();
            }
            // SEGUNDA OPCIÓN: Fallback - usar sonido específico de este objeto
            else if (collectSound != null)  // Si este objeto tiene sonido propio
            {
                // AudioSource.PlayClipAtPoint crea una fuente temporal de audio
                // Ventaja: no necesita AudioSource preexistente
                // Desventaja: no se integra con sistema de volumen global
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            // NOTA: Si ambos sistemas fallan, no se reproduce sonido (podría añadir sonido por defecto)

            // ========== SISTEMA DE PUNTUACIÓN ==========

            // Comunicarse con el GameManager para registrar la recolección
            if (GameManager.instance != null)
            {
                // Llamar a la función CollectObject() que incrementa el contador
                GameManager.instance.CollectObject();
            }
            else
            {
                // Error crítico: sin GameManager no se puede llevar registro del juego
                Debug.LogError("GameManager.instance es null!");
                // En un juego real, esto podría causar crash o comportamiento inesperado
            }

            // ========== DESTRUCCIÓN DEL OBJETO ==========

            // Destruir este GameObject (el objeto recolectable)
            Destroy(gameObject);
            // gameObject se refiere al objeto que tiene este script adjunto

            // NOTA: Destroy() marca el objeto para destrucción al final del frame
            // El objeto sigue existiendo durante el resto de este frame
            // Por eso podemos seguir accediendo a transform.position para el sonido
        }

        // Si el objeto que entró NO es el jugador, no hacer nada
        // Ejemplo: podría ser un enemigo, proyectil, o objeto decorativo
    }
}