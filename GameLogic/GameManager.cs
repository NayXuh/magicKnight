// This script is a Manager that controls the the flow and control of the game. It keeps
// track of player data (score, death count, total game time) and interfaces with
// the UI Manager. All game commands are issued through the static methods of this class

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	//This class holds a static reference to itself to ensure that there will only be
	//one in existence. Other scripts access this one through its public static methods
	public static GameManager current;

	public static int score;							//keeps track of score in the level
	public float deathSequenceDuration = 0f;	//How long player death takes before restarting

	List<Coin> coins;							//The collection of scene coins

	public int numberOfDeaths;							//Number of times player has died
	public float totalGameTime = 0f;			//Length of the total game time
	bool isGameOver;							//Is the game currently over?


	void Awake()
	{
		//If a Game Manager exists and this isn't it...
		if (current != null && current != this)
		{
			//...destroy this and exit. There can only be one Game Manager
			Destroy(gameObject);
			return;
		}

		//Set this as the current game manager
		current = this;

		//Create out collection to hold the coins
		coins = new List<Coin>();

		//Persis this object between scene reloads
		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		//If the game is over, exit
		if (isGameOver)
			return;

		//Update the total game time and tell the UI Manager to update
		totalGameTime += Time.deltaTime;
		UIManager.UpdateTimeUI(totalGameTime);
	}

	public static bool IsGameOver()
	{
		//If there is no current Game Manager, return false
		if (current == null)
			return false;

		//Return the state of the game
		return current.isGameOver;
	}

	public static void RegisterCoin(Coin coin)
	{
		//If there is no current Game Manager, exit
		if (current == null)
			return;

		//If the ocoin collection doesn't already contain this coin, add it
		if (!current.coins.Contains(coin))
			current.coins.Add(coin);
	}
	public static void PlayerGrabbedCoin(Coin coin)
	{
		//If there is no current Game Manager, exit
		if (current == null)
			return;

		//If the coin collection doesn't have this coin, exit
		if (!current.coins.Contains(coin))
			return;

		//Remove the collected coin and add score
		current.coins.Remove(coin);
		score += 100;
		//Tell the UIManager to update the coin text
		UIManager.UpdateScoreUI(score);
	}

	public static void PlayerDied()
	{
		//If there is no current Game Manager, exit
		if (current == null)
			return;

		//Increment the number of player deaths and tell the UIManager
		current.numberOfDeaths++;
		UIManager.UpdateDeathUI(current.numberOfDeaths);

		//resets the score after death
		score = 0;
		UIManager.UpdateScoreUI(score);

		//Invoke the RestartScene() method after a delay
		current.Invoke("RestartScene", current.deathSequenceDuration);
	}

	public static void PlayerWon()
	{
		//If there is no current Game Manager, exit
		if (current == null)
			return;
		//The game is now over
		current.isGameOver = true;
		//Tell UI Manager to show the game over text
		UIManager.DisplayGameOverText();
	}

	public static void EnemyDefeated()
	{
		score += 100;
		UIManager.UpdateScoreUI(score);
	}

	public void RestartScene()
	{
		UIManager.DisableLevelComplete();
		//Reload the current scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
		isGameOver = false;
	}

	public void QuitLevel()
	{
		UIManager.current.levelCompleteText.SetActive(false);
		SceneManager.LoadScene(0);
		isGameOver = false;
	}

	public void ResetStats()
	{
		GameManager.current.totalGameTime = 0f;
        GameManager.current.numberOfDeaths = 0;
        GameManager.score = 0;
        UIManager.UpdateScoreUI(0);
        UIManager.UpdateDeathUI(0);
	}
}
