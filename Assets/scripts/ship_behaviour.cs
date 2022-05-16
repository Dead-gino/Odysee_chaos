using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ship_behaviour : MonoBehaviour
{

    public Transform tile, center;
    public bool move;
    public int ring_index, tile_index;
    public int ring_count, tile_count;
    public board_behaviour board;
    private bool inward, outward, clockwise, counter_wise;
    public bool shift;

    // Start is called before the first frame update
    void Start()
    {
        move = true;
        shift = false;
        ring_count = board.transform.childCount - 1;
        tile_count = board.transform.GetChild(0).childCount - 1;
        inward = outward = clockwise = counter_wise = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (board.state == 3)
        {
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
        }

        Check_pos();

        if (move)
        {
            if (ring_index == -1)
            {
                tile = center;
            }
            else
            {
                tile = board.transform.GetChild(ring_index).GetChild(tile_index);
            }
            //move = false;
            this.transform.position = tile.position;
        }

        if (shift)
        {
            shift = false;
            if (ring_index == 0)
            {
                ring_index = -1;
                board.lose = true;
            } else
            {
                inward = true;
            }
        }

        board.Reveal_tile(ring_index, tile_index);
    }

    void Check_pos()
    {
        if (inward)
        {
            inward = false;
            if (ring_index > 0)
            {
                ring_index--;
                board.Lower_Count();
            }

        }

        if (outward)
        {
            outward = false;
            if (ring_index < ring_count)
            {
                ring_index++;
                board.Lower_Count();
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
            board.Lower_Count();
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
            board.Lower_Count();
        }
    }
}
