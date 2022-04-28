using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile_behaviour : MonoBehaviour
{
    public Transform center;
    public int ring;
    public bool shift;
    private Transform position;

    // Start is called before the first frame update
    void Start()
    {
        position = this.transform;
        shift = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(shift)
        {
            shift = false;
            Shift_in();
        }
    }

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
}
