using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _superHappySprite, _happySprite, _normalSprite, _sadSprite;
    [SerializeField] private float _superHappyValue, _happyValue, _normalValue;

    public float SuperHappyValue => _superHappyValue;
    public float HappyValue => _happyValue;
    public float NormalValue => _normalValue;

    public void SuperHappy()
    {
        _spriteRenderer.sprite = _superHappySprite;
    }

    public void Happy()
    {
        _spriteRenderer.sprite = _happySprite;
    }

    public void Normal()
    {
        _spriteRenderer.sprite = _normalSprite;
    }

    public void Sad()
    {
        _spriteRenderer.sprite = _sadSprite;
    }

    public void Leave()
    {
        gameObject.SetActive(false);
    }
}
