using UnityEngine;
using UnityEngine.Events;

namespace FMOD_Thuleanx {
	public class AudioTrack {
		FMOD.Studio.EventInstance track;
		public string reference;
		// MarkerReader reader;

		public AudioTrack(string reference) {
			this.reference = reference;
			track = FMODUnity.RuntimeManager.CreateInstance(reference);
			// reader = new MarkerReader(track);
		}

		public int GetTrackTimeMS() {
			int timelinePos;
			if (track.getTimelinePosition(out timelinePos) == FMOD.RESULT.OK) 
				return timelinePos;
			else return -1;
		}

		public void SetTime(int time) {
			if (track.setTimelinePosition(time) != FMOD.RESULT.OK) {
				Debug.LogError(string.Format("Cannot seem to set timeline position for the track: {0}", reference));
			}
		}

		public int GetTrackDuration() {
			FMOD.Studio.EventDescription description;
			if (track.getDescription(out description) != FMOD.RESULT.OK) {
				Debug.LogError(string.Format("Cannot seem to get duration of track", reference));
				return -1;
			}
			int length;
			if (description.getLength(out length) != FMOD.RESULT.OK)
				return -1;
			return length;
		}

		public bool IsPlaying() {
			FMOD.Studio.PLAYBACK_STATE state;
			if (track.getPlaybackState(out state) != FMOD.RESULT.OK) {
				return false;
			}
			return state == FMOD.Studio.PLAYBACK_STATE.PLAYING;
		}
		public void Play() => track.start();
		public void Pause() => track.setPaused(true);
		public void Resume() => track.setPaused(false);
		public void Stop() => track.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		// public void Dispose() => reader.StopAndClear();

		// public void OnMarker(UnityAction<string> handler) {
		// 	reader.AttachCallback(handler);
		// }

		// public void RemoveTracker(UnityAction<string> handler) {
		// 	reader.RemoveCallback(handler);
		// }
	}
}