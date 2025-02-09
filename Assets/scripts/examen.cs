using UnityEngine;

public class examen : MonoBehaviour
{
    [SerializeField] float speed = 0f;
    [SerializeField] float X_Bound = 0f;
    [SerializeField] ParticleSystem particles;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if (transform.position.x > X_Bound)
        {
            particles.Stop();
            transform.position = new Vector3(-X_Bound,0,0);
            particles.Play();
        }
    }
}
