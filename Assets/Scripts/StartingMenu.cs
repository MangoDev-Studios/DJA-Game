using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartingMenu : MonoBehaviour
{
    public Button SoloButton;
    public GameObject StartingMenuPrefab;

    private void Start()
    {

    }

    public void KillPrefab()
    {
        Destroy(gameObject);
    }

    void SpawnPrefab()
    {
        Instantiate(StartingMenuPrefab, transform.position, Quaternion.identity);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
