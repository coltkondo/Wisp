using UnityEngine;

public class Floater : MonoBehaviour
{
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public LayerMask groundLayer;

    private Vector3 posOffset = new Vector3();
    private Vector3 tempPos = new Vector3();
    private bool isGrounded = false;

    private Rigidbody rb;

    void Start()
    {
        posOffset = transform.position;

        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        if (isGrounded)
        {
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            transform.position = tempPos;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer & (1 << collision.gameObject.layer)) != 0)
        {
            isGrounded = true;
            rb.isKinematic = true;
            posOffset = transform.position;
        }
    }
}
