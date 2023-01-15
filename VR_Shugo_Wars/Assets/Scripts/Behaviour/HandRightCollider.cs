using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRightCollider : MonoBehaviour
{
    // Rightbody �ł� velocity �̎Z�o������̂ŁA
    // �P�t���[����������̃X�N���v�g�ŎZ�o�B


    #region field
    Vector3 _prevPosition;
    Vector3 _velocity;      // �O������ԏ�̑��x
    float _speed;           // ���x�̃X�J���[�l
    #endregion


    #region Geter
    public Vector3 GetVelocity() { return _velocity; }
    public float GetSpeed() { return _speed; }
    #endregion


    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        // 1�t���[���O�̈ʒu
        _prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // deltaTime �� 0 �̏ꍇ�͉������Ȃ�
        if (Mathf.Approximately(Time.deltaTime, 0))
            return;

        // ���݂̑��x�Ɨ͂̑傫�����v�Z���A�O�t���[���̈ʒu���X�V
        var position = transform.position;
        _velocity = (position - _prevPosition) / Time.deltaTime;
        _speed = Mathf.Sqrt(Mathf.Pow(_velocity.x, 2) + Mathf.Pow(_velocity.y, 2) + Mathf.Pow(_velocity.z, 2));
        _prevPosition = position;
    }
    #endregion
}
