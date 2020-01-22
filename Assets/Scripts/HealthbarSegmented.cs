using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarSegmented : MonoBehaviour
{
    public Image[] HealthBar;
    public Sprite FilledHeart;
    public Sprite EmptyHeart;
    public ParticleSystem ParticleEffects;

    private int _health;
    private Camera _camera;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        SetHealth(0);
    }

    public void DecreaseHealth(int amount)
    {
        _health -= amount;
        if (_health < 0)
            _health = 0;

        for (int i = _health; i < _health + amount && i < HealthBar.Length; i++)
        {
            if (EmptyHeart != null)
                HealthBar[i].sprite = EmptyHeart;
            else
                HealthBar[i].color = Color.clear;

            Vector2 point = _camera.ScreenToWorldPoint(HealthBar[i].transform.position);
            if (ParticleEffects != null)
                Instantiate(ParticleEffects, point, new Quaternion(0, 0, 0, 0));
        }
    }

    public void SetHealth(int amount)
    {
        if (amount > HealthBar.Length)
        {
            Debug.Log("Setting health greater than maximum possible value");
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            HealthBar[i].sprite = FilledHeart;
            if (HealthBar[i].color == Color.clear)
                HealthBar[i].color = Color.white;
        }
        for (int i = amount; i < HealthBar.Length; i++)
        {
            if (EmptyHeart != null)
                HealthBar[i].sprite = EmptyHeart;
            else
                HealthBar[i].color = Color.clear;
        }

        _health = amount;
    }
}
