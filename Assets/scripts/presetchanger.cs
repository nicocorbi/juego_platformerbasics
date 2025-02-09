using UnityEngine;

public class presetchanger : MonoBehaviour
{
    [SerializeField] MovementStats[] statsList;
    int ListNum = 0;
    public MovementStats stat;

    [SerializeField] private Movimiento movimientoScript; // Referencia al script de Movimiento

    void Start()
    {
        // Asegúrate de tener la referencia al script de Movimiento
        if (movimientoScript == null)
        {
            movimientoScript = GetComponent<Movimiento>(); // Obtener referencia automáticamente si no se ha asignado
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ListNum++;

            if (ListNum >= statsList.Length)
            {
                ListNum = 0;
            }

            // Asignar el nuevo ScriptableObject al stat
            stat = statsList[ListNum];

            // Actualizamos la referencia en el script de Movimiento
            if (movimientoScript != null)
            {
                movimientoScript.UpdateStat(stat); // Llamar al método que actualiza stat en Movimiento
            }

            print("Preset cambiado a: " + stat.name);
        }
    }
}



