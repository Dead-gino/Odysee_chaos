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

    [Range(0.0f, 20.0f)]
    public float number;


    // Start is called before the first frame update
    void Start()
    {
        position = this.transform;
        shift = false;
        reveal = false;
        number = 0;
        text = this.transform.GetChild(0).GetComponent<TextMeshPro>();
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

    // Shift the tile in one ring
    // if it would enter the center, dissable for now.
    void Shift_in()
    {
        if (ring <= 1)
            {
            this.gameObject.SetActive(false);
            }
        else
            {
            Vector3 direction = this.transform.position - center.position;
            direction = direction / ring;
            this.transform.Translate(-direction);
            ring--;
            }
    }

    // Display the given float as text on the tile
    void Display_num(float num)
    {
        int val = (int) num;
        string text_num = val.ToString();
        text.text = text_num;
    }
}