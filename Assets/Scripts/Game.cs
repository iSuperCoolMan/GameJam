using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField] private List<SlotTape> _tapes;
    [SerializeField] private float _startRoundDelay;

    private List<Slot> _choosenSlots;
    private int _index;
    private WaitForSeconds _wait;
    private event UnityAction SlotsChoosen;

    public event UnityAction<int> RewardCalculated;

    private void Awake()
    {
        _index = 0;
        _wait = new WaitForSeconds(_startRoundDelay);
        _choosenSlots = new List<Slot>();
    }

    private void OnEnable()
    {
        SlotsChoosen += Run;
        SlotsChoosen += CalculateReward;
        RewardCalculated += PrintReward;

        foreach (SlotTape tape in _tapes)
            tape.Choosen += AddChoosenSlot;
    }

    private void OnDisable()
    {
        SlotsChoosen -= Run;
        SlotsChoosen -= CalculateReward;

        foreach (SlotTape tape in _tapes)
            tape.Choosen -= AddChoosenSlot;
    }

    private void Start()
    {
        Run();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_tapes[_index].IsAvtive)
            {
                _tapes[_index].Stop();
                ChangeIndex();
                _tapes[_index].Active();
            }
        }
    }

    private void PrintReward(int reward)
    {
        Debug.Log(reward);
    }

    private void AddChoosenSlot(Slot slot)
    {
        _choosenSlots.Add(slot);
    }

    private IEnumerator RunCoroutine()
    {
        yield return _wait;
        foreach (SlotTape tape in _tapes)
            tape.Play();

        yield return _wait;
        _tapes[_index].Active();
    }

    private void Run()
    {
        StartCoroutine(RunCoroutine());
    }

    private void ChangeIndex()
    {
        _index = _choosenSlots.Count;

        if (_index >= _tapes.Count)
        {
            _index = 0;
            SlotsChoosen.Invoke();
            CalculateReward();
        }

        Debug.Log(_index);
    }

    private void CalculateReward()
    {
        int reward = 0;
        bool valuesEquals = true;

        for (int i = 1; i < _choosenSlots.Count; i++)
            valuesEquals = valuesEquals && _choosenSlots[i - 1].Value == _choosenSlots[i].Value;

        if (valuesEquals)
            reward = (int)_choosenSlots[0].Value;


        _choosenSlots.Clear();
        RewardCalculated?.Invoke(reward);
    }
}
