using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Message", menuName = "Message/Create new Message", order = 51)]
public class TextMessage : ScriptableObject
{
    [SerializeField] private List<string> _texts;

    public IReadOnlyList<string> Texts => _texts;
}
