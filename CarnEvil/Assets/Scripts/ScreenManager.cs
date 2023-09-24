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

    public GameObject FadeInObject;

    private void Start()
    {
        StartCoroutine(ScreenSwitch("Park Entrance"));
    }   

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero, 100);

            if(hit && hit.transform.tag == "Door")
            {
                StartCoroutine(ScreenSwitch(hit.transform.GetComponent<DoorScript>().NextAreaName));               
            }
            else if(hit && hit.transform.tag == "Item")
            {

            }
        }
    }

    public IEnumerator ScreenSwitch(string ScreenName)
    {
        Color objectColor = FadeInObject.GetComponent<Image>().color;
        float FadeAmount;
        int fadeSpeed = 5;


        //Fade Out
        while (FadeInObject.GetComponent<Image>().color.a < 1)
        {
            FadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, FadeAmount);

            FadeInObject.GetComponent<Image>().color = objectColor;

            yield return null;
        }

        //-------------------------------------------------------

        //Screen Change
        for (int i = 0; i < Screens.Length; i++)
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

        //-------------------------------------------------------

        //Fade In
        while (FadeInObject.GetComponent<Image>().color.a > 0)
        {
            FadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, FadeAmount);

            FadeInObject.GetComponent<Image>().color = objectColor;

            yield return null;
        }
    }


}
