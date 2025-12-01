// Importar namespace básico de Unity
using UnityEngine;

// Clase que gestiona el comportamiento de puertas interactivas en el juego
public class Door : MonoBehaviour  // Hereda de MonoBehaviour para ser componente
{
    // HEADER organiza variables en el Inspector de Unity
    [Header("Configuración Puerta")]
    public float openDistance = 3f;     // Distancia máxima a la que el jugador puede abrir la puerta
    public float openSpeed = 2f;        // Velocidad a la que se abre/cierra la puerta (unidades por segundo)
    public Vector3 openPosition;        // Posición final cuando la puerta está completamente abierta

    // Variables privadas para estado interno
    private Vector3 closedPosition;     // Posición original (cerrada) de la puerta
    private bool isOpen = false;        // Estado actual: true = abierta, false = cerrada
    private Transform player;           // Referencia al transform del jugador

    // START - Se ejecuta cuando el objeto se activa en la escena
    void Start()
    {
        // Guardar la posición inicial como posición cerrada
        closedPosition = transform.position;

        // Buscar el jugador en la escena por su tag "Player"
        // ?. (null-conditional operator) evita error si no encuentra jugador
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    // UPDATE - Se ejecuta cada frame
    void Update()
    {
        // Verificar que se encontró al jugador
        if (player != null)
        {
            // Calcular distancia entre la puerta y el jugador usando Vector3.Distance
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Si el jugador está suficientemente cerca Y presiona la tecla E
            if (distanceToPlayer <= openDistance && Input.GetKeyDown(KeyCode.E))
            {
                ToggleDoor();  // Cambiar estado de la puerta
            }
        }

        // Determinar posición objetivo según el estado actual
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;

        // Mover suavemente la puerta hacia la posición objetivo usando Lerp (Linear Interpolation)
        // Lerp interpola entre dos posiciones: desde posición actual hacia targetPosition
        // openSpeed * Time.deltaTime controla la suavidad del movimiento (framerate independiente)
        transform.position = Vector3.Lerp(transform.position, targetPosition, openSpeed * Time.deltaTime);
    }

    // Función que alterna el estado de la puerta (abrir/cerrar)
    void ToggleDoor()
    {
        // Cambiar estado: si estaba abierta → cerrar, si estaba cerrada → abrir
        isOpen = !isOpen;

        // Mensaje de depuración en consola con estado actual
        Debug.Log("Puerta " + (isOpen ? "abierta" : "cerrada"));

        // Reproducir sonido de puerta si existe AudioManager
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayDoorSound();  // Llamar al método del AudioManager
        }
    }

    // MÉTODO ESPECIAL DEL EDITOR - Solo se ejecuta en el Editor de Unity, no en el juego final
    void OnDrawGizmosSelected()
    {
        // Establecer color amarillo para los gizmos
        Gizmos.color = Color.yellow;

        // Dibujar una esfera wireframe que muestra visualmente la distancia de apertura
        // Útil para ajustar openDistance en el Inspector
        Gizmos.DrawWireSphere(transform.position, openDistance);
    }
}