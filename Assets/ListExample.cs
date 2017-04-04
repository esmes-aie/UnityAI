using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameSettings
{
    public int WinLimit;
    public int DeathLimit;
    public int NumberOfRounds;
}

public class ListExample : MonoBehaviour
{
    public GameSettings settings;

    // is of dynamic size, can be added to at runtime
    public List<int> collectionOfIntegers;

    // is of fixed size, cannot be added to at runtime
    public int[] arrayOfIntegers;

    public List<GameSettings> settingsss;

    private void Start()
    {
        collectionOfIntegers.Add(69);

        collectionOfIntegers[0] = 5;
        arrayOfIntegers[0] = 5;
    }
}