using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "EnemySpawn/SpawnBehavior/ConstantSpawning")]
public class SO_SB_ConstantSpawning : SO_SpawnBehavior
{
    public float UnitSpawnDelay = 1.0f;
    public float InitialSpawnDelayMultiplicator = 3;

    private float minDist = 5f;
    private float maxDist = 15f;

    public override IEnumerator SpawnBehavior(EnemyManager enemymanager)
    {
        //lets the initial spawn delay be longer
        //gives the player more time to get moving
        yield return new WaitForSeconds(InitialSpawnDelayMultiplicator * UnitSpawnDelay);

        while (enemymanager.SpawnEnemies)
        {
            //wait for the delay before spawning another enemy
            yield return new WaitForSeconds(UnitSpawnDelay);
            yield return new WaitForFixedUpdate();           
            //check again if we want to spawn enemies
            //in case all players died and the game is over 
            //while waiting for the delay
            if (enemymanager.SpawnEnemies == false || enemymanager.CanSpawnMoreEnemies == false)
                yield break;

            //Get random prefab
            //Get random position off screen
            //spawn enemy
            GameObject enemyObj = Instantiate(enemymanager.GetRandomEnemyPrefab(), FindSpawnPosition(), Quaternion.identity);
            //get the enemyscript from the newly spawned enemy
            //we have to get the statecontroller since the enemy class is no monobehavior
            //and it has a reference to its enemy part
            ShipHealth enemy = enemyObj.GetComponent<ShipHealth>();
            //check if any methods are subscribed to that event..
            if (enemymanager.OnEnemySpawnedEvent != null)
            {
                //..then let the enemymanager know we spawned an enemy
                enemymanager.OnEnemySpawnedEvent(enemy);
            }
        }
    }
    /// <summary>
    /// gets a random position around the player and returns it
    /// if no player is in the scene
    /// then it returns Vector2.Zero
    /// </summary>
    /// <returns></returns>
    public override Vector2 FindSpawnPosition()
    {
        GameObject player = null;

        //get random player from GameManager if there is an Instance
        //available
        if (GameManager.Instance)
            player = GameManager.Instance.GetRandomPlayerInGame();


        //else we find a player in the scene
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        //if we have a player..
        if (player != null)
        {
            //sample 30 positions..
            for (int i = 0; i < 30; i++)
            {
                //get random position but so we dont spawn in player view
                float x = Random.Range(0, 2) == 0 ? Random.Range(-maxDist, -minDist) : Random.Range(minDist, maxDist);
                float y = Random.Range(0, 2) == 0 ? Random.Range(-maxDist, -minDist) : Random.Range(minDist, maxDist);

                //add that offset to the playerposition
                Vector2 Spawnpos = new Vector2(player.transform.position.x + x, player.transform.position.y + y);

                NavMeshHit hit;
                //.. check if the position is on the navmesh
                if (NavMesh.SamplePosition(Spawnpos, out hit, 10f, NavMesh.AllAreas))
                {
                    //return it 
                    return hit.position;
                }
                //.. else try again
            }
        }

        //return zero vector if we cannot get a player from the gameManager or the scene
        return Vector2.zero;
    }
}