using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button StartGameBtn;
    [SerializeField] private Button ContinueGameBtn;

    public TextMeshProUGUI progressText; 
    public Slider progressBar;
    public GameObject loadContainer;

    private void Start()
    {
        StartGameBtn.onClick.AddListener(LoadScene);
        //StartGame.onClick.AddListener(()=> LoadSceneAsync("GameScene"));

    #region Setting Continue
        bool existSaveData = DataManager.instance.ExistSaveData();
        ContinueGameBtn.gameObject.SetActive(existSaveData);

        ContinueGameBtn.onClick.AddListener(ContinueGame);
    #endregion
    }

    public void LoadScene() 
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    #region Load Async
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneProgress(sceneName));
    }

    IEnumerator LoadSceneProgress(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        loadContainer.SetActive(true);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Normalize progress to 0-1 for display

            if (progressText != null)
            {
                progressText.text = (int)(progress * 100f) + "%";
            }

            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            yield return null;
        }

        yield return new WaitForSeconds(1);

        loadContainer.SetActive(false);
    }
    #endregion

    public void ContinueGame() 
    {
        DataManager.instance.LoadGameData();

        LoadSceneAsync("GameScene");
        //LoadSceneAsync("SaveLoadScene");
    }
}
