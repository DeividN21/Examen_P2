using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logica_Player : MonoBehaviour
{
    public float normalSpeed = 10.0f;
    public float boostedSpeed = 20.0f;
    public float rotationSpeed = 100.0f;
    public AudioClip drivingSound; // Clip de audio a reproducir
    public GameObject particleEffect; // Objeto de partículas a activar
    public float particleDuration = 2.0f; // Tiempo adicional para que las partículas permanezcan activas

    private float currentSpeed;
    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isDriving = false;
    private bool isBoosted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentSpeed = normalSpeed;

        // Configura la detección de colisiones
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // Configura el audio source
        if (audioSource != null)
        {
            audioSource.clip = drivingSound;
            audioSource.loop = true; // Hacer que el sonido se repita en loop
            audioSource.playOnAwake = false; // Asegurarse de que no se reproduzca al iniciar la escena
        }

        // Asegurarse de que el objeto de partículas esté desactivado al inicio
        if (particleEffect != null)
        {
            particleEffect.SetActive(false);
        }
    }

    void Update()
    {
        // Verifica si la tecla Shift está siendo presionada
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = boostedSpeed;
            if (!isBoosted && particleEffect != null)
            {
                particleEffect.SetActive(true); // Activa las partículas
                isBoosted = true;
                StopCoroutine(DisableParticlesAfterDelay()); // Detener cualquier corrutina previa
            }
        }
        else
        {
            currentSpeed = normalSpeed;
            if (isBoosted && particleEffect != null)
            {
                StartCoroutine(DisableParticlesAfterDelay()); // Iniciar corrutina para desactivar las partículas después de un retraso
                isBoosted = false;
            }
        }

        // Movimiento hacia adelante y hacia atrás en el eje Z
        float translation = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
        // Rotación en el eje Y
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        // Aplicar la traslación en el eje Z
        transform.Translate(0, 0, translation);
        // Aplicar la rotación en el eje Y
        transform.Rotate(0, rotation, 0);

        // Reproduce el sonido cuando se presiona la flecha hacia adelante
        if (Input.GetKey(KeyCode.LeftShift) )
        {
            if (!isDriving)
            {
                audioSource.Play();
                isDriving = true;
            }
        }
        else
        {
            if (isDriving)
            {
                audioSource.Stop();
                isDriving = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Comprobar si el objeto con el que hemos colisionado tiene el tag "Barrera"
        if (collision.gameObject.CompareTag("Barrera"))
        {
            Debug.Log("Colisión con una barrera!");
        }
    }
    private IEnumerator DisableParticlesAfterDelay()
    {
        yield return new WaitForSeconds(particleDuration);
        if (particleEffect != null)
        {
            particleEffect.SetActive(false); // Desactiva las partículas
        }
    }
}
