﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour {

    //Screens
    public GameObject gameModeScreen;
    public GameObject[] screens;

    [Space(10f)] [Header("Input Fields")]

    public InputField wordInputField;
    public InputField letterInputField;

    [Space(10f)] [Header("Game Screen")]

    public Text playerTurnInfo;
    public Text playerOneErrors;
    public Text playerTwoErrors;

    [Space(10f)] [Header("Win Screen")]

    public Text playerWinner;
    public Text[] playersWords;
    public Text[] playersErrors;

    [Space(10f)] [Header("Others")]

    public GameObject[] playersWordsObjects;
    public Text[] playerOneEmptyTexts;
    public Text[] playerTwoEmptyTexts;

    [HideInInspector]
    public string currentInputFieldText;

    private void Start()
    {
        if (wordInputField == null || letterInputField == null)
            return;

        wordInputField.characterLimit = 10;
        wordInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateChar(addedChar);
        };

        letterInputField.characterLimit = 1;
        letterInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateChar(addedChar);
        };
    }

    public virtual void SetWinnerScreen(int winner)
    {
        if (winner > 1 || winner < 0)
            Debug.LogError("Winner out of index");

        int losser = (winner + 1) % 2;

        playerWinner.text = string.Format("PLAYER {0} WIN!", winner + 1);

        playersWords[winner].text = GameManager.Singleton.players[winner].word;
        playersWords[losser].text = GameManager.Singleton.players[losser].word;

        playersErrors[0].text = playerOneErrors.text;
        playersErrors[1].text = playerTwoErrors.text;
    }

    public virtual void OnWordInputFieldEndEdit(string value)
    {
        currentInputFieldText = value;
    }

    public virtual void OnWordInputFieldValueChanged(string value)
    {
        currentInputFieldText = value.ToUpper();
    }

    public virtual void OnLetterInputFieldEndEdit(string value)
    {
        currentInputFieldText = value;
    }

    public virtual void OnLetterInputFieldValueChanged(string value)
    {
        currentInputFieldText = value.ToUpper();
    }

    public virtual void UpdateErrors(int playerInTurn)
    {
        GameManager.Singleton.players[playerInTurn].IncreaseErrorsCount();

        switch (playerInTurn)
        {
            case 0:
                playerOneErrors.text = string.Format("Player 1 Errors: {0}/10", GameManager.Singleton.players[0].errorsCount);
                break;

            case 1:
                playerTwoErrors.text = string.Format("Player 2 Errors: {0}/10", GameManager.Singleton.players[1].errorsCount);
                break;

            default:
                break;
        }
    }

    public virtual void UpdateSuccess(int playerInTurn, Dictionary<int, char> correctChars)
    {
        int otherPlayer = (playerInTurn + 1) % 2;

        for (int i = 0; i < GameManager.Singleton.players[otherPlayer].wordCharsArray.Length; i++)
        {
            if (correctChars.ContainsKey(i))
            {
                GameManager.Singleton.players[playerInTurn].IncreaseSuccessCount();

                switch (otherPlayer)
                {
                    case 0:
                        playerOneEmptyTexts[i].text = GameManager.Singleton.players[0].wordCharsArray[i].ToString();
                        break;

                    case 1:
                        playerTwoEmptyTexts[i].text = GameManager.Singleton.players[1].wordCharsArray[i].ToString();
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public virtual void ClearInputFields()
    {
        wordInputField.text = "";
        letterInputField.text = "";
    }

    private char ValidateChar(char charToValidate)
    {
        if (!char.IsLetter(charToValidate) && !char.IsWhiteSpace(charToValidate))
            charToValidate = ' ';

        return char.ToUpper(charToValidate);
    }
}
