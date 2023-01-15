using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItemBehavior : MonoBehaviour, IGrabableComponent
{
    #region define

    #endregion

    #region serialize field
    [SerializeField, Range(5.0f, 10.0f)] float _LifeTime = 10.0f;
    #endregion

    #region field
    /// <summary> 自身ににアタッチされたコンポーネントを取得するための変数群 </summary>
    private Transform _GrabedPoint;

    private float _time;
    #endregion

    #region property

    #endregion

    #region Unity function

    private void OnCollisionEnter(Collision collision)
    {
        IPlayerGetItemComponent princess = collision.gameObject.GetComponent<IPlayerGetItemComponent>();

        // 姫でなければ以下の処理は行わない。
        if (princess == null) return;

        // アイテムの効果発動
        ItemAbility(princess);

        // アイテム消滅
        DestroyThisItem();
    }
    #endregion

    #region public function

    #endregion

    #region protected function
    /// <summary>
    /// 基底クラス共通の変数を初期化する
    /// </summary>
    protected void SetUpBase()
    {
        _GrabedPoint = transform.Find("GrabedPoint").gameObject.transform;
        _time = 0.0f;
    }

    protected bool TryHeightUpdate()
    {
        bool IsTry = true;
        
        if (transform.position.y < -1)
        {
            DestroyThisItem();
            IsTry = false;
        }

        return IsTry;
    }

    /// <summary>
    /// タイムリミットを超えると消滅させる。
    /// </summary>
    protected bool TryTimeUpdate()
    {
        bool IsTry = true;

        _time += Time.deltaTime;

        if (_time > _LifeTime)
        {
            DestroyThisItem();
            IsTry = false;
        }

        return IsTry;
    }

    /// <summary>
    /// アイテムの効果を発動させる。
    /// 継承先の各種アイテムクラスで内容を決める
    /// </summary>
    protected abstract void ItemAbility(IPlayerGetItemComponent princess);
    #endregion

    #region private function
    /// <summary>
    /// アイテムを消去する
    /// </summary>
    private void DestroyThisItem()
    {
        ItemManager.Instance.AliveItemCount--;

        // アイテム消滅
        Destroy(this.gameObject);
    }
    #endregion

    /// <summary> 摘まむ指先から、摘ままれたオブジェクトにアクセスするためのインターフェース </summary>
    #region IGrabableObject
    /// <summary> 摘ままれているかどうか </summary>
    public bool IsGrabed { get; set; }

    /// <summary> 掴む座標を渡す </summary>
    public Transform Get_GrabedPoint()
    {
        return _GrabedPoint;
    }
    #endregion
}
