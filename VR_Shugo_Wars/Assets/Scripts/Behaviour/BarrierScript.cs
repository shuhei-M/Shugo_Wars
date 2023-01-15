using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    private GameObject SpawnAir;
    private SpawnArea_Air spawnArea_AirScript;
    private GameObject SpawnGround;
    private SpawnArea_Ground spawnArea_GroundScript;
    private GameObject SpawnShip;
    private SpawnArea_Ship spawnArea_ShipScript;


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
        if (GameObject.Find("Spawn_Air") != null)
        {
            SpawnAir = GameObject.Find("Spawn_Air");
            spawnArea_AirScript = SpawnAir.GetComponent<SpawnArea_Air>();
        }

        if (GameObject.Find("Spawn_Ship") != null)
        {
            SpawnShip = GameObject.Find("Spawn_Ship");
            spawnArea_ShipScript = SpawnShip.GetComponent<SpawnArea_Ship>();
        }
        
        if (GameObject.Find("Spawn_Ground") != null)
        {
            SpawnGround = GameObject.Find("Spawn_Ground");
            spawnArea_GroundScript = SpawnGround.GetComponent<SpawnArea_Ground>();
        }
#else //本番用
　　　　　　SpawnAir = GameObject.Find("Spawn_Air");
            spawnArea_AirScript = SpawnAir.GetComponent<SpawnArea_Air>();

　　　　　　SpawnShip = GameObject.Find("Spawn_Ship");
            spawnArea_ShipScript = SpawnShip.GetComponent<SpawnArea_Ship>();

　　　　　　SpawnGround = GameObject.Find("Spawn_Ground");
            spawnArea_GroundScript = SpawnGround.GetComponent<SpawnArea_Ground>();
#endif
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        if (hitcollision.gameObject.tag == "enemy_ship_r")
        {
#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
            if (spawnArea_AirScript != null)
            {
                var OtherData = hitcollision.gameObject.GetComponent<Ship_RScript>();
                if (!OtherData.IsBlownAway)
                {
                    spawnArea_AirScript.enemy_count[0]--;
                    Destroy(hitcollision.gameObject);
                }
                    
            }
#else //本番用
                var OtherData = hitcollision.gameObject.GetComponent<Ship_RScript>();
                if (!OtherData.IsBlownAway)
                {
                    spawnArea_AirScript.enemy_count[0]--;
                    Destroy(hitcollision.gameObject);
                }
#endif
        }
        else if (hitcollision.gameObject.tag == "enemy_soldier")
        {
#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
            if (spawnArea_ShipScript != null)
            {
                var OtherData = hitcollision.gameObject.GetComponent<SoldierMove>();
                if (!OtherData.IsBlownAway)
                {
                    spawnArea_ShipScript.enemy_count[0]--;
                    Destroy(hitcollision.gameObject);
                }
            }
#else //本番用
               var OtherData = hitcollision.gameObject.GetComponent<SoldierMove>();
                if (!OtherData.IsBlownAway)
                {
                    spawnArea_ShipScript.enemy_count[0]--;
                    Destroy(hitcollision.gameObject);
                }
#endif
        }
        else if (hitcollision.gameObject.tag == "enemy_tank")
        {
#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
            if (spawnArea_GroundScript != null)
            {
                var OtherData = hitcollision.gameObject.GetComponent<TankMove>();
                if (!OtherData.IsBlownAway)
                {
                    spawnArea_GroundScript.enemy_count[0]--;
                    Destroy(hitcollision.gameObject);
                }
            }
#else //本番用
                var OtherData = hitcollision.gameObject.GetComponent<TankMove>();
                if (!OtherData.IsBlownAway)
                {
                    spawnArea_GroundScript.enemy_count[0]--;
                    Destroy(hitcollision.gameObject);
                }
#endif
        }
        else if (hitcollision.gameObject.tag == "enemy_ship_g")
        {
#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
            if (spawnArea_AirScript != null)
            {
                var OtherData = hitcollision.gameObject.GetComponent<GatringMove>();
                if (!OtherData.IsBlownAway)
                {
                    spawnArea_AirScript.enemy_count[1]--;
                    Destroy(hitcollision.gameObject);
                }
            }
#else //本番用
                var OtherData = hitcollision.gameObject.GetComponent<GatringMove>();
                if (!OtherData.IsBlownAway)
                {
                    spawnArea_AirScript.enemy_count[1]--;
                    Destroy(hitcollision.gameObject);
                }
#endif
        }
        else if (hitcollision.gameObject.tag == "enemy_bullet")
        {
            Destroy(hitcollision.gameObject);
        }
        else if(hitcollision.gameObject.tag == "Princess")
        {
            Destroy(hitcollision.gameObject);//仮
            //落ちた姫の再スポーン↓
        }
        else
        {
            return;
        }
    }
}
