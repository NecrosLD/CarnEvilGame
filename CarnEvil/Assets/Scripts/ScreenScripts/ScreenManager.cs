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

    public DoorScript CarriageObject;

    public InventoryScript IS;

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
                StartCoroutine(ItemPickup(hit.transform.gameObject));
            }
            else if(hit && hit.transform.tag == "CarriageButton")
            {
                switch (hit.transform.GetComponent<CarriageButton>().CarriageNumber)
                {
                    case 5:
                        CarriageObject.NextAreaName = "Carriage 6";
                        break;
                    case 4:
                        CarriageObject.NextAreaName = "Carriage 5";
                        break;
                    case 3:
                        CarriageObject.NextAreaName = "Carriage 4";
                        break;
                    case 2:
                        CarriageObject.NextAreaName = "Carriage 3";
                        break;
                    case 1:
                        CarriageObject.NextAreaName = "Carriage 2";
                        break;
                    case 0:
                        CarriageObject.NextAreaName = "Carriage 1";
                        break;
                    default:
                        CarriageObject.NextAreaName = "Carriage 1";
                        break;
                }
            }
        }
    }

    public IEnumerator ItemPickup(GameObject Item)
    {

        if(IS.Inventory.Count == 6)
        {
            Debug.Log("Cannot pickup Item");
        }
        else
        {
            IS.AddItemToInventory(Item);

            Destroy(Item);            
        }

        yield return null;
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

                if (Backgrounds[index] != null)
                {
                    BackgroundImage.sprite = Backgrounds[index];
                }
                else
                {
                    BackgroundImage.sprite = null;
                }
                           
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
