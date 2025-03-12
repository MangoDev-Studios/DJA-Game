using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    private GameObject Player;

    private void Start()
    {
        Player = GameObject.Find("Player(Clone)");
        cameraPosition = Player.transform.GetChild(2).transform;
    }
    private void Update()
    {
        transform.position = cameraPosition.position;
        
    }       

    
}
