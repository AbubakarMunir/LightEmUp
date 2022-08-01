using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

	public AudioSource characterSound, enemySound, obstacleSound, mainMusic, BtnTapSound, rewardAudioSource;

	public AudioClip characterHurt, characterAttack, characterKilled,
		characterHitBlock, characterHitCoin, characterHitMushroom, characterGoDown, characterKill, outOfTime, characterDie, finishLevel, levelFailed, characterShoot,
		characterStrongJump, characterShrink, checkpoint, rewardSound , characterHitCage,bossHit,bossStartFight, confettiPopSound, metalHit,enemyKilled,princessSound;

	public AudioClip mushRoomAppear,enemyShoot;

	public AudioClip mainMenuMusicMfx, gpMusicMfx, bonusMusicMfx;

	public AudioClip BtnClick;

	public AudioClip teleport, pressSound, levelFinishVoice, confettiSound;
	public static AudioManager Instance { get { return instance; } }

	private static AudioManager instance;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
		} else
			DestroyImmediate (this.gameObject);
        
	}
    private void Start()
    {
		mainMusic.volume = PreferenceManager.GetMusicVolume();
		characterSound.volume = PreferenceManager.GetSoundVolume();
		BtnTapSound.volume = PreferenceManager.GetSoundVolume();
		enemySound.volume = PreferenceManager.GetSoundVolume();
		obstacleSound.volume = PreferenceManager.GetSoundVolume();
	}
    public void PlayCharacterHurt()
    {
		characterSound.PlayOneShot(characterHurt);
    }
	public void PlayCharacterAttack ()
	{
		characterSound.PlayOneShot (characterAttack);
	}

	public void PlayCharacterKilled ()
	{
		characterSound.PlayOneShot (characterKilled);
	}

	public void PlayCharacterHitBlock()
	{
		obstacleSound.PlayOneShot(characterHitBlock);
	}

	
	public void PlayMetalHitSound()
    {
		obstacleSound.PlayOneShot(metalHit);
    }
	public void PlayCheckpointSound()
    {
		characterSound.PlayOneShot(checkpoint);
    }
	public void PlayCharacterHitCoin ()
	{
		characterSound.PlayOneShot (characterHitCoin);
	}

	public void PlayCharacterHitCage()
	{
		characterSound.PlayOneShot(characterHitCage);
	}

	public void PlayMushRoomAppear ()
	{
		characterSound.PlayOneShot (mushRoomAppear);
	}

	public void PlayBossHitSound()
    {
		enemySound.PlayOneShot(bossHit);
    }

	public void PlayEnemykilledSound()
	{
		enemySound.PlayOneShot(enemyKilled);
	}

	public void PlayBossStartFightSound()
	{
		enemySound.PlayOneShot(bossStartFight);
	}
	public void PlayHitMushRoom ()
	{
		characterSound.PlayOneShot (characterHitMushroom);
	}

	public void PlayCharacterGoDown ()
	{
		characterSound.PlayOneShot (characterGoDown);
	}

	public void PlayCharacterKill ()
	{
		characterSound.PlayOneShot (characterKill);
	}

	public void PlayOutOfTime ()
	{
		mainMusic.mute = true;
		characterSound.PlayOneShot (outOfTime);
	}

	public void PlayConfettiPopSound()
	{
		characterSound.PlayOneShot(confettiPopSound);
	}
	public void PlayMainMusic ()
	{
		mainMusic.Stop ();
		mainMusic.clip = mainMenuMusicMfx;
		mainMusic.Play ();
	}
	public void PlayGameplayMusic()
	{
		mainMusic.Stop();
		mainMusic.clip = gpMusicMfx;
		mainMusic.Play();
	}
	public void PlayBonusMusic ()
	{
		mainMusic.Stop ();
		mainMusic.clip = bonusMusicMfx;
		mainMusic.Play ();
	}

	public void PlayFinishLevel ()
	{
		characterSound.PlayOneShot (finishLevel);
	}

	public void PlayLevelFailed()
	{
		characterSound.PlayOneShot(levelFailed);
	}

	public void PlayCharacterShoot ()
	{
		characterSound.PlayOneShot (characterShoot);
	}

	public void PlayStrongJump ()
	{
		characterSound.PlayOneShot (characterStrongJump);
	}

	public void PlayEnemyShoot ()
	{
		characterSound.PlayOneShot (enemyShoot);
	}
	public void PlayBtnClick()
    {
		BtnTapSound.PlayOneShot(BtnClick);
    }
	public void PlayRewardSound()
    {

        rewardAudioSource.PlayOneShot(rewardSound);
    }
	public void OffSound()
	{
		characterSound.mute = true;
		BtnTapSound.mute = true;
		enemySound.mute = true;
		obstacleSound.mute = true;
		rewardAudioSource.mute = true;
	}
	public void OffMusic()
    {
		mainMusic.mute = true;
	}
	public void OnMusic()
    {
		mainMusic.mute = false;
		//Debug.LogError("All Music Muted false");
    }
	public void OnSound()
	{
		characterSound.mute = false;
		BtnTapSound.mute = false;
		enemySound.mute = false;
		obstacleSound.mute = false;
		rewardAudioSource.mute = false;
		//Debug.LogError("All Sounds Muted false");
	}

	public void SoundVolume(float vol)
    {
		PreferenceManager.SetSoundVolume(vol);
		characterSound.volume = vol;
		BtnTapSound.volume = vol;
		enemySound.volume = vol;
		obstacleSound.volume = vol;
		rewardAudioSource.volume = vol;
	}

	public void MusicVolume(float vol)
    {
		PreferenceManager.SetMusicVolume(vol);
		mainMusic.volume = vol;
	}
	public void PlayTeleport()
	{
		characterSound.PlayOneShot(teleport);
	}
	public void PlayPressSound()
	{
		characterSound.PlayOneShot(pressSound);
	}
	public void PlayLevelFinishVoice()
	{
		characterSound.PlayOneShot(levelFinishVoice);
	}
	public void PlayConfettiSound()
    {
		characterSound.PlayOneShot(confettiSound);
    }

	public void PlayPrincessSound()
	{
		characterSound.PlayOneShot(princessSound);
	}

}
