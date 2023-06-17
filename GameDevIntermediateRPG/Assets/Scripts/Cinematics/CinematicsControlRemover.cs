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

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
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

