using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PowerUpSpawnManager : MonoBehaviour
{
    public PowerUp PowerUpPrefab;
    [Space]
    public SO_Powerup[] SpawnablePowerUpEffects;
    [Space]
    public float PowerUpSpawnRate;
    public int MaxAmountOfPowerupsInGame;

    private bool _spawnPowerUp;
    public bool SpawnPowerUp
    {
        get { return _spawnPowerUp; }
        set
        {
            _spawnPowerUp = value;
            if (_spawnPowerUp)
                StartCoroutine("SpawnNextPowerup");
            else
                StopCoroutine("SpawnNextPowerup");
        }
    }

    private float maxDist = 8.0f;
    private float minDist = 4.0f;

    private int _currentAmountOfPowerUps;
    private List<PowerUp> _currentPowerups = new List<PowerUp>();
    
    private IEnumerator SpawnNextPowerup()
    {
        yield return new WaitForSeconds(PowerUpSpawnRate);

        //check if we have reached the max num of powerups
        if (_currentAmountOfPowerUps < MaxAmountOfPowerupsInGame)
        {
            //check again if we still want to spawn a powerup after the delay has run
            if (SpawnPowerUp && SpawnablePowerUpEffects != null && SpawnablePowerUpEffects.Length > 0)
            {
                //spawn the powerup sceneobject
                PowerUp power = Instantiate(PowerUpPrefab, GetRandomPositionInPlayerRange(), Quaternion.identity,transform);
                _currentPowerups.Add(power);
                _currentAmountOfPowerUps++;
                //set the powerup effect within the sceneobject
                power.SetPowerUP(SpawnablePowerUpEffects[Random.Range(0, SpawnablePowerUpEffects.Length)]);
            }
        }
        if (SpawnPowerUp)
            StartCoroutine("SpawnNextPowerup");
    }

    /// <summary>
    /// Returns a random position around the player.
    /// </summary>
    private Vector2 GetRandomPositionInPlayerRange()
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
            // we sample on the navmesh so we are 100% sure the player
            //can get to the powerup
            for (int i = 0; i < 30; i++)
            {
                //get random position but so we dont spawn in player view
                float x = Random.Range(0, 2) == 1 ? Random.Range(-maxDist, -minDist) : Random.Range(minDist, maxDist);
                float y = Random.Range(0, 2) == 1 ? Random.Range(-maxDist, -minDist) : Random.Range(minDist, maxDist);

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

    private void OnPowerUpUsed(PowerUp _powerUp)
    {
        if(_currentPowerups != null && _currentPowerups.Contains(_powerUp))
        {
            _currentPowerups.Remove(_powerUp);
            _currentAmountOfPowerUps--;
        }
    }

}