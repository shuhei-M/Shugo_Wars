using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define

    #endregion

    #region serialize field

    #endregion

    #region field
    private Vector3 _StartPosition;
    #endregion

    #region property

    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _StartPosition = GameModeController.Instance.Princess.transform.position;
        _StartPosition += new Vector3(0.0f, 0.1f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "IndexFinger")
        {
            GameModeController.Instance.Princess.transform.position = _StartPosition;
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function

    #endregion
}
