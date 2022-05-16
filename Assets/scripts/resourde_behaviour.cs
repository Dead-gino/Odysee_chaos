using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class resourde_behaviour : MonoBehaviour
{

    public int energ;
    public int fue;

    // Start is called before the first frame update
    void Start()
    {
        fue = energ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Display_resources(fue, energ);


        //automate to be dependant on location
        if (Input.GetKeyDown(KeyCode.F))
        {
            fue++;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            energ++;
        }
    }

    void Display_resources(int fuel, int energy)
    {
        string fuel_s = "Fuel:\t\t";
        string energy_s = "Energy:\t";
        string new_line = "\n";
        string combined = fuel_s + fuel.ToString() + new_line + energy_s + energy.ToString();
        this.gameObject.GetComponent<TextMeshProUGUI>().text = combined;

    }
}
