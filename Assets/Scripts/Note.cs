using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public GameObject NoteOverlay;
    public GameObject KeyAnimation;

    private AudioSource _audio_source;
    private GameObject _player;
    private bool _triggered;

    void Start()
    {
        _audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_triggered)
            if (Input.GetKeyDown("j"))
            {
                if (NoteOverlay.activeSelf)
                {
                    _player.GetComponent<PlayerMovement>().enabled = true;
                    _player.GetComponent<PlayerCombat>().enabled = true;
                    NoteOverlay.SetActive(false);
                }
                else
                {
                    _player.GetComponent<PlayerMovement>().Stop();
                    _player.GetComponent<PlayerMovement>().enabled = false;
                    _player.GetComponent<PlayerCombat>().enabled = false;
                    NoteOverlay.SetActive(true);
                    _audio_source.Play();
                }
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _player = collision.GetComponentInParent<PlayerMovement>().gameObject;
            KeyAnimation.SetActive(true);
            _triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        KeyAnimation.SetActive(false);
        _triggered = false;
    }
}
