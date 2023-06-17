using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//note savings typically goes to local drive then users then mitch then appdata then locallow then defaultcompany then gamedevintermediatrpg then save
//if changes are made to how the file is saved you need to get in there and manually deleted the noncompatiable save or else it can break on load

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        PlayerInput playerInput;

        [SerializeField] float fadeInTime = .4f;
        const string defaultSaveFile = "save";

        private void Awake()
        {
            playerInput = new PlayerInput();

            playerInput.PlayerControls.SaveCommandLoad.performed += _ => Load();
            playerInput.PlayerControls.SaveCommandSave.performed += _ => Save();
            playerInput.PlayerControls.DeleteSaveFile.performed += _ => DeleteSaveFile();

            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.fadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        private void OnEnable()
        {
            //note can be more specific or vague to get specific commands or an entire action map
           //playerInput.PlayerControls.SaveCommandLoad.Enable();
            playerInput.PlayerControls.Enable();
        }

        private void OnDisable()
        {
            playerInput.PlayerControls.Disable();
        }

        public void Load()
        {
            print("Load");
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            print("save");
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void DeleteSaveFile()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }

    }
}
