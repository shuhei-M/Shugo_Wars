using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreaScript : MonoBehaviour
{
    #region Protected field
    protected GameObject gamemodeobj;
    protected GameModeController gamemode;

    protected GameObject Timeobj;
    protected SpawnTime spawntime;

    protected int spawn_point;
    protected float time;
    #endregion


    /// <summary>
    /// スポーン処理のテンプレート
    /// 継承先でこの処理を書く
    /// </summary>
    #region Unity function
    //void Start()
    //{
    //    Setup()
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (gamemode.State == GameModeStateEnum.Play)
    //    {
    //        time = GameModeController.Instance.GameTime;
    //        それぞれの処理
    //    }
    //}
    #endregion


    #region Protected method
    protected void Setup()
    {
        gamemodeobj = GameObject.Find("GameModeController");
        gamemode = gamemodeobj.GetComponent<GameModeController>();

        Timeobj = GameObject.Find("SpawnTime");
        spawntime = Timeobj.GetComponent<SpawnTime>();

        spawn_point = 0;
    }

    //敵の種類リストからピックアップしてその敵だけ、□秒間置きに、最大数〇体までスポーンさせる。
    protected void Spawnpickup(GameObject[] Enemy_List,int element, float[] enemy_spawn_time, float[] enemy_count, float[] enemy_time, float[] max_enemy, GameObject range_A, GameObject range_B)
    {
        enemy_time[element] = enemy_time[element] + Time.deltaTime;

        if (enemy_time[element] > enemy_spawn_time[element])
        {
            enemy_time[element] = 0f;

            if (enemy_count[element] < max_enemy[element])
            {
                enemy_count[element]++;
                Spawn_Enemy(Enemy_List, element, range_A, range_B);
            }
        }
    }

    //敵の種類リストから、□秒間置きに、最大数〇体までスポーンさせる。
    protected void Spawncount(GameObject[] Enemy_List, float[] enemy_spawn_time, float[] enemy_count, float[] enemy_time, float[] max_enemy, GameObject range_A, GameObject range_B)
    {
        for (int i = 0; i < Enemy_List.Length; i++)
        {
            enemy_time[i] = enemy_time[i] + Time.deltaTime;

            if (enemy_time[i] > enemy_spawn_time[i])
            {
                enemy_time[i] = 0f;

                if (enemy_count[i] < max_enemy[i])
                {
                    enemy_count[i]++;
                    Spawn_Enemy(Enemy_List, i, range_A, range_B);
                }
            }
        }
    }
    
    //宇宙船からスポーンさせる。
    protected void Spawncount_ship(GameObject[] Enemy_List, float[] enemy_spawn_time, float[] enemy_count, float[] enemy_time, float[] max_enemy, GameObject spawn1, GameObject spawn2, GameObject spawn3)
    {
        for (int i = 0; i < Enemy_List.Length; i++)
        {
            enemy_time[i] = enemy_time[i] + Time.deltaTime;

            if (enemy_time[i] > enemy_spawn_time[i])
            {
                enemy_time[i] = 0f;

                if (enemy_count[i] < max_enemy[i])
                {
                    enemy_count[i]++;
                    Spawn_Enemy_ship(Enemy_List, i, spawn1, spawn2, spawn3);
                }
            }
        }
    }
    
    void Spawn_Enemy(GameObject[] Enemy_List, int enemy, GameObject range_A, GameObject range_B)
    {
        float x = Random.Range(range_A.transform.position.x, range_B.transform.position.x);
        float y = Random.Range(range_A.transform.position.y, range_B.transform.position.y);
        float z = Random.Range(range_A.transform.position.z, range_B.transform.position.z);

        Instantiate(Enemy_List[enemy], new Vector3(x, y, z), Quaternion.identity);
    }

    void Spawn_Enemy_ship(GameObject[] Enemy_List, int enemy, GameObject spawn1, GameObject spawn2, GameObject spawn3)
    {
        Vector3 vector = spawn1.transform.position;

        if (spawn_point == 0)
        {
            vector = spawn1.transform.position;
            spawn_point++;
        }
        else if (spawn_point == 1)
        {
            vector = spawn2.transform.position;
            spawn_point++;
        }
        else if (spawn_point == 2)
        {
            vector = spawn3.transform.position;
            spawn_point = 0;
        }

        Instantiate(Enemy_List[enemy], vector, Quaternion.identity);
    }

    #endregion

}
