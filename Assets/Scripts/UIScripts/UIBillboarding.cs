using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    private new Camera camera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = camera.transform.forward;
    }
}
