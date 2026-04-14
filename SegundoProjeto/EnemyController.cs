using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    private bool isAwake;
    void Start()
    {
        isAwake = false;
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            Debug.Log("Player encontrado");
        }
    }

    void FixedUpdate()
    {
        if (isAwake && player != null)
        {
            transform.LookAt(player.transform);
            if (!IsInvoking("Fire")) 
            {
                Invoke("Fire", 2f);
            }
        }
    }

    private void OnEnable()
    {
        TrapEnemyController.triggerTrap += Awakening;
    }

    private void OnDisable()
    {
        TrapEnemyController.triggerTrap -= Awakening;
    }

    private void Awakening()
    {
        isAwake = true;
    }

    private void Fire()
    {
        var bullet = Instantiate(bulletPrefab,
                                 bulletSpawn.position,
                                 bulletSpawn.rotation);
        AudioSource shoot = GetComponent<AudioSource>(); // OBS: Pega o primeiro AudioSource que tem no GameObject desse script
        shoot.Play();
        bullet.GetComponent<Rigidbody>().linearVelocity = bullet.transform.forward * 6.0f;
        Destroy(bullet, 5f); // (GameObject, tempo em segundos) - OBS: Funciona em Thread separada
        
    }
}
