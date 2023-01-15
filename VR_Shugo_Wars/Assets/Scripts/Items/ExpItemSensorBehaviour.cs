using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItemSensorBehaviour : MonoBehaviour
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define

    #endregion

    #region serialize field

    #endregion

    #region field
    private bool _IsFindPlayer;   // 検知範囲内にプレイヤーがはいったか
    #endregion

    #region property
    /// <summary> 親オブジェクトの経験値アイテムから取得される。 </summary>
    public bool IsFindPlayer { get { return _IsFindPlayer; } }
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _IsFindPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Princess")
        {
            _IsFindPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Princess")
        {
            _IsFindPlayer = false;
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function

    #endregion
}
