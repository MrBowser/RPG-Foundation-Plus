using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int SceneIndexToLoad =-1;
        [SerializeField] Transform SpawnPoint;

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
            
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(SceneIndexToLoad);

            otherPortal = GetOtherPortal();

            UpdatePlayer(otherPortal);
            
            Destroy(gameObject);
        }



        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) { continue; }

                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = otherPortal.SpawnPoint.position;
            player.transform.rotation = otherPortal.SpawnPoint.rotation;

        }
       
    }
}
