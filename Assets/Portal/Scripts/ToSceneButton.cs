using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToSceneButton : MonoBehaviour
{

    public void ToScene1()
    {
        SceneManager.LoadScene("Detroit");
    }

    public void ToScene2()
    {
        SceneManager.LoadScene("Atlanta");
    }

    public void ToScene3()
    {
        SceneManager.LoadScene("Dubai");
    }

    public void ToScene4()
    {
        SceneManager.LoadScene("Greece");
    }

    public void ToScene5()
    {
        SceneManager.LoadScene("Paris");
    }

    public void ToScene6()
    {
        SceneManager.LoadScene("Portugal");
    }

    public void ToScene7()
    {
        SceneManager.LoadScene("Shanghai");
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
