using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tile_behaviour : MonoBehaviour
{
    public Transform center;
    public int ring;
    public bool shift;
    public bool reveal;
    private Transform position;
    private TextMeshPro text;
    private bool rolled;
    public bool clicked;
    public int ring_amount;


    public int number;


    public GameObject Troy;
    public GameObject Ithaca;
    public GameObject Scheria;
    public GameObject Djerba;
    public GameObject Sparta;
    public GameObject Pylos;
    public GameObject Ogygia;
    public GameObject Aeaea;
    public GameObject Laestrygonia;
    public GameObject Sirens;
    public GameObject Wormhole;
    public GameObject Blockade;
    public GameObject Portal;
    public GameObject Wasteland;
    public GameObject Anomaly;
    public GameObject Olympus;
    public GameObject Cloud;
    public GameObject Underworld;
    public GameObject Asteroid;

    private board_behaviour board;


    // Start is called before the first frame update
    void Start()
    {
        position = this.transform;
        shift = false;
        reveal = false;
        number = -1;
        text = this.transform.GetChild(0).GetComponent<TextMeshPro>();
        rolled = false;
        board = this.transform.parent.parent.gameObject.GetComponent<board_behaviour>();
        clicked = false;
        
        Troy.SetActive(false);
        Ithaca.SetActive(false);
        Scheria.SetActive(false);
        Djerba.SetActive(false);
        Sparta.SetActive(false);
        Pylos.SetActive(false);
        Ogygia.SetActive(false);
        Aeaea.SetActive(false);
        Laestrygonia.SetActive(false);
        Sirens.SetActive(false);
        Wormhole.SetActive(false);
        Blockade.SetActive(false);
        Portal.SetActive(false);
        Wasteland.SetActive(false);
        Anomaly.SetActive(false);
        Olympus.SetActive(false);
        Cloud.SetActive(false);
        Underworld.SetActive(false);
        Asteroid.SetActive(false);
        
        Roll_position(-1);
    }

    // Update is called once per frame
    void Update()
    {
        

        //display the selected number
        Display_num(number);

        //if it should shift in one ring, do so
        if(shift)
        {
            shift = false;
            Shift_in();
        }
        
        //make the text visible or not is needed
        bool visible = text.enabled;
        if (reveal && !visible)
        {
            text.enabled = true;
        } else if (!reveal && visible)
        {
            text.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (!clicked && !reveal && (board.state == 1 || board.state == 6))
        {
            if (number == 16)
            {
                board.step_bonus = 1;
            } else if (number == 17)
            {
                board.free_fuel = true;
            } else if (number == 18)
            {
                board.Raise_Count();
            }
            clicked = true;
            reveal = true;
            board.Lower_Count(number);
        }
    }
    private void OnMouseUp()
    {
        clicked = false;
    }

    // Mouse hover text

    private void OnMouseOver() 
    {
       if (reveal == true)
        {
            switch (number)
            {
                case 1:  //troy
                    Troy.SetActive(true);
                    break;                
                case 2:  //Laestrygonia
                    Laestrygonia.SetActive(true);
                    break;
                case 3: //Wasteland
                    Wasteland.SetActive(true);
                    break;
                case 4: //Asteroid
                    Asteroid.SetActive(true);
                    break;
                case 5: //Anomaly
                    Anomaly.SetActive(true);
                    break;
                case 6: //Blockade
                    Blockade.SetActive(true);
                    break;
                case 7: //Wormhole
                    Wormhole.SetActive(true);
                    break;
                case 8: //Sirens
                    Sirens.SetActive(true);
                    break;
                case 9: //Portal
                    Portal.SetActive(true);
                    break;
                case 10:  //Ogygia
                    Ogygia.SetActive(true);
                    break;
                case 11:  //Aeaea
                    Aeaea.SetActive(true);
                    break;
                case 12:  //Djerba
                    Djerba.SetActive(true);
                    break;
                case 13:  //Scheria
                    Scheria.SetActive(true);
                    break;
                case 14:  //Sparta
                    Sparta.SetActive(true);
                    break;
                case 15:  //Pylos
                    Pylos.SetActive(true);
                    break;
                case 16: //Olympus
                    Olympus.SetActive(true);
                    break;
                case 17: //Cloud
                    Cloud.SetActive(true);
                    break;
                case 18: //Underworld
                    Underworld.SetActive(true);
                    break;
                case 19:  //Ithaca
                    Ithaca.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnMouseExit() 
    {
        Troy.SetActive(false);
        Ithaca.SetActive(false);
        Scheria.SetActive(false);
        Djerba.SetActive(false);
        Sparta.SetActive(false);
        Pylos.SetActive(false);
        Ogygia.SetActive(false);
        Aeaea.SetActive(false);
        Laestrygonia.SetActive(false);
        Sirens.SetActive(false);
        Wormhole.SetActive(false);
        Blockade.SetActive(false);
        Portal.SetActive(false);
        Wasteland.SetActive(false);
        Anomaly.SetActive(false);
        Olympus.SetActive(false);
        Cloud.SetActive(false);
        Underworld.SetActive(false);
        Asteroid.SetActive(false);
    }

    // Shift the tile in one ring
    // if it would enter the center, dissable for now.
    void Shift_in()
    {
        Vector3 direction = this.transform.position - center.position;
        direction = direction / ring;

        if (ring == 1)
            {
            this.transform.Translate(direction * (ring_amount - 1));
            reveal = false;
            Roll_position(number);
            }
        else
            {
            this.transform.Translate(-direction);
            }
    }

    // Display the given float as text on the tile
    void Display_num(float num)
    {
        string text_num = num.ToString();
        text.text = text_num;
    }

    public void Roll_position(int n)
    {
        int num = board.Roll_location(n);
        number = num;
    }
}
