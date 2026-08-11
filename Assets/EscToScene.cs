using UnityEngine;
using UnityEngine.SceneManagement;

public class EscToScene : MonoBehaviour
{
    [SerializeField] string scene;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(scene);
        }
    }
}
