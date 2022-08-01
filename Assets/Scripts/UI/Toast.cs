using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public  class Toast : MonoBehaviour
{
	public static Toast current;

	public Text toast_Text;
	private CanvasGroup _canvasGroup;

	private float alphachange = 0.1f;
	public GameObject levelBar;

	private float timer = 0, timeToClose = 3f;
	private bool activeTimer = false;
	void Awake()
	{
		current = this;
		_canvasGroup = GetComponentInChildren<CanvasGroup>();
		//toast_Text = GetComponentInChildren<Text> ();
		_canvasGroup.alpha = 0;
		//DontDestroyOnLoad (this.gameObject);
	}
	public bool isActive
	{
		get
		{
			return this._canvasGroup.alpha > 0;
		}
	}

	void Update()
	{
		if (!this.isActive)
			return;

		this._canvasGroup.alpha -= this.alphachange / 100;
		if (activeTimer)
		{
			timer += Time.deltaTime;
			if (timer >= timeToClose)
			{
				Hide();
				timer = 0;
				activeTimer = false;
			}
		}

	}


	public void Show(string message)
    {
		StartCoroutine(Show(message, 0.08f));
    }
	public IEnumerator Show(string message, float hideDelay = 0.08f)
	{
		levelBar.SetActive(true);
		activeTimer = true;
		this.alphachange = 0.1f;
		this._canvasGroup.alpha = 1;
		toast_Text.text = message;
		yield return new WaitForSeconds(3f);
		this.alphachange = 1 / hideDelay;
		yield return null;
		//CancelInvoke ("Hide");
		//Invoke ("Hide", hideDelay);
	}
	/*
	public void Show (string message, float hideDelay = 1f)
	{	
		toast_Text.text = message;
		_canvasGroup.alpha = 1;		
		this.alphachange = 1 / hideDelay;
        //CancelInvoke ("Hide");
        //Invoke ("Hide", hideDelay);
	}
	*/


	private void Hide()
	{
		_canvasGroup.alpha = 0;
		levelBar.SetActive(false);
	}
}
