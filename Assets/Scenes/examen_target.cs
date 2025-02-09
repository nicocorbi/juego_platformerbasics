using UnityEngine;

public class examen_target : MonoBehaviour
{
    public Transform target_position;
    public int MaxSpeed = 0;
    Vector3 direction;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * MaxSpeed * Time.deltaTime;
        transform.up = target_position.position - transform.position;
        direction =(target_position.position - transform.position).normalized;
        


    }
}
