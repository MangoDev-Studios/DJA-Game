using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            
            // Spawn host player immediately
            if (IsHost)
            {
                SpawnPlayer(NetworkManager.Singleton.LocalClientId);
            }
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            SpawnPlayer(clientId);
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        int spawnIndex = (int)clientId % spawnPoints.Length;
        Vector3 spawnPos = spawnPoints[spawnIndex].position;
        Quaternion spawnRot = spawnPoints[spawnIndex].rotation;
        
        GameObject player = Instantiate(
            NetworkManager.Singleton.NetworkConfig.PlayerPrefab,
            spawnPos,
            spawnRot
        );
        
        NetworkObject networkObject = player.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(clientId);
    }
}
