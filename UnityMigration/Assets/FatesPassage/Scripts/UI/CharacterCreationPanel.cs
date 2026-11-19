using FatesPassage.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FatesPassage.UI
{
    public sealed class CharacterCreationPanel : MonoBehaviour
    {
        [SerializeField] private InputField nameInput;
        [SerializeField] private Dropdown ancestryDropdown;
        [SerializeField] private Dropdown specializationDropdown;
        [SerializeField] private Text feedback;

        public void CreateCharacter()
        {
            try
            {
                var character = CharacterCreationService.Create(
                    nameInput.text,
                    ancestryDropdown.options[ancestryDropdown.value].text,
                    specializationDropdown.options[specializationDropdown.value].text);
                CharacterSaveService.Save(character);
                if (feedback != null) feedback.text = "Character saved: " + character.name;
            }
            catch (System.Exception ex)
            {
                if (feedback != null) feedback.text = ex.Message;
                Debug.LogWarning(ex.Message);
            }
        }
    }
}
