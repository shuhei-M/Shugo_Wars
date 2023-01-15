using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTestEnemy : EnemyParent
{
    #region
    private HandsSetUpController handsSet;
    #endregion


    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        handsSet = GameObject.Find("HandSetUpPanel").GetComponent<HandsSetUpController>();

        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // êÅÇ¡îÚÇÒÇ≈ÇÈç≈íÜÇ©ìñÇΩÇ¡ÇΩÇ‡ÇÃÇ™éËà»äOÇ≈Ç†ÇÍÇŒà»â∫ÇÃèàóùÇÇµÇ»Ç¢
        if (collision.gameObject.tag == "HandCapsuleRigidbody" && !IsBlownAway)
        {
            HandSetUpOK(collision.gameObject);
            BlownAway(collision.gameObject);
        }
    }
    #endregion


    #region private function
    private void HandSetUpOK(GameObject gameObject)
    {
        var handObj = gameObject.transform.parent.parent.gameObject;
        if (handObj.name == "LeftOVRHandPrefab")
        {
            handsSet.IsLeftHandSet = true;
        }
        else if (handObj.name == "RightOVRHandPrefab")
        {
            handsSet.IsRightHandSet = true;
        }
        else
        {
            return;
        }
        
    }
    #endregion
}
