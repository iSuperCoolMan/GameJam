using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField] private TextField _textField;
    [SerializeField] private AudioSource _winSound;
    [SerializeField] private AudioSource _topUpSound;
    [SerializeField] private List<SlotTape> _tapes;
    [SerializeField] private uint _price;
    [SerializeField] private float _startRoundDelay;
    [SerializeField] private float _startRollTapesDelay;

    private Slot[] _choosenSlots;
    private int _index;
    private WaitForSeconds _waitStartRound;
    private WaitForSeconds _waitStartRoll;
    private WaitForSeconds _waitWin;
    private event UnityAction SlotsChoosen;

    public event UnityAction<uint> Started;
    public event UnityAction<uint> RewardCalculated;

    public uint Price => _price;

    private void Awake()
    {
        _index = 0;
        _waitStartRound = new WaitForSeconds(_startRoundDelay);
        _waitStartRoll = new WaitForSeconds(_startRollTapesDelay);
        _choosenSlots = new Slot[_tapes.Count];
    }

    private void OnEnable()
    {
        SlotsChoosen += Run;
        SlotsChoosen += CalculateReward;

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

                if (_index != 0)
                    _tapes[_index].Active();
            }
        }
    }

    private void AddChoosenSlot(Slot slot)
    {
        _choosenSlots[_index] = slot;
    }

    private IEnumerator RunCoroutine()
    {
        yield return _waitStartRound;
        yield return _waitWin;
        _topUpSound.Play();
        yield return _waitStartRound;

        foreach (SlotTape tape in _tapes)
        {
            yield return _waitStartRoll;
            tape.Play();
        }

        Started?.Invoke(_price);
        _tapes[_index].Active();

        _waitWin = null;
    }

    private void Run()
    {
        StartCoroutine(RunCoroutine());
    }

    private void ChangeIndex()
    {
        _index++;

        if (_index >= _tapes.Count)
        {
            _index = 0;
            SlotsChoosen.Invoke();
        }
    }

    private void CalculateReward()
    {
        uint reward = 0;
        bool valuesEquals = true;

        for (int i = 1; i < _choosenSlots.Length; i++)
            valuesEquals = valuesEquals && _choosenSlots[i - 1].Value == _choosenSlots[i].Value;

        if (valuesEquals)
        {
            reward = _choosenSlots[0].Value;
            _winSound.Play();
            _waitWin = new WaitForSeconds(_startRoundDelay);
        }

        _choosenSlots = new Slot[_tapes.Count];
        _textField.TryOpen(reward);
        RewardCalculated?.Invoke(reward);
    }
}
