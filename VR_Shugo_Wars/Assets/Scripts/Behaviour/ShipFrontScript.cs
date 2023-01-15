using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFrontScript : MonoBehaviour
{
    public GameObject Shipobj;
    Ship_RScript shipScript;

    public GameObject Front_P;

    // Start is called before the first frame update
    void Start()
    {
        shipScript = Shipobj.GetComponent<Ship_RScript>();
    }

    void Update()
    {
        var dir = shipScript.course - Front_P.transform.position;
        dir.Normalize();
        var look = Quaternion.LookRotation(dir); //å¸Ç´ÇïœçXÇ∑ÇÈ
        //look.x = 0;
        //look.z = 0;
        Front_P.transform.rotation = look;
    }

    private void OnTriggerEnter(Collider hitother)
    {
        if (hitother.gameObject.name == Shipobj.name)
        {
            shipScript.movef = false;

            var Others = hitother.gameObject.GetComponent<Ship_RScript>();
            if (Others.movef == false)
            {
                Others.movef = true;
            }
        }
    }

    private void OnTriggerExit(Collider hitother)
    {
        //Debug.Log("x");
        if (hitother.gameObject.name == Shipobj.name)
        {
            Invoke("Movef_t", 2.0f);
        }
    }

    void Movef_t()
    {
        //Debug.Log("m");
        shipScript.movef = true;
    }
}
