using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A,B,C,D,E,F,G,H
        }


        [SerializeField] int SceneIndexToLoad =-1;
        [SerializeField] Transform SpawnPoint;
        [SerializeField] DestinationIdentifier destination;

        [SerializeField] float FadeInTime = 1f;
        [SerializeField] float FadeOutTime = 1f;
        [SerializeField] float FadeWaitTime = .25f;

        Portal otherPortal;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag =="Player")
            {

                StartCoroutine(Transition());
                
            }
        }

        private IEnumerator Transition()
        {
            if(SceneIndexToLoad <0)
            {
                Debug.Log("forgot to set transition");
                yield break;
            }

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(FadeOutTime);

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            //saves the important points of the exiting scene
            wrapper.Save();

            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(SceneIndexToLoad);

            wrapper.Load();

            otherPortal = GetOtherPortal();

            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(FadeWaitTime);
            yield return fader.FadeIn(FadeInTime);

            Destroy(gameObject);
        }



        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) { continue; }
                if(portal.destination != destination) { continue; }
                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            //note the disables and enables of the nav mesh are due to some quirks of how the nav and terrain meshes seem to interact
            // when dealing with scene persistance and accidently trying to send the player to past transform position from earliar scene 
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.SpawnPoint.position;
            player.transform.rotation = otherPortal.SpawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;

        }
       
    }
}
