using TMPro;
using UnityEngine;



public class Cube : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] numberTexts;

    [SerializeField] private MeshRenderer m_MeshRenderer;

    public CubeProperty property;

    private bool isCollided = false;

    private void Start()
    {
        //Setup();
    }
    public void Setup()
    {
        property = CubePropertyGenerator.Instance.CreateCubeProperty();

        foreach (TextMeshProUGUI numberText in numberTexts)
        {
            numberText.text = property.number.ToString();
        }

        m_MeshRenderer.material = property.colorMaterial;
    }
    public void Setup(int numberPower)
    {
        property = CubePropertyGenerator.Instance.CreateCubeProperty(numberPower);

        //Debug.Log("Testing => 1 => " + property.number);

        foreach (TextMeshProUGUI numberText in numberTexts)
        {
            numberText.text = property.number.ToString();
            //Debug.Log("Testing => 2 => " + numberText.text);
        }

        m_MeshRenderer.material = property.colorMaterial;
        //Time.timeScale = 0;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Cube"))
        {
            if (this.property.number == collision.collider.GetComponent<Cube>().property.number && !isCollided)
            {
                isCollided = true;

                if (this.gameObject.GetInstanceID() > collision.gameObject.GetInstanceID())
                {

                    Vector3 newPos = collision.contacts[0].point + new Vector3(0, 0.35f, .35f);

                    CubeController.Instance.InstantiateCube(newPos, (int)Mathf.Log(this.property.number,2) + 1);
                }

                Destroy(this.gameObject);
            }
        }
    }
}
