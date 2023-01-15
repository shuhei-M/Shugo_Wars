using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reborn : MonoBehaviour
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
            if (GameModeController.Instance.State != GameModeStateEnum.GameOver) return;
            // 現在のSceneを取得
            Scene loadScene = SceneManager.GetActiveScene();
            // 現在のシーンを再読み込みする
            SceneManager.LoadScene(loadScene.name);
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function

    #endregion
}
