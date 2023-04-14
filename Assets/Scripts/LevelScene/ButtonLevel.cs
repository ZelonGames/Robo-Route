using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    private Canvas canvas;

    public Text buttonText;

    public int levelNumber;
    public static int selectedLevelNumber;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void Update()
    {
        Rect canvasRect = canvas.GetComponent<RectTransform>().rect;

        float buttonWidth = rectTransform.rect.width * rectTransform.localScale.x;
        int maxColumns = Mathf.FloorToInt(canvasRect.width / buttonWidth) - 1;

        // get the canvas width and height in pixels
        float canvasWidth = canvasRect.width * canvas.scaleFactor;
        float canvasHeight = canvasRect.height * canvas.scaleFactor;

        int columnPosition = levelNumber - 1;

        // calculate the row and column for the current button
        int row = columnPosition / maxColumns;
        int column = columnPosition % maxColumns;

        // calculate the x and y position of the button in pixels
        float x = (buttonWidth / 2f - canvasWidth / 2f) + (column * buttonWidth) + (column * 20f) + (buttonWidth / 2f);
        float y = (canvasHeight / 2f - rectTransform.rect.height / 2f) - (row * rectTransform.rect.height) - (row * 20f) - (rectTransform.rect.height / 2f);

        // set the position of the button
        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    public void LoadLevel()
    {
        selectedLevelNumber = levelNumber;
        SceneManager.LoadScene("Gameplay");
    }
}
