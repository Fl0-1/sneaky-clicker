using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void Awake()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
