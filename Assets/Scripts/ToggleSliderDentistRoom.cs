﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class ToggleSliderDentistRoom : MonoBehaviour {

	/* Global variables needed for Tutorial scene */

    double belowThresholdTimeTutorial;
	bool belowThresholdTutorial	= true;
	static public bool stepOne = true;
	static public bool stepTwo = false;
	static public bool TutorialDone = false;
	static public bool done = false;
	public int threshold = 20;

	//anxiety slider text
	public Text sliderText;

	//the anxiety slider
	public Slider slider;

	//is the anxiety canvas active?
	bool isActive;

	//max value of slider
	public int maxValue = 100;

	//min value of slider
	public int minValue = 0;

	//default value of slider
	int val = 50;

	//following time variables used for making the slider disappear after a couple seconds. There's a better way of doing this
	double startingtime;
	double heldTimeA;
	double heldTimeB;
	double belowThresholdTime;
	bool belowThreshold = false;
    bool hasIntroduced = false;
	public Scene scene;

    Animator anim;
    int SayHi = Animator.StringToHash("Jump");


    //note to future self: Learn about C#'s Time.time
    public static double ConvertToUnixTimestamp(DateTime date){
	    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
	    TimeSpan diff = date.ToUniversalTime() - origin;
	    return Math.Floor(diff.TotalSeconds);
	}

	public void toggleAndIncrement(){
		startingtime = ConvertToUnixTimestamp(DateTime.Now);
		if(int.Parse(sliderText.text) < maxValue){
			val = int.Parse(sliderText.text) + 1;
		}
		else{
			val = maxValue;
		}
		sliderText.text = val.ToString();
        slider.value = val;
		isActive = true;
	}
	public void toggleAndDecrement(){
		startingtime = ConvertToUnixTimestamp(DateTime.Now);
		if(int.Parse(sliderText.text) > minValue){
			val = int.Parse(sliderText.text) - 1;
		}
		else{
			val = minValue;
		}
		sliderText.text = val.ToString();
        slider.value = val;
		isActive = true;
	}


	// Use this for initialization
	void Start () {	
		slider.value = 50;
		belowThresholdTutorial	= true;
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
           if(other.gameObject.tag == "change scene")
        {
            anim.Play(SayHi);
        }
    }

    // Update is called once per frame
    void Update () {
		GlobalVariables.sliderValue = slider.value;
		//keyboard shortcuts to manipulate anxiety slider
		if(Input.GetKey("1")){
			toggleAndIncrement();
		}
		if(Input.GetKey("2")){
			toggleAndDecrement();
		}

		/* 1. Handles Oculus touch input and incrementing/decrementing anxiety value */
		if(OVRInput.GetDown(OVRInput.Button.One)){
			heldTimeA = ConvertToUnixTimestamp(DateTime.Now);
            GlobalVariables.sliderValue = val;
            GlobalFunction.LogToPatientFile(GlobalVariables.Filename, "Another patient Dentist room", "Final", Math.Floor(Time.time - GlobalVariables.startTime), GlobalVariables.sliderValue);
            toggleAndIncrement();
		}
		else if(OVRInput.Get(OVRInput.Button.One)){
			if(ConvertToUnixTimestamp(DateTime.Now) - heldTimeA >= .75){
                GlobalVariables.sliderValue = val;
                GlobalFunction.LogToPatientFile(GlobalVariables.Filename, "Another patient Dentist room", "Final", Math.Floor(Time.time - GlobalVariables.startTime), GlobalVariables.sliderValue);
                toggleAndIncrement();
			}
		}
		if(OVRInput.GetDown(OVRInput.Button.Two)){
			heldTimeB = ConvertToUnixTimestamp(DateTime.Now);
            GlobalVariables.sliderValue = val;
            GlobalFunction.LogToPatientFile(GlobalVariables.Filename, "Another patient Dentist room", "Final", Math.Floor(Time.time - GlobalVariables.startTime), GlobalVariables.sliderValue);
            toggleAndDecrement();
		}
		else if(OVRInput.Get(OVRInput.Button.Two)){
			if(ConvertToUnixTimestamp(DateTime.Now) - heldTimeB >= .75){
                GlobalVariables.sliderValue = val;
                GlobalFunction.LogToPatientFile(GlobalVariables.Filename, "Another patient Dentist room", "Final", Math.Floor(Time.time - GlobalVariables.startTime), GlobalVariables.sliderValue);
                toggleAndDecrement();
			}
		}
				
		if(val <= threshold)
        {
            GlobalVariables.scenesRank.Add("Another patient Dentist room", GlobalVariables.sliderValue);
            SceneManager.LoadScene("4 Patient");
		}
		
	}

}
