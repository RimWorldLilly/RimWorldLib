using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007E6 RID: 2022
	public class SubSoundDef : Editable
	{
		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06003FFF RID: 16383 RVA: 0x001983E0 File Offset: 0x001965E0
		public FloatRange Duration
		{
			get
			{
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				foreach (ResolvedGrain resolvedGrain in this.resolvedGrains)
				{
					num = Mathf.Min(num, resolvedGrain.duration);
					num2 = Mathf.Max(num2, resolvedGrain.duration);
				}
				if (num == float.PositiveInfinity || num2 == float.NegativeInfinity)
				{
					return new FloatRange(0f, 0f);
				}
				return new FloatRange(num / Mathf.Abs(this.pitchRange.min), num2 / Mathf.Abs(this.pitchRange.max));
			}
		}

		// Token: 0x06004000 RID: 16384 RVA: 0x0019849C File Offset: 0x0019669C
		public virtual void TryPlay(SoundInfo info)
		{
			if (this.resolvedGrains.Count == 0)
			{
				string[] array = new string[5];
				array[0] = "Cannot play ";
				int num = 1;
				SoundDef soundDef = this.parentDef;
				array[num] = ((soundDef != null) ? soundDef.ToString() : null);
				array[2] = " (subSound ";
				array[3] = ((this != null) ? this.ToString() : null);
				array[4] = "_: No resolved grains.";
				Log.Error(string.Concat(array));
				return;
			}
			if (!Find.SoundRoot.oneShotManager.CanAddPlayingOneShot(this.parentDef, info))
			{
				return;
			}
			if (Current.Game != null && !this.gameSpeedRange.Includes((int)Find.TickManager.CurTimeSpeed))
			{
				return;
			}
			ResolvedGrain resolvedGrain = this.RandomizedResolvedGrain();
			ResolvedGrain_Clip resolvedGrain_Clip = resolvedGrain as ResolvedGrain_Clip;
			if (resolvedGrain_Clip != null)
			{
				if (SampleOneShot.TryMakeAndPlay(this, resolvedGrain_Clip.clip, info) == null)
				{
					return;
				}
				SoundSlotManager.Notify_Played(this.parentDef.slot, resolvedGrain_Clip.clip.length);
			}
			this.Notify_GrainPlayed(resolvedGrain);
		}

		// Token: 0x06004001 RID: 16385 RVA: 0x00198580 File Offset: 0x00196780
		public void Notify_GrainPlayed(ResolvedGrain chosenGrain)
		{
			if (this.distinctResolvedGrainsCount > 1)
			{
				if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
				{
					while (this.recentlyPlayedResolvedGrains.Count >= this.numToAvoid)
					{
						this.recentlyPlayedResolvedGrains.Dequeue();
					}
					if (this.recentlyPlayedResolvedGrains.Count < this.numToAvoid)
					{
						this.recentlyPlayedResolvedGrains.Enqueue(chosenGrain);
						return;
					}
				}
				else if (this.repeatMode == RepeatSelectMode.NeverTwice)
				{
					this.lastPlayedResolvedGrain = chosenGrain;
				}
			}
		}

		// Token: 0x06004002 RID: 16386 RVA: 0x001985F0 File Offset: 0x001967F0
		public bool IsSameOrHasSameTag(SubSoundDef other)
		{
			return this == other || (!this.tag.NullOrEmpty() && !other.tag.NullOrEmpty() && this.tag == other.tag);
		}

		// Token: 0x06004003 RID: 16387 RVA: 0x00198628 File Offset: 0x00196828
		public ResolvedGrain RandomizedResolvedGrain()
		{
			ResolvedGrain chosenGrain = null;
			for (;;)
			{
				chosenGrain = this.resolvedGrains.RandomElement<ResolvedGrain>();
				if (this.distinctResolvedGrainsCount <= 1)
				{
					break;
				}
				if (this.repeatMode == RepeatSelectMode.NeverLastHalf)
				{
					if (!this.recentlyPlayedResolvedGrains.Any((ResolvedGrain g) => g.Equals(chosenGrain)))
					{
						break;
					}
				}
				else if (this.repeatMode != RepeatSelectMode.NeverTwice || !chosenGrain.Equals(this.lastPlayedResolvedGrain))
				{
					break;
				}
			}
			return chosenGrain;
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x001986A2 File Offset: 0x001968A2
		public float RandomizedVolume()
		{
			return this.volumeRange.RandomInRange / 100f;
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x001986B5 File Offset: 0x001968B5
		public override void ResolveReferences()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.resolvedGrains.Clear();
				foreach (AudioGrain audioGrain in this.grains)
				{
					foreach (ResolvedGrain item in audioGrain.GetResolvedGrains())
					{
						this.resolvedGrains.Add(item);
					}
				}
				this.distinctResolvedGrainsCount = this.resolvedGrains.Distinct<ResolvedGrain>().Count<ResolvedGrain>();
				this.numToAvoid = Mathf.FloorToInt((float)this.distinctResolvedGrainsCount / 2f);
				if (this.distinctResolvedGrainsCount >= 6)
				{
					this.numToAvoid++;
				}
				if (this.resolvedGrains.Count == 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
					defaultInterpolatedStringHandler.AppendFormatted(this.parentDef.defName);
					defaultInterpolatedStringHandler.AppendLiteral(" SubSound ");
					defaultInterpolatedStringHandler.AppendFormatted<SubSoundDef>(this);
					defaultInterpolatedStringHandler.AppendLiteral(" has no resolvedGrains.");
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			});
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x001986C8 File Offset: 0x001968C8
		public override IEnumerable<string> ConfigErrors()
		{
			SubSoundDef.<ConfigErrors>d__38 <ConfigErrors>d__ = new SubSoundDef.<ConfigErrors>d__38(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06004007 RID: 16391 RVA: 0x001986D8 File Offset: 0x001968D8
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04002A7A RID: 10874
		public const string DefaultName = "UnnamedSubSoundDef";

		// Token: 0x04002A7B RID: 10875
		[Description("A name to help you identify the sound.")]
		[DefaultValue("UnnamedSubSoundDef")]
		[MayTranslate]
		public string name = "UnnamedSubSoundDef";

		// Token: 0x04002A7C RID: 10876
		[Description("Whether this sound plays on the camera or in the world.\n\nThis must match what the game expects from the sound Def with this name.")]
		[DefaultValue(false)]
		public bool onCamera;

		// Token: 0x04002A7D RID: 10877
		[Description("Whether to mute this subSound while the game is paused (either by the pausing in play or by opening a menu)")]
		[DefaultValue(false)]
		public bool muteWhenPaused;

		// Token: 0x04002A7E RID: 10878
		[Description("Whether this subSound's tempo should be affected by the current tick rate.")]
		[DefaultValue(false)]
		public bool tempoAffectedByGameSpeed;

		// Token: 0x04002A7F RID: 10879
		[Description("The sound grains used for this sample. The game will choose one of these randomly when the sound plays. Sustainers choose one for each sample as it begins.")]
		public List<AudioGrain> grains = new List<AudioGrain>();

		// Token: 0x04002A80 RID: 10880
		[EditSliderRange(0f, 100f)]
		[Description("This sound will play at a random volume inside this range.\n\nSustainers will choose a different random volume for each sample.")]
		[DefaultFloatRange(50f, 50f)]
		public FloatRange volumeRange = new FloatRange(50f, 50f);

		// Token: 0x04002A81 RID: 10881
		[EditSliderRange(0.05f, 2f)]
		[Description("This sound will play at a random pitch inside this range.\n\nSustainers will choose a different random pitch for each sample.")]
		[DefaultFloatRange(1f, 1f)]
		public FloatRange pitchRange = FloatRange.One;

		// Token: 0x04002A82 RID: 10882
		[EditSliderRange(0f, 200f)]
		[Description("This sound will play max volume when it is under minDistance from the camera.\n\nIt will fade out linearly until the camera distance reaches its max.")]
		[DefaultFloatRange(25f, 70f)]
		public FloatRange distRange = new FloatRange(25f, 70f);

		// Token: 0x04002A83 RID: 10883
		[Description("When the sound chooses the next grain, you may use this setting to have it avoid repeating the last grain, or avoid repeating any of the grains in the last X played, X being half the total number of grains defined.")]
		[DefaultValue(RepeatSelectMode.NeverLastHalf)]
		public RepeatSelectMode repeatMode = RepeatSelectMode.NeverLastHalf;

		// Token: 0x04002A84 RID: 10884
		[Description("Mappings between game parameters (like fire size or wind speed) and properties of the sound.")]
		[DefaultEmptyList(typeof(SoundParameterMapping))]
		public List<SoundParameterMapping> paramMappings = new List<SoundParameterMapping>();

		// Token: 0x04002A85 RID: 10885
		[Description("The filters to be applied to this sound.")]
		[DefaultEmptyList(typeof(SoundFilter))]
		public List<SoundFilter> filters = new List<SoundFilter>();

		// Token: 0x04002A86 RID: 10886
		[Description("A range of possible times between when this sound is triggered and when it will actually start playing.")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange startDelayRange = FloatRange.Zero;

		// Token: 0x04002A87 RID: 10887
		[Description("A range of game speeds this sound can be played on.")]
		public IntRange gameSpeedRange = new IntRange(0, 999);

		// Token: 0x04002A88 RID: 10888
		[Description("One shots sharing the same tag are treated as the same sound when determining importance.")]
		[NoTranslate]
		public string tag;

		// Token: 0x04002A89 RID: 10889
		[Description("If true, each sample in the sustainer will be looped and ended only after sustainerLoopDurationRange. If not, the sounds will just play once and end after their own length.")]
		[DefaultValue(true)]
		public bool sustainLoop = true;

		// Token: 0x04002A8A RID: 10890
		[EditSliderRange(0f, 10f)]
		[Description("The range of durations that individual looped samples in the sustainer will have. Each sample ends after a time randomly chosen in this range.\n\nOnly used if the sustainer is looped.")]
		[DefaultFloatRange(9999f, 9999f)]
		public FloatRange sustainLoopDurationRange = new FloatRange(9999f, 9999f);

		// Token: 0x04002A8B RID: 10891
		[EditSliderRange(-2f, 2f)]
		[Description("The time between when one sample ends and the next starts.\n\nSet to negative if you wish samples to overlap.")]
		[LoadAlias("sustainInterval")]
		[DefaultFloatRange(0f, 0f)]
		public FloatRange sustainIntervalRange = FloatRange.Zero;

		// Token: 0x04002A8C RID: 10892
		[Description("The time between when one sample ends and the next starts.\n\nSet to negative if you wish samples to overlap.")]
		[LoadAlias("sustainIntervalFactorByAggregateSize")]
		public SimpleCurve sustainIntervalFactorByAggregateSize;

		// Token: 0x04002A8D RID: 10893
		[EditSliderRange(0f, 2f)]
		[Description("The fade-in time of each sample. The sample will start at 0 volume and fade in over this number of seconds.")]
		[DefaultValue(0f)]
		public float sustainAttack;

		// Token: 0x04002A8E RID: 10894
		[Description("Skip the attack on the first sustainer sample.")]
		[DefaultValue(true)]
		public bool sustainSkipFirstAttack = true;

		// Token: 0x04002A8F RID: 10895
		[EditSliderRange(0f, 2f)]
		[Description("The fade-out time of each sample. At this number of seconds before the sample ends, it will start fading out. Its volume will be zero at the moment it finishes fading out.")]
		[DefaultValue(0f)]
		public float sustainRelease;

		// Token: 0x04002A90 RID: 10896
		[Description("Whether to start the sample at a random point in the sound.")]
		[DefaultValue(false)]
		public bool randomStartPoint;

		// Token: 0x04002A91 RID: 10897
		[DefaultValue(true)]
		public bool canVacuumDampen = true;

		// Token: 0x04002A92 RID: 10898
		[Unsaved(false)]
		public SoundDef parentDef;

		// Token: 0x04002A93 RID: 10899
		[Unsaved(false)]
		private List<ResolvedGrain> resolvedGrains = new List<ResolvedGrain>();

		// Token: 0x04002A94 RID: 10900
		[Unsaved(false)]
		private ResolvedGrain lastPlayedResolvedGrain;

		// Token: 0x04002A95 RID: 10901
		[Unsaved(false)]
		private int numToAvoid;

		// Token: 0x04002A96 RID: 10902
		[Unsaved(false)]
		private int distinctResolvedGrainsCount;

		// Token: 0x04002A97 RID: 10903
		[Unsaved(false)]
		private Queue<ResolvedGrain> recentlyPlayedResolvedGrains = new Queue<ResolvedGrain>();
	}
}
