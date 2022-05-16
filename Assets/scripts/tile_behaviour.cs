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
    private bool clicked;
    public int ring_amount;

    [Range(1, 21)]
    public int number;

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
        if (!clicked && !reveal && board.state == 1)
        {
            clicked = true;
            reveal = true;
            board.Lower_Count();
        }
    }
    private void OnMouseUp()
    {
        clicked = false;
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

    void Roll_position(int n)
    {
        int num = board.Roll_location(n);
        number = num;
    }
}
