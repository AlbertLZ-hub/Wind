// Importar namespace básico de Unity
using UnityEngine;

// Clase que gestiona efectos de partículas en el juego
public class ParticleEffect : MonoBehaviour  // Hereda de MonoBehaviour para poder ser componente
{
    // HEADER organiza variables en el Inspector de Unity para mejor visualización
    [Header("Configuración Partículas")]
    public float lifetime = 2f;       // Tiempo en segundos que dura el efecto de partículas antes de autodestruirse
    public bool autoDestroy = true;   // Si es true, el objeto se destruye automáticamente después del lifetime

    // START - Se ejecuta cuando el objeto con este script se activa en la escena
    void Start()
    {
        // Verificar si el efecto debe autodestruirse
        if (autoDestroy)
        {
            // Llamar a Destroy() con parámetro de tiempo: destruye el GameObject después de 'lifetime' segundos
            // gameObject se refiere al objeto que tiene este script adjunto
            Destroy(gameObject, lifetime);
        }
    }

    // ========== MÉTODO ESTÁTICO (utilidad) ==========

    // Función estática que puede ser llamada desde cualquier script sin necesidad de instanciar la clase
    // Propósito: Crear un efecto de partículas en una posición específica del mundo
    public static void PlayEffect(ParticleSystem effect, Vector3 position)
    {
        // Verificar que se haya pasado un sistema de partículas válido (no nulo)
        if (effect != null)
        {
            // Instantiate() crea una copia (instancia) del sistema de partículas en la escena
            // Parámetros:
            // 1. effect: Prefab del sistema de partículas a instanciar
            // 2. position: Posición en el mundo 3D donde aparecerá
            // 3. Quaternion.identity: Rotación por defecto (sin rotación)
            Instantiate(effect, position, Quaternion.identity);

            // NOTA: El efecto instanciado tendrá el script ParticleEffect adjunto si el prefab lo incluye
            // por lo que se autodestruirá según su configuración de lifetime
        }
    }
}