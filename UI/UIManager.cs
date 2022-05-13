// This script is a Manager that controls the UI HUD (deaths, time, and coins) for the 
// project. All HUD UI commands are issued through the static methods of this class

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	//This class holds a static reference to itself to ensure that there will only be one in existence.
	//Other scripts access this one through its public static methods
	public static UIManager current;

	public TextMeshProUGUI scoreText;		//Text element showing ammount of score
	public TextMeshProUGUI timeText;		//Text element showing amount of time
	public TextMeshProUGUI deathText;		//Text element showing number or deaths
	public TextMeshProUGUI scoreTextEnd;	//Showing ammount of score at the end of level
	public TextMeshProUGUI timeTextEnd;		//Showing amount of time passed playing
	public TextMeshProUGUI deathTextEnd;	//Showing number or deaths at the end of level
	public GameObject levelCompleteText;	//Showing the "Level complete" message


	void Awake()
	{
		//If an UIManager exists and it is not this...
		if (current != null && current != this)
		{
			//...destroy this and exit. There can be only one UIManager
			Destroy(gameObject);
			return;
		}

		//This is the current UIManager and it should persist between scene loads
		current = this;
		DontDestroyOnLoad(gameObject);
	}

	//updates score on screen
	public static void UpdateScoreUI(int score)
	{
		//If there is no current UIManager, exit
		if (current == null)
			return;
		//Update the text score element
		current.scoreText.text = "Score: " + score.ToString();
	}

	//updates time on screen
	public static void UpdateTimeUI(float time)
	{
		//If there is no current UIManager, exit
		if (current == null)
			return;

		//Take the time and convert it into the number of minutes and seconds
		int minutes = (int)(time / 60);
		float seconds = time % 60f;

		//Create the string in the appropriate format for the time
		current.timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}

	//updates deaths on screen
	public static void UpdateDeathUI(int deathCount)
	{
		//If there is no current UIManager, exit
		if (current == null)
			return;

		//update the player death count element
		current.deathText.text = "Deaths: " + deathCount.ToString();
	}

	//hides HUD and shows level complete screen
	public static void DisplayGameOverText()
	{
		//If there is no current UIManager, exit
		if (current == null)
			return;

		EnableLevelComplete();
	}

	//Shows level complete screen
	public static void EnableLevelComplete()
	{
		current.scoreTextEnd.text = current.scoreText.text;
		current.timeTextEnd.text = current.timeText.text;
		current.deathTextEnd.text = current.deathText.text;
		DisableHUD();
		current.levelCompleteText.SetActive(true);
	}

	//Turns off level complete screen
	public static void DisableLevelComplete()
	{
		EnableHUD();
		current.levelCompleteText.SetActive(false);
	}

	//Disables all HUD elements
	public static void DisableHUD()
	{
		current.scoreText.enabled = false;
		current.timeText.enabled = false;
		current.deathText.enabled = false;
	}

	//Enables all HUD elements
	public static void EnableHUD()
	{
		current.scoreText.enabled = true;
		current.timeText.enabled = true;
		current.deathText.enabled = true;
	}
}
