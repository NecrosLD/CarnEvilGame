using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttractionManager_Dragon : MonoBehaviour
{
    public bool RollercoasterIsMoving;
    public float RollercoasterProgress;

    public int CalibrationProgress;

    public float SyncRate_Fire;
    public float SyncRate_Sound;
    public float SyncRate_Movement;

    //
    public enum DragonPosition { OnStage, Notice, Leaning, OverTrack, TooClose, Danger };

    public float MotionDetectorRate;
    public bool MotionDetected;


    public float DragonAttentionRate;
    public int DragonState;
    public DragonPosition CurrentPosition;

    //

    public float MouseInputX;
    public float MouseInputY;

    public bool PlayerIsMoving;

    public bool Victory;

    public List<TMP_Text> GUI;

    // Start is called before the first frame update
    void Start()
    {
        MotionDetectorRate = 0;

        SyncRate_Fire = 100;
        SyncRate_Sound = 100;
        SyncRate_Movement = 100;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (RollercoasterIsMoving)
        {
            RollercoasterProgress = RollercoasterProgress + 0.1f; 
            
            //Sync Decay
            if(SyncRate_Fire > 0)
            {
                SyncRate_Fire = SyncRate_Fire - 0.05f;
            }
            if(SyncRate_Sound > 0)
            {
                SyncRate_Sound = SyncRate_Sound - 0.03f;
            }
            if(SyncRate_Movement > 0)
            {
                SyncRate_Movement = SyncRate_Movement - 0.01f;
            }
            //

            if (RollercoasterProgress > 100)
            {
                if(DragonState >= 3)
                {
                    CalibrationProgress = 0;
                }
                else if (DragonState < 3 && SyncRate_Fire > 75 && SyncRate_Sound > 75 && SyncRate_Movement > 75)
                {
                    CalibrationProgress = CalibrationProgress + 1;
                }

                //Reset
                RollercoasterProgress = 0;
                RollercoasterIsMoving = false;
                MotionDetectorRate = 0;
                DragonAttentionRate = 0;
                DragonState = 0;
                SyncRate_Fire = 100;
                SyncRate_Movement = 100;
                SyncRate_Sound = 100;
                
                //WinCondition
                if(CalibrationProgress == 3)
                {
                    YouWin();
                }
            }

            //Motion Detector System

            if (PlayerIsMoving)
            {
                if (MotionDetectorRate < 100)
                {
                    MotionDetectorRate = MotionDetectorRate + 2.0f;

                    Debug.Log("Player Is Moving");

                    if (MotionDetectorRate > 50)
                    {
                        MotionDetected = true;
                    }
                }
                else
                {
                    MotionDetectorRate = 100;
                    MotionDetected = true;
                }

            }
            else
            {
                if (MotionDetectorRate > 0)
                {
                    MotionDetectorRate = MotionDetectorRate - 0.5f;

                    Debug.Log("Player Is Slowing Down");
                }
                else
                {
                    MotionDetectorRate = 0;
                    Debug.Log("Player Has Stopped");
                    MotionDetected = false;
                }
            }


            //Dragon Animatronic
            if (MotionDetected)
            {
                if(DragonAttentionRate < 100)
                {
                    DragonAttentionRate = DragonAttentionRate + 1.0f;
                }
                
                if(DragonAttentionRate >= 100 && DragonState != 5)
                {
                    DragonAttentionRate = 0;
                    DragonState = DragonState + 1;
                }
            }
            else
            {
                if(DragonAttentionRate > -100 && DragonState != 0)
                {
                    DragonAttentionRate = DragonAttentionRate - 1.0f;
                }
                if( DragonAttentionRate <= -100 && DragonState != 0)
                {
                    DragonAttentionRate = 0;
                    DragonState = DragonState - 1;
                }
            }
        }
    }

    public void Update()
    {
        MouseInputX = Input.GetAxis("Mouse X");
        MouseInputY = Input.GetAxis("Mouse Y");

        if (MouseInputX != 0 || MouseInputY != 0)
        {
            PlayerIsMoving = true;
        }
        else if (MouseInputX == 0 && MouseInputY == 0)
        {
            PlayerIsMoving = false;
        }

        switch (DragonState)
        {
            case 0:
                CurrentPosition = DragonPosition.OnStage;
                break;
            case 1:
                CurrentPosition = DragonPosition.Notice;
                break;
            case 2:
                CurrentPosition = DragonPosition.Leaning;
                break;
            case 3:
                CurrentPosition = DragonPosition.OverTrack;
                break;
            case 4:
                CurrentPosition = DragonPosition.TooClose;
                break;
            case 5:
                CurrentPosition = DragonPosition.Danger;
                break;
            default:
                CurrentPosition = DragonPosition.OnStage;
                break;
        }


        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero, 100);

            if (hit && hit.transform.tag == "Button_Fire")
            {
                SyncRate_Fire = SyncRate_Fire + 10f;

                if(SyncRate_Fire > 100)
                {
                    SyncRate_Fire = 100;
                }
            }
            else if (hit && hit.transform.tag == "Button_Sound")
            {
                SyncRate_Sound = SyncRate_Sound + 10f;

                if (SyncRate_Sound > 100)
                {
                    SyncRate_Sound = 100;
                }
            }
            else if (hit && hit.transform.tag == "Button_Movement")
            {
                SyncRate_Movement = SyncRate_Movement + 10f;

                if (SyncRate_Movement > 100)
                {
                    SyncRate_Movement = 100;
                }
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            RollercoasterIsMoving = true;
        }

        UIUpdate();
    }
    public void UIUpdate()
    {
        GUI[0].text = "Rollercoaster Progress = " + Mathf.Round(RollercoasterProgress) + "%";
        GUI[1].text = "Dragon State = [" + CurrentPosition + "]";
        GUI[2].text = "Motion Detected = " + MotionDetected;
        GUI[3].text = "Fire Effects Sync Rate = " + Mathf.Round(SyncRate_Fire) + "%";
        GUI[4].text = "Sound Effects Sync Rate = " + Mathf.Round(SyncRate_Sound) + "%";
        GUI[5].text = "Movement Sync Rate = " + Mathf.Round(SyncRate_Movement) + "%";
        GUI[6].text = "Calibration Progress = " + CalibrationProgress + "/3";
    }

    public void YouWin()
    {
        Victory = true;
    }
}
