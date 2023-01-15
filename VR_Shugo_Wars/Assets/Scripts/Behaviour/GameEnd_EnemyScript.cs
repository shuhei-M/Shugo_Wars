using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd_EnemyScript : MonoBehaviour
{
    private GameObject gamemodeobj;
    private GameModeController gamemode;

    void Start()
    {
        gamemodeobj = GameObject.Find("GameModeController");
        gamemode = gamemodeobj.GetComponent<GameModeController>();
    }

    void Update()
    {
        if (gamemode.State == GameModeStateEnum.Clear || gamemode.State == GameModeStateEnum.GameOver)
        {
            //���Ă͂߂��I�u�W�F�N�g�ɂ���ď����̕ύX�B�K�v�ɉ����ċL�q��ǉ�) 
            if (this.tag == "enemy_ship_r")//�D�@�~�T�C��
            {
                Destroy(this.gameObject);
            }
            else if (this.tag == "enemy_soldier")//���m�@�e
            {
                Destroy(this.gameObject);
            }
            else if (this.tag == "enemy_tank")//���
            {
                Destroy(this.gameObject);
            }
            else if (this.tag == "enemy_ship_g")//�D �K�g�����O
            {
                Destroy(this.gameObject);
            }
            else if (this.tag == "enemy_bullet")//�e
            {
                Destroy(this.gameObject);
            }
            else
            {
                return;
            }
        }
    }
}
