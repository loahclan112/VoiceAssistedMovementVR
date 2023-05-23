using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 1000f;

    public GameObject spherePrefab;

    public GameObject cylinderPrefab;

    public List<GameObject> instantiatedCylinders = new List<GameObject>();

    public List<GameObject> instantiatedSpheres = new List<GameObject>();

    public Vector3 basePosition;
    public Quaternion baseRotation;

    public bool isMoving = false;

    public enum Direction {
        None,
        Up,
        Down,
        Forward,
        Back,
        Left,
        Right,    
    }

    // Update is called once per frame

    private void Start()
    {
        basePosition = playerTransform.position;
        baseRotation = playerTransform.rotation;
    }
    void Update()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.Find("XRRig").transform;
        }

        if (isMoving)
        {
            playerTransform.Translate(Camera.main.transform.rotation * Camera.main.transform.forward * Time.deltaTime);
        }
    }

    public static string controllerName = "GameController";

    public List<Direction> directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();

    public void DoAction(string recognizedText) {

        if (recognizedText == null)
        {
            return;
        }
        else if (recognizedText.Contains("audio"))
        {
            VoskSpeechToText voskSpeechToText = GameObject.Find("Controller").GetComponent<VoskSpeechToText>();
            voskSpeechToText.isAudioOn = !voskSpeechToText.isAudioOn;
        }
        else if (recognizedText.Contains("reset"))
        {
            playerTransform.position = basePosition;
            playerTransform.rotation = baseRotation;
            return;
        }
        if (recognizedText.Contains("stop"))
        {
            isMoving = false;
            return;
        }
        if (recognizedText.Contains("start"))
        {
            isMoving = true;
            return;
        }

        else if (recognizedText.Contains("move"))
        {
            Direction direction = directions.Find(x => recognizedText.ToLower().Contains(x.ToString().ToLower()));
            if (direction != Direction.None)
            { 
                MovementLogic(direction);
                return;
            }
            MovementLogic(Direction.Forward);
        }
        else if (recognizedText.Contains("rotate"))
        {
            Direction direction = directions.Find(x => recognizedText.ToLower().Contains(x.ToString().ToLower()));
            if (direction != Direction.None)
            {
                RotateLogic(direction);
                return;
            }
        }
        else if (recognizedText.Contains("spawn"))
        {
            SpawnLogic(recognizedText);
            return;
        }
        else if (recognizedText.Contains("destroy"))
        {
            DestroyLogic(recognizedText);
            return;
        }

        else if (recognizedText.Contains("exit"))
        {
            ExitLogic();
            return;
        }
    }

    private void SpawnLogic(string gameObjectType)
    {
        if (gameObjectType.Contains("cylinder"))
        {
            GameObject instantiated = Instantiate(cylinderPrefab, Camera.main.transform.position + (Camera.main.transform.forward * 2), Camera.main.transform.rotation);
            instantiatedCylinders.Add(instantiated);

        }
        else {
            GameObject instantiated = Instantiate(spherePrefab, Camera.main.transform.position + (Camera.main.transform.forward * 2), Camera.main.transform.rotation);
            instantiatedSpheres.Add(instantiated);
        }
    }

    private void DestroyLogic(string gameObjectType)
    {
        if (gameObjectType.Contains("cylinder"))
        {
            int count = instantiatedCylinders.Count;
            if (count > 0)
            {
                GameObject go = instantiatedCylinders[count - 1];
                instantiatedCylinders.Remove(go);
                Destroy(go);
            }
        }
        else 
        {
            int count = instantiatedSpheres.Count;
            if (count > 0)
            {
                GameObject go = instantiatedSpheres[count - 1];
                instantiatedSpheres.Remove(go);
                Destroy(go);
            }
        }
    }

    public void MovementLogic(Direction direction) 
    {
        Debug.Log("Move called");

        moveSpeed = 2f;
        Vector3 moveDirection = new Vector3();
        Quaternion rotation = Camera.main.transform.rotation;
        switch (direction)
        {
            case Direction.Forward:
                moveDirection = rotation * Vector3.forward;
                break;
            case Direction.Back:
                moveDirection = rotation * Vector3.back;
                break;
            case Direction.Left:
                moveDirection = rotation * Vector3.left;
                break;
            case Direction.Right:
                moveDirection = rotation * Vector3.right;
                break;
        }

        playerTransform.position = playerTransform.position + moveDirection * moveSpeed;
        Debug.Log("Move executed");
    }

    public void RotateLogic(Direction direction)
    {
        Debug.Log("Rotate called");
        switch (direction)
        {
            case Direction.Left:
                playerTransform.Rotate(new Vector3(0,-90, 0));
                break;
            case Direction.Right:
                playerTransform.Rotate(new Vector3(0, 90, 0));
                break;
        }
        Debug.Log("Rotate executed");

    }

    public void Stop()
    {
        Debug.Log("Stop called");
        Debug.Log("Stop executed");

    }

    public void ExitLogic()
    {
        Debug.Log("Exit called");

        Application.Quit();
        Debug.Log("Exit executed");

    }
}
