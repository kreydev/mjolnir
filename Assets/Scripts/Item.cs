using System;
using UnityEngine;

public class Item : MonoBehaviour {
    public int count;
    public float handScale;
    public float worldScale;
    public bool inHand;

    void FixedUpdate() {
        if (inHand) {
            transform.localScale = new Vector3(handScale, handScale, handScale);
        } else {
            transform.localScale = new Vector3(worldScale, worldScale, worldScale);
        }
    }
}

[Serializable] public struct ItemDrop {
    public Item item;
    public int count;
}