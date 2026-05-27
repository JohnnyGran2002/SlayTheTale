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
     multModifier = 1.0f;
 }

 public Modifiable(string Name)
 {
  name = Name;
  multModifier = 1.0f;
 }
 
 public Modifiable(string Name, T initial)
 {
     value = initial;
     name = Name;
     multModifier = 1.0f;
 }
 public Modifiable(string Name, T initial, T addMod, float multMod)
 {
     value = initial;
     name = Name;
     addModifier = addMod;
     multModifier = multMod;
     
 }
}


