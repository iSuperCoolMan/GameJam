using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotView : MonoBehaviour
{
    [SerializeField] private List<Slot> _slots;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private SpriteRenderer _sprite;

    private int _biggestSlotValue;

    public Slot CurrentSlot { get; private set; }
    public SpriteRenderer Sprite { get; private set; }

    private void Awake()
    {
        if (_slots.Count == 0)
            return;

        _biggestSlotValue = 0;

        foreach (Slot slot in _slots)
        {
            if (_biggestSlotValue < (int)slot.Value)
                _biggestSlotValue = (int)slot.Value;
        }
    }


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        TryChooseRandomSlot();
        Render();
    }

    private void Render()
    {
        _text.text = CurrentSlot.Text;
    }

    private void TryChooseRandomSlot()
    {
        if (_slots.Count != 0)
            ChooseRandomSlot();
        else
            throw new System.Exception("Slots list are empty.");
    }

    private void ChooseRandomSlot()
    {
        bool isRunning = true;

        while (isRunning)
        {
            Slot slot = _slots[Random.Range(0, _slots.Count)];
            int random = Random.Range(1, _biggestSlotValue + 1);

            if (random >= slot.Value)
            {
                CurrentSlot = slot;
                isRunning = false;
            }
        }
    }
}