using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItemBehaviour : MonoBehaviour, IGrabableComponent
{
    #region define

    #endregion

    #region serialize field
    [SerializeField, Range(5.0f, 10.0f)] float _LifeTime = 10.0f;
    #endregion

    #region field
    /// <summary> 自身ににアタッチされたコンポーネントを取得するための変数群 </summary>
    private Transform _GrabedPoint;

    private int _HealPoint = 1;

    private float time;
    #endregion

    #region property

    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _GrabedPoint = transform.Find("GrabedPoint").gameObject.transform;
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -1) DestroyThisItem();

        time += Time.deltaTime;

        if (time > _LifeTime) DestroyThisItem();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IPlayerGetItemComponent princess = collision.gameObject.GetComponent<IPlayerGetItemComponent>();

        // 姫でなければ以下の処理は行わない。
        if (princess == null) return;

        // 回復させる
        princess.HealLifePoint(_HealPoint);

        // エフェクトを発生させる
        EffectManager.Instance.Play(EffectManager.EffectID.Heal, transform.position);

        // アイテム消滅
        DestroyThisItem();
    }
    #endregion

    #region public function
    /// <summary>
    /// 回復ポイントを変更する
    /// ドロップ元の敵クラスから、回復量を調節できるようにする。
    /// </summary>
    /// <param name="point"></param>
    public void SetHealPoint(int point)
    {
        _HealPoint = point;
    }
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
