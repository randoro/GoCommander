using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LobbyData : ICloneable
{
    public int id;
    public string name;

    public LobbyData(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
