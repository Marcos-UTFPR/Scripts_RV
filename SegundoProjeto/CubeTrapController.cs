using UnityEngine;
using System;

public class CubeTrapController : MonoBehaviour
{
    public static event Action hitPlayer;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitPlayer?.Invoke();
            Destroy(collision.gameObject);
        }
    }
}
