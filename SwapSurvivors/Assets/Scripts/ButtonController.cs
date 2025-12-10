using UnityEngine;
using UnityEngine.SceneManagement;      

public class ButtonController : MonoBehaviour
{
    public void QuitButton(){
        Application.Quit();
    }

    public void OpenUIButton(GameObject ui){
        ui.SetActive(true);
    }

    public void CloseUIButton(GameObject ui){
        ui.SetActive(false);
    }

    public void LoadScene(int id)
    {
        SceneData.sceneToLoad = id;
        SceneManager.LoadScene("LoadingScene");
    }
}
