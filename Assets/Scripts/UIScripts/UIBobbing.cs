using System;
using UnityEngine;

public class UIBobbing : MonoBehaviour
{
    [Range(0, 100), SerializeField] private float speed = 2f, height = 0.2f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        var y = startPos.y + Mathf.Sin(Time.time * speed) * height;

        transform.position = new Vector3(startPos.x, y, startPos.z);
    }
}
