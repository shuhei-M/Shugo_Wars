/// <summary> 松島 </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define

    #endregion

    #region serialize field
    [SerializeField] private RideAreaBehaviour.HandTypeEnum _HandType;

    [SerializeField] OVRHand MYHand;
    [SerializeField] OVRSkeleton MYSkelton;
    [SerializeField] GameObject IndexSphere;
    #endregion

    #region field
    private bool isIndexPinching;
    private float ThumbPinchStrength;

    private bool _IsFinishDirectionSetting = false;
    #endregion

    #region property
    public bool IsFinishDirectionSetting { get { return _IsFinishDirectionSetting; } }
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isIndexPinching = MYHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

        ThumbPinchStrength = MYHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);

        var index = (int)OVRSkeleton.BoneId.Hand_IndexTip;

        if(MYSkelton.Bones.Count > 0)
        {
            Vector3 indexTipPos = MYSkelton.Bones[index].Transform.position;
            Quaternion indexTipRotate = MYSkelton.Bones[index].Transform.rotation;
            IndexSphere.transform.position = indexTipPos;
            IndexSphere.transform.rotation = indexTipRotate;
        }

        if(!_IsFinishDirectionSetting) DirectionSettingFunction();
    }

    void OnTriggerStay(Collider other)
    {
        /// <summary> 自分側の手に既にプレイヤーが乗っていれば掴むことはできない </summary>
        if (!CanGlabAnyObject()) return;

        /// <summary> 触れたオブジェクトがインターフェイスを持っているか </summary>
        IGrabableComponent _IGrabableObject = other.gameObject.GetComponent<IGrabableComponent>();
        if (_IGrabableObject == null) return;

        /// <summary> オブジェクトにおける特定の処理 </summary>
        /// 姫
        /// 死んでいたら掴めない
        if (other.gameObject.tag == "Princess")
        {
            PrincessBehaviour princessBehaviour = other.gameObject.GetComponent<PrincessBehaviour>();
            if (princessBehaviour.IsDead) return;
        }
        // 経験値アイテムがアイテム
        else if (other.gameObject.tag == "ExpItem")
        {
            ExpItemBehavior expItemBehavior = other.gameObject.GetComponent<ExpItemBehavior>();
            if (expItemBehavior.IsMove) return;
        }

        /// <summary> 掴んだ時の処理 </summary>
        if (ThumbPinchStrength>0.9f)///つかんだ
        {
            // 掴んだオブジェクトのデータにアクセス
            _IGrabableObject.IsGrabed = true;
            Transform grabPoint = _IGrabableObject.Get_GrabedPoint();

            // 掴んだオブジェクトのポジションを変更
            Vector3 gap = grabPoint.position - other.gameObject.transform.position;
            other.gameObject.transform.position = IndexSphere.transform.position - gap;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.parent = IndexSphere.transform;
        }
        else///はなした
        {
            // 掴んだオブジェクトのデータにアクセス
            _IGrabableObject.IsGrabed = false;

            // 掴んだオブジェクトのポジションを変更
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.transform.parent = null;
        }

        if (other.gameObject.tag == "Grabable")
        {
            
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function
    /// <summary>
    /// オブジェクトをルかむことが出来るかどうか判定する
    /// 自分側の手に既にプレイヤーが乗っていれば掴むことはできない
    /// </summary>
    /// <returns>掴むことが出来るかどうか</returns>
    private bool CanGlabAnyObject()
    {
        if (_HandType == RideAreaBehaviour.HandTypeEnum.None)
        {
            Debug.Log("グラブセンサーのハンドタイプに不正な値が入力されています。");
            return false;
        }
        else
        {
            if (_HandType == GameModeController.Instance.Princess.RideSide) return false;
        }

        return true;
    }

    private void DirectionSettingFunction()
    {
        if (GameModeController.Instance.State != GameModeStateEnum.DirectionSetting) return;

        /// <summary> 人差し指と親指をくっつけていない場合 </summary>
        if (ThumbPinchStrength < 0.9f) return;

        _IsFinishDirectionSetting = true;
    }
    #endregion
}
