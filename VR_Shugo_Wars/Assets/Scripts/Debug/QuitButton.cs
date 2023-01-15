using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define

    #endregion

    #region serialize field

    #endregion

    #region field
    private float _PushTime = 0.0f;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "IndexFinger")
        {
            _PushTime += Time.deltaTime;
            if(_PushTime > 2.0f)
            {
                _PushTime = 0.0f;
                UnityEngine.Application.Quit();
            }    
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function

    #endregion
}
