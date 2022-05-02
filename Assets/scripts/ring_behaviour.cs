using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ring_behaviour : MonoBehaviour
{
    public bool shift;
    public List<GameObject> tiles;

    // Start is called before the first frame update
    void Start()
    {
        shift = false;

        tiles = new List<GameObject>();
        foreach (Transform child in transform)
        {
            GameObject chil = child.gameObject;
            tiles.Add(chil);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if the ring needs to shift
        if (shift)
        {
            shift = false;
            //shift each tile in the ring in once
            foreach (Transform child in transform)
            {
                GameObject chil = child.gameObject;
                tile_behaviour behav = (tile_behaviour)chil.GetComponent("tile_behaviour");
                behav.shift = true;
            }
        }
    }
}
