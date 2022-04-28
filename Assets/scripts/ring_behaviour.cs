using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ring_behaviour : MonoBehaviour
{
    public bool shift;

    // Start is called before the first frame update
    void Start()
    {
        shift = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shift)
        {
            shift = false;
            foreach (Transform child in transform)
            {
                GameObject chil = child.gameObject;
                tile_behaviour behav = (tile_behaviour)chil.GetComponent("tile_behaviour");
                behav.shift = true;
            }
        }
    }
}
