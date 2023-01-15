using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の弾などのダメージ判定のある処理の親クラス
public class EnemyWeapon : MonoBehaviour
{
    #region Public field
    public int _applyDamage = 1;
    #endregion


    #region field
    protected GameObject _OVRHandLeft;
    protected GameObject _OVRHandRight;
    private PlayerData _handLeftData;
    private PlayerData _handRightData;
    protected GameObject Target;
    public float neck;
    private bool _IsApplyDamage = false;
    #endregion


    /// <summary>
    /// ダメージを与える処理のテンプレート
    /// 継承先でこの処理を書く
    /// </summary>
    #region Unity function
    // Start is called before the first frame update
    //void Start()
    //{
    //    SetUp();
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    DamageJudgment(collision);
    //}
    #endregion


    #region Protected method
    // セットアップ処理
    protected void SetUp()
    {
        // ヒエラルキー内の手(--OVRHandPrefab)のオブジェクトを取得
        _OVRHandLeft = GameObject.Find("LeftOVRHandPrefab");
        _OVRHandRight = GameObject.Find("RightOVRHandPrefab");

        // 片手ごとのステータスを取得
        _handLeftData = _OVRHandLeft.GetComponent<PlayerData>();
        _handRightData = _OVRHandRight.GetComponent<PlayerData>();


        //Target=姫
        Target = GameObject.Find("Princess");

    }

    // ダメージ判定
    protected void DamageJudgment(Collision collision)
    {
        if (_IsApplyDamage)
            return;

        IBattleComponent battleObj = collision.gameObject.GetComponent<IBattleComponent>();

        if (collision.gameObject.tag != "HandCapsuleRigidbody" && battleObj == null)
            return;

        // プレイヤー（手）に当たったらダメージを与える
        if (collision.gameObject.tag == "HandCapsuleRigidbody")
        {
            if (collision.transform.parent.parent.gameObject == _OVRHandLeft)
            {
                //Debug.Log("HandHit!：" + collision);
                battleObj = _OVRHandLeft.GetComponent<IBattleComponent>();
                battleObj.ApplyDamage(_applyDamage);
                _IsApplyDamage = true;
                Destroy(this.gameObject);
                return;
            }
            else if (collision.transform.parent.parent.gameObject == _OVRHandRight)
            {
                //Debug.Log("HandHit!：" + collision);
                battleObj = _OVRHandRight.GetComponent<IBattleComponent>();
                battleObj.ApplyDamage(_applyDamage);
                _IsApplyDamage = true;
                Destroy(this.gameObject);
                return;
            }
        }

        // 当たった相手が姫ならダメージを与えて消える
        if (collision.gameObject.tag == "Princess")
        {
            battleObj.ApplyDamage(_applyDamage);
            _IsApplyDamage = true;
            Destroy(this.gameObject);
            return;
        }
    }
    #endregion
}
