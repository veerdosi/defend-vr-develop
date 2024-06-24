using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Scene2Script : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;

    [Header("Main Menu Buttons")]
    public Button yesButton;
    public Button noButton;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        //Hook events
        yesButton.onClick.AddListener(StartTutorial);
        noButton.onClick.AddListener(GameDirectly);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }
    }

    public void StartTutorial()
    {
        HideAll();
        SceneManager.LoadScene("Scene3");
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
    }
    public void GameDirectly()
    {
        SceneManager.LoadScene("Scene7");
    }
}
