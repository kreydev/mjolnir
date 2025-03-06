using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour {
    public string id;
    public int count;
    public float handScale;
    public float worldScale;
    public Transform playerHand;
    public float throwPower;
    public bool fullyGrabbed;
    public UnityEvent bindEvents;

    [SerializeField] protected AudioClip hitSound;
    protected AudioSource Audio;
    protected Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        Audio = GetComponent<AudioSource>();
    }

    IEnumerator LerpToHand() {
        while (Vector3.Distance(transform.position, playerHand.position) > .1f) {
            transform.position = Vector3.Lerp(transform.position, playerHand.position, Mathf.Clamp(1/Vector3.Distance(transform.position, playerHand.position), 0, 0.2f));
            transform.rotation = Quaternion.Lerp(transform.rotation, playerHand.rotation, .2f);
            yield return new WaitForFixedUpdate();
        }
        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;
        foreach (var col in GetComponents<Collider>()) {
            col.enabled = false;
        }
        fullyGrabbed = true;
    }

    public void BindHand(Player player) {
        playerHand = player.hand;
        player.holding = this;
        transform.SetParent(playerHand);
        rb.isKinematic = true;
        transform.localScale = new Vector3(handScale, handScale, handScale);
        bindEvents.Invoke();
        StartCoroutine(LerpToHand());
    }

    public void UnbindHand(Player player) {
        fullyGrabbed = false;
        playerHand = null;
        transform.SetParent(null);
        player.holding = null;
        rb.isKinematic = false;
        transform.localScale = new Vector3(worldScale, worldScale, worldScale);
        foreach (var col in GetComponents<Collider>()) {
            col.enabled = true;
        }
        rb.AddForce(transform.forward * throwPower, ForceMode.Impulse);
    }

    void OnCollisionEnter() {
        Audio.pitch = Random.Range(.9f, 1.1f);
        Audio.PlayOneShot(hitSound);
    }
}

[Serializable] public struct ItemDrop {
    public Item item;
    public int count;
}