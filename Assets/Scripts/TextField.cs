using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextField : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TextMessage _rewardMessage;

    private TextMessage _msg;
    private int _index;

    private void OnEnable()
    {
        _index = 0;
        ChangeText(_msg.Texts[_index++]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_index < _msg.Texts.Count)
                ChangeText(_msg.Texts[_index++]);
            else
                gameObject.SetActive(false);
        }
    }

    public void Open(TextMessage msg)
    {
        _msg = msg;
        gameObject.SetActive(true);
    }

    public void TryOpen(float reward)
    {
        if (reward > 0)
            Open(_rewardMessage);
    }

    private void ChangeText(string text)
    {
        _text.text = text;
    }
}
