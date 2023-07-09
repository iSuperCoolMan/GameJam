using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextField : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private List<TextMessage> _rewardMessages;
    [SerializeField] private float _waitTime;

    private WaitForSeconds _timer;
    private TextMessage _msg;
    private int _roundsCount;

    private void OnEnable()
    {
        StartCoroutine(ShowMessages());
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
            else if (Random.Range(0, 10) == 0)
                Open(_rewardMessages[Random.Range(0, _rewardMessages.Count - 1)]);
        }
    }

    private IEnumerator ShowMessages()
    {
        foreach (string text in _msg.Texts)
        {
            _text.text = text;
            yield return _timer;
        }

        gameObject.SetActive(false);
    }
}
