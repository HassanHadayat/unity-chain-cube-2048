using TMPro;
using UnityEngine;


public class Cube : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] m_NumberTexts;
    [SerializeField] private MeshRenderer m_MeshRenderer;
    public LineRenderer m_LineRenderer;
    public TrailRenderer m_TrailRenderer;
    public Collider m_Collider;
    public Rigidbody m_Rigidbody;
    public Animator m_Animator;

    public CubeProperty property;

    private bool isCollided = false;

    private bool isLineRendererON = false;



    private void Update()
    {
        if (isLineRendererON)
        {
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, transform.position + transform.forward * 8f);
        }
    }

    public void TurnOnLineRenderer()
    {
        isLineRendererON = true;
        m_LineRenderer.enabled = true;
    }
    public void TurnOffLineRenderer()
    {
        isLineRendererON = false;
        m_LineRenderer.enabled = false;
    }
    public void TurnOnTrailRenderer()
    {
        m_TrailRenderer.enabled = true;
    }
    public void TurnOffTrailRenderer()
    {
        m_TrailRenderer.enabled = false;
    }

    public void Setup(CubeProperty property, bool isLineRenderer = false)
    {
        this.property = property;
        // Set the Numbers Text
        foreach (TextMeshProUGUI numberText in m_NumberTexts)
        {
            numberText.text = property.number.ToString();
        }

        // Set the Material
        m_MeshRenderer.material = property.colorMaterial;

        if (isLineRenderer)
            TurnOnLineRenderer();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            // Trun Trail OFF
            TurnOffTrailRenderer();

            // Check if same numbers cube collided
            if (this.property.number == collision.collider.GetComponent<Cube>().property.number && !isCollided)
            {
                isCollided = true;

                // Give access to only Latest Instantiated Cube to Perform Collsion Actions
                if (this.gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    // Get the middle Pos for the new Cube
                    Vector3 newInstantiatePos = collision.contacts[0].point + new Vector3(0, 0.35f, .35f);

                    // Get the new number power
                    int newNumberPower = (int)Mathf.Log(this.property.number, 2) + 1;
                    // Ask Generator to Generate new Cube
                    CubeGenerator.Instance.CreateCube(newInstantiatePos, newNumberPower);

                    // Remove Collided Cubes from Number Cubes List
                    CubeGenerator.Instance.RemoveCube(property.number, this.gameObject);
                    CubeGenerator.Instance.RemoveCube(collision.collider.GetComponent<Cube>().property.number, collision.gameObject);
                }

            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BoundryLine"))
        {

            // Trigger OFF & Gravity ON
            this.GetComponent<Collider>().isTrigger = false;
            this.GetComponent<Rigidbody>().useGravity = true;


            // Player Cube thrown Trigger
            if (this.CompareTag("PlayerCube"))
            {
                // Change the tag of Player Cube -> Cube
                this.tag = "Cube";

                // Remove the reference from Cube Controller
                CubeController.Instance.RemoveCubeInHand();

                // Add Cube to the Number Cubes List
                CubeGenerator.Instance.AddCube(property.number, this.gameObject);
            }
            // Cube Trigger the Finish Line
            else if (this.CompareTag("Cube"))
            {
                // GAME OVER
                Debug.Log("Finished");
                Time.timeScale = 0;
            }

        }
    }
}
