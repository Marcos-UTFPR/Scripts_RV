using UnityEngine;
using System;
public class TrapCameraController : MonoBehaviour
{

    public static event Action triggerTrap;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource sound = GetComponent<AudioSource>();
            sound.Play(); // Deveria testar se n�o � nulo antes de rodar em um mundo justo e perfeito, mas n�o estamos em tal mundo
            
            triggerTrap?.Invoke();
            GetComponent<MeshRenderer>().enabled = true; // Fica vis�vel quando ativada
            
        }
    }
}
