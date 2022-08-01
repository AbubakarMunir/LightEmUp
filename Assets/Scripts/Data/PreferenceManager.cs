using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceManager : MonoBehaviour
{
    private const string shieldCount = "ShieldCount";
    private const string bombCount = "bombCount";
    private const string potionCount = "potionCount";
    private const string coins = "Coin";
    private const string keys = "Keys";
    private const string levelStarCount = "levelStarCount";
    private const string mysteryBox = "MysteryBox";
    private const string scoreEarned = "ScoreEarned";
    private const string privacyStatus = "PrivacyStatus";
    private const string adsStatus = "AdsStatus";
    private const string musicVol = "MusicVol";
    private const string soundVol = "SoundVol";
    private const string currentState = "CurrentState";
    private const string runBoostTime = "RunBoostTime";
    private const string bombLimit = "BombLimit";
    private const string bombLevel = "BombLevel";
    private const string runBoostLevel = "RunBoostLevel";
    private const string shieldTime = "ShieldTime";
    private const string shieldLevel = "ShieldLevel";
    private const string livesLimit = "LivesLimit";
    private const string livesLevel = "LivesLevel";
    private const string coinX2Time = "CoinX2Time";
    private const string coinX2Level = "CoinX2Level";
    private const string playSessionLevelCount = "PlaySessionLevelCount";
    private const string characterUnlocked = "CharacterUnlocked";
    private const string characterSelected = "characterSelected";
    private const string levelAdCount = "levelAdCount";
    private const string adsRemovalStatus = "adsRemovalStatus";
    private const string unlimitedLivesStatus = "unlimitedLivesStatus";
    private const string rewardIndex = "RewardIndex";
    private const string lastSavedTime = "LastSavedTime";
    private const string isUnlockPanelActivatedOnce = "isUnlockPanelActivatedOnce";
    private const string isFreeSkinRewarded = "isFreeSkinRewarded";
    private const string attemptedLevelCount = "attemptedLevelCount";
    private const string TempattemptedLevelCount = "TempattemptedLevelCount";
    private const string multiplierPlayedOnceForFree = "isMultiplierPlayedOnceForFree";
    private const string fiftyCoinsPlayedOnceForFree = "isHome50CoinsPlayedOnceForFree";
    private const string isGamePlayedForFirstTime = "isGamePlayedForFirstTime";
    private const string unlockedKeys = "unlockedKeys";
    private const string unlockedSkinPercentage = "unlockedSkinPercentage";


    public static float GetUnlockedSkinPercentage()
    {
        return PlayerPrefs.GetFloat(unlockedSkinPercentage,0);
    }
    public static void SetUnlockedSkinPercentage(float val)
    {
        PlayerPrefs.SetFloat(unlockedSkinPercentage,val);
    }
    public static int GetunlockedKeysCount()
    {
        return PlayerPrefs.GetInt(unlockedKeys, 0);
    }

    public static void IncrementunlockedKeysCount()
    {
        SetunlockedKeysCount(GetunlockedKeysCount()+1);
    }
    public static void ResetunlockedKeysCount()
    {
        SetunlockedKeysCount(0);
    }
    public static void SetunlockedKeysCount(int count)
    {
        PlayerPrefs.SetInt(unlockedKeys, count);
    }
    public static int GetunlimitedLivesStatus()
    {
        return PlayerPrefs.GetInt(unlimitedLivesStatus, 0);
    }
    public static void SetunlimitedLivesStatus(int status)
    {
        PlayerPrefs.SetInt(unlimitedLivesStatus, status);
    }
    public static int GetAttemptedLevelCount()
    {
        return PlayerPrefs.GetInt(attemptedLevelCount, 0);
    }
    public static void SetAttemptedLevelCount(int attemptedLvlCount)
    {
        PlayerPrefs.SetInt(attemptedLevelCount, attemptedLvlCount);
    }

    public static int GetTemporaryAttemptedLevelCount()
    {
        return PlayerPrefs.GetInt(TempattemptedLevelCount, 0);
    }
    public static void SetTemporaryAttemptedLevelCount(int attemptedLvlCount)
    {
        PlayerPrefs.SetInt(TempattemptedLevelCount, attemptedLvlCount);
    }

    public static int GetadsRemovalStatus()
    {
        return PlayerPrefs.GetInt(adsRemovalStatus, 0);
    }
    public static void SetadsRemovalStatus(int status)
    {
        PlayerPrefs.SetInt(adsRemovalStatus, status);
    }

    public static int GetlevelAdCount()
    {
        return PlayerPrefs.GetInt(levelAdCount,0);
    }
    public static void SetlevelAdCount(int count)
    {
        PlayerPrefs.SetInt(levelAdCount,count);
    }

    public static float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat(soundVol, 1.0f);
    }
    public static void SetSoundVolume(float vol)
    {
        PlayerPrefs.SetFloat(soundVol, vol);
    }
    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(musicVol, 1f);
    }
    public static void SetMusicVolume(float vol)
    {
        PlayerPrefs.SetFloat(musicVol, vol);
    }
    public static int GetAdStatus()
    {
        return PlayerPrefs.GetInt(adsStatus, 1);
    }
    public static void SetAdsStatus()
    {
        PlayerPrefs.SetInt(adsStatus, 0);
    }
    public static int GetPrivacyPolicyStatus()
    {
        return PlayerPrefs.GetInt(privacyStatus, 0);
    }
    public static void SetPrivacyPolicyStatus()
    {
        PlayerPrefs.SetInt(privacyStatus, 1);
    }

    public static int GetTotalScore()
    {
        return PlayerPrefs.GetInt(scoreEarned, 0);
    }
    public static void SetTotalScore(int value)
    {
        PlayerPrefs.SetInt(scoreEarned, value);
    }
    public static float GetMysteryBoxValue()
    {
        return PlayerPrefs.GetFloat(mysteryBox, 0);
    }
    public static void SetMysteryBoxValue(float value)
    {
        PlayerPrefs.SetFloat(mysteryBox, value);
    }
    public static int GetKeys()
    {
        return PlayerPrefs.GetInt(keys, 0);
    }
    public static void SetKeys(int value)
    {
        PlayerPrefs.SetInt(keys, value);
    }
    public static int GetCoins()
    {
        return PlayerPrefs.GetInt(coins, 0);
    }
    public static void SetCoins(int value)
    {
        PlayerPrefs.SetInt(coins, value);
    }
    public static int GetShieldCount()
    {
        return PlayerPrefs.GetInt(shieldCount, 0);
    }

    public static void SetShieldCount(int value)
    {
        PlayerPrefs.SetInt(shieldCount,value);
    }

    public static int GetBombCount()
    {
        return PlayerPrefs.GetInt(bombCount, 0);
    }

    public static void SetBombCount(int value)
    {
        PlayerPrefs.SetInt(bombCount, value);      
    }

    public static int GetPotionCount()
    {
        return PlayerPrefs.GetInt(potionCount, 0);
    }

    public static void SetPotionCount(int value)
    {
        PlayerPrefs.SetInt(potionCount, value);
    }

    public static int GetLevelStarCount(int lvlNo)
    {
        return PlayerPrefs.GetInt(levelStarCount + lvlNo, 0);
    }

    public static void SetLevelStarCount(int lvlNo, int starCount)
    {
        PlayerPrefs.SetInt(levelStarCount + lvlNo, starCount);
    }
    
    public static int GetRunBoostTime()
    {
        return PlayerPrefs.GetInt(runBoostTime, 5);
    }
    public static void SetRunBoostTime(int _boostTime)
    {
        PlayerPrefs.SetInt(runBoostTime, _boostTime);
    }
    public static int GetBombLimit()
    {
        return PlayerPrefs.GetInt(bombLimit, 3);
    }
    public static void SetBombLimit(int _bombLimit)
    {
        PlayerPrefs.SetInt(bombLimit, _bombLimit);
    }
    public static int GetBombLevel()
    {
        return PlayerPrefs.GetInt(bombLevel, 1);
    }
    public static void SetBombLevel(int _bombLevel)
    {
        PlayerPrefs.SetInt(bombLevel, _bombLevel);
    }
    public static int GetRunBoostLevel()
    {
        return PlayerPrefs.GetInt(runBoostLevel, 1);
    }
    public static void SetRunBoostLevel(int _boostLevel)
    {
        PlayerPrefs.SetInt(runBoostLevel, _boostLevel);
    }
    public static int GetShieldTime()
    {
        return PlayerPrefs.GetInt(shieldTime, 5);
    }
    public static void SetShieldTime(int _shieldTime)
    {
        PlayerPrefs.SetInt(shieldTime, _shieldTime);
    }
    public static int GetShieldLevel()
    {
        return PlayerPrefs.GetInt(shieldLevel, 1);
    }
    public static void SetShieldLevel(int _shieldLevel)
    {
        PlayerPrefs.SetInt(shieldLevel, _shieldLevel);
    }
    public static int GetLivesLevel()
    {
        return PlayerPrefs.GetInt(livesLevel, 1);
    }
    public static void SetLivesLevel(int _livesLevel)
    {
        PlayerPrefs.SetInt(livesLevel, _livesLevel);
    }
    public static int GetLivesLimit()
    {
        return PlayerPrefs.GetInt(livesLimit, 3);
    }
    public static void SetLivesLimit(int _livesLimit)
    {
        PlayerPrefs.SetInt(livesLimit, _livesLimit);
    }
    public static int GetCoinX2Time()
    {
        return PlayerPrefs.GetInt(coinX2Time, 5);
    }
    public static void SetCoinX2Time(int _coinX2Time)
    {
        PlayerPrefs.SetInt(coinX2Time, _coinX2Time);
    }
    public static int GetCoinX2Level()
    {
        return PlayerPrefs.GetInt(coinX2Level, 1);
    }
    public static void SetCoinX2Level(int _coinX2Level)
    {
        PlayerPrefs.SetInt(coinX2Level, _coinX2Level);
    }
    
    public static int GetPlaySessionLevelCount()
    {
        return PlayerPrefs.GetInt(playSessionLevelCount, 0);
    }

    public static void SetPlaySessionLevelCount(int levelSessionCount)
    {
        PlayerPrefs.SetInt(playSessionLevelCount, levelSessionCount);
    }

    public static int GetCharacterUnlockedStatus(int characterIndex)
    {
        return PlayerPrefs.GetInt(characterUnlocked + characterIndex.ToString(), 0);
    }

    public static void SetCharacterUnlockedStatus(int characterIndex)
    {
        PlayerPrefs.SetInt(characterUnlocked + characterIndex.ToString(), 1);
    }
    public static int GetCharacterSelectedIndex()
    {
        return PlayerPrefs.GetInt(characterSelected, 0);
    }

    public static void SetCharacterSelectedIndex(int index)
    {
        PlayerPrefs.SetInt(characterSelected, index);
    }

    public static int GetRewardIndex()
    {
        return PlayerPrefs.GetInt(rewardIndex, 0);
    }
    public static void SetRewardIndex(int _rewardindex)
    {
        PlayerPrefs.SetInt(rewardIndex, _rewardindex);
    }
    public static string GetLastSavedTime()
    {
        return PlayerPrefs.GetString(lastSavedTime, DateTime.Now.ToBinary().ToString());
    }
    public static void SetLastSavedTime(string _lastSavedTime)
    {
        PlayerPrefs.SetString(lastSavedTime, _lastSavedTime);
    }

    public static int GetIsHomeFiftyCoinsPlayedOnce()
    {
        return PlayerPrefs.GetInt(fiftyCoinsPlayedOnceForFree, 0);
    }

    public static void SetIsHomeFiftyCoinsPlayedOnce()
    {
        PlayerPrefs.SetInt(fiftyCoinsPlayedOnceForFree, 1);
    }

    public static int GetIsFreeSkinRewarded()
    {
        return PlayerPrefs.GetInt(isFreeSkinRewarded, 0);
    }

    public static void SetIsFreeSkinRewarded()
    {
        PlayerPrefs.SetInt(isFreeSkinRewarded, 1);
    }

    public static int GetIsMultiplierPlayedOnce()
    {
        return PlayerPrefs.GetInt(multiplierPlayedOnceForFree, 0);
    }

    public static void SetIsMultiplierPlayedOnce()
    {
        PlayerPrefs.SetInt(multiplierPlayedOnceForFree, 1);
    }

    public static int GetIsUnlockPanelActivatedOnce()
    {
        return PlayerPrefs.GetInt(isUnlockPanelActivatedOnce, 0);
    }

    public static void SetIsUnlockPanelActivatedOnce()
    {
        PlayerPrefs.SetInt(isUnlockPanelActivatedOnce, 1);
    }

    public static int GetIsGamePlayedForFirstTime()
    {
        return PlayerPrefs.GetInt(isGamePlayedForFirstTime, 1);
    }

    public static void SetIsGamePlayedForFirstTime()
    {
        PlayerPrefs.SetInt(isGamePlayedForFirstTime, 0);
    }

}
