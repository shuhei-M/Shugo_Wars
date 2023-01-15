using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItemBehavior : MonoBehaviour, IGrabableComponent
{
    #region define

    #endregion

    #region serialize field
    [SerializeField, Range(5.0f, 10.0f)] float _LifeTime = 10.0f;
    #endregion

    #region field
    /// <summary> 自身ににアタッチされたコンポーネントを取得するための変数群 </summary>
    private Transform _GrabedPoint;
    private ExpItemSensorBehaviour _ExpItemSensor;
    private Rigidbody _Rigidbody;

    private int _AddExpPoint = 1;   // 加算する経験値

    private Vector3 _GapVec;   // 姫へのベクトル
    private float _Speed;   // 姫へ向かって動く際のスピード

    private bool _IsMove;   // アイテムが姫に向かって動いているか

    private float time;
    #endregion

    #region property
    public bool IsMove { get { return _IsMove; } }
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _GrabedPoint = transform.Find("GrabedPoint").gameObject.transform;
        _ExpItemSensor = transform.Find("Sensor").gameObject.GetComponent<ExpItemSensorBehaviour>();
        _Rigidbody = GetComponent<Rigidbody>();

        _GapVec = Vector3.zero;
        _Speed = 0.0f;
        _IsMove = false;

        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1) DestroyThisItem();
        _IsMove = _ExpItemSensor.IsFindPlayer;

        if(time > _LifeTime) DestroyThisItem();

        if (_ExpItemSensor.IsFindPlayer)
        {
            _GapVec = GameModeController.Instance.Princess.transform.position - transform.position;
            _Rigidbody.useGravity = false;

            _GapVec = _GapVec.normalized;
            _Speed += (Time.deltaTime * 0.025f);
            transform.position += (_GapVec * _Speed);
        }
        else
        {
            // 姫を見失った場合はスピードをリセット
            _Speed = 0.0f;
            _Rigidbody.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        IPlayerGetItemComponent princess = collision.gameObject.GetComponent<IPlayerGetItemComponent>();

        // 姫でなければ以下の処理は行わない。
        if (princess == null) return;

        // 経験値を与える
        princess.AddExp(_AddExpPoint);

        // エフェクトを発生させる
        EffectManager.Instance.Play(EffectManager.EffectID.Exp, transform.position);

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
    public void SetAddExpPoint(int point)
    {
        _AddExpPoint = point;
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
