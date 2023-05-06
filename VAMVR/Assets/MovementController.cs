using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{

    private Transform playerTransform;
    public enum Direction { 
        FORWARD,
        BACKWARDS,
        LEFT,
        RIGHT,
    }

    [Tooltip("The UI text element to show app messages.")]
    public Text logText;

    private void Log(string msg)
    {
        Debug.Log(msg);
        logText.text = msg;
    }

    public float moveSpeed = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1f;
        playerTransform = GameObject.Find("XR Origin").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null )
        {
            playerTransform = GameObject.Find("XR Origin").transform;
        }
    }

    public void Move(string[] infos)
    {
        moveSpeed = 1f;
        playerTransform = GameObject.Find("XR Origin").transform;

        Debug.Log("Move called");

        if (infos.Length < 1)
        {
            return;
        }
        Direction directionType;
        Enum.TryParse<Direction>(infos[0].ToUpper(), out directionType);
        Vector3 moveDirection = new Vector3();
        Quaternion rotation = Camera.main.transform.rotation;
        switch (directionType)
        {
            case Direction.FORWARD:
                moveDirection = rotation * Vector3.forward;
                break;
            case Direction.BACKWARDS:
                moveDirection = rotation * Vector3.back;
                break;
            case Direction.LEFT:
                moveDirection = rotation * Vector3.left;
                break;
            case Direction.RIGHT:
                moveDirection = rotation * Vector3.right;
                break;
            default:
                moveDirection = rotation * Vector3.forward;
                break;
        }
        //FindObjectOfType<CharacterController>().Move(moveDirection * moveSpeed * Time.deltaTime);
        playerTransform.position = playerTransform.position + moveDirection * moveSpeed;
        Debug.Log("Move executed");

    }

    public void Rotate(string[] infos)
    {
        Debug.Log("Rotate called");

        if (infos.Length < 1)
        {
            return;
        }
        Direction directionType;
        Enum.TryParse<Direction>(infos[0].ToUpper(), out directionType);
        Quaternion rotation = Camera.main.transform.rotation;
        switch (directionType)
        {
            case Direction.LEFT:
                rotation = rotation;
                break;
            case Direction.RIGHT:
                rotation = rotation;
                break;
            default:
                rotation = rotation;
                break;
        }
        //characterController.transform.rotation = rotation;
        Debug.Log("Rotate executed");

    }

    public void Stop()
    {
        Debug.Log("Stop called");
        Debug.Log("Stop executed");

    }
}
