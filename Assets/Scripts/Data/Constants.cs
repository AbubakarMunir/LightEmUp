using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{
    //checks if character is deselected or not
    public static bool isCharacterDropped;
    public static bool isCharacterBeingDragged;

    public static bool isDraggable = true;

    //checks if player was scaled to enemy piece scale once
    public static bool isPlayerScaledOnce;


    public static int levelToBeLoaded = 0;
    public static int isLevelCompleted = 2;//0 is failed,1 is completed,2 is none, //3 is caged unlocked

    public static float verticalDistanceBwBlocks= 3.4f;   //to move tower blocks on being conquered
    public static float enemyTowerPieceXPos;        //to check while moving to other side that is enemeyTower block conquered or already conquered

    public static int coinsEarnedAtThisLevel;
    public static bool showInterstitial = true;

    public static string _MrecEnabled = "1";//mrec 0 disabled, mrec 1 enabled
    public static float _MrecDelay = 4f;
    public static string enableAds;
    public static int temporarilyUnlockedCharacter=-1;
    public static bool isAdPlaying = false;

    public static float timeBwIA = 40;

    public static int splashToHome = 0;
}
