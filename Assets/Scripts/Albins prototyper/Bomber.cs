using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomber : MonoBehaviour
{
    private Vector3 spawnPos;

    [SerializeField] private GameObject prefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Bomb()
    {
        //haha tihi die
        spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(prefab, spawnPos, quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        //if (mouse)
        //{
       //     Bomb();
       // }
    }
}
