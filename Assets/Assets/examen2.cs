using UnityEngine;

public class examen2 : MonoBehaviour
{
    public float speed = 0f;
    Vector3 movementInput;

   

    // Update is called once per frame
    void Update()
    {
        movementInput = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movementInput.y += 1;
        if (Input.GetKey(KeyCode.S))
            movementInput.y -= 1;
        if (Input.GetKey(KeyCode.D))
            movementInput.x += 1;
        if (Input.GetKey(KeyCode.A))
            movementInput.x -= 1;

        Move(true);

       
        
    }
    void Move(bool changeDirection)
    {
        if (movementInput.x != 0 && movementInput.y != 0)
            movementInput.Normalize();

        float actualSpeed = speed;
        if (changeDirection)
            actualSpeed = Mathf.Abs(speed);

        transform.position += movementInput * actualSpeed * Time.deltaTime;
    }
}

