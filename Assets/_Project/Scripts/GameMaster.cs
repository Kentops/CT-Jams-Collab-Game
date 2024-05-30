using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Scene = UnityEngine.SceneManagement.Scene;

public class GameMaster : MonoBehaviour
{
    public GameObject player;
    public Textbox textbox;
    public SaveData mySaveData;
    public UI_On_Screen itemPopup;
    public float mouseSensitivity = 1;
    public string sceneName = "SampleScene";

    public static GameObject instance = null;

    private GameObject myCanvas;
    [SerializeField] private Image loadingImage;

    // Start is called before the first frame update
    void Awake()
    {
        //Singleton (essentially)
        if (instance == null)
        {
            instance = gameObject;
            //The game master will be present in every scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Awake goes before start iirc
        player = GameObject.FindGameObjectWithTag("Player");
        DontDestroyOnLoad(player);
        textbox = GameObject.FindGameObjectWithTag("Textbox").GetComponent<Textbox>();
        mySaveData = GetComponent<SaveData>();
        myCanvas = transform.GetChild(0).gameObject;
        itemPopup = GameObject.FindGameObjectWithTag("Item Popup").GetComponent<UI_On_Screen>();
        myCanvas.SetActive(false);
        loadingImage.fillAmount = 0;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reload()
    {
        Awake();
    }

    public IEnumerator loadScene(string sceneName)
    {
        this.sceneName = sceneName;
        var scene = SceneManager.LoadSceneAsync(sceneName);

        myCanvas.SetActive(true);
        loadingImage.fillAmount = 0;
        yield return new WaitForSeconds(0.5f);

        scene.allowSceneActivation = false;
        //Do-while loops check if they should repeat at the end of each iteration, rather than the start.
        while(!scene.isDone)
        {
            yield return new WaitForSeconds(1f);
            loadingImage.fillAmount = scene.progress;
            if(scene.progress > 0.9f)
            {
                scene.allowSceneActivation = true;
            }
        }

        myCanvas.SetActive(false);



    }
}
