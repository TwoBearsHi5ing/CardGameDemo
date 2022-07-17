using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HourGlass : MonoBehaviour, IEventSystemHandler
{

	[SerializeField] Image fillTopImage;
	[SerializeField] Image fillBottomImage;
	[SerializeField] Text TimeText;
	[SerializeField] Image sandDotsImage;
	[SerializeField] RectTransform sandPyramidRect;
	[SerializeField] Button endTurnButton;

	[SerializeField]
	public UnityEvent TimerExpired = new UnityEvent();

	[Space(30f)]
	[SerializeField]
	float roundDuration;

	public bool DisableButton;
	public bool stopTimer;
	public	float currentTime = 0f;

	public bool Counting;
	public bool StartNewRound = false;

	float defaultSandPyramidYPos;
	public int AllowEvent = 0;

	public IEnumerator Czekaj()
	{
		yield return new WaitForSeconds(1);
	}

	void Awake()
	{
		currentTime = 20f;
		SetRoundText(roundDuration);
		defaultSandPyramidYPos = sandPyramidRect.anchoredPosition.y;
		sandDotsImage.DOFade(0f, 0f);

		fillTopImage.fillAmount = 0f;
		fillBottomImage.fillAmount = 1f;

		TimeText.DOFade(0f, 0f);

	
		
	}

	public void Efekty_pisaku()
	{
		AllowEvent = 0;
		resetRotation();
		SetClock();
		TimeText.DOFade(1f, .8f);
		sandDotsImage.DOFade(1f, .8f);
		sandDotsImage.material.DOOffset(Vector2.down * -roundDuration, roundDuration).From(Vector2.zero).SetEase(Ease.Linear);

		//Scale Pyramid
		sandPyramidRect.DOScaleY(1f, roundDuration / 3f).From(0f);
		sandPyramidRect.DOScaleY(0f, roundDuration / 1.5f).SetDelay(roundDuration / 3f).SetEase(Ease.Linear);

		sandPyramidRect.anchoredPosition = Vector2.up * defaultSandPyramidYPos;
		sandPyramidRect.DOAnchorPosY(0f, roundDuration).SetEase(Ease.Linear);


		fillTopImage.fillAmount = 1f;
		fillBottomImage.fillAmount = 0f;

		fillTopImage
			.DOFillAmount(0, roundDuration)
			.SetEase(Ease.Linear)
			.OnUpdate(OnTimeUpdate)
			.OnComplete(FinishRound);
	}

	void OnTimeUpdate()
	{
		fillBottomImage.fillAmount = 1f - fillTopImage.fillAmount;
	}

	void Rotate_and_Shake()
	{
		
		TimeText.DOFade(0f, 0f);
		sandDotsImage.DOFade(0f, 0f);

		//TimeText.DOFade(0f, 0f);

		transform
		.DORotate(Vector3.forward * 180f, .8f, RotateMode.FastBeyond360)
		.From(Vector3.zero)
		.SetEase(Ease.InOutBack)
		.OnComplete(Efekty_pisaku);

		transform.DOShakeScale(.8f, .3f, 10, 90f, true);
	}

	void resetRotation()
	{
		transform.Rotate(0, 0, 180);
		fillTopImage.fillAmount = 1f;
		fillBottomImage.fillAmount = 0f;
	}
	void SetRoundText(float value)
	{
		TimeText.text = value.ToString();
	}

	public void FinishRound()
	{
		//transform.rotation = Quaternion.Euler(Vector3.zero);
		fillTopImage.fillAmount = 0f;
		fillBottomImage.fillAmount = 1f;
		//TimeText.DOFade(1f, .8f);
		Counting = false;
		
	}

	public void ResetClock()
	{
		currentTime = 0f;
		TimeText.text = "0";
		
	}
	public void SetClock()
	{
		currentTime = roundDuration;
	}
	
	public void StartTurn()
	{
		//StartNewRound = false;
		Rotate_and_Shake();
	}

	private void OnMouseDown()
	{
		
	}
	void EndTurnVisual()
	{
		KillTween();
		FinishRound();
		Shake();
		TimeText.DOFade(0, 0.1f).OnComplete(ResetClock);
		
	}
	void Shake()
	{
		transform.DOShakeScale(.8f, .3f, 10, 90f, true);
		transform.DOShakePosition(0.8f, new Vector3(0,300,0), 5, 0f, false, true);

	}
	void KillTween()
	{
		fillBottomImage.DOKill();
		fillTopImage.DOKill();
		transform.DOKill();
		sandDotsImage.DOKill();
		sandPyramidRect.DOKill();
	}
	public bool IsTurnOnGoing()
	{
		if (currentTime <= 0)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	public void StopTimer()
	{
		Counting = false;
		EndTurnVisual();
		stopTimer = true;

	}
	
	private void Update()
	{
		

		if (IsTurnOnGoing())
        {
			if (!stopTimer)
			{
				Counting = true;
				currentTime -= 1 * Time.deltaTime;
				TimeText.text = currentTime.ToString("0");
			}
			

		}


		if (!IsTurnOnGoing())
		{
			
		}
		
		

	}
	public void EndTurn()
	{
		Counting = false;

		for (int i = 0; i < 2; i++)
		{
			if (IsTurnOnGoing())
			{

				EndTurnVisual();
				//TimerExpired.Invoke();
				//StartNewRound = true;

				//Invoke("Rotate_and_Shake", 1f);
			}
			else if (!IsTurnOnGoing())
			{
				//Rotate_and_Shake();
				//TimerExpired.Invoke();
				StartNewRound = true;
				
				break;

			}
		}

	}
	
	
    private void FixedUpdate()
    {
		if (currentTime <= 0 && !Counting)
		{
			AllowEvent++;
			Eventinvoke();

		}

		if (DisableButton)
		{
			endTurnButton.interactable = false;
		}
		if (!DisableButton)
		{
			endTurnButton.interactable = true;
		}
	}

	public void Eventinvoke()
	{
		if (currentTime <= 0 && !Counting && AllowEvent == 1)
		{
			StartNewRound = false;
			TimerExpired.Invoke();

		}
	}
}
