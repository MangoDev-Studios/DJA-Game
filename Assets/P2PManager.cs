using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;

public class P2P_Manager : MonoBehaviour
{
    public TMP_InputField ipInputField;
    public ushort port = 7777;

    private UnityTransport transport;

    private void Awake()
    {
        transport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
    }

    public void OnHostButtonClicked()
    {
        transport.ConnectionData.Address = "0.0.0.0"; // Listen on all interfaces (including Radmin)
        transport.ConnectionData.Port = port;

        NetworkManager.Singleton.StartHost();
        Debug.Log("Hosting with NGO on port " + port);
    }

    public void OnJoinButtonClicked()
    {
        string targetIP = ipInputField.text.Trim();

        if (string.IsNullOrEmpty(targetIP))
        {
            Debug.LogWarning("IP Address field is empty!");
            return;
        }

        transport.ConnectionData.Address = targetIP; // Radmin IP input
        transport.ConnectionData.Port = port;

        NetworkManager.Singleton.StartClient();
        Debug.Log("Connecting to NGO Host at " + targetIP + ":" + port);
    }
}
