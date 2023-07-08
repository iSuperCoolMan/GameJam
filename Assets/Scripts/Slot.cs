using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Slot", menuName = "Slot/Create new Slot", order = 51)]
public class Slot : ScriptableObject
{
    [SerializeField] string _text;
    [SerializeField] uint _value;

    public string Text => _text;
    public uint Value => _value;
}
