using System;
using UnityEngine;

public class Safe : MonoBehaviour
{

}

public class SafeButton : MonoBehaviour
{
    [SerializeField] private Action OnPress;
}