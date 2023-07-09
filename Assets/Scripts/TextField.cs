using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextField : MonoBehaviour
{
    [SerializeField] private Image _sprite;
    [SerializeField] private List<Sprite> _rewardMessages;
    [SerializeField] private float _waitTime;

    private WaitForSeconds _timer;
    private Sprite _msg;
    private int _roundsCount;

    private void Awake()
    {
        _timer = new WaitForSeconds(_waitTime);
    }

    private void OnEnable()
    {
        StartCoroutine(ShowMessages());
    }

    public void TryOpen(float reward)
    {
        if (reward > 0)
        {
            if (_roundsCount++ == 0)
                Open(_rewardMessages[Random.Range(0, _rewardMessages.Count - 1)]);
            else if (Random.Range(0, 10) == 0)
                Open(_rewardMessages[Random.Range(0, _rewardMessages.Count - 1)]);
        }
    }

    private void Open(Sprite msg)
    {
        _msg = msg;
        gameObject.SetActive(true);
    }

    private IEnumerator ShowMessages()
    {
        _sprite.sprite = _msg;
        yield return _timer;

        gameObject.SetActive(false);
    }
}
