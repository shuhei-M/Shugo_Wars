using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskBehaviour : MonoBehaviour
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define

    #endregion

    #region serialize field

    #endregion

    #region field
    private Vector3 _StartPosition;
    private Vector3 _PrincessPosition;

    /// <summary> ゲームモードのステートを取得 </summary>
    private GameModeStateEnum _CurrentInGameState;
    private GameModeStateEnum _PrevInGameState;

    private RideAreaBehaviour _LeftRideArea;
    private RideAreaBehaviour _RightRideArea;

    private bool _IsFinishSetUp = false;
    #endregion

    #region property
    public bool IsFinishSetUp { get { return _IsFinishSetUp; } }
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _StartPosition = GameModeController.Instance.Princess.gameObject.transform.position;
        _PrincessPosition = _StartPosition;

        GameObject leftParent = GameObject.Find("LeftOVRHandPrefab").transform.parent.gameObject;
        GameObject leftRideArea = leftParent.transform.Find("LeftRideArea").gameObject;
        GameObject rightParent = GameObject.Find("RightOVRHandPrefab").transform.parent.gameObject;
        GameObject rightRideArea = rightParent.transform.Find("RightRideArea").gameObject;

        _LeftRideArea = leftRideArea.GetComponent<RideAreaBehaviour>();
        _RightRideArea = rightRideArea.GetComponent<RideAreaBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        _CurrentInGameState = GameModeController.Instance.State;

        _PrincessPosition = GameModeController.Instance.Princess.gameObject.transform.position;

        UpdateDeskState();

        _PrevInGameState = _CurrentInGameState;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Princess")
        {
            GameModeController.Instance.Princess.IsGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Princess")
        {
            GameModeController.Instance.Princess.IsGrounded = false;
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function
    private void ChangeDeskState()
    {
        switch (_CurrentInGameState)
        {
            case GameModeStateEnum.None:
                {
                }
                break;
            case GameModeStateEnum.DirectionSetting:
                {
                }
                break;
            case GameModeStateEnum.CountDown:
                {
                }
                break;
            case GameModeStateEnum.HandsSetUp:
                {
                    float sumY = _LeftRideArea.transform.position.y +
                        _RightRideArea.transform.position.y;

                    Vector3 temp = new Vector3(
                        transform.position.x,
                        sumY * 0.5f,
                        transform.position.z);

                    transform.position = temp;

                    GameModeController.Instance.StartHight = transform.position.y + 0.05f;

                    _StartPosition = new Vector3(
                        _StartPosition.x,
                        _StartPosition.y + 0.01f,
                        _StartPosition.z);

                    GameModeController.Instance.Princess.Respawn(_StartPosition);

                    _IsFinishSetUp = true;
                }
                break;
            case GameModeStateEnum.Play:
                {
                }
                break;
            case GameModeStateEnum.Clear:
                {
                }
                break;
            case GameModeStateEnum.GameOver:
                {
                    
                }
                break;
        }

    }

    /// <summary>
    /// 状態毎の毎フレーム呼ばれる処理
    /// </summary>
    private void UpdateDeskState()
    {
        if (IsEntryThisState()) { ChangeDeskState(); return; }

        switch (_CurrentInGameState)
        {
            case GameModeStateEnum.None:
                {
                }
                break;
            case GameModeStateEnum.DirectionSetting:
                {
                }
                break;
            case GameModeStateEnum.CountDown:
                {
                    
                }
                break;
            case GameModeStateEnum.HandsSetUp:
                {
                }
                break;
            case GameModeStateEnum.Play:
                {
                    if (_PrincessPosition.y < transform.position.y - 0.5f)
                    {
                        GameModeController.Instance.Princess.Respawn(_StartPosition);
                    }

                    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
                    {
                        GameModeController.Instance.Princess.Respawn(_StartPosition);
                    }
                }
                break;
            case GameModeStateEnum.Clear:
                {
                }
                break;
            case GameModeStateEnum.GameOver:
                {
                }
                break;
        }
    }

    /// <summary>
    /// ちょうどそのステートに入った所かどうか
    /// </summary>
    /// <returns></returns>
    private bool IsEntryThisState()
    {
        return (_PrevInGameState != _CurrentInGameState);
    }
    #endregion
}
