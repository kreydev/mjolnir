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
    public Transform hand;
    
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
        if (grounded) leaveMovm = new(movm.x, movm.y);
        if (Input.GetKey(KeyCode.Space) && grounded) {
            grounded = false;
            rb.AddForce(0, jumpAmount, 0, ForceMode.Impulse);
        }
        rb.linearVelocity = new(0, rb.linearVelocity.y, 0);
        rb.linearVelocity += (transform.forward * (grounded ? movm.y : leaveMovm.y) + transform.right * (grounded ? movm.x : leaveMovm.x)) * speed;
        if (Input.GetKeyDown(KeyCode.E) && holding == null) {
            foreach (Item item in FindObjectsByType<Item>(FindObjectsSortMode.None)) {
                if (Vector3.Distance(item.transform.position, transform.position) < 2) {
                    item.BindHand(this);
                    print("Binding item: " + item.name);
                    break;
                }
            }
        } if (Input.GetKeyDown(KeyCode.Q) && holding != null) {
            print("Unbinding item: " + holding.name);
            holding.UnbindHand(this);
        }
        if (Input.GetKeyDown(KeyCode.R) && holding == null) {
            foreach (var hammer in FindObjectsByType<Mjolnir>(0)) {
                if (hammer.owner != null) {
                    hammer.BindHand(this);
                    break;
                }
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "floor") {
            grounded = true;
        }
    }
}
