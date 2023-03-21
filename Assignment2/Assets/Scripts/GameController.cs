using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    // create lst of buttons
    public List<Button> btns = new List<Button>();

    [SerializeField]
    private Sprite blank;

    public Sprite[] pieces;
    public List<Sprite> gamePieces = new List<Sprite>();

    private bool firstGuess;
    private bool secondGuess;
    private int countGuess; 
    private int correctGuess;  // correct guesses
    private int gameGuess; // number of total game guess

    private int firstGuessIndex;
    private int secondGuessIndex;

    private string firstGuessID;
    private string secondGuessID;

     void Awake()
    {
        pieces = Resources.LoadAll<Sprite>("Sprites");
    }
    // Start is called before the first frame update
    void Start()
    {
        getButtons(); //call method
        AddListeners(); // add btn listener
        AddGamePuzzels();
        RandomSpot(gamePieces);
        gameGuess = gamePieces.Count / 2;
    }

    // method to get
    void getButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("gameButton"); // find buttons
        
        // loop to add to list of buttons
        for(int i = 0; i <objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = blank;
        }
    }

    // add 2 of each piece to the game for the number of buttons we have
    void AddGamePuzzels()
    {
        int looper = btns.Count;
        int index = 0;

        // loop to increment
        for(int i = 0; i < looper; i++) 
        {
            if(index == looper / 2)
            {
                index = 0;
            }

            gamePieces.Add(pieces[index]); // add to list
            index++;
        }
    }

    // add a listener method
    void AddListeners()
    {
        // loop for each button in list add a onClick
        foreach(Button btn in btns)
        {
            btn.onClick.AddListener(() => PuzzelPicker()); // set listener
        }
    }

    //OnClick method for buttons
    public void PuzzelPicker()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name; // get names of buttons

        // if statement to check cards
        if (!firstGuess)
        {
            firstGuess = true; // set true so we cant change first guess
            firstGuessIndex = int.Parse(name); // get name

            firstGuessID = gamePieces[firstGuessIndex].name; // get ID

            btns[firstGuessIndex].image.sprite = gamePieces[firstGuessIndex]; // cahnge sprite
        }
        else if(!secondGuess){ // same as a bove but for a second guess
            secondGuess = true;
            secondGuessIndex = int.Parse(name);

            secondGuessID = gamePieces[secondGuessIndex].name;

            btns[secondGuessIndex].image.sprite = gamePieces[secondGuessIndex];

            countGuess++; // add 1 to our guess counter

            StartCoroutine(CheckMatch()); // start coroutine
        }
    }

    // check match couroutine
    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(.5f); // wait timer

        // checkl guess
        if (firstGuessID == secondGuessID) 
        {
            yield return new WaitForSeconds(.5f); // wait timer

            // make buttons not useable
            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            // make buttons disappear 
            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            CheckGameOver(); // check game over
        } else
        {
            // if wrong guess cahnge image back to back of card
            btns[firstGuessIndex].image.sprite = blank;
            btns[secondGuessIndex].image.sprite = blank;
        }

        yield return new WaitForSeconds(.5f); // wait timer
        firstGuess = secondGuess = false; // set guesses back to false
    }

    // check game over
    void CheckGameOver()
    {
        correctGuess++;

        if (correctGuess == gameGuess)
        {
            SceneManager.LoadScene("Scenes/GameOver");
        }
    }

    // randomize where cards go
    void RandomSpot(List<Sprite> list)
    {
        for(int i =0; i <list.Count; i++) 
        {
            Sprite temp = list[i]; // get temp srpite ref
            int ran = Random.Range(0, list.Count); // give me a random index

            list[i] = list[ran]; // assign the random index to i
            list[ran] = temp; // reassign to temp
        }
    }
}
