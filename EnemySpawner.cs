using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public BoxCollider triggerCollider;

    public GameObject sword;

    [System.Serializable]
    public class WaveContent 
    {
        [SerializeField][NonReorderable] GameObject[] enemySpawn;

        public GameObject[] GetEnemySpawnList()
        {
            return enemySpawn;
        }
    }

    [SerializeField][NonReorderable] WaveContent[] waves;
    int currentWave;
    public List<GameObject> currentEnemy;
    bool playerEnteredTrigger = false;
    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered spawn trigger");
        //To ensure it is only triggered once
        if(!playerEnteredTrigger) 
        {
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Sword")
                {
                    playerEnteredTrigger = true;
                    SpawnWave();
                }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("FPS = " + (int)(1f / Time.unscaledDeltaTime));
        if (playerEnteredTrigger) // Check if the player has entered the trigger zone
        {      
            if(currentEnemy.Count == 0 && waves[currentWave].GetEnemySpawnList().Length != 0)
            {
                if(waves[currentWave].GetEnemySpawnList().Length == 12)
                {
                    sword.SetActive(true);
                }
                currentWave++;
                SpawnWave();
            }
        }
    }

    void SpawnWave()
    {
        for(int i = 0; i < waves[currentWave].GetEnemySpawnList().Length; i++)
        {
            Vector3 spawnPosition = position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-6f, 6f));
            GameObject newspawn = Instantiate(waves[currentWave].GetEnemySpawnList()[i], 
            spawnPosition, Quaternion.identity);
            currentEnemy.Add(newspawn);
        
            EnemyController enemy = newspawn.GetComponent<EnemyController>();
            enemy.SetSpawner(this);
        }
    }

}
