using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Logica_Contrincante : MonoBehaviour
{
    public List<Transform> checkpoints; // Lista de checkpoints a seguir
    public float normalSpeed = 10.0f;
    public float boostedSpeed = 20.0f;
    public float closeEnoughDistance = 1.0f; // Distancia mínima para considerar que llegó al checkpoint

    private int currentCheckpointIndex = 0;
    private NavMeshAgent agent;
    private bool isBoosted = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null && checkpoints.Count > 0)
        {
            agent.speed = normalSpeed;
            agent.autoBraking = false; // Evita frenar bruscamente al acercarse al checkpoint
            agent.updateRotation = true;
            agent.updateUpAxis = false;
            SetNextDestination();
        }
    }

    void Update()
    {
        // Verifica si el agente está cerca del destino actual
        if (!agent.pathPending && agent.remainingDistance <= closeEnoughDistance)
        {
            SetNextDestination();
        }
    }

    void SetNextDestination()
    {
        if (checkpoints.Count == 0)
            return;

        // Actualiza el índice del checkpoint
        currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpoints.Count;
        // Establece el próximo destino
        agent.SetDestination(checkpoints[currentCheckpointIndex].position);
    }

    void OnTriggerEnter(Collider other)
    {
        // Comprobar si el objeto con el que hemos colisionado tiene el tag "Barrera"
        if (other.CompareTag("Barrera"))
        {
            Debug.Log("Colisión con una barrera!");
        }
    }

    public void BoostSpeed(bool boost)
    {
        isBoosted = boost;
        if (agent != null)
        {
            agent.speed = boost ? boostedSpeed : normalSpeed;
        }
    }
}
