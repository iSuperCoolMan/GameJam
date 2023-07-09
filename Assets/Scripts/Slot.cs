using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Slot", menuName = "Slot/Create new Slot", order = 51)]
public class Slot : ScriptableObject
{
    //[SerializeField] string _text;
    [SerializeField] private Sprite _sprite;
    [SerializeField] uint _value;

    //public string Text => _text;
    public Sprite Sprite => _sprite;
    public uint Value => _value;
}
