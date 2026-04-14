using Unity.VisualScripting;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{

    public GameObject mainDummy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainDummy = GameObject.FindWithTag("MainDummy");
        Debug.Log("Dummy encontrado");
    }

    void LateUpdate()
    {
        if (mainDummy != null)
        {
            transform.LookAt(mainDummy.transform);
            transform.Translate(Vector3.right * Time.deltaTime);
        }
    }
}