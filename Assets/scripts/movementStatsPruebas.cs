using UnityEngine;


[CreateAssetMenu]
public class MovementStats : ScriptableObject
{
    public float Acceleration = 5f;
    public float MaxSpeed = 10f;
    public float JumpForce = 12f;
    public float SustainedJumpForce = 8f;
    public float MaxJumpDuration = 0.2f;
    public int JumpLimit = 1;
    public float GroundCheckDistance = 0.6f;
    public float GradValue = 1f;
    
    public float GroundFriction = 2f;
    public float AirFriction = 2f;
    public float GroundAcceleration = 10f;
    public float AirAcceleration = 5f;
    public float PeakGravity = 0f;
    public float FallingGravity = 0f;
    public float DefaultGravity = 0f;
}

