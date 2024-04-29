using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ChangeGameState(GameState state);
public static class SignalHub
{

    public static ChangeGameState OnChangedGameState;
}
