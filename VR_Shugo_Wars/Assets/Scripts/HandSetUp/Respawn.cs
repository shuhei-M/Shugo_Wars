using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    #region define

    #endregion

    #region serialize field

    #endregion

    #region field
    private HandsSetUpController handsSet;
    #endregion

    #region property

    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "IndexFinger")
        {
            handsSet = GameObject.Find("HandSetUpPanel").GetComponent<HandsSetUpController>();
            handsSet.IsRespawn = true;
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function

    #endregion
}
