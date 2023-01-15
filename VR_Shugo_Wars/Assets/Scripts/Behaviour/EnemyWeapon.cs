using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G�̒e�Ȃǂ̃_���[�W����̂��鏈���̐e�N���X
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
    /// �_���[�W��^���鏈���̃e���v���[�g
    /// �p����ł��̏���������
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
    // �Z�b�g�A�b�v����
    protected void SetUp()
    {
        // �q�G�����L�[���̎�(--OVRHandPrefab)�̃I�u�W�F�N�g���擾
        _OVRHandLeft = GameObject.Find("LeftOVRHandPrefab");
        _OVRHandRight = GameObject.Find("RightOVRHandPrefab");

        // �Ў育�Ƃ̃X�e�[�^�X���擾
        _handLeftData = _OVRHandLeft.GetComponent<PlayerData>();
        _handRightData = _OVRHandRight.GetComponent<PlayerData>();


        //Target=�P
        Target = GameObject.Find("Princess");

    }

    // �_���[�W����
    protected void DamageJudgment(Collision collision)
    {
        if (_IsApplyDamage)
            return;

        IBattleComponent battleObj = collision.gameObject.GetComponent<IBattleComponent>();

        if (collision.gameObject.tag != "HandCapsuleRigidbody" && battleObj == null)
            return;

        // �v���C���[�i��j�ɓ���������_���[�W��^����
        if (collision.gameObject.tag == "HandCapsuleRigidbody")
        {
            if (collision.transform.parent.parent.gameObject == _OVRHandLeft)
            {
                //Debug.Log("HandHit!�F" + collision);
                battleObj = _OVRHandLeft.GetComponent<IBattleComponent>();
                battleObj.ApplyDamage(_applyDamage);
                _IsApplyDamage = true;
                Destroy(this.gameObject);
                return;
            }
            else if (collision.transform.parent.parent.gameObject == _OVRHandRight)
            {
                //Debug.Log("HandHit!�F" + collision);
                battleObj = _OVRHandRight.GetComponent<IBattleComponent>();
                battleObj.ApplyDamage(_applyDamage);
                _IsApplyDamage = true;
                Destroy(this.gameObject);
                return;
            }
        }

        // �����������肪�P�Ȃ�_���[�W��^���ď�����
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
