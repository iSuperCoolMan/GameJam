using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [SerializeField] private List<SlotTape> _tapes;
    [SerializeField] private float _startRoundDelay;

    private int _index;
    private WaitForSeconds _wait;

    public event UnityAction EndRound;

    private void Awake()
    {
        _index = 0;
        _wait = new WaitForSeconds(_startRoundDelay);
    }

    private void OnEnable()
    {
        EndRound += Run;
    }

    private void OnDisable()
    {
        EndRound -= Run;
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

    private IEnumerator RunCoroutine()
    {
        yield return _wait;
        foreach (SlotTape tape in _tapes)
            tape.Play();

        _tapes[_index].Active();
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
            EndRound?.Invoke();
        }
    }
}
