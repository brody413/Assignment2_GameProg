using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour
{
    // crreate fields to be set
    [SerializeField]
    private Transform GameField; // panel for objects
    [SerializeField]
    private GameObject btn; // button object

    // Start is called before the first frame update
    void Awake()
    {
        // loop to add buttons on screen
        for (int i = 0; i < 16; i++) 
        {
            GameObject button = Instantiate(btn);
            button.name = "" + i;
            button.transform.SetParent(GameField);
        }

    }

   
}
