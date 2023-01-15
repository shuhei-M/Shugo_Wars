using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea_Air : SpawnAreaScript
{
    public GameObject[] Enemy_List = new GameObject[1];//敵の種類
    public float[] enemy_spawn_time = new float[1];//敵それぞれのスポーン時間
    public float[] max_enemy = new float[1];//敵のそれぞれの最大数 (船の上限はコースポイントの関係上最大4まで)

    public float[] enemy_count;//敵のそれぞれの数
    private float[] enemy_time;//敵それぞれのスポーン時間(計測用)

    public GameObject range_A;
    public GameObject range_B;

    //public List<GameObject> enemyshiplist = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        //五種類の敵それぞれのスポーン時間(計測用)
        enemy_time = new float[Enemy_List.Length];
        //敵のそれぞれの数
        enemy_count = new float[Enemy_List.Length];

    }

    // Update is called once per frame
    void Update()
    {

        if (gamemode.State == GameModeStateEnum.Play)
        {
            //Spawncount(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);

            time = GameModeController.Instance.GameTime;
            // Debug.Log(time);

            if (spawntime.Wave2(time))//1分から(残り2分から) 0=ミサイルのみ
            {
                Spawnpickup(Enemy_List, 0, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);
            }
            else if (spawntime.Wave3(time))//2分から(残り1分から) ミサイルとガトリング
            {
                Spawncount(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, range_A, range_B);
            }
        }
    }

}
