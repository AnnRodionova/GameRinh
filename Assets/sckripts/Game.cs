using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System;

public class Game : MonoBehaviour
{
    public new AudioSource audio;
    Button[,] buttons;
    Image[] images;
    Lines lines;
    
    void Start()
    {
        int k=0;
        PlayerPrefs.SetInt("Record", k);
        lines = new Lines(ShowBox, PlayCut);
        InitButtons();
        InitImages();
        lines.Start();
    }

    public void ShowBox(int x, int y, int ball)
    {
        buttons[x, y].GetComponent<Image>().sprite = images[ball].sprite;
    }
    public void PlayCut()
    {
        audio.Play(); 
    }
    public void Click()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int nr = GetNumber(name);
        int x = nr % Lines.SIZE;
        int y = nr / Lines.SIZE;
        Debug.Log($"cliked {name} {x} {y} ");
        lines.Click(x, y);
        audio.Play();
    }
    private void InitButtons()
    {
        buttons = new Button[Lines.SIZE, Lines.SIZE];
        for (int nr = 0; nr < Lines.SIZE * Lines.SIZE; nr++)
            buttons[nr % Lines.SIZE, nr / Lines.SIZE] =
            GameObject.Find($"Button ({nr})").GetComponent<Button>();
    }


    private void InitImages()
    {
        images = new Image[Lines.BALLS];
        for (int j = 0; j < Lines.BALLS; j++)
            images[j] =
                GameObject.Find($"Image ({j})").GetComponent<Image>();
    }

    private int GetNumber(string name)
    {
        Regex regex = new Regex("\\((\\d+)\\)");
        Match match = regex.Match(name);
        if (!match.Success)
            throw new Exception( "Unrecognized object name");
        Group group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number);
    } 
}