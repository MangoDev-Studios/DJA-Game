using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnController : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    private bool hasInitialized = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer && !hasInitialized)
        {
            // Host/Server spawns their own player first
            if (IsHost)
            {
                SpawnPlayer(NetworkManager.Singleton.LocalClientId);
            }
            
            // Then listen for client connections
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            hasInitialized = true;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer && clientId != NetworkManager.Singleton.LocalClientId)
        {
            SpawnPlayer(clientId);
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        int spawnIndex = (int)clientId % spawnPoints.Length;
        Vector3 spawnPosition = spawnPoints[spawnIndex].position;
        Quaternion spawnRotation = spawnPoints[spawnIndex].rotation;

        GameObject playerInstance = Instantiate(
            NetworkManager.Singleton.NetworkConfig.PlayerPrefab,
            spawnPosition,
            spawnRotation
        );

        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
        networkObject.SpawnAsPlayerObject(clientId);

        Debug.Log($"Spawned player for client {clientId} at {spawnPosition}");
    }

    public override void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
        base.OnDestroy();
    }
}