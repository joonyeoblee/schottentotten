using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class RunHelper : Singleton<RunHelper>
{
}


public class Run
{
	private bool m_IsDone;
	private bool m_IsAbort;

	private IEnumerator m_Action;


	// EachFrame
	public static Run EachFrame(UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunEachFrame(run, action);
		run.Start();

		return run;
	}
	private static IEnumerator RunEachFrame(Run run, UnityAction action)
	{
		run.m_IsDone = false;

		while (true)
		{
			if (!run.m_IsAbort)
			{
				action?.Invoke();
			}
			else
			{
				break;
			}

			yield return null;
		}

		run.m_IsDone = true;
	}

	// Interval
	public static Run Interval(float delay, float interval, UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunInterval(run, delay, interval, 0, action);
		run.Start();

		return run;
	}
	public static Run Interval(float delay, float interval, int count, UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunInterval(run, delay, interval, count, action);
		run.Start();

		return run;
	}
	private static IEnumerator RunInterval(Run run, float delay, float interval, int count, UnityAction action)
	{
		run.m_IsDone = false;

		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		else
		{
			int frameCount = Mathf.RoundToInt(-delay);
			for (int i = 0; i < frameCount; ++i)
			{
				yield return null;
			}
		}

		int repeat = count == 0 ? int.MaxValue : count;

		while (repeat > 0)
		{
			if (!run.m_IsAbort)
			{
				action?.Invoke();
			}
			else
			{
				break;
			}

			if (interval > 0)
			{
				yield return new WaitForSeconds(interval);
			}
			else
			{
				int frameCount = Mathf.Max(1, Mathf.RoundToInt(-interval));
				for (int i = 0; i < frameCount; ++i)
				{
					yield return null;
				}
			}

			repeat -= 1;
		}

		run.m_IsDone = true;
	}

	// After
	private static IEnumerator RunAfter(Run run, float delay, UnityAction action)
	{
		run.m_IsDone = false;

		yield return new WaitForSeconds(delay);

		if (!run.m_IsAbort)
		{
			action?.Invoke();
		}

		run.m_IsDone = true;
	}
	public static Run After(float delay, UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunAfter(run, delay, action);
		run.Start();

		return run;
	}

	// AfterRealTime
	private static IEnumerator RunAfterRealtime(Run run, float delay, UnityAction action)
	{
		run.m_IsDone = false;

		yield return new WaitForSecondsRealtime(delay);

		if (!run.m_IsAbort)
		{
			action?.Invoke();
		}

		run.m_IsDone = true;
	}
	public static Run AfterRealtime(float delay, UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunAfterRealtime(run, delay, action);
		run.Start();

		return run;
	}

	// NextFrame
	private static IEnumerator RunNextFrame(Run run, UnityAction action)
	{
		run.m_IsDone = false;

		yield return new WaitForEndOfFrame();

		if (!run.m_IsAbort)
		{
			action?.Invoke();
		}

		run.m_IsDone = true;
	}
	public static Run NextFrame(UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunNextFrame(run, action);
		run.Start();

		return run;
	}

	// NextUpdate
	private static IEnumerator RunNextUpdate(Run run, UnityAction action)
	{
		run.m_IsDone = false;

		yield return null;

		if (!run.m_IsAbort)
		{
			action?.Invoke();
		}

		run.m_IsDone = true;
	}
	public static Run NextUpdate(UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunNextUpdate(run, action);
		run.Start();

		return run;
	}

	// NextFixedUpdate
	private static IEnumerator RunNextFixedUpdate(Run run, UnityAction action)
	{
		run.m_IsDone = false;

		yield return new WaitForFixedUpdate();

		if (!run.m_IsAbort)
		{
			action?.Invoke();
		}

		run.m_IsDone = true;
	}
	public static Run NextFixedUpdate(UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunNextFixedUpdate(run, action);
		run.Start();

		return run;
	}

	// Wait
	private static IEnumerator RunWait(Run run, Func<bool> predict, UnityAction action)
	{
		run.m_IsDone = false;

		yield return new WaitUntil(predict);

		if (!run.m_IsAbort)
		{
			action?.Invoke();
		}

		run.m_IsDone = true;
	}
	public static Run Wait(Func<bool> predict, UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunWait(run, predict, action);
		run.Start();

		return run;
	}

	// AfterCoroutine
	private static IEnumerator RunAfterCoroutine(Run run, IEnumerator coroutine, UnityAction action)
	{
		run.m_IsDone = false;

		yield return RunHelper.Instance.StartCoroutine(coroutine);

		if (!run.m_IsAbort)
		{
			action?.Invoke();
		}

		run.m_IsDone = true;
	}
	public static Run AfterCoroutine(IEnumerator coroutine, UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunAfterCoroutine(run, coroutine, action);
		run.Start();

		return run;
	}

	// Lerp
	private static IEnumerator RunLerp(Run run, float duration, UnityAction<float> action)
	{
		run.m_IsDone = false;

		float t = 0f;
		while (t < 1.0f)
		{
			t = Mathf.Clamp01(t + Time.deltaTime / duration);
			if (!run.m_IsAbort)
			{
				action?.Invoke(t);
			}

			yield return null;
		}

		run.m_IsDone = true;
	}
	public static Run Lerp(float duration, UnityAction<float> action)
	{
		Run run = new Run();
		run.m_Action = RunLerp(run, duration, action);
		run.Start();

		return run;
	}

	// ExcuteWhenDone
	private IEnumerator RunWaitUntilDone(UnityAction action)
	{
		while (!m_IsDone)
		{
			yield return null;
		}

		action?.Invoke();
	}
	public Run ExcuteWhenDone(UnityAction action)
	{
		Run run = new Run();
		run.m_Action = RunWaitUntilDone(action);
		run.Start();

		return run;
	}


	public void Abort()
	{
		m_IsAbort = true;
	}

	private void Start()
	{
		if (m_Action != null)
		{
			RunHelper.Instance.StartCoroutine(m_Action);
		}
	}
}
