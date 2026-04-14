using UnityEngine;

public class TrapController : MonoBehaviour
{
    public GameObject cubePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource sound = GetComponent<AudioSource>();
            sound.Play(); // Deveria testar se não é nulo antes de rodar em um mundo justo e perfeito, mas não estamos em tal mundo
            // É normal trocar os audio sources tem tempo de execução ou tocar eles em array

            for (int i = 0; i < 7; i++) 
            { 
                float randomX = UnityEngine.Random.Range(-20, 20);
                float randomZ = UnityEngine.Random.Range(-20, 20);

                GetComponent<MeshRenderer>().enabled = true; // Fica visível quando ativada

                var cube = (GameObject)Instantiate(cubePrefab, new Vector3(randomX, 10, randomZ), Quaternion.identity);
                Destroy(cube, 5.0f);
            }

            
        }
    }
}
