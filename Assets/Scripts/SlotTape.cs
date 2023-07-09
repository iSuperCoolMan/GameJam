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
    [SerializeField] AudioSource _rollSound;
    [SerializeField] AudioSource _stopSound;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _minSpeed;
    [SerializeField] float _changeSpeedMultiplier;
    [SerializeField] float _sizeMultiplier;
    [SerializeField] float _rotateSpeed;
    [SerializeField] float _choiceSpeed;
    [SerializeField] bool _isPitchable;

    private WaitForEndOfFrame _waitForEndOfFrame;
    private float _speed;
    private bool _isPlaying;

    public event UnityAction<Slot> Choosen;

    public bool IsAvtive { get; private set; }

    private void Awake()
    {
        _waitForEndOfFrame = new WaitForEndOfFrame();
    }

    private void Update()
    {
        foreach (var slot in _slots)
        {
            if (slot.transform.position.y > transform.position.y)
            {
                slot.ChangeRotation(new Quaternion(
                    (transform.position.y + slot.transform.position.y) / _startPoint.transform.position.y, 0, 0, 1
                    )
                );
            }
            if (slot.transform.position.y < transform.position.y)
            {
                slot.ChangeRotation(new Quaternion(
                    (transform.position.y + slot.transform.position.y) / _endPoint.position.y, 0, 0, 1
                    )
                );
            }
        }
    }

    public void Play()
    {
        _rollSound.Play();
        _speed = _maxSpeed;
        _rollSound.pitch = 1f;
        _isPlaying = true;
        StartCoroutine(Spin());
    }

    public void Active()
    {
        foreach (var slot in _slots)
            slot.ChangeSize(_sizeMultiplier);

        IsAvtive = true;
        StartCoroutine(ChangeCurrentSpeed());
    }

    public void Stop()
    {
        _rollSound.Stop();
        _stopSound.Play();

        foreach (var slot in _slots)
            slot.ChangeSize(1);

        IsAvtive = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);

        if (hit.collider.TryGetComponent(out SlotView currentSlot))
        {
            Choosen?.Invoke(currentSlot.CurrentSlot);
            _isPlaying = false;
            StartCoroutine(Choice(currentSlot.transform));
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
                    slot.ChangeRotation(new Quaternion(1, 0, 0, 1));
                    slot.Init();
                }

                slot.transform.position = Vector3.MoveTowards(
                    slot.transform.position,
                    _endPoint.position,
                    _speed * Time.deltaTime
                    );

                //slot.transform.rotation = Quaternion.RotateTowards(
                //    slot.transform.rotation,
                //    new Quaternion(-45, 0, 0, 0),
                //    _speed * Time.deltaTime
                    //);
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

            if (_isPitchable)
                _rollSound.pitch = Mathf.Sqrt(Mathf.Sqrt(_speed /_maxSpeed));

            yield return _waitForEndOfFrame;
        }
    }
}
