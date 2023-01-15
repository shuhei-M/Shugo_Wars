using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierRifle : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SoldierMove.gun == true)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
}
