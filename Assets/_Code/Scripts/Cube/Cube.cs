using TMPro;
using UnityEngine;


public class Cube : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] numberTexts;
    [SerializeField] private MeshRenderer m_MeshRenderer;
    public Collider m_Collider;
    public Rigidbody m_Rigidbody;

    public CubeProperty property;

    private bool isCollided = false;


    public void Setup(CubeProperty property)
    {
        this.property = property;
        // Set the Numbers Text
        foreach (TextMeshProUGUI numberText in numberTexts)
        {
            numberText.text = property.number.ToString();
        }

        // Set the Material
        m_MeshRenderer.material = property.colorMaterial;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
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

                    // Destroy Collided Cubes
                    Destroy(this.gameObject);
                    Destroy(collision.gameObject);
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
