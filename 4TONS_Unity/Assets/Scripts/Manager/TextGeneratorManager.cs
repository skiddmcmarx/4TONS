using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGeneratorManager : MonoBehaviour {

    public float horizontalSpacing;
    public float verticalSpacing;
    public float maxRowLength;
    public Sprite[] font;
    [SerializeField]
    private string fontDecryptionKey;

    public Transform characterTrans;
    public GameObject textObject;

    public string[] messages;

    public Transform[] spawnLocations;

    private Dictionary<string, Sprite> fontDictionary = new Dictionary<string, Sprite>();
    // Use this for initialization
    void Start() {
        InitializeTextGenerator();
    }
    public void InitializeTextGenerator()
    {
        foreach (Sprite sprite in font)
        {
            fontDictionary.Add(sprite.name, sprite);
        }

        for(int i = 0; i < messages.Length; i++)
        {
            writeText(spawnLocations[i], messages[i]);
        }
    }
	
    public void writeText(Transform startingPoint, string text)
    {
        Vector3 spawnPoint = startingPoint.position;
        string[] characters = new string[text.Length];
        for (int i = 0; i < text.Length; i++)
        {
            characters[i] = System.Convert.ToString(text[i]).ToUpper();
        }
        float currentRowLength = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            GameObject GO = Instantiate(textObject, spawnPoint, Quaternion.identity);
            GO.name = characters[i] + " characterObj";
            GO.transform.parent = startingPoint;
            SpriteRenderer sr = GO.GetComponent<SpriteRenderer>();
            if (fontDictionary.ContainsKey(characters[i]))
            {
                sr.sprite = fontDictionary[characters[i]];
            }
            else
            {
                sr.sprite = fontDictionary["_"];
            }
            sr.sortingOrder = -i;
            if ((currentRowLength + horizontalSpacing) < maxRowLength)
            {
                currentRowLength += horizontalSpacing;
                spawnPoint += new Vector3(horizontalSpacing, 0f, 0f);
            }
            else
            {
                spawnPoint += new Vector3(-currentRowLength, -verticalSpacing, 0f);
                currentRowLength = 0;
            }
        }
    }
}
