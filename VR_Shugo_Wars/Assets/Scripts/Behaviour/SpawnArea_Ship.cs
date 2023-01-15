using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea_Ship : SpawnAreaScript
{
    public GameObject[] Enemy_List = new GameObject[1];//�G�̎��
    public float[] enemy_spawn_time = new float[1];//�G���ꂼ��̃X�|�[������
    public float[] max_enemy = new float[1];//�G�̂��ꂼ��̍ő吔

    public float[] enemy_count;//�G�̂��ꂼ��̐�
    private float[] enemy_time;//�G���ꂼ��̃X�|�[������(�v���p)

    public GameObject[] spawn_p = new GameObject[1];

    public int down_time;//�~���̂ɂǂ̂��炢�|���邩�H
    public float down_point_y;//�~��鑍�� ��1.5f
    private bool ship_downf;//�X�|�[����p�̑D���w��̍��W�܂ō~�肫�������H
    private float onetime_translate_value;//��b�Ԃňړ����鋗��

    //�����X�|�[��
    public GameObject[] Spawn_Start = new GameObject[1];
    public bool gamestartf;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        //�܎�ނ̓G���ꂼ��̃X�|�[������(�v���p)
        enemy_time = new float[Enemy_List.Length];
        //�G�̂��ꂼ��̐�
        enemy_count = new float[Enemy_List.Length];

        ship_downf = false;

        onetime_translate_value = down_point_y / down_time;//��b�Ԃňړ����鋗��

        //gamestartf = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (gamemode.State == GameModeStateEnum.Play)//�Q�[����
        {
            time = GameModeController.Instance.GameTime;

            if(gamestartf == false)//�����z�u
            {
                for (int i = 0; i < Spawn_Start.Length; i++)
                {
                    Instantiate(Enemy_List[0], Spawn_Start[i].transform.position, Quaternion.identity);
                }
                gamestartf = true;
            }

            if (spawntime.Wave1(time))//0����1��(�c��3������2��)
            {
                max_enemy[0] = 10;
            }
            else if (spawntime.Wave2(time)) //1����2��(�c��2������1��)
            {
                max_enemy[0] = 15;
            }
            else if (spawntime.Wave3(time)) //2����3��(�c��1������0�܂�)
            {
                max_enemy[0] = 20;
            }

            if (ship_downf == false)//�D���~���
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
            else if (ship_downf == true)//�D���~�肫������A�X�|�[���J�n
            {
                Spawncount_ship(Enemy_List, enemy_spawn_time, enemy_count, enemy_time, max_enemy, spawn_p[0], spawn_p[1], spawn_p[2]);
            }
        }
        else if (gamemode.State == GameModeStateEnum.Clear || gamemode.State == GameModeStateEnum.GameOver)//�Q�[���I�����A�D�̋A��
        {
            if (this.transform.position.y <= 10f)
            {
                this.transform.Translate(0, onetime_translate_value * Time.deltaTime, 0);
            }
        }

        if (enemy_count[0] < 0)
        {
            enemy_count[0] = 0;
        }//�����̔z�u�̍ۂ̐��̍����C��
    }
}
