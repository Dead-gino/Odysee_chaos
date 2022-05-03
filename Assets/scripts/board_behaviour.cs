using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class board_behaviour : MonoBehaviour
{

    public bool Ithaca;
    public bool Troy;
    public bool shift;
    public bool lose;

    private Mutex mut = new Mutex();

    // Start is called before the first frame update
    void Start()
    {
        Ithaca = false;
        Troy = false;
        shift = false;
        lose = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shift = true;
        }

        //if the board needs to shift
        if (shift)
        {
            shift = false;

            bool sibling = true;
            //shift each ring on the board once
            foreach (Transform child in transform)
            {
                ring_behaviour behav = child.gameObject.GetComponent<ring_behaviour>();
                if (behav != null)
                {
                    behav.shift = true;
                   
                }
            }

            this.transform.GetChild(0).SetAsLastSibling();

            ship_behaviour ship = this.transform.parent.GetChild(0).GetComponent<ship_behaviour>();
            if (ship != null)
            {
                ship.shift = true;
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
        int max = 20;

        if (!Ithaca)
        {
            max = 23;
        }
        if (!Troy)
        {
            min = -1;
        }
        loc = Random.Range(min, max);


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
