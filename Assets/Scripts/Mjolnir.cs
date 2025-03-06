using System;
using System.Collections;
using UnityEngine;

public class Mjolnir : Item {
    [SerializeField] AudioClip recallSound;
    public Player owner;

    IEnumerator IRecall() {
        Audio.clip = recallSound;
        Audio.Play();
        while (!fullyGrabbed)
            yield return new WaitForFixedUpdate();
        Audio.Stop();
        owner = playerHand.GetComponentInParent<Player>();
    }

    public void Recall() {
        print("mjolnir called!!");
        StartCoroutine(IRecall());
    }
}