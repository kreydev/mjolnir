using Unity.Mathematics;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Item holding;
    public float speed;
    public float lookSpeed;
    public float tiltAmount;
    public float jumpAmount;
    
    Vector2 movm;
    Vector2 mouse;
    Transform cam;
    Rigidbody rb;
    bool grounded;
    Vector2 leaveMovm;

    void Start() {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        mouse = Input.mousePositionDelta;
        movm = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.eulerAngles = new(0, transform.eulerAngles.y + mouse.x * lookSpeed * Time.deltaTime, 0);
        cam.localEulerAngles = new(cam.localEulerAngles.x - mouse.y * lookSpeed * Time.deltaTime, 0, movm.x * tiltAmount);
        // transform.Translate(Vector3.forward * movm.y * speed * Time.deltaTime);
        // transform.Translate(Vector3.right * movm.x * speed * Time.deltaTime);
        rb.linearVelocity = new(0, rb.linearVelocity.y, 0);
        rb.linearVelocity += (transform.forward * movm.y + transform.right * movm.x) * speed;
        if (Input.GetKey(KeyCode.Space) && grounded) {
            grounded = false;
            rb.AddForce(0, jumpAmount, 0, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "floor") {
            grounded = true;
            leaveMovm = new(movm.x, movm.y);
        }
    }
}
