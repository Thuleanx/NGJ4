using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thuleanx.Math
{
	public struct TimerUnchained
	{
		public float Duration {get; private set; }
		bool paused;
		float timeLeftLagged;

		public float TimeLeft {
			get { 
				if (!paused)
					timeLeftLagged = Mathf.Max(timeLeftLagged - (Time.unscaledTime - TimeLastSampled), 0);
				TimeLastSampled = Time.unscaledTime;
				return timeLeftLagged;
			}
			set {
				timeLeftLagged = value;
				TimeLastSampled = Time.unscaledTime;
			}
		}

		public float TimeLastSampled {get; private set;}

		public TimerUnchained(float durationSeconds, bool pausedDefault = false) {
			Duration = durationSeconds;
			timeLeftLagged = 0;
			TimeLastSampled = Time.unscaledTime;
			paused = pausedDefault;
		}

		public void Start() { 
			TimeLeft = Duration; 
		}
		public void Pause() { 
			float left = TimeLeft; 
			paused = true;
		}
		public void UnPause() { 
			float left = TimeLeft; 
			paused = false;
		}
		public void Stop() { TimeLeft = 0; }

		public static implicit operator bool(TimerUnchained timer) => timer.TimeLeft > 0;
		public static explicit operator TimerUnchained(float durationSeconds) => new TimerUnchained(durationSeconds);
	}
}