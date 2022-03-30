using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionBoundaries : MonoBehaviour
{
    public GameObject PlayerSelector;
    public ParticleSystem rightFingerParticle;
    public ParticleSystem leftFingerParticle;

    public static PlayerSelectionBoundaries main;
    private void Awake()
    {
        main = this;
    }
}
