using System;
using UnityEngine;


public class CubeController : MonoBehaviour
{
    public static CubeController Instance;

    public float maxX;
    public float minX;

    public float shootForce = 15f;
    public Transform dynamicTrans;

    private GameObject cube;
    private bool canMoveFinger = true;
    private Touch currTouch;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }
    
    private void Start()
    {
        // Initial Cube
        cube = CubeGenerator.Instance.CreatePlayerCube(transform);
    }

    private void Update()
    {
        if (Input.touchCount > 0 && canMoveFinger && cube)
        {
            // Get Touch
            currTouch = Input.GetTouch(0);

            // If touch Move
            if (currTouch.phase == TouchPhase.Moved)
            {
                // get the finger moved distance
                float deltaX = currTouch.deltaPosition.x;

                // move cube across the horizontal axis
                cube.transform.position += Vector3.right * deltaX * Time.deltaTime;

                // check the horizontal limits of cube
                if (cube.transform.position.x > maxX)
                {
                    Vector3 newPos = cube.transform.position;
                    newPos.x = maxX;
                    cube.transform.position = newPos;
                }
                else if (cube.transform.position.x < minX)
                {
                    Vector3 newPos = cube.transform.position;
                    newPos.x = minX;
                    cube.transform.position = newPos;
                }

            }
            else if (currTouch.phase == TouchPhase.Ended)
            {
                // Stop Finger Movement
                canMoveFinger = false;

                // Shoot the Cube in Hand
                ShootCube();
            }
        }
    }

    public void ShootCube()
    {
        // Get the Rigidbody
        Rigidbody cubeRb = cube.GetComponent<Rigidbody>();

        // Apply Shoot Force
        cubeRb.velocity = Vector3.zero;
        cubeRb.AddForce(Vector3.forward * shootForce, ForceMode.Impulse);
    }

    public void RemoveCubeInHand()
    {
        if (cube)
        {
            // Move it to DYNAMIC
            cube.transform.parent = dynamicTrans;

            // Get New Cube From Cube Generator
            cube = null;
            cube = CubeGenerator.Instance.CreatePlayerCube(transform);

            // Resume Finger Movement
            canMoveFinger = true;
        }
    }
}
