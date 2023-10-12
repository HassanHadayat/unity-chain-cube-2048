using System;
using UnityEngine;


public class CubeController : MonoBehaviour
{
    public static CubeController Instance;

    [SerializeField] private GameObject cubePrefab;

    public GameObject cube;
    public GameObject prevCube;
    public float maxX;
    public float minX;

    Touch currTouch;
    public bool canMove = true;
    public bool isInstantiating = false;
    public Transform dynamic;

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
        cube = Instantiate(cubePrefab, transform);
        cube.GetComponent<Cube>().Setup();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && canMove && cube)
        {
            // Get Touch
            currTouch = Input.GetTouch(0);

            // If touch Move
            if (currTouch.phase == TouchPhase.Moved)
            {
                // move cube across the horizontal axis
                float deltaX = currTouch.deltaPosition.x;

                cube.transform.position += Vector3.right * deltaX * Time.deltaTime;

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
                canMove = false;

                Rigidbody cubeRb = cube.GetComponent<Rigidbody>();
                cubeRb.useGravity = false;
                cubeRb.velocity = Vector3.zero;
                cubeRb.AddForce(Vector3.forward * 10, ForceMode.Impulse);

                Invoke("ExcludeCube", 0.7f);
            }
        }
    }

    public void ExcludeCube()
    {
        if (cube)
        {
            cube.GetComponent<Rigidbody>().useGravity = true;
            cube.transform.parent = dynamic;
            cube = null;
        }
        canMove = true;
        cube = Instantiate(cubePrefab, transform);
        cube.GetComponent<Cube>().Setup();
    }
    public static int check = 0;
    public void InstantiateCube(Vector3 position, int numberPower)
    {
        if (!isInstantiating)
        {
            isInstantiating = true;
            Debug.Log(Time.realtimeSinceStartup.ToString());
            GameObject cubeGO = Instantiate(cubePrefab, position, Quaternion.identity, dynamic);
            cubeGO.GetComponent<Cube>().Setup(numberPower);

            cubeGO.GetComponent<Rigidbody>().AddForce(Vector3.up * (1.25f * numberPower), ForceMode.Impulse);
            Vector3 randomTorque = UnityEngine.Random.insideUnitSphere * .25f;
            cubeGO.GetComponent<Rigidbody>().AddTorque(randomTorque, ForceMode.Impulse);

            isInstantiating = false;
        }

    }
}
