﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class E_PlayerEntity : E_Unit
{
    public int playerID;
    public UnityEvent onSwitchTeamEvent;

    public void SetTeam(int newTeamID)
    {
        teamID = newTeamID;

        onSwitchTeamEvent.Invoke();
    }
}
