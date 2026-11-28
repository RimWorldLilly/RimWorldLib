using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020001BE RID: 446
	public class SoundDef : Def
	{
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00040BEC File Offset: 0x0003EDEC
		private bool HasSubSoundsOnCamera
		{
			get
			{
				for (int i = 0; i < this.subSounds.Count; i++)
				{
					if (this.subSounds[i].onCamera)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x00040C28 File Offset: 0x0003EE28
		public bool HasSubSoundsInWorld
		{
			get
			{
				for (int i = 0; i < this.subSounds.Count; i++)
				{
					if (!this.subSounds[i].onCamera)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x00040C61 File Offset: 0x0003EE61
		public int MaxSimultaneousSamples
		{
			get
			{
				return this.maxSimultaneous * this.subSounds.Count;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00040C78 File Offset: 0x0003EE78
		public FloatRange Duration
		{
			get
			{
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				foreach (SubSoundDef subSoundDef in this.subSounds)
				{
					num = Mathf.Min(num, subSoundDef.Duration.min);
					num2 = Mathf.Max(num2, subSoundDef.Duration.max);
				}
				return new FloatRange((num == float.PositiveInfinity) ? 0f : num, (num2 == float.NegativeInfinity) ? 0f : num2);
			}
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00040D1C File Offset: 0x0003EF1C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.subSounds.Count; i++)
			{
				this.subSounds[i].parentDef = this;
				if (this.subSounds[i].name == "UnnamedSubSoundDef")
				{
					this.subSounds[i].name = this.defName + "_" + i.ToString();
				}
				this.subSounds[i].ResolveReferences();
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00040DAD File Offset: 0x0003EFAD
		public override IEnumerable<string> ConfigErrors()
		{
			SoundDef.<ConfigErrors>d__23 <ConfigErrors>d__ = new SoundDef.<ConfigErrors>d__23(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00040DC0 File Offset: 0x0003EFC0
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (this.testSustainer == null)
			{
				if (widgetRow.ButtonIcon(TexButton.Play, null, null, null, null, true, -1f))
				{
					this.ResolveReferences();
					SoundInfo info;
					if (this.HasSubSoundsInWorld)
					{
						IntVec3 mapPosition = Find.CameraDriver.MapPosition;
						info = SoundInfo.InMap(new TargetInfo(mapPosition, Find.CurrentMap, false), MaintenanceType.PerFrame);
						for (int i = 0; i < 5; i++)
						{
							FleckMaker.ThrowDustPuff(mapPosition, Find.CurrentMap, 1.5f);
						}
					}
					else
					{
						info = SoundInfo.OnCamera(MaintenanceType.PerFrame);
					}
					info.testPlay = true;
					if (this.sustain)
					{
						this.testSustainer = this.TrySpawnSustainer(info);
						return;
					}
					this.PlayOneShot(info);
					return;
				}
			}
			else
			{
				this.testSustainer.Maintain();
				if (widgetRow.ButtonIcon(TexButton.Stop, null, null, null, null, true, -1f))
				{
					this.testSustainer.End();
					this.testSustainer = null;
				}
			}
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00040ED0 File Offset: 0x0003F0D0
		public static SoundDef Named(string defName)
		{
			SoundDef namedSilentFail = DefDatabase<SoundDef>.GetNamedSilentFail(defName);
			if (namedSilentFail != null)
			{
				return namedSilentFail;
			}
			if (!Prefs.DevMode)
			{
				object obj = SoundDef.undefinedSoundDefsLock;
				lock (obj)
				{
					if (SoundDef.undefinedSoundDefs.ContainsKey(defName))
					{
						return SoundDef.UndefinedDefNamed(defName);
					}
				}
			}
			List<SoundDef> allDefsListForReading = DefDatabase<SoundDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].eventNames.Count > 0)
				{
					for (int j = 0; j < allDefsListForReading[i].eventNames.Count; j++)
					{
						if (allDefsListForReading[i].eventNames[j] == defName)
						{
							return allDefsListForReading[i];
						}
					}
				}
			}
			if (DefDatabase<SoundDef>.DefCount == 0)
			{
				Log.Warning("Tried to get SoundDef named " + defName + ", but sound defs aren't loaded yet (is it a static variable initialized before play data?).");
			}
			return SoundDef.UndefinedDefNamed(defName);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00040FD4 File Offset: 0x0003F1D4
		private static SoundDef UndefinedDefNamed(string defName)
		{
			object obj = SoundDef.undefinedSoundDefsLock;
			SoundDef soundDef;
			lock (obj)
			{
				if (!SoundDef.undefinedSoundDefs.TryGetValue(defName, out soundDef))
				{
					soundDef = new SoundDef();
					soundDef.isUndefined = true;
					soundDef.defName = defName;
					soundDef.ResolveDefNameHash();
					SoundDef.undefinedSoundDefs.Add(defName, soundDef);
				}
			}
			return soundDef;
		}

		// Token: 0x04000BA0 RID: 2976
		[Description("If checked, this sound is a sustainer.\n\nSustainers are used for sounds with a defined beginning and end (as opposed to OneShots, which just fire at a given instant).\n\nThis value must match what the game expects from the SubSoundDef with this name.")]
		[DefaultValue(false)]
		public bool sustain;

		// Token: 0x04000BA1 RID: 2977
		[Description("When the sound is allowed to play: only when the map view is active, only when the world view is active, or always (map + world + main menu).")]
		[DefaultValue(SoundContext.Any)]
		public SoundContext context;

		// Token: 0x04000BA2 RID: 2978
		[Description("Event names for this sound. \n\nThe code will look up sounds to play them according to their name. If the code finds the event name it wants in this list, it will trigger this sound.\n\nThe Def name is also used as an event name. Obsolete")]
		public List<string> eventNames = new List<string>();

		// Token: 0x04000BA3 RID: 2979
		[Description("For one-shots, this is the number of individual sounds from this Def than can be playing at a time.\n\n For sustainers, this is the number of sustainers that can be running with this sound (each of which can have sub-sounds). Sustainers can fade in and out as you move the camera or objects move, to keep the nearest ones audible.\n\nThis setting may not work for on-camera sounds.")]
		[DefaultValue(4)]
		public int maxVoices = 4;

		// Token: 0x04000BA4 RID: 2980
		[Description("The number of instances of this sound that can play at almost exactly the same moment. Handles cases like six gunners all firing their identical guns at the same time because a target came into view of all of them at the same time. Ordinarily this would make a painfully loud sound, but you can reduce it with this.")]
		[DefaultValue(3)]
		public int maxSimultaneous = 3;

		// Token: 0x04000BA5 RID: 2981
		[Description("If the system has to not play some instances of this sound because of maxVoices, this determines which ones are ignored.\n\nYou should use PrioritizeNewest for things like gunshots, so older still-playing samples are overridden by newer, more important ones.\n\nSustained sounds should usually prioritize nearest, so if a new fire starts burning nearby it can override a more distant one.")]
		[DefaultValue(VoicePriorityMode.PrioritizeNewest)]
		public VoicePriorityMode priorityMode;

		// Token: 0x04000BA6 RID: 2982
		[Description("The special sound slot this sound takes. If a sound with this slot is playing, new sounds in this slot will not play.\n\nOnly works for on-camera sounds.")]
		[DefaultValue("")]
		public string slot = "";

		// Token: 0x04000BA7 RID: 2983
		[LoadAlias("sustainerStartSound")]
		[Description("The name of the SoundDef that will be played when this sustainer starts.")]
		[DefaultValue("")]
		public SoundDef sustainStartSound;

		// Token: 0x04000BA8 RID: 2984
		[LoadAlias("sustainerStopSound")]
		[Description("The name of the SoundDef that will be played when this sustainer ends.")]
		[DefaultValue("")]
		public SoundDef sustainStopSound;

		// Token: 0x04000BA9 RID: 2985
		[Description("After a sustainer is ended, the sound will fade out over this many real-time seconds.")]
		[DefaultValue(0f)]
		public float sustainFadeoutTime;

		// Token: 0x04000BAA RID: 2986
		[LoadAlias("sustainerFadeoutStartSound")]
		[Description("The name of the SoundDef that will be played when this sustainer starts to fade out.")]
		[DefaultValue("")]
		public SoundDef sustainFadeoutStartSound;

		// Token: 0x04000BAB RID: 2987
		[Description("All the sounds that will play when this set is triggered.")]
		public List<SubSoundDef> subSounds = new List<SubSoundDef>();

		// Token: 0x04000BAC RID: 2988
		[Unsaved(false)]
		public bool isUndefined;

		// Token: 0x04000BAD RID: 2989
		[Unsaved(false)]
		public Sustainer testSustainer;

		// Token: 0x04000BAE RID: 2990
		private static Dictionary<string, SoundDef> undefinedSoundDefs = new Dictionary<string, SoundDef>();

		// Token: 0x04000BAF RID: 2991
		private static object undefinedSoundDefsLock = new object();
	}
}
