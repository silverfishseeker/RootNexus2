using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAction : IBaseAction {

    public SpriteRenderer objetivo;
    public Sprite newSprite;

    protected override void SubRun(){
        objetivo.sprite = newSprite;
    }
}