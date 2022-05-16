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

    private int step_count;

    //0 = waiting for input for reveal
    //1 = reveal input recieved
    //2 = waiting for movement input
    //3 = movement input recieved
    public int state;

    private Mutex mut = new Mutex();

    // Start is called before the first frame update
    void Start()
    {
        Ithaca = false;
        Troy = false;
        shift = false;
        lose = false;
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shift = true;
        }

        bool show_state = true;

        if (Input.GetKeyDown(KeyCode.KeypadEnter) && (state == 0 || state == 2)) {
            int input = int.Parse(input_field.GetComponent<TMP_InputField>().text);
            int cost = 1;
            if (input == 2)
            {
                cost = 1;
            } else if (input == 3)
            {
                cost = 2;
            } else if (input == 4)
            {
                cost = 4;
            } else if (input == 5)
            {
                cost = 6;
            } else if (input == 6)
            {
                cost = 9;
            } else if (input == 7)
            {
                cost = 12;
            } else if (input > 7)
            {
                count_display.GetComponent<TextMeshProUGUI>().text = "Please input a value lower than 8";
                show_state = false;
            }

            if (state == 0)
            {
                if (cost <= resources.energ)
                {
                    resources.energ -= cost;
                    step_count = input;
                    input_field.GetComponent<TMP_InputField>().text = "";
                    state++;
                }
                else if (cost <= (resources.energ + (resources.fue / 2)))
                {
                    int overflow = cost - resources.energ;
                    resources.energ = 0;
                    resources.fue -= overflow * 2;
                    step_count = input;
                    input_field.GetComponent<TMP_InputField>().text = "";
                    state++;
                }
            } else if (state == 2)
            {
                if (cost <= resources.fue)
                {
                    resources.fue -= cost;
                    step_count = input;
                    input_field.GetComponent<TMP_InputField>().text = "";
                    state++;
                }
                else if (cost <= (resources.fue + (resources.energ / 2)))
                {
                    int overflow = cost - resources.fue;
                    resources.fue = 0;
                    resources.energ -= overflow * 2;
                    step_count = input;
                    input_field.GetComponent<TMP_InputField>().text = "";
                    state++;
                }
            }
        }

        if ((state == 1 || state == 3) && show_state) 
        {
            count_display.GetComponent<TextMeshProUGUI>().text = "moves left: " + step_count.ToString();
        } else if ((state == 0 || state == 2) && show_state) 
        {
            count_display.GetComponent<TextMeshProUGUI>().text = "Awaiting input";
        }


        //turn into something automated dependant on location
        if (Input.GetKeyDown(KeyCode.Q) && (state == 1 || state == 3))
        {
            step_count = 1;
            Lower_Count();
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

    public void Lower_Count()
    {
        step_count--;
        if (step_count == 0)
        {
            state++;
        }
        if (state == 4)
        {
            state = 0;
        }
    }

    public void Reveal_tile(int ring, int tile)
    {
        this.transform.GetChild(ring).GetChild(tile).gameObject.GetComponent<tile_behaviour>().reveal = true;
    }
}
