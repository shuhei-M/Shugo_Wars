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
    /// <summary> ���g�ɂɃA�^�b�`���ꂽ�R���|�[�l���g���擾���邽�߂̕ϐ��Q </summary>
    private Transform _GrabedPoint;

    private float _time;
    #endregion

    #region property

    #endregion

    #region Unity function

    private void OnCollisionEnter(Collision collision)
    {
        IPlayerGetItemComponent princess = collision.gameObject.GetComponent<IPlayerGetItemComponent>();

        // �P�łȂ���Έȉ��̏����͍s��Ȃ��B
        if (princess == null) return;

        // �A�C�e���̌��ʔ���
        ItemAbility(princess);

        // �A�C�e������
        DestroyThisItem();
    }
    #endregion

    #region public function

    #endregion

    #region protected function
    /// <summary>
    /// ���N���X���ʂ̕ϐ�������������
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
    /// �^�C�����~�b�g�𒴂���Ə��ł�����B
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
    /// �A�C�e���̌��ʂ𔭓�������B
    /// �p����̊e��A�C�e���N���X�œ��e�����߂�
    /// </summary>
    protected abstract void ItemAbility(IPlayerGetItemComponent princess);
    #endregion

    #region private function
    /// <summary>
    /// �A�C�e������������
    /// </summary>
    private void DestroyThisItem()
    {
        ItemManager.Instance.AliveItemCount--;

        // �A�C�e������
        Destroy(this.gameObject);
    }
    #endregion

    /// <summary> �E�܂ގw�悩��A�E�܂܂ꂽ�I�u�W�F�N�g�ɃA�N�Z�X���邽�߂̃C���^�[�t�F�[�X </summary>
    #region IGrabableObject
    /// <summary> �E�܂܂�Ă��邩�ǂ��� </summary>
    public bool IsGrabed { get; set; }

    /// <summary> �͂ލ��W��n�� </summary>
    public Transform Get_GrabedPoint()
    {
        return _GrabedPoint;
    }
    #endregion
}
