﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Player : NetworkBehaviour {


    public int index
    {
        get; protected set;
    }

    [SyncVar(hook = "SetWord")]
    public string word;
    

    public int sucessCount
    {
        get; private set;
    }

    public int errorsCount
    {
        get; private set;
    }

    public char[] wordCharsArray
    {
        get; private set;
    }
    //Word chars indexer
    public char this[int i]
    {
        get
        {
            return wordCharsArray[i];
        }
        private set
        {
            wordCharsArray[i] = value;
        }
    }

    private void Start()
    {   
    
    }

    public void SetIndex(int index)
    {
        this.index = index;

        switch (index)
        {
            case 0:
                Observer.Singleton.onPlayerTwoEndsTurn += Turn;
                Observer.Singleton.onPlayerOneEndsTurn += EndTurn;
                break;
            case 1:
                Observer.Singleton.onPlayerOneEndsTurn += Turn;
                Observer.Singleton.onPlayerTwoEndsTurn += EndTurn;
                break;
            default:
                return;
        }

        Debug.Log(string.Format("Player {0} created!", index));
    }

    public void SetWord(string word)
    {
        this.word = word;

        wordCharsArray = word.ToCharArray();

        switch (index)
        {
            case 0:
                for (int i = UIFacade.Singleton.playerOneEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                    UIFacade.Singleton.playerOneEmptyTexts[i].gameObject.SetActive(false);
                break;

            case 1:
                for (int i = UIFacade.Singleton.playerTwoEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                    UIFacade.Singleton.playerTwoEmptyTexts[i].gameObject.SetActive(false);
                break;

            default:
                break;
        }

        Debug.Log(string.Format("Player {0} word: {1}", index, word));
    }

    public Dictionary<int, char> CheckForCharsInWord(char inputChar)
    {
        Dictionary<int, char> charsInWord = new Dictionary<int, char>();

        for (int i = 0; i < wordCharsArray.Length; i++)
        {
            if (inputChar == wordCharsArray[i])
                charsInWord.Add(i, wordCharsArray[i]);
        }

        return charsInWord;
    }

    public void IncreaseSuccessCount()
    {
        sucessCount++;
    }

    public void IncreaseErrorsCount()
    {
        errorsCount++;
    }

    private void Turn()
    {
        gameObject.SetActive(true);

        if (GameManager.Singleton.turn > 1)
        {
            switch (index)
            {
                case 0:
                    UIFacade.Singleton.playersWords[1].SetActive(true);
                    break;

                case 1:
                    UIFacade.Singleton.playersWords[0].SetActive(true);
                    break;

                default:
                    break;
            }
        }
    }

    private void EndTurn()
    {
        gameObject.SetActive(false);

        if (GameManager.Singleton.turn > 1)
        {
            switch (index)
            {
                case 0:
                    UIFacade.Singleton.playersWords[1].SetActive(false);
                    break;

                case 1:
                    UIFacade.Singleton.playersWords[0].SetActive(false);
                    break;

                default:
                    break;
            }
        }
    }


}
