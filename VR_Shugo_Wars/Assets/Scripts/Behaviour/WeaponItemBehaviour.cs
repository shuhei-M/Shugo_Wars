using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None = -1,
    Gun,
    Launcher,
}

public class WeaponItemBehaviour : MonoBehaviour, IGrabableComponent
{
    #region serialize field
    [SerializeField] private WeaponType weaponType;
    #endregion


    #region field
    /// <summary> 自身ににアタッチされたコンポーネントを取得するための変数群 </summary>
    private Transform _GrabedPoint;
    #endregion


    #region Geter
    public WeaponType GetWeaponType() { return weaponType; }
    #endregion


    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _GrabedPoint = transform.Find("GrabedPoint").gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1) Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IPlayerGetItemComponent princess = collision.gameObject.GetComponent<IPlayerGetItemComponent>();

        // 姫でなければ以下の処理は行わない。
        if (princess == null) return;

        // 武器を与える
        princess.EquipWeapon(GetWeaponType());

        // エフェクトを発生させる
        EffectManager.Instance.Play(EffectManager.EffectID.Heal, transform.position);

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
