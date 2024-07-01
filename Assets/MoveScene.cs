using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScene : MonoBehaviour
{
    public void Click(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
    public void Youtube()
    {
        System.Diagnostics.Process.Start("https://www.youtube.com/@user-vb9by5mp9f");
    }
    public void OfficalSite()
    {
        System.Diagnostics.Process.Start("https://1392year.pythonanywhere.com/w/d7ByHgoruh");
    }
}
