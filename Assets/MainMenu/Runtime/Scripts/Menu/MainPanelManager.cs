using Compixel;
using Compixel.UI;
using Supertactic.Mukani;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Michsky.UI.Dark
{
    public class MainPanelManager : MonoBehaviour
    {
        [Header("PANEL LIST")]
        public List<GameObject> panels = new List<GameObject>();

        [SerializeField] GameObject[] panelEffects;
        [SerializeField] ModalWindowManager exitModalWindow;

        [Header("RESOURCES")]
        public BlurManager homeBlurManager;

        [Header("SETTINGS")]
        public int currentPanelIndex = 0;
        public bool enableBrushAnimation = true;
        public bool enableHomeBlur = true;

        private GameObject currentPanel;
        private GameObject nextPanel;
        private Animator currentPanelAnimator;
        private Animator nextPanelAnimator;

        string panelFadeIn = "Panel In";
        string panelFadeOut = "Panel Out";

        PanelBrushManager currentBrush;
        PanelBrushManager nextBrush;

        [Header("BUTTON PARENT")]
        public RectTransform[] buttonParents;
        public RectTransform buttonHoverTransform;

        public Button loadGameButton;

        public Button pressStartButton;

        [Header("INPUT HANDLER")]
        public PlayerInput playerInput;
        public AudioSource titleCalling;
        string _currentControlScheme;
        bool _isButtonAPressed;
        bool _isTitleCalled;
        bool _isExitModalDisplaying;
        [SerializeField] private SceneReference m_DemoScene;
        [SerializeField] private bool _quitApplicationInsteadLoad;

        void Start()
        {
            currentPanel = panels[currentPanelIndex];
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.Play(panelFadeIn);

            if (enableHomeBlur == true)
                homeBlurManager.BlurInAnim();

            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            //Disable or enable the Load Game based on whether there is a save file
            // loadingButton.interactable = SaveManager.HasSave();
        }

        #region AButton Emplementation

        void OnEnable()
        {
           if (playerInput == null) return;

            //playerInput.actions["Submit"].performed += JumpPerformed;
            playerInput.actions["Jump"].performed += JumpPerformed;
            playerInput.actions["Jump"].canceled += JumpPerformed;
            playerInput.actions["Jump"].started += JumpPerformed;
        }

        private void OnDisable()
        {
            if (playerInput == null) return;

            //playerInput.actions["Submit"].performed -= JumpPerformed;
            playerInput.actions["Jump"].performed -= JumpPerformed;
            playerInput.actions["Jump"].canceled -= JumpPerformed;
            playerInput.actions["Jump"].started -= JumpPerformed;
        }

        private void JumpPerformed(InputAction.CallbackContext context)
        {
            _isButtonAPressed = context.ReadValueAsButton();
        }

        // set the control scheme externally
        public void SetControlsScheme(string controlScheme)
        {
            _currentControlScheme = controlScheme;
            Debug.Log("Control Scheme was changed to: "+controlScheme);
        }

        private IEnumerator PanelAnimRoutine(int panelIndex, float time)
        {
            pressStartButton.gameObject.SetActive(false);
            titleCalling.Play();
            LoadingManager.Show("CARREGANDO", time);
            yield return new WaitForSeconds(time);
            PanelAnim(1);
        }

        #endregion

        private void Update()
        {
            #region PressStart Emplementation

            if (_isTitleCalled || exitModalWindow.isShowing)
                return;

            //We are in title screen and using Keyboard
            if (currentPanelIndex == 0 && _currentControlScheme == "Keyboard&Mouse")
            {
                Mouse currentMouse = Mouse.current;
                if (currentMouse != null)
                {
                    if (currentMouse.leftButton.wasPressedThisFrame)
                    {
                        SetPressStart();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetPressStart();
                }
            }

            //We are in title screen and using gamepad
            else if (currentPanelIndex == 0 && _currentControlScheme == "Gamepad")
            {
                if (_isButtonAPressed)
                {
                    SetPressStart();
                }
            }
            #endregion
        }

        private void SetPressStart()
        {
            _isTitleCalled = true;
            StartCoroutine(PanelAnimRoutine(1, 5f));               
        }

        public void SetButtonHoverParent(int newParent)
        {
            buttonHoverTransform.parent = buttonParents[newParent];
        }

        public void NewGame()
        {
            if (_quitApplicationInsteadLoad)
            {
                Application.Quit();
                return;
            }

            LoadingManager.LoadScene(m_DemoScene.Name);

            //StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, null));
            //Set the scene to the player's house
            //SceneTransitionManager.Instance.SwitchLocation(SceneTransitionManager.Location.PlayerHome);
        }

        public void ContinueGame()
        {
            //StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, LoadGame));
        }

        //To be called after the scene is loaded
        public void LoadGame()
        {

        }

        //IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFirstFrameLoaded)
        //{
        //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        //    //Make sure GameObject persistent so it can continue to run after the scene is loaded
        //    DontDestroyOnLoad(gameObject);

        //    //Wait for the scene to load
        //    while (!asyncLoad.isDone)
        //    {
        //        yield return null;
        //        Debug.Log("Loading...");
        //    }

        //    //Scene loaded
        //    Debug.Log("Loaded!");

        //    yield return new WaitForEndOfFrame();

        //    Debug.Log("First frame was loaded");

        //    onFirstFrameLoaded?.Invoke();

        //    Destroy(gameObject);
        //}

        public void OpenFirstTab()
        {
            currentPanel = panels[currentPanelIndex];
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.Play(panelFadeIn);

            if (enableHomeBlur == true)
                homeBlurManager.BlurInAnim();
        }

        public void PanelAnim(int newPanel)
        {
            if (newPanel != currentPanelIndex)
            {
                //Title screen panel
                if (newPanel == 0)
                {
                    foreach (GameObject go in panelEffects)
                    {
                        go.SetActive(true);
                    }

                    _isTitleCalled = false;
                    pressStartButton.gameObject.SetActive(true);
                }
                else
                //Home panel
                if (newPanel == 1)
                {
                    //Player Main Theme Track
                    MusicManager.instance.PlayTrack(TrackType.MenuThemeTrack, 3);

                    foreach (GameObject go in panelEffects)
                    {
                        go.SetActive(false);
                    }

                    // Put the button hover at button parent 0 of the list
                    SetButtonHoverParent(0);
                }
                else
                //Play game panel
                if (newPanel == 2)
                {
                    // Put the button hover at button parent 1 of the list
                    SetButtonHoverParent(1);
                }
                else if (newPanel == 3)
                {
                    // Put the button hover at button parent 2 of the list
                    SetButtonHoverParent(2);
                }
                //else if(newPanel == 4)
                //{
                //    SetButtonHoverParent(3);
                //}

                currentPanel = panels[currentPanelIndex];

                currentPanelIndex = newPanel;
                nextPanel = panels[currentPanelIndex];

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOut);
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBrushAnimation == true)
                {
                    currentBrush = currentPanel.GetComponent<PanelBrushManager>();
                    if (currentBrush.brushAnimator != null)
                        currentBrush.BrushSplashOut();
                    nextBrush = nextPanel.GetComponent<PanelBrushManager>();
                    if (nextBrush.brushAnimator != null)
                        nextBrush.BrushSplashIn();
                }

                if (currentPanelIndex == 0 && enableHomeBlur == true)
                    homeBlurManager.BlurInAnim();
                else if (currentPanelIndex != 0 && enableHomeBlur == true)
                    homeBlurManager.BlurOutAnim();
            }
        }

        public void NextPage()
        {
            if (currentPanelIndex <= panels.Count - 2)
            {
                currentPanel = panels[currentPanelIndex];
                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOut);

                currentPanelIndex += 1;
                nextPanel = panels[currentPanelIndex];

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBrushAnimation == true)
                {
                    currentBrush = currentPanel.GetComponent<PanelBrushManager>();
                    if (currentBrush.brushAnimator != null)
                        currentBrush.BrushSplashOut();
                    nextBrush = nextPanel.GetComponent<PanelBrushManager>();
                    if (nextBrush.brushAnimator != null)
                        nextBrush.BrushSplashIn();
                }

                if (currentPanelIndex == 0 && enableHomeBlur == true)
                    homeBlurManager.BlurInAnim();
                else if (currentPanelIndex != 0 && enableHomeBlur == true)
                    homeBlurManager.BlurOutAnim();
            }
        }

        public void PrevPage()
        {
            if (currentPanelIndex >= 1)
            {
                currentPanel = panels[currentPanelIndex];
                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOut);

                currentPanelIndex -= 1;
                nextPanel = panels[currentPanelIndex];

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBrushAnimation == true)
                {
                    currentBrush = currentPanel.GetComponent<PanelBrushManager>();
                    if (currentBrush.brushAnimator != null)
                        currentBrush.BrushSplashOut();
                    nextBrush = nextPanel.GetComponent<PanelBrushManager>();
                    if (nextBrush.brushAnimator != null)
                        nextBrush.BrushSplashIn();
                }

                if (currentPanelIndex == 0 && enableHomeBlur == true)
                    homeBlurManager.BlurInAnim();
                else if (currentPanelIndex != 0 && enableHomeBlur == true)
                    homeBlurManager.BlurOutAnim();
            }
        }
    }
}