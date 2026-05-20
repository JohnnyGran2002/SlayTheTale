using System;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]

public class Modifiable <T> 
{
 public string name; 
 [SerializeField] public T value;
 [SerializeField] public T addModifier;
 [SerializeField] public float multModifier;

 public Modifiable()
 {
  
 }

 public Modifiable(string Name)
 {
  name = Name;
 }
}


