using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship_behaviour : MonoBehaviour
{

    public Transform tile;
    public bool move;
    public int ring_index, tile_index;
    public int ring_count, tile_count;
    public board_behaviour board;
    public bool inward, outward, clockwise, counter_wise;

    // Start is called before the first frame update
    void Start()
    {
        move = true;
        ring_count = board.transform.childCount - 3;
        tile_count = board.transform.GetChild(0).childCount - 1;
        inward = outward = clockwise = counter_wise = false;
    }

    // Update is called once per frame
    void Update()
    {
        Check_pos();

        if (Input.GetKeyDown(KeyCode.W))
        {
            outward = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            inward = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            counter_wise = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            clockwise = true;
        }

        if (move)
        {
            tile = board.transform.GetChild(ring_index).GetChild(tile_index);
            //move = false;
            this.transform.position = tile.position;
        }
    }

    void Check_pos()
    {
        if (inward)
        {
            inward = false;
            if (ring_index > 0)
            {
                ring_index--;
            }

        }

        if (outward)
        {
            outward = false;
            if (ring_index < ring_count)
            {
                ring_index++;
            }
        }

        if (clockwise)
        {
            clockwise = false;
            if (tile_index == tile_count)
            {
                tile_index = 0;
            }
            else
            {
                tile_index++;
            }
        }

        if (counter_wise)
        {
            counter_wise = false;
            if (tile_index == 0)
            {
                tile_index = tile_count;
            }
            else
            {
                tile_index--;
            }
        }
    }
}
