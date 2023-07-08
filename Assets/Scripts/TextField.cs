using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextField : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private List<TextMessage> _rewardMessages;

    private TextMessage _msg;
    private int _index;
    private int _roundsCount;

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
        {
            if (_roundsCount++ == 0)
                Open(_rewardMessages[0]);
            else if (Random.Range(0, 5) == 0)
                Open(_rewardMessages[Random.Range(0, _rewardMessages.Count - 1)]);
        }
    }

    private void ChangeText(string text)
    {
        _text.text = text;
    }
}
