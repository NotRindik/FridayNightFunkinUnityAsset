using FridayNightFunkin.GamePlay;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FridayNightFunkin.UI
{
    public class VolumeController : MonoBehaviour
    {

        private const string VOLUME_PARAM = "master";
        private const string VOLUME_SAVE = "VolumeSave";
        private const string VOLUME_SAVE_INDEX = "VolumeSaveIndex";
        private const string MUTE_SAVE = "MuteSave";


        private const float START_VOLUME = -10;
        public const int START_VOLUME_TRAY_INDEX = 4;

        public AudioMixerGroup masterMixer;
        public Canvas MainCanvas;
        public Image soundTraySprite;
        public Animator soundTrayAnim;

        private FnfInput fnfInput => InputManager.inputActions;

        private Sprite[] soundTraySprites;

        private float currentVolume;

        private int volumeTrayIndex;

        private bool isMute;

        private void Awake()
        {
            GetSoundtraySprites();
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            MainCanvas.transform.SetParent(transform);
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt(MUTE_SAVE) == 1)
            {
                isMute = true;
                InitializeVolume(-80, 0);
            }
            InitializeInput();
            if (!isMute)
            {
                if (!PlayerPrefs.HasKey(VOLUME_SAVE))
                {
                    InitializeVolume(START_VOLUME, START_VOLUME_TRAY_INDEX);
                }
                else
                {
                    InitializeVolume(PlayerPrefs.GetFloat(VOLUME_SAVE), PlayerPrefs.GetInt(VOLUME_SAVE_INDEX));
                }
            }

            volumeTrayIndex = PlayerPrefs.GetInt(VOLUME_SAVE_INDEX);
            masterMixer.audioMixer.GetFloat(VOLUME_PARAM, out currentVolume);
        }

        private void InitializeInput()
        {
            fnfInput.MenuNavigation.IncreaseSound.Enable();
            fnfInput.MenuNavigation.DecreseSound.Enable();
            fnfInput.MenuNavigation.Mute.Enable();
            fnfInput.MenuNavigation.IncreaseSound.started += IncreseSound;
            fnfInput.MenuNavigation.DecreseSound.started += DecreseSound;
            fnfInput.MenuNavigation.Mute.started += Mute;
        }

        public void Mute(InputAction.CallbackContext obj)
        {
            isMute = !isMute;

            if (isMute)
            {
                UpdateVolume(-80, 0);
                PlayerPrefs.SetInt(MUTE_SAVE, 1);
            }
            else
            {
                PlayerPrefs.SetInt(MUTE_SAVE, 0);
                UpdateVolume(PlayerPrefs.GetFloat(VOLUME_SAVE), PlayerPrefs.GetInt(VOLUME_SAVE_INDEX));
            }
        }

        public void IncreseSound(InputAction.CallbackContext obj)
        {
            if (currentVolume == -80)
            {
                currentVolume = -18;
                AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}soundtray/Volup");
            }
            if (currentVolume < 0)
            {
                AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}soundtray/Volup");
                currentVolume += 2;
            }
            else
                AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}soundtray/VolMAX");

            if (volumeTrayIndex < 9)
                volumeTrayIndex += 1;

            UpdateVolume(currentVolume, volumeTrayIndex);
            SaveVolumeParameters(currentVolume, volumeTrayIndex);
            ForceUnMute();
        }

        public void DecreseSound(InputAction.CallbackContext obj)
        {
            if (currentVolume > -18)
            {
                currentVolume -= 2;
            }
            if (currentVolume == -18)
            {
                currentVolume = -80;
            }
            if (volumeTrayIndex > 0)
                volumeTrayIndex -= 1;
            AudioManager.instance.PlaySoundEffect($"{FilePaths.resources_sfx}soundtray/Voldown");

            UpdateVolume(currentVolume, volumeTrayIndex);
            SaveVolumeParameters(currentVolume, volumeTrayIndex);
            ForceUnMute();
        }

        private void ForceUnMute()
        {
            isMute = false;
            PlayerPrefs.SetInt(MUTE_SAVE, 0);
        }

        private void InitializeVolume(float volume, int VolumeTrayindex)
        {
            masterMixer.audioMixer.SetFloat(VOLUME_PARAM, volume);
            soundTraySprite.sprite = soundTraySprites[VolumeTrayindex];
            SaveVolumeParameters(volume, VolumeTrayindex);
        }

        private void SaveVolumeParameters(float volume, int VolumeTrayindex)
        {
            PlayerPrefs.SetFloat(VOLUME_SAVE, volume);
            PlayerPrefs.SetInt(VOLUME_SAVE_INDEX, VolumeTrayindex);
        }

        private void UpdateVolume(float volume, int VolumeTrayindex)
        {
            StopAllCoroutines();
            soundTrayAnim.Play("Appear");
            masterMixer.audioMixer.SetFloat(VOLUME_PARAM, volume);
            soundTraySprite.sprite = soundTraySprites[VolumeTrayindex];
            StartCoroutine(DisappearSoundTray());
        }

        private IEnumerator DisappearSoundTray()
        {
            bool startTimer = true;
            while (startTimer)
            {
                yield return new WaitForSeconds(1);
                soundTrayAnim.Play("Disappear");
                startTimer = false;
            }
        }

        private void GetSoundtraySprites()
        {
            soundTraySprites = new Sprite[10];
            for (int i = 1; i <= 10; i++)
            {
                soundTraySprites[i - 1] = Resources.Load<Sprite>($"{FilePaths.resources_images_soundTray}bars_{i}");
            }
        }

        private void OnEnable()
        {
            fnfInput.MenuNavigation.IncreaseSound.Enable();
            fnfInput.MenuNavigation.DecreseSound.Enable();
            fnfInput.MenuNavigation.Mute.Enable();
        }

        private void OnDisable()
        {
            fnfInput.MenuNavigation.IncreaseSound.Disable();
            fnfInput.MenuNavigation.DecreseSound.Disable();
            fnfInput.MenuNavigation.Mute.Disable();
        }
    }
}