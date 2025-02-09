using UnityEngine;

public class Examen4 : MonoBehaviour
{
    [SerializeField] float speed = 10f; // Velocidad del personaje
    [SerializeField] float maxDistanceFromCenter = 5f; // Distancia máxima desde el centro
    Vector3 direction; // Dirección actual del personaje
    [SerializeField] ParticleSystem particles;

    void Start()
    {
        ResetDirection();
    }

    void Update()
    {
        // Mueve el objeto en la dirección actual
        transform.position += direction * speed * Time.deltaTime;

        // Asegura que el triángulo apunte en la dirección del movimiento
        transform.up = direction;

        // Calcula la distancia al centro
        float distanceToCenter = Vector3.Distance(transform.position, Vector3.zero);

        // Si supera la distancia máxima, reinicia la posición y la dirección
        if (distanceToCenter > maxDistanceFromCenter)
        {
            particles.Stop();
            transform.position = Vector3.zero;
            ResetDirection();
            particles.Play();
        }
    }

    void ResetDirection()
    {
        // Genera un vector aleatorio
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        Vector3 randomDirection = new Vector3(randomX, randomY, 0f);

        // Normaliza el vector manualmente
        float length = Mathf.Sqrt(randomDirection.x * randomDirection.x + randomDirection.y * randomDirection.y);
        if (length > 0)
        {
            randomDirection = randomDirection / length; // Normaliza
        }

        // Escala la dirección al tamaño deseado (en este caso, no es necesario escalar porque ya es unitario)
        direction = randomDirection;
    }
}
