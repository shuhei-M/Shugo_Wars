using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRightCollider : MonoBehaviour
{
    // Rightbody での velocity の算出が難しいので、
    // １フレーム事こちらのスクリプトで算出。


    #region field
    Vector3 _prevPosition;
    Vector3 _velocity;      // 三次元空間上の速度
    float _speed;           // 速度のスカラー値
    #endregion


    #region Geter
    public Vector3 GetVelocity() { return _velocity; }
    public float GetSpeed() { return _speed; }
    #endregion


    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        // 1フレーム前の位置
        _prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // deltaTime が 0 の場合は何もしない
        if (Mathf.Approximately(Time.deltaTime, 0))
            return;

        // 現在の速度と力の大きさを計算し、前フレームの位置を更新
        var position = transform.position;
        _velocity = (position - _prevPosition) / Time.deltaTime;
        _speed = Mathf.Sqrt(Mathf.Pow(_velocity.x, 2) + Mathf.Pow(_velocity.y, 2) + Mathf.Pow(_velocity.z, 2));
        _prevPosition = position;
    }
    #endregion
}
