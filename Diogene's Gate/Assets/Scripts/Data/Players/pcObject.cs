﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PC Card", menuName = "new PC")]
public class pcObject : ScriptableObject
{
    // Start is called before the first frame update
    public string pcName;
    public int dodge;
    public int health;
    public int mana;
    public List<skillObject> skills = new List<skillObject>();

    public bool Equals(pcObject other)
    {
        if (other == null)
        {
            return false;
        }

        if (string.Compare(this.pcName, other.pcName) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
