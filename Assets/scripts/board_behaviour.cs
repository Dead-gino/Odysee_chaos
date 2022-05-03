using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class board_behaviour : MonoBehaviour
{

    public bool Ithaca;
    public bool Troy;
    public bool shift;

    private Mutex mut = new Mutex();

    // Start is called before the first frame update
    void Start()
    {
        Ithaca = false;
        Troy = false;
        shift = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if the board needs to shift
        if (shift)
        {
            shift = false;
            //shift each ring on the board once
            foreach (Transform child in transform)
            {
                GameObject chil = child.gameObject;
                ring_behaviour behav = (ring_behaviour)chil.GetComponent<ring_behaviour>();
                behav.shift = true;
            }
        }
    }

    public int Roll_location(int old)
    {
        mut.WaitOne(-1);
        int loc = -1;

        if (old == 1)
        {
            Troy = false;
        } else if (old == 20)
        {
            Ithaca = false;
        }

        int min = 2;
        int max = 19;

        if (!Ithaca)
        {
            max = 20;
        } else if (!Troy)
        {
            min = 1;
        } else
        {
            loc = Random.Range(min, max);
        }

        if (loc <= 1)
        {
            Troy = true;
            loc = 1;
        } else if (loc >= 20)
        {
            Ithaca = true;
            loc = 20;
        }
            

        mut.ReleaseMutex();
        return loc;
    }
}
