using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;
using System.Net;
using System.Net.Sockets;

public class P2P_Manager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField ipInputField;
    public ushort port = 25000;

    private UnityTransport transport;

    private void Start()
    {
        // Initialize UI

        // Get transport reference
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (transport == null)
        {
            Debug.LogError("UnityTransport component missing!");
            Debug.Log("Error: Missing Transport");
            return;
        }

        // Unity 6 specific initialization
        transport.Initialize();
    }

    public void OnHostButtonClicked()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            Debug.LogWarning("Already hosting!");
            return;
        }

        // Configure transport for Unity 6
        transport.SetConnectionData(
            "0.0.0.0",  // Listen on all interfaces
            port,       // Port number
            "0.0.0.0"   // Default connect address
        );

        // Setup callbacks
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        // Start host
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log($"Host started on {GetLocalIPAddress()}:{port}");
        }
        else
        {
            Debug.LogError("Host failed to start!");
        }
    }

    public void OnJoinButtonClicked()
    {
        string targetIP = ipInputField.text.Trim();
        if (string.IsNullOrEmpty(targetIP))
        {
            return;
        }

        transport.SetConnectionData(targetIP, port);

        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log($"Client started to connect to {targetIP}:{port}");
        }
        else
        {
            Debug.LogError("Failed to start client!");
        }
    }

    private void OnServerStarted()
    {
        Debug.Log("Server started successfully!");
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log($"Client connected: {clientId}");
        }
        else
        {
            Debug.Log("Successfully connected to host");
        }
    }

    // Helper method to get local IP address
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1";
    }

    // Debug method to test raw socket binding
    public void TestPortAvailability()
    {
        try
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            listener.Stop();
            Debug.Log($"Port {port} is available!");
        }
        catch (SocketException e)
        {
            Debug.LogError($"Port {port} in use: {e.Message}");
        }
    }
}