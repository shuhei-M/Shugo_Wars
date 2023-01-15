using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea_Ship : SpawnAreaScript
{
    public GameObject[] Enemy_List = new GameObject[1];//敵の種類
    public float[] enemy_spawn_time = new float[1];//敵それぞれのスポーン時間
    public float[] max_enemy = new float[1];//敵のそれぞれの最大数

    public float[] enemy_count;//敵のそれぞれの数
    private float[] enemy_time;//敵それぞれのスポーン時間(計測用)

    public GameObject[] spawn_p = new GameObject[1];

    public int down_time;//降りるのにどのくらい掛かるか？
    public float down_point_y;//降りる総量 例1.5f
    private bool ship_downf;//スポーン専用の船が指定の座標まで降りきったか？
    private float onetime_translate_value;//一秒間で移動する距離

    //初期スポーン
    public GameObject[] Spawn_Start = new GameObject[1];
    public bool gamestartf;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        //五種類の敵それぞれのスポーン時間(計測用)
        enemy_time = new float[Enemy_List.Length];
        //敵のそれぞれの数
        enemy_count = new float[Enemy_List.Length];

        ship_downf = false;

        onetime_translate_value = down_point_y / down_time;//一秒間で移動する距離

        //gamestartf = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (gamemode.State == GameModeStateEnum.Play)//ゲーム中
        {
            time = GameModeController.Instance.GameTime;

            if(gamestartf == false)//初期配置
            {
                for (int i = 0; i < Spawn_Start.Length; i++)
                {
                    Instantiate(Enemy_List[0], Spawn_Start[i].transform.position, Quaternion.identity);
                }
                gamestartf = true;
            }

            if (spawntime.Wave1(time))//0から1分(残り3分から2分)
            {
                max_enemy[0] = 10;
            }
            else if (spawntime.Wave2(time)) //1から2分(残り2分から1分)
            {
                max_enemy[0] = 15;
            }
            else if (spawntime.Wave3(time)) //2から3分(残り1分から0まで)
            {
                max_enemy[0] = 20;
            }

            if (ship_downf == false)//船が降りる
            {
                if (time < down_time)
                {
                    this.transform.Translate(0, -onetime_translate_value * Time.deltaTime, 0);
                }
                else
                {
                    ship_downf = true;
                }
            }
            else if (ship_downf == true)//船が降りきった後、スポーン開始
            {
                Spawncount_ship(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, spawn_p[0], spawn_p[1], spawn_p[2]);
            }
        }
        else if (gamemode.State == GameModeStateEnum.Clear || gamemode.State == GameModeStateEnum.GameOver)//ゲーム終了時、船の帰還
        {
            if (this.transform.position.y <= 10f)
            {
                this.transform.Translate(0, onetime_translate_value * Time.deltaTime, 0);
            }
        }

        if (enemy_count[0] < 0)
        {
            enemy_count[0] = 0;
        }//初期の配置の際の数の差を修正
    }
}
