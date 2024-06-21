using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sistema_Vueltas : MonoBehaviour
{
    public float timer = 0;
    private bool startTimer = false;
    public float bestLapTime = 0;

    private bool checkpoint1 = false;
    private bool checkpoint2 = false;

    public int vueltaCount = 0;
    private int vueltaBestTime = 0; // Variable para almacenar la vuelta con el mejor tiempo

    public Text Tiempo;
    public Text Vueltas;

    void Start()
    {
        timer = 0;
        bestLapTime = 0;
        vueltaCount = 0;
        Vueltas.text = vueltaCount.ToString();
    }

    void Update()
    {
        if (startTimer)
        {
            timer += Time.deltaTime;
            Tiempo.text = timer.ToString("F2");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "StartFinish")
        {
            if (checkpoint1 && checkpoint2)
            {
                vueltaCount += 1;

                if (bestLapTime == 0 || timer < bestLapTime)
                {
                    bestLapTime = timer;
                    vueltaBestTime = vueltaCount;
                }

                if (vueltaCount >= 2)
                {
                    Debug.Log("Mejor Tiempo");
                    Debug.Log("Vuelta: " + vueltaBestTime);
                    Debug.Log("Tiempo: " + bestLapTime.ToString("F2") + " segundos");
#if UNITY_EDITOR
                    // Muestra un mensaje en el Editor de Unity
                    Debug.Log("Fin del juego.");
                    UnityEditor.EditorApplication.isPlaying = false;
#else
                    // Salir del juego en una build
                    Application.Quit();
#endif
                }
                else
                {
                    startTimer = true;
                    timer = 0;
                    checkpoint1 = false;
                    checkpoint2 = false;
                }

                Vueltas.text = vueltaCount.ToString();
            }
            else if (vueltaCount == 0)
            {
                // Primera vez que cruza la línea de salida
                startTimer = true;
                timer = 0;
            }
        }

        if (other.gameObject.name == "CheckPoint1")
        {
            Debug.Log("CheckPoint1");
            checkpoint1 = true;
        }

        if (other.gameObject.name == "CheckPoint2")
        {
            Debug.Log("CheckPoint2");
            checkpoint2 = true;
        }
    }

}
