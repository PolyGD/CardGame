﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{

    public Player Player1 = null;
    public Player Player2 = null;
	private Player Winner;

	public int WinningCredit = 100;

	private enum State
	{
		Starting,
		Waiting,
		Player1,
		Player2,
		Ending
	}

	private State state;

	// Use this for initialization
	void Start()
	{
		state = State.Starting;
        Player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>();
        Player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>();

        /*
         Create the two players with the constructors, 
         pass deck with a base deck
         pass a base faction
         */
    }

    private int counter = 0;
	// Update is called once per frame
	void Update()
	{
		switch (state)
		{
			case State.Starting:
				if (Player1 != null || Player2 != null)
				{
					state = State.Waiting;
				}
				break;

			case State.Waiting:
				if (Player1 != null && Player2 != null)
				{
					Player1.GivePermissions();
					Player2.TakeAwayPermissions();
					state = State.Player1;
				}
				break;

			case State.Player1:
				if (Player1.End)
				{
					PrepareTransition(Player1, Player2);
					state = State.Player2;
				}
				break;

			case State.Player2:
				if (Player2.End)
				{
					PrepareTransition(Player2, Player1);
					state = State.Player1;
				}
				break;

			case State.Ending:
				break;
		}
	}

    private Player ActivePlayer() {
        switch (state) {
            case State.Player1: return Player1;
            case State.Player2: return Player2;
        }
        return null;
    }

    public void EndTurn()
    {
        ActivePlayer().End = true;
    }

    void PrepareTransition(Player from, Player to)
	{
		if (to.Credit >= WinningCredit)
		{
			state = State.Ending;
			Winner = to;
			return;
		}
		from.TakeAwayPermissions();
		to.GivePermissions();
		to.GenerateCredit();
	}
}
