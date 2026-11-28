using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x0200083C RID: 2108
	public static class ToilEffects
	{
		// Token: 0x06004245 RID: 16965 RVA: 0x001A2A54 File Offset: 0x001A0C54
		public static Toil PlaySoundAtStart(this Toil toil, SoundDef sound)
		{
			toil.AddPreInitAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06004246 RID: 16966 RVA: 0x001A2A94 File Offset: 0x001A0C94
		public static Toil PlaySoundAtEnd(this Toil toil, SoundDef sound)
		{
			toil.AddFinishAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06004247 RID: 16967 RVA: 0x001A2AD4 File Offset: 0x001A0CD4
		public static Toil PlaySustainerOrSound(this Toil toil, SoundDef soundDef, float pitchFactor = 1f)
		{
			return toil.PlaySustainerOrSound(() => soundDef, pitchFactor);
		}

		// Token: 0x06004248 RID: 16968 RVA: 0x001A2B04 File Offset: 0x001A0D04
		public static Toil PlaySustainerOrSound(this Toil toil, Func<SoundDef> soundDefGetter, float pitchFactor = 1f)
		{
			Sustainer sustainer = null;
			toil.AddPreInitAction(delegate
			{
				SoundDef soundDef = soundDefGetter();
				if (soundDef != null && !soundDef.sustain)
				{
					soundDef.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
				}
			});
			toil.AddPreTickAction(delegate
			{
				if (sustainer == null || sustainer.Ended)
				{
					SoundDef soundDef = soundDefGetter();
					if (soundDef != null && soundDef.sustain)
					{
						SoundInfo info = SoundInfo.InMap(toil.actor, MaintenanceType.PerTick);
						info.pitchFactor = pitchFactor;
						sustainer = soundDef.TrySpawnSustainer(info);
						return;
					}
				}
				else
				{
					sustainer.Maintain();
				}
			});
			return toil;
		}

		// Token: 0x06004249 RID: 16969 RVA: 0x001A2B68 File Offset: 0x001A0D68
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, TargetIndex ind, Color? overrideColor = null)
		{
			return toil.WithEffect(() => effectDef, ind, overrideColor);
		}

		// Token: 0x0600424A RID: 16970 RVA: 0x001A2B98 File Offset: 0x001A0D98
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, Func<LocalTargetInfo> effectTargetGetter, Color? overrideColor = null)
		{
			return toil.WithEffect(() => effectDef, effectTargetGetter, overrideColor);
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x001A2BC8 File Offset: 0x001A0DC8
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, TargetIndex ind, Color? overrideColor = null)
		{
			return toil.WithEffect(effecterDefGetter, () => toil.actor.CurJob.GetTarget(ind), overrideColor);
		}

		// Token: 0x0600424C RID: 16972 RVA: 0x001A2C04 File Offset: 0x001A0E04
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Thing thing, Color? overrideColor = null)
		{
			return toil.WithEffect(effecterDefGetter, () => thing, overrideColor);
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x001A2C34 File Offset: 0x001A0E34
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Func<LocalTargetInfo> effectTargetGetter, Color? overrideColor = null)
		{
			Effecter effecter = null;
			toil.AddPreTickAction(delegate
			{
				if (effecter == null)
				{
					EffecterDef effecterDef = effecterDefGetter();
					if (effecterDef == null)
					{
						return;
					}
					toil.actor.rotationTracker.FaceTarget(effectTargetGetter());
					effecter = effecterDef.Spawn();
					effecter.Trigger(toil.actor, effectTargetGetter().ToTargetInfo(toil.actor.Map), -1);
					if (overrideColor == null)
					{
						return;
					}
					using (List<SubEffecter>.Enumerator enumerator = effecter.children.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SubEffecter subEffecter = enumerator.Current;
							SubEffecter_Sprayer subEffecter_Sprayer = subEffecter as SubEffecter_Sprayer;
							if (subEffecter_Sprayer != null)
							{
								subEffecter_Sprayer.colorOverride = overrideColor;
							}
						}
						return;
					}
				}
				effecter.EffectTick(toil.actor, effectTargetGetter().ToTargetInfo(toil.actor.Map));
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x001A2CA0 File Offset: 0x001A0EA0
		public static Toil WithProgressBar(this Toil toil, TargetIndex ind, Func<float> progressGetter, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f, bool alwaysShow = false)
		{
			Effecter effecter = null;
			toil.AddPreTickIntervalAction(delegate(int delta)
			{
				if (toil.actor.Faction != Faction.OfPlayer)
				{
					return;
				}
				if (effecter == null)
				{
					EffecterDef progressBar = EffecterDefOf.ProgressBar;
					effecter = progressBar.Spawn();
					return;
				}
				LocalTargetInfo localTargetInfo = (ind == TargetIndex.None) ? LocalTargetInfo.Invalid : toil.actor.CurJob.GetTarget(ind);
				if (!localTargetInfo.IsValid || (localTargetInfo.HasThing && !localTargetInfo.Thing.Spawned))
				{
					effecter.EffectTick(toil.actor, TargetInfo.Invalid);
				}
				else if (interpolateBetweenActorAndTarget)
				{
					effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), toil.actor);
				}
				else
				{
					effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), TargetInfo.Invalid);
				}
				MoteProgressBar mote = ((SubEffecter_ProgressBar)effecter.children[0]).mote;
				if (mote != null)
				{
					mote.progress = Mathf.Clamp01(progressGetter());
					mote.offsetZ = offsetZ;
					mote.alwaysShow = alwaysShow;
				}
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x001A2D1C File Offset: 0x001A0F1C
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ, false);
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x001A2D54 File Offset: 0x001A0F54
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, int toilDuration, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toilDuration, interpolateBetweenActorAndTarget, offsetZ, false);
		}
	}
}
