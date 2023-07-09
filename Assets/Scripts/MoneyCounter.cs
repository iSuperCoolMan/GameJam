using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private Casino _casino;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _casino.MoneyChanged += ChangeMoney;
    }

    private void OnDisable()
    {
        _casino.MoneyChanged -= ChangeMoney;
    }

    private void ChangeMoney(int money)
    {
        if (money < 0)
            money = 0;

        _text.text = money.ToString();
    }
}
