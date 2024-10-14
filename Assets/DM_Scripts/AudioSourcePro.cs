using antilunchbox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
[AddComponentMenu("AntiLunchBox/AudioSourcePro")]
[ExecuteInEditMode]
public class AudioSourcePro : MonoBehaviour
{
	public AudioSource audioSource;

	public ClipType clipType;

	public string clipName = string.Empty;

	public string groupName = string.Empty;

	private bool isBound;

	private bool isVisible;

	public bool OnStartActivated;

	public bool OnVisibleActivated;

	public bool OnInvisibleActivated;

	public bool OnCollisionEnterActivated;

	public bool OnCollisionExitActivated;

	public bool OnTriggerEnterActivated;

	public bool OnTriggerExitActivated;

	public bool OnMouseEnterActivated;

	public bool OnMouseClickActivated;

	public bool OnEnableActivated;

	public bool OnDisableActivated;

	public bool OnCollision2dEnterActivated;

	public bool OnCollision2dExitActivated;

	public bool OnTriggerEnter2dActivated;

	public bool OnTriggerExit2dActivated;

	public bool OnParticleCollisionActivated;

	public bool ShowEditor3D;

	public bool ShowEditor2D;

	public bool ShowEventTriggers;

	public int numSubscriptions;

	public List<AudioSubscription> audioSubscriptions = new List<AudioSubscription>();

	public bool componentsAreValid
	{
		get
		{
			for (int i = 0; i < audioSubscriptions.Count; i++)
			{
				if (!audioSubscriptions[i].isStandardEvent && !audioSubscriptions[i].componentIsValid)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool audioIsValid
	{
		get
		{
			bool flag = false;
			switch (clipType)
			{
			case ClipType.AudioClip:
				flag = (audioSource.clip != null);
				if (flag)
				{
				}
				break;
			case ClipType.ClipFromSoundManager:
				flag = SoundManager.ClipNameIsValid(clipName);
				if (flag)
				{
				}
				break;
			case ClipType.ClipFromGroup:
				flag = SoundManager.GroupNameIsValid(groupName);
				if (flag)
				{
				}
				break;
			}
			if (!flag)
			{
			}
			return flag;
		}
	}

	public bool bypassEffects
	{
		get
		{
			return audioSource.bypassEffects;
		}
		set
		{
			audioSource.bypassEffects = value;
		}
	}

	public bool bypassListenerEffects
	{
		get
		{
			return audioSource.bypassListenerEffects;
		}
		set
		{
			audioSource.bypassListenerEffects = value;
		}
	}

	public bool bypassReverbZones
	{
		get
		{
			return audioSource.bypassReverbZones;
		}
		set
		{
			audioSource.bypassReverbZones = value;
		}
	}

	public AudioClip clip
	{
		get
		{
			switch (clipType)
			{
			case ClipType.ClipFromSoundManager:
				return SoundManager.Load(clipName);
			case ClipType.ClipFromGroup:
				return SoundManager.LoadFromGroup(groupName);
			default:
				return audioSource.clip;
			}
		}
		set
		{
			switch (clipType)
			{
			case ClipType.ClipFromSoundManager:
			case ClipType.ClipFromGroup:
				clipType = ClipType.AudioClip;
				audioSource.clip = value;
				break;
			default:
				audioSource.clip = value;
				break;
			}
		}
	}

	public float dopplerLevel
	{
		get
		{
			return audioSource.dopplerLevel;
		}
		set
		{
			audioSource.dopplerLevel = value;
		}
	}

	public bool ignoreListenerPause
	{
		get
		{
			return audioSource.ignoreListenerPause;
		}
		set
		{
			audioSource.ignoreListenerPause = value;
		}
	}

	public bool ignoreListenerVolume
	{
		get
		{
			return audioSource.ignoreListenerVolume;
		}
		set
		{
			audioSource.ignoreListenerVolume = value;
		}
	}

	public bool isPlaying => audioSource.isPlaying;

	public bool loop
	{
		get
		{
			return audioSource.loop;
		}
		set
		{
			audioSource.loop = value;
		}
	}

	public float maxDistance
	{
		get
		{
			return audioSource.maxDistance;
		}
		set
		{
			audioSource.maxDistance = value;
		}
	}

	public float minDistance
	{
		get
		{
			return audioSource.minDistance;
		}
		set
		{
			audioSource.minDistance = value;
		}
	}

	public bool mute
	{
		get
		{
			return audioSource.mute;
		}
		set
		{
			audioSource.mute = value;
		}
	}

	public AudioMixerGroup outputAudioMixerGroup
	{
		get
		{
			return audioSource.outputAudioMixerGroup;
		}
		set
		{
			audioSource.outputAudioMixerGroup = value;
		}
	}

	public float panStereo
	{
		get
		{
			return audioSource.panStereo;
		}
		set
		{
			audioSource.panStereo = value;
		}
	}

	public float pitch
	{
		get
		{
			return audioSource.pitch;
		}
		set
		{
			audioSource.pitch = value;
		}
	}

	public bool playOnAwake
	{
		get
		{
			return audioSource.playOnAwake;
		}
		set
		{
			audioSource.playOnAwake = value;
		}
	}

	public int priority
	{
		get
		{
			return audioSource.priority;
		}
		set
		{
			audioSource.priority = value;
		}
	}

	public float reverbZoneMix
	{
		get
		{
			return audioSource.reverbZoneMix;
		}
		set
		{
			audioSource.reverbZoneMix = value;
		}
	}

	public AudioRolloffMode rolloffMode
	{
		get
		{
			return audioSource.rolloffMode;
		}
		set
		{
			audioSource.rolloffMode = value;
		}
	}

	public float spatialBlend
	{
		get
		{
			return audioSource.spatialBlend;
		}
		set
		{
			audioSource.spatialBlend = value;
		}
	}

	public float spread
	{
		get
		{
			return audioSource.spread;
		}
		set
		{
			audioSource.spread = value;
		}
	}

	public float time
	{
		get
		{
			return audioSource.time;
		}
		set
		{
			audioSource.time = value;
		}
	}

	public int timeSamples
	{
		get
		{
			return audioSource.timeSamples;
		}
		set
		{
			audioSource.timeSamples = value;
		}
	}

	public AudioVelocityUpdateMode velocityUpdateMode
	{
		get
		{
			return audioSource.velocityUpdateMode;
		}
		set
		{
			audioSource.velocityUpdateMode = value;
		}
	}

	public float volume
	{
		get
		{
			return audioSource.volume;
		}
		set
		{
			audioSource.volume = value;
		}
	}

	private void Awake()
	{
		if (audioSource == null)
		{
			audioSource = base.gameObject.AddComponent<AudioSource>();
		}
		if (Application.isPlaying)
		{
			if (audioIsValid && (clipType == ClipType.ClipFromSoundManager || clipType == ClipType.ClipFromGroup) && playOnAwake)
			{
				Play(0uL);
			}
			isVisible = false;
		}
	}

	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			Unbind();
		}
		if (audioSource != null)
		{
			audioSource.clip = null;
		}
	}

	public void Bind()
	{
		if (!isBound)
		{
			foreach (AudioSubscription audioSubscription in audioSubscriptions)
			{
				if (!audioSubscription.isStandardEvent)
				{
					audioSubscription.Bind(this);
				}
				else
				{
					BindStandardEvent(audioSubscription.standardEvent, activated: true);
				}
			}
			isBound = true;
		}
	}

	public void Unbind()
	{
		if (isBound)
		{
			isBound = false;
			foreach (AudioSubscription audioSubscription in audioSubscriptions)
			{
				if (!audioSubscription.isStandardEvent)
				{
					audioSubscription.Unbind();
				}
				else
				{
					BindStandardEvent(audioSubscription.standardEvent, activated: false);
				}
			}
		}
	}

	public void BindStandardEvent(AudioSourceStandardEvent evt, bool activated)
	{
		switch (evt)
		{
		case AudioSourceStandardEvent.OnStart:
			OnStartActivated = activated;
			break;
		case AudioSourceStandardEvent.OnVisible:
			OnVisibleActivated = activated;
			break;
		case AudioSourceStandardEvent.OnInvisible:
			OnInvisibleActivated = activated;
			break;
		case AudioSourceStandardEvent.OnCollisionEnter:
			OnCollisionEnterActivated = activated;
			break;
		case AudioSourceStandardEvent.OnCollisionExit:
			OnCollisionExitActivated = activated;
			break;
		case AudioSourceStandardEvent.OnTriggerEnter:
			OnTriggerEnterActivated = activated;
			break;
		case AudioSourceStandardEvent.OnTriggerExit:
			OnTriggerExitActivated = activated;
			break;
		case AudioSourceStandardEvent.OnMouseEnter:
			OnMouseEnterActivated = activated;
			break;
		case AudioSourceStandardEvent.OnMouseClick:
			OnMouseClickActivated = activated;
			break;
		case AudioSourceStandardEvent.OnEnable:
			OnEnableActivated = activated;
			break;
		case AudioSourceStandardEvent.OnDisable:
			OnDisableActivated = activated;
			break;
		case AudioSourceStandardEvent.OnCollisionEnter2D:
			OnCollision2dEnterActivated = activated;
			break;
		case AudioSourceStandardEvent.OnCollisionExit2D:
			OnCollision2dExitActivated = activated;
			break;
		case AudioSourceStandardEvent.OnTriggerEnter2D:
			OnTriggerEnter2dActivated = activated;
			break;
		case AudioSourceStandardEvent.OnTriggerExit2D:
			OnTriggerExit2dActivated = activated;
			break;
		case AudioSourceStandardEvent.OnParticleCollision:
			OnParticleCollisionActivated = activated;
			break;
		}
	}

	private void PlaySoundInternal(AudioSourceStandardEvent evt)
	{
		AudioSubscription audioSubscription = FindSubscriptionForEvent(evt);
		if (audioSubscription != null)
		{
			switch (audioSubscription.actionType)
			{
			case AudioSourceAction.None:
				break;
			case AudioSourceAction.Play:
				PlayHandler();
				break;
			case AudioSourceAction.PlayLoop:
				PlayLoopHandler();
				break;
			case AudioSourceAction.PlayCapped:
				PlayCappedHandler(audioSubscription.cappedName);
				break;
			case AudioSourceAction.Stop:
				StopHandler();
				break;
			}
		}
	}

	private AudioSubscription FindSubscriptionForEvent(AudioSourceStandardEvent evt)
	{
		return audioSubscriptions.Find((AudioSubscription obj) => obj.isStandardEvent && obj.standardEvent == evt);
	}

	private void PlayHandler()
	{
		Play(0uL);
	}

	private void PlayCappedHandler(string cappedID)
	{
		SoundManager.PlayCappedSFX(audioSource, clip, cappedID, volume, pitch);
	}

	private void PlayLoopHandler()
	{
		SoundManager.PlaySFX(audioSource, clip, looping: true, 0f, volume, pitch);
	}

	private void StopHandler()
	{
		Stop();
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			if (!isBound && componentsAreValid && audioIsValid)
			{
				Bind();
			}
			if (OnStartActivated)
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnStart);
			}
		}
	}

	private void OnBecameVisible()
	{
		if (OnVisibleActivated && !isVisible)
		{
			isVisible = true;
			PlaySoundInternal(AudioSourceStandardEvent.OnVisible);
		}
	}

	private void OnBecameInvisible()
	{
		if (OnInvisibleActivated)
		{
			isVisible = false;
			PlaySoundInternal(AudioSourceStandardEvent.OnInvisible);
		}
	}

	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			if (!isBound && componentsAreValid && audioIsValid)
			{
				Bind();
			}
			if (OnEnableActivated)
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnEnable);
			}
		}
	}

	private void OnDisable()
	{
		if (Application.isPlaying)
		{
			if (OnDisableActivated)
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnDisable);
			}
			Unbind();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (OnTriggerEnter2dActivated)
		{
			AudioSubscription audioSubscription = FindSubscriptionForEvent(AudioSourceStandardEvent.OnTriggerEnter2D);
			if (audioSubscription != null && (!audioSubscription.filterLayers || (audioSubscription.layerMask & (1 << other.gameObject.layer)) == 0) && (!audioSubscription.filterTags || audioSubscription.tags.Contains(other.gameObject.tag)) && (!audioSubscription.filterNames || audioSubscription.names.Contains(other.gameObject.name)))
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnTriggerEnter2D);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (OnTriggerExit2dActivated)
		{
			AudioSubscription audioSubscription = FindSubscriptionForEvent(AudioSourceStandardEvent.OnTriggerExit2D);
			if (audioSubscription != null && (!audioSubscription.filterLayers || (audioSubscription.layerMask & (1 << other.gameObject.layer)) == 0) && (!audioSubscription.filterTags || audioSubscription.tags.Contains(other.gameObject.tag)) && (!audioSubscription.filterNames || audioSubscription.names.Contains(other.gameObject.name)))
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnTriggerExit2D);
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (OnCollision2dEnterActivated)
		{
			AudioSubscription audioSubscription = FindSubscriptionForEvent(AudioSourceStandardEvent.OnCollisionEnter2D);
			if (audioSubscription != null && (!audioSubscription.filterLayers || (audioSubscription.layerMask & (1 << collision.gameObject.layer)) == 0) && (!audioSubscription.filterTags || audioSubscription.tags.Contains(collision.gameObject.tag)) && (!audioSubscription.filterNames || audioSubscription.names.Contains(collision.gameObject.name)))
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnCollisionEnter2D);
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (OnCollision2dExitActivated)
		{
			AudioSubscription audioSubscription = FindSubscriptionForEvent(AudioSourceStandardEvent.OnCollisionExit2D);
			if (audioSubscription != null && (!audioSubscription.filterLayers || (audioSubscription.layerMask & (1 << collision.gameObject.layer)) == 0) && (!audioSubscription.filterTags || audioSubscription.tags.Contains(collision.gameObject.tag)) && (!audioSubscription.filterNames || audioSubscription.names.Contains(collision.gameObject.name)))
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnCollisionExit2D);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (OnCollisionEnterActivated)
		{
			AudioSubscription audioSubscription = FindSubscriptionForEvent(AudioSourceStandardEvent.OnCollisionEnter);
			if (audioSubscription != null && (!audioSubscription.filterLayers || (audioSubscription.layerMask & (1 << collision.gameObject.layer)) == 0) && (!audioSubscription.filterTags || audioSubscription.tags.Contains(collision.gameObject.tag)) && (!audioSubscription.filterNames || audioSubscription.names.Contains(collision.gameObject.name)))
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnCollisionEnter);
			}
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (OnCollisionExitActivated)
		{
			AudioSubscription audioSubscription = FindSubscriptionForEvent(AudioSourceStandardEvent.OnCollisionExit);
			if (audioSubscription != null && (!audioSubscription.filterLayers || (audioSubscription.layerMask & (1 << collision.gameObject.layer)) == 0) && (!audioSubscription.filterTags || audioSubscription.tags.Contains(collision.gameObject.tag)) && (!audioSubscription.filterNames || audioSubscription.names.Contains(collision.gameObject.name)))
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnCollisionExit);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (OnTriggerEnterActivated)
		{
			AudioSubscription audioSubscription = FindSubscriptionForEvent(AudioSourceStandardEvent.OnTriggerEnter);
			if (audioSubscription != null && (!audioSubscription.filterLayers || (audioSubscription.layerMask & (1 << other.gameObject.layer)) == 0) && (!audioSubscription.filterTags || audioSubscription.tags.Contains(other.gameObject.tag)) && (!audioSubscription.filterNames || audioSubscription.names.Contains(other.gameObject.name)))
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnTriggerEnter);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (OnTriggerExitActivated)
		{
			AudioSubscription audioSubscription = FindSubscriptionForEvent(AudioSourceStandardEvent.OnTriggerExit);
			if (audioSubscription != null && (!audioSubscription.filterLayers || (audioSubscription.layerMask & (1 << other.gameObject.layer)) == 0) && (!audioSubscription.filterTags || audioSubscription.tags.Contains(other.gameObject.tag)) && (!audioSubscription.filterNames || audioSubscription.names.Contains(other.gameObject.name)))
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnTriggerExit);
			}
		}
	}

	private void OnParticleCollision(GameObject other)
	{
		if (OnParticleCollisionActivated)
		{
			AudioSubscription audioSubscription = FindSubscriptionForEvent(AudioSourceStandardEvent.OnParticleCollision);
			if (audioSubscription != null && (!audioSubscription.filterLayers || (audioSubscription.layerMask & (1 << other.layer)) == 0) && (!audioSubscription.filterTags || audioSubscription.tags.Contains(other.tag)) && (!audioSubscription.filterNames || audioSubscription.names.Contains(other.name)))
			{
				PlaySoundInternal(AudioSourceStandardEvent.OnParticleCollision);
			}
		}
	}

	private void OnMouseEnter()
	{
		if (OnMouseEnterActivated)
		{
			PlaySoundInternal(AudioSourceStandardEvent.OnMouseEnter);
		}
	}

	private void OnMouseDown()
	{
		if (OnMouseClickActivated)
		{
			PlaySoundInternal(AudioSourceStandardEvent.OnMouseClick);
		}
	}

	public void GetOutputData(float[] samples, int channel)
	{
		audioSource.GetOutputData(samples, channel);
	}

	public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
	{
		audioSource.GetSpectrumData(samples, channel, window);
	}

	public void Pause()
	{
		audioSource.Pause();
	}

	public void Play(ulong delay = 0uL)
	{
		switch (clipType)
		{
		case ClipType.ClipFromSoundManager:
		case ClipType.ClipFromGroup:
			SoundManager.PlaySFX(audioSource, clip, loop, Convert.ToSingle(delay), volume, pitch);
			break;
		default:
			audioSource.Play(delay);
			break;
		}
	}

	public void PlayDelayed(float delay)
	{
		switch (clipType)
		{
		case ClipType.ClipFromSoundManager:
			StartCoroutine(PlayDelayedHelper(delay, clipName));
			break;
		case ClipType.ClipFromGroup:
			StartCoroutine(PlayDelayedHelper(delay, groupName));
			break;
		default:
			audioSource.PlayDelayed(delay);
			break;
		}
	}

	private IEnumerator PlayDelayedHelper(float delay, string nameValue)
	{
		yield return new WaitForSeconds(delay);
		switch (clipType)
		{
		case ClipType.ClipFromSoundManager:
			SoundManager.PlaySFX(audioSource, SoundManager.Load(nameValue), loop, 0f, volume, pitch);
			break;
		case ClipType.ClipFromGroup:
			SoundManager.PlaySFX(audioSource, SoundManager.LoadFromGroup(nameValue), loop, 0f, volume, pitch);
			break;
		}
	}

	public void PlayOneShot(AudioClip clip, float volumeScale)
	{
		audioSource.PlayOneShot(clip, volumeScale);
	}

	public void PlayScheduled(double time)
	{
		switch (clipType)
		{
		case ClipType.ClipFromSoundManager:
		case ClipType.ClipFromGroup:
			return;
		}
		audioSource.PlayScheduled(time);
	}

	public void SetScheduledEndTime(double time)
	{
		switch (clipType)
		{
		case ClipType.ClipFromSoundManager:
		case ClipType.ClipFromGroup:
			return;
		}
		audioSource.SetScheduledEndTime(time);
	}

	public void SetScheduledStartTime(double time)
	{
		switch (clipType)
		{
		case ClipType.ClipFromSoundManager:
		case ClipType.ClipFromGroup:
			return;
		}
		audioSource.SetScheduledStartTime(time);
	}

	public void Stop()
	{
		switch (clipType)
		{
		case ClipType.ClipFromSoundManager:
		case ClipType.ClipFromGroup:
			SoundManager.StopSFXObject(audioSource);
			break;
		default:
			audioSource.Stop();
			break;
		}
	}

	public void UnPause()
	{
		audioSource.UnPause();
	}

	public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
	{
		SoundManager.PlaySFX(clip, looping: false, 0f, volume, SoundManager.GetPitchSFX(), position);
	}
}
