using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI generalDialogueText;
    [SerializeField] private TextMeshProUGUI firstOptionText;
    [SerializeField] private TextMeshProUGUI secondOptionText;
    [SerializeField] private TextMeshProUGUI thirdOptionText;

    private Enemy enemy;

    public void SetupDialogue(string _generalDialogue, string _firstOption, string _secondOption, string _thirdOption, Enemy owner)
    {
        generalDialogueText.text = _generalDialogue;
        firstOptionText.text = _firstOption;
        secondOptionText.text = _secondOption;
        thirdOptionText.text = _thirdOption;
        enemy = owner;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            enemy.ChooseDialogueOption(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            enemy.ChooseDialogueOption(2);;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            enemy.ChooseDialogueOption(3);
        }
    }
}
