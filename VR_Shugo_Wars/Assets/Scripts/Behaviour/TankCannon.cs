using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCannon : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TankMove.cannon == true)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            TankMove.cannon = false;

        }
    }
}
