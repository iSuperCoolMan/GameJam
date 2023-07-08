using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotTape : MonoBehaviour
{
    [SerializeField] List<SlotView> _slots;
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _endPoint;
    [SerializeField] Transform _fadeInPoint;
    [SerializeField] Transform _fadeOutPoint;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _minSpeed;
    [SerializeField] float _changeSpeedMultiplier;
    [SerializeField] float _choiceSpeed;

    private WaitForEndOfFrame _waitForEndOfFrame;
    private float _speed;
    private bool _isPlaying;

    public event UnityAction<Slot> Choosen;

    public bool IsAvtive { get; private set; }

    private void Awake()
    {
        _waitForEndOfFrame = new WaitForEndOfFrame();
    }

    public void Play()
    {
        _speed = _maxSpeed;
        _isPlaying = true;
        StartCoroutine(Spin());
    }

    public void Active()
    {
        IsAvtive = true;
        StartCoroutine(ChangeCurrentSpeed());
    }

    public void Stop()
    {
        IsAvtive = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);

        if (hit.collider.TryGetComponent(out SlotView slot))
        {
            Choosen?.Invoke(slot.CurrentSlot);
            _isPlaying = false;
            StartCoroutine(Choice(slot.transform));
        }
    }

    private IEnumerator Spin()
    {
        while(_isPlaying)
        {
            foreach (var slot in _slots)
            {
                if (slot.transform.position.y <= _endPoint.position.y)
                {
                    slot.transform.position = 
                        _startPoint.position - (_endPoint.transform.position - slot.transform.position);
                    slot.Init();
                }

                slot.transform.position = Vector3.MoveTowards(
                    slot.transform.position,
                    _endPoint.position + new Vector3(0, -1, 0),
                    _speed * Time.deltaTime
                    );
            }

            yield return null;
        }
    }

    private IEnumerator Choice(Transform currentSlotTransform)
    {
        while (currentSlotTransform.position != transform.position)
        {
            foreach (var slot in _slots)
            {
                Vector3 endPoint = 
                    slot.transform.position - currentSlotTransform.position + transform.position;

                slot.transform.position = Vector3.MoveTowards(
                    slot.transform.position, endPoint, _choiceSpeed * Time.deltaTime
                    );
            }

            yield return null;
        }
    }

    private IEnumerator ChangeCurrentSpeed()
    {
        while (_speed > _minSpeed && IsAvtive)
        {
            _speed = Mathf.MoveTowards(_speed, _minSpeed, _changeSpeedMultiplier * _maxSpeed / _minSpeed * Time.deltaTime);
            yield return _waitForEndOfFrame;
        }
    }
}
