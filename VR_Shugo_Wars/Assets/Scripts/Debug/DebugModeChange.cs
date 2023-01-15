using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModeChange : MonoBehaviour
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define

    #endregion

    #region serialize field

    #endregion

    #region field

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
            GameModeController.Instance.Debug_ChangeDebugMode();
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function

    #endregion
}
