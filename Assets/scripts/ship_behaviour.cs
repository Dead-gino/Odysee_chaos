using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship_behaviour : MonoBehaviour
{

    public Transform tile;
    public bool move;

    // Start is called before the first frame update
    void Start()
    {
        move = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            move = false;
            this.transform.position = tile.position;
        }
    }
}
