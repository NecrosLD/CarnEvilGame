using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    public GameObject[] Screens;

    public int CurrentScreen;

    public Sprite[] Backgrounds;

    public SpriteRenderer BackgroundImage;
    
    public TMP_Text ScreenTitle;

    private void Start()
    {
        ScreenSwitch("Park Entrance");
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero, 100);

            if(hit && hit.transform.tag == "Door")
            {
                ScreenSwitch(hit.transform.GetComponent<DoorScript>().NextAreaName);
            }
            else if(hit && hit.transform.tag == "Item")
            {

            }
        }
    }

    public void ScreenSwitch(string ScreenName)
    {
        for(int i = 0; i < Screens.Length; i++)
        {
            int index = i;

            if (Screens[index].gameObject.name == ScreenName)
            {
                Screens[index].gameObject.SetActive(true);

                //BackgroundImage.sprite = Backgrounds[index];

                ScreenTitle.text = ScreenName;
            }
            else
            {
                Screens[index].gameObject.SetActive(false);
            }
            
        }
    }
}
