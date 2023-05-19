using RPG.Core;
using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;



namespace RPG.Cinematics
{
    public class CinematicsControlRemover : MonoBehaviour
    {

        GameObject player;

        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");

        }

        void DisableControl(PlayableDirector pd)
        {
           
            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerControls>().enabled= false;

        }

        void EnableControl(PlayableDirector pd)
        {
            
            player.GetComponent<PlayerControls>().enabled = true;
        }

    }
}

