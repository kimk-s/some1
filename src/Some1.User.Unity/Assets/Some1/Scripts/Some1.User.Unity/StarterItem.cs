using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity
{
    public class StarterItem : MonoBehaviour
    {
        public ProgramEnvironment environment;
        public TMP_Text environmentText;
        public Toggle inMemoryToggle;
        public TMP_InputField waitServerAddressInputField;
        public Button startButton;

        private const string Config_InMemory = "_Config_InMemory";
        private const string Config_WaitServerAddress = "_Config_WaitServerAddress";

        public ProgramArgs GetArgs() => new(
            environment,
            new(
                inMemoryToggle.isOn,
                waitServerAddressInputField.text));

        private void Start()
        {
            environmentText.text = environment.ToString();
            Load();
        }

        private void Load()
        {
            inMemoryToggle.isOn = PlayerPrefs.GetInt($"{environment}_{Config_InMemory}", 0) == 1;
            waitServerAddressInputField.text = PlayerPrefs.GetString($"{environment}_{Config_WaitServerAddress}", "");
        }

        internal void Save()
        {
            PlayerPrefs.SetInt($"{environment}_{Config_InMemory}", inMemoryToggle.isOn ? 1 : 0);
            PlayerPrefs.SetString($"{environment}_{Config_WaitServerAddress}", waitServerAddressInputField.text);
            PlayerPrefs.Save();
        }
    }
}
