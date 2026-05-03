using System;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]

public class Modifiable <T> 
{
 public string name; 
 [SerializeField] public T value;
 [SerializeField] public float modifier;

 public Modifiable()
 {
  
 }

 public Modifiable(string Name)
 {
  name = Name;
 }
}


