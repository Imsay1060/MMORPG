using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MMORPGGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] List<Transform> spawnLocations = new List<Transform>();
    [SerializeField] TMP_Text playersText;
    
    private Dictionary<int, string> activePlayers = new Dictionary<int, string>(); // Dictionary to store player info with unique ID
    private int previousPlayerCount;

    void Start()
    {        
        // Spawn player at a random spawn point
        int randSpawn = Random.Range(0, spawnLocations.Count);
        PhotonNetwork.Instantiate("Player", spawnLocations[randSpawn].position, spawnLocations[randSpawn].rotation);

        // Spawn enemies periodically
        if (PhotonNetwork.IsMasterClient)
        {
            InvokeRepeating("SpawnEnemies", 5f, 30f); // Spawning enemies every 30 seconds
        }
        
        previousPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        UpdatePlayerListUI();
    }

    // Spawn enemies method for MMORPG setting
    public void SpawnEnemies()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Spawn walking enemies
            foreach (var spawnPoint in spawnLocations)
            {
                PhotonNetwork.Instantiate("WalkEnemy", spawnPoint.position, spawnPoint.rotation);
            }
            // You can add additional types of enemies and spawn logic here
        }
    }

    // Method to manage exiting the game
    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    // On leaving the room, return to lobby
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
        UpdatePlayerList();
    }

    // Update player list across clients
    [PunRPC]
    public void UpdatePlayerList()
    {
        activePlayers.Clear();
        GameObject[] playersInGame = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject player in playersInGame)
        {
            if (!player.GetComponent<PlayerController>().isDead)
            {
                activePlayers[player.GetComponent<PhotonView>().Owner.ActorNumber] = player.name;
            }
        }

        UpdatePlayerListUI();
    }

    // Method to update UI for active players
    private void UpdatePlayerListUI()
    {
        playersText.text = "Players in game: " + activePlayers.Count.ToString();
    }

    // Periodically check player connections and disconnections
    private void Update()
    {
        int currentPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        if (currentPlayerCount != previousPlayerCount)
        {
            UpdatePlayerList();
            previousPlayerCount = currentPlayerCount;
        }
    }

    // End game or handle game-over logic when only one player is left
    private void CheckEndGameCondition()
    {
        if (activePlayers.Count <= 1 && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(EndGameAfterDelay(5f));
        }
    }

    // End game with a delay
    private IEnumerator EndGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LoadLevel("Lobby"); // Load lobby or another scene as needed
    }
}
