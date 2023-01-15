using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySample : EnemyParent
{
    //[SerializeField] float blownPower = 10f;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ‚Á”ò‚ñ‚Å‚éÅ’†‚©“–‚½‚Á‚½‚à‚Ì‚ªèˆÈŠO‚Å‚ ‚ê‚ÎˆÈ‰º‚Ìˆ—‚ğ‚µ‚È‚¢
        if (collision.gameObject.tag == "HandCapsuleRigidbody" && !IsBlownAway)
        {
            BlownAway(collision.gameObject);
        }
    }
}
