/// <summary> ���� </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour
{
    /// <summary> �\�[�X�������Ƃ��̃����v���[�g </summary>

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
        /// <summary> �������̎�Ɋ��Ƀv���C���[������Ă���Β͂ނ��Ƃ͂ł��Ȃ� </summary>
        if (!CanGlabAnyObject()) return;

        /// <summary> �G�ꂽ�I�u�W�F�N�g���C���^�[�t�F�C�X�������Ă��邩 </summary>
        IGrabableComponent _IGrabableObject = other.gameObject.GetComponent<IGrabableComponent>();
        if (_IGrabableObject == null) return;

        /// <summary> �I�u�W�F�N�g�ɂ��������̏��� </summary>
        /// �P
        /// ����ł�����͂߂Ȃ�
        if (other.gameObject.tag == "Princess")
        {
            PrincessBehaviour princessBehaviour = other.gameObject.GetComponent<PrincessBehaviour>();
            if (princessBehaviour.IsDead) return;
        }
        // �o���l�A�C�e�����A�C�e��
        else if (other.gameObject.tag == "ExpItem")
        {
            ExpItemBehavior expItemBehavior = other.gameObject.GetComponent<ExpItemBehavior>();
            if (expItemBehavior.IsMove) return;
        }

        /// <summary> �͂񂾎��̏��� </summary>
        if (ThumbPinchStrength>0.9f)///����
        {
            // �͂񂾃I�u�W�F�N�g�̃f�[�^�ɃA�N�Z�X
            _IGrabableObject.IsGrabed = true;
            Transform grabPoint = _IGrabableObject.Get_GrabedPoint();

            // �͂񂾃I�u�W�F�N�g�̃|�W�V������ύX
            Vector3 gap = grabPoint.position - other.gameObject.transform.position;
            other.gameObject.transform.position = IndexSphere.transform.position - gap;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.parent = IndexSphere.transform;
        }
        else///�͂Ȃ���
        {
            // �͂񂾃I�u�W�F�N�g�̃f�[�^�ɃA�N�Z�X
            _IGrabableObject.IsGrabed = false;

            // �͂񂾃I�u�W�F�N�g�̃|�W�V������ύX
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
    /// �I�u�W�F�N�g�������ނ��Ƃ��o���邩�ǂ������肷��
    /// �������̎�Ɋ��Ƀv���C���[������Ă���Β͂ނ��Ƃ͂ł��Ȃ�
    /// </summary>
    /// <returns>�͂ނ��Ƃ��o���邩�ǂ���</returns>
    private bool CanGlabAnyObject()
    {
        if (_HandType == RideAreaBehaviour.HandTypeEnum.None)
        {
            Debug.Log("�O���u�Z���T�[�̃n���h�^�C�v�ɕs���Ȓl�����͂���Ă��܂��B");
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

        /// <summary> �l�����w�Ɛe�w���������Ă��Ȃ��ꍇ </summary>
        if (ThumbPinchStrength < 0.9f) return;

        _IsFinishDirectionSetting = true;
    }
    #endregion
}
