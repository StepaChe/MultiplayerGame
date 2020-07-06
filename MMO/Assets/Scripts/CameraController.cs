using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform m_target;
    [SerializeField] Vector3 offset;
    [SerializeField] float zoomSpeed = 4f;
    [SerializeField] float minZoom = 5f;
    [SerializeField] float maxZoom = 15f;
    [SerializeField] float pitch = 2f;

    Transform m_transform;

    float currentZoom = 10f;
    float currentRotX = 0f;
    float currentRotY = 0f;
    float prevMouseX;
    float prevMouseY;

    public Transform target { set { m_target = value; } }

    void Start()
    {
        m_transform = transform;
    }

    void Update()
    {
        if (m_target != null)
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            if (Input.GetMouseButton(2))
            {
                currentRotX += Input.mousePosition.x - prevMouseX;
                //currentRotY += Input.mousePosition.y - prevMouseY;
            }
        }

        prevMouseX = Input.mousePosition.x;
        //prevMouseY = Input.mousePosition.y;
    }

    void LateUpdate()
    {
        if (m_target != null)
        {
            m_transform.position = m_target.position - offset * currentZoom;
            m_transform.LookAt(m_target.position + Vector3.up * pitch);
            m_transform.RotateAround(m_target.position, Vector3.up, currentRotX);
            //m_transform.RotateAround(m_target.position, Vector3.forward, currentRotY);
        }
    }
}
