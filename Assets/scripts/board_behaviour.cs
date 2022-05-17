using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public class board_behaviour : MonoBehaviour
{

    public bool Ithaca;
    public bool Troy;
    public bool shift;
    public bool lose;

    public GameObject input_query;
    public GameObject input_field;
    public GameObject count_display;

    public resourde_behaviour resources;
    public ship_behaviour ship;
    public GameObject lost;

    private int step_count;
    private int step_bonus;

    public int portals;

    //0 = waiting for input for reveal
    //1 = reveal input recieved
    //2 = waiting for movement input
    //3 = movement input recieved
    //4 = trade, if possible
    //5 = shift
    //6 = special state for revealing 3 locations
    public int state;

    private bool free_fuel;

    private bool shift_lock;

    private Mutex mut = new Mutex();

    // Start is called before the first frame update
    void Start()
    {
        Ithaca = false;
        Troy = false;
        shift = false;
        lose = false;
        state = 0;
        shift_lock = false;
        step_bonus = 0;
        portals = 0;
        free_fuel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lose)
        {
            lost.SetActive(true);
            this.enabled = false;
        }

        bool show_state = true;

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && (state == 0 || state == 2))
        {
            int input = int.Parse(input_field.GetComponent<TMP_InputField>().text);
            int cost = 1;
            if (input == 2)
            {
                cost = 1;
            }
            else if (input == 3)
            {
                cost = 2;
            }
            else if (input == 4)
            {
                cost = 4;
            }
            else if (input == 5)
            {
                cost = 6;
            }
            else if (input == 6)
            {
                cost = 9;
            }
            else if (input == 7)
            {
                cost = 12;
            }
            else if (input > 7)
            {
                count_display.GetComponent<TextMeshProUGUI>().text = "Please input a value lower than 8";
                show_state = false;
            }

            if (state == 0)
            {
                if (cost <= resources.energ)
                {
                    resources.energ -= cost;
                    step_count = input + step_bonus;
                    input_field.GetComponent<TMP_InputField>().text = "";
                    state++;
                }
                else if (cost <= (resources.energ + (resources.fue / 2)))
                {
                    int overflow = cost - resources.energ;
                    resources.energ = 0;
                    resources.fue -= overflow * 2;
                    step_count = input + step_bonus;
                    input_field.GetComponent<TMP_InputField>().text = "";
                    state++;
                }
            }
            else if (state == 2)
            {
                if (free_fuel)
                {
                    step_count = input;
                    input_field.GetComponent<TMP_InputField>().text = "";
                    state++;
                }
                else if (cost <= resources.fue)
                {
                    resources.fue -= cost;
                    step_count = input + step_bonus;
                    input_field.GetComponent<TMP_InputField>().text = "";
                    state++;
                }
                else if (cost <= (resources.fue + (resources.energ / 2)))
                {
                    int overflow = cost - resources.fue;
                    resources.fue = 0;
                    resources.energ -= overflow * 2;
                    step_count = input + step_bonus;
                    input_field.GetComponent<TMP_InputField>().text = "";
                    state++;
                }
            }
        }

        if ((state == 1 || state == 6) && show_state)
        {
            count_display.GetComponent<TextMeshProUGUI>().text = "reveal charges left: " + step_count.ToString();
        } else if (state == 3 && show_state)
        {
            count_display.GetComponent<TextMeshProUGUI>().text = "moves left: " + step_count.ToString();
        }
        else if ((state == 0 || state == 2) && show_state)
        {
            count_display.GetComponent<TextMeshProUGUI>().text = "Awaiting input";
        }


        //if the board needs to shift
        if (shift)
        {
            shift = false;

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

        if (state == 4)
        {
            if (resources.trade)
            {
                count_display.GetComponent<TextMeshProUGUI>().text = "Space to proceed\n E to gain energy\n F to gain fuel";
            } else
            {
                count_display.GetComponent<TextMeshProUGUI>().text = "Space to proceed\n";
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = 5;
            }
        }

        if (state == 5)
        {
            if (!shift_lock)
            {
                shift = true;
            }
            else
            {
                shift_lock = false;
            }
            state = 0;
        }

    }

    public int Roll_location(int old)
    {
        mut.WaitOne(-1);
        int loc = -1;

        if (old == 1)
        {
            Troy = false;
        } else if (old == 19)
        {
            Ithaca = false;
        } else if (old == 9)
        {
            portals--;
        }

        int min = 2;
        int max = 18;

        if (!Ithaca)
        {
            max = 23;
        }
        if (!Troy)
        {
            min = -1;
        }
        loc = Random.Range(min, max);

        while (loc == 9 && portals > 1)
        {
            loc = Random.Range(min, max);
        }


        if (loc <= 1)
        {
            Troy = true;
            loc = 1;
        } else if (loc >= 19)
        {
            Ithaca = true;
            loc = 19;
        }
        if (loc == 9)
        {
            portals++;
        }
            

        mut.ReleaseMutex();
        return loc;
    }

    public void Lower_Count(int id)
    {
        step_count--;

        if (state == 5)
        {
            state = 0;
        }
        else if (step_count == 0)
        {
            if (state == 3) { Planet_effect(id); }
            state++;
        }
        
    }

    public void Reveal_tile(int ring, int tile)
    {
        transform.GetChild(ring).GetChild(tile).gameObject.GetComponent<tile_behaviour>().reveal = true;
    }

    //Re-rolls each tile
    void Re_roll()
    {
        foreach (Transform child in transform)
        {
            foreach(Transform grand_child in child)
            {
                grand_child.gameObject.GetComponent<tile_behaviour>().Roll_position(grand_child.gameObject.GetComponent<tile_behaviour>().number);
                grand_child.gameObject.GetComponent<tile_behaviour>().reveal = false;
            }
        }
    }

    //transport ship to other portal
    //creates 2nd portal if needed
    void Transport()
    {
        if (portals == 2)
        {
            foreach (Transform child in transform)
            {
                foreach (Transform grand in child)
                {
                    tile_behaviour tile = grand.gameObject.GetComponent<tile_behaviour>();
                    int tile_index = tile.gameObject.transform.GetSiblingIndex();
                    if (tile.number == 9 && tile.ring != ship.ring_index && tile_index != ship.tile_index)
                    {
                        ship.tile_index = tile_index;
                        ship.ring_index = tile.ring;
                    }
                    break;
                }
            }
        } else if (portals == 1)
        {
            int til = Random.Range(0, 7);
            int rin = Random.Range(0, transform.childCount-1);
            while (til == ship.tile_index &&  rin == ship.ring_index) {
                til = Random.Range(0, 7);
                rin = Random.Range(0, transform.childCount - 1);
            }
            transform.GetChild(rin).GetChild(til).gameObject.GetComponent<tile_behaviour>().number = 9;
            ship.ring_index = rin;
            ship.tile_index = til;
            
        }
    }

    void Planet_effect(int id)
    {
        switch (id)
        {
            case 1:
                Half_fuel();
                Half_energy();
                break;
            case 2:
                resources.energ = 0;
                break;
            case 8:
                move_random();
                break;
            case 12:
                shift_lock = true;
                break;
            case 13:
                resources.energ += 3;
                resources.fue += 3;
                break;
            case 14:
                resources.trade = true;
                reveal_adjecent();
                break;
            case 15:
                resources.trade = true;
                reveal_adjecent();
                break;
            default:
                break;
        }
        resources.fue += 3;
        resources.energ += 3;
    }

    public void Hazard_effect(int id, int direction)
    {
        switch(id)
        {
            case 3:
                Half_energy();
                break;
            case 4:
                Half_fuel();
                break;
            case 5:
                ship.shift = true;
                break;
            case 6:
                if (direction == 0)
                {
                    ship.inward = true;
                } else if (direction == 1)
                {
                    ship.clockwise = true;
                } else if (direction == 2)
                {
                    ship.outward = true;
                } else if (direction == 3)
                {
                    ship.counter_wise = true;
                }
                step_count = 0;
                state++;
                break;
            case 7:
                Re_roll();
                break;
            case 9:
                Transport();
                break;
            case 10:
                step_count = 3;
                state = 6;
                break;
            case 11:
                free_fuel = true;
                step_count = 0;
                state++;
                break;
        }
    }

    void Half_fuel()
    {
        int f = (int) Mathf.Floor(resources.fue / 2);
        resources.fue -= f;
    }

    void Half_energy()
    {
        int e = (int)Mathf.Floor(resources.energ / 2);
        resources.energ -= e;
    }

    void move_random()
    {
        int dir = Random.Range(1, 4);
        if (dir == 1)
        {
            ship.ring_index++;
        } else if (dir == 2)
        {
            ship.tile_index++;
        } else if (dir == 3)
        {
            ship.ring_index--;
        } else if (dir == 4)
        {
            if (ship.tile_index == 0)
            {
                ship.tile_index = 7;
            }
            else
            {
                ship.tile_index--;
            }
        }
    }

    void reveal_adjecent()
    {
        int rin = ship.ring_index;
        int out_rin = rin + 1;
        int in_rin = -1;
        if (rin > 1)
        {
            in_rin = rin - 1;
        }

        int til = ship.tile_index;
        int lef_til = til - 1;
        if (lef_til == -1)
        {
            lef_til = 7;
        }
        int rig_til = til + 1;
        if (rig_til == 8)
        {
            rig_til = 0;
        }

        if (in_rin != -1)
        {
            transform.GetChild(in_rin).GetChild(til).gameObject.GetComponent<tile_behaviour>().reveal = true;
        }
        transform.GetChild(rin).GetChild(lef_til).gameObject.GetComponent<tile_behaviour>().reveal = true;
        transform.GetChild(rin).GetChild(rig_til).gameObject.GetComponent<tile_behaviour>().reveal = true;
        transform.GetChild(out_rin).GetChild(til).gameObject.GetComponent<tile_behaviour>().reveal = true;
    }
}
