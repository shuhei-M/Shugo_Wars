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
            //当てはめたオブジェクトによって処理の変更。必要に応じて記述を追加) 
            if (this.tag == "enemy_ship_r")//船　ミサイル
            {
                Destroy(this.gameObject);
            }
            else if (this.tag == "enemy_soldier")//兵士　銃
            {
                Destroy(this.gameObject);
            }
            else if (this.tag == "enemy_tank")//戦車
            {
                Destroy(this.gameObject);
            }
            else if (this.tag == "enemy_ship_g")//船 ガトリング
            {
                Destroy(this.gameObject);
            }
            else if (this.tag == "enemy_bullet")//弾
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
