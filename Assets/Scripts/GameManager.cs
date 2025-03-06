using System;
using UnityEngine;
using rand = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Game load settings")]
    public Player playerPrefab;
    public ItemDrop[] spawnItems;
    public Vector2 xBounds;
    public Vector2 zBounds;

    [Header("Runtime variables")]
    public Player player;

    void Start() {
        player = Instantiate(playerPrefab);
        foreach (ItemDrop item in spawnItems) {
            for (int i = 0; i < item.count; ++i) {
                GameObject itemGO = Instantiate(item.item).gameObject;
                itemGO.transform.position = new Vector3(rand.Range(xBounds.x, xBounds.y), 3, rand.Range(zBounds.x, zBounds.y));
            }
        }
    }
}