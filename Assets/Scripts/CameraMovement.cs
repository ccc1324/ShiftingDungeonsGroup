using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject Player;
    public string CameraState;

    private float start;
    private float end;

    private void Start()
    {
        Player = FindObjectOfType<PlayerMovement>().gameObject;
        if (Player == null)
        {
            Debug.Log("Camera could not find player");
        }
    }

    void Update()
    {
        switch (CameraState)
        {
            case "Follow":
                transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
                break;
            case "Locked":
                break;
        }
    }

    public void ShiftRight(float dungeon_size)
    {
        if (CameraState == "Locked")
        {
            start = transform.position.x;
            end = transform.position.x + dungeon_size;
            StartCoroutine("LerpCamera");
        }
    }

    public void ShiftLeft(float dungeon_size)
    {
        if (CameraState == "Locked")
        {
            start = transform.position.x;
            end = transform.position.x - dungeon_size;
            StartCoroutine("LerpCamera");
        }
    }

    //Lerps Camera across the x axis
    IEnumerator LerpCamera()
    {
        float start_time = Time.time;
        Player.GetComponent<PlayerMovement>().enabled = false;
        while (Time.time < start_time + 1) //1 = transition time
        {
            transform.position = new Vector3(Mathf.Lerp(start, end, (Time.time - start_time)/1), transform.position.y, -10);
            yield return null;
        }
        transform.position = new Vector3(end, transform.position.y, -10);
        Player.GetComponent<PlayerMovement>().enabled = true;
    }
}
