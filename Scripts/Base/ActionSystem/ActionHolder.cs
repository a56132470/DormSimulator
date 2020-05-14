using System.Collections.Generic;
using UnityEngine;

public class ActionHolder : ScriptableObject
{
    [SerializeField]
    public List<CharacterAction> StudyActions;

    [SerializeField]
    public List<CharacterAction> AmusementActions;

    [SerializeField]
    public List<CharacterAction> LaborActions;
}