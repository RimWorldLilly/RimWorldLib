using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001475 RID: 5237
	public class Psycast : Ability
	{
		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x06008078 RID: 32888 RVA: 0x002DD018 File Offset: 0x002DB218
		public override AcceptanceReport CanCast
		{
			get
			{
				AcceptanceReport canCast = base.CanCast;
				if (!canCast)
				{
					return canCast;
				}
				if (this.def.EntropyGain > 1E-45f)
				{
					if (this.pawn.GetPsylinkLevel() < this.def.level && this.def.level > 0)
					{
						return false;
					}
					return !this.pawn.psychicEntropy.WouldOverflowEntropy(this.def.EntropyGain);
				}
				else
				{
					if (this.def.PsyfocusCost > this.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
					{
						return false;
					}
					return true;
				}
			}
		}

		// Token: 0x06008079 RID: 32889 RVA: 0x002DD0C9 File Offset: 0x002DB2C9
		public Psycast(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600807A RID: 32890 RVA: 0x002DD0D2 File Offset: 0x002DB2D2
		public Psycast(Pawn pawn, AbilityDef def) : base(pawn, def)
		{
		}

		// Token: 0x0600807B RID: 32891 RVA: 0x002DD0DC File Offset: 0x002DB2DC
		public override IEnumerable<Command> GetGizmos()
		{
			Psycast.<GetGizmos>d__8 <GetGizmos>d__ = new Psycast.<GetGizmos>d__8(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600807C RID: 32892 RVA: 0x002DD0EC File Offset: 0x002DB2EC
		public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckRoyalty("Psycast"))
			{
				return false;
			}
			if (this.def.EntropyGain > 1E-45f && !this.pawn.psychicEntropy.TryAddEntropy(this.def.EntropyGain, null, true, false))
			{
				return false;
			}
			float num = base.FinalPsyfocusCost(target);
			if (num > 1E-45f)
			{
				this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-num);
			}
			if (this.def.showPsycastEffects)
			{
				if (base.EffectComps.Any((CompAbilityEffect c) => c.Props.psychic))
				{
					if (this.def.HasAreaOfEffect)
					{
						FleckMaker.Static(target.Cell, this.pawn.Map, FleckDefOf.PsycastAreaEffect, this.def.EffectRadius);
						SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
					}
					else
					{
						SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
					}
				}
				else if (this.def.HasAreaOfEffect && this.def.canUseAoeToGetTargets)
				{
					SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
				}
			}
			return base.Activate(target, dest);
		}

		// Token: 0x0600807D RID: 32893 RVA: 0x002DD26C File Offset: 0x002DB46C
		public override bool Activate(GlobalTargetInfo target)
		{
			if (this.def.EntropyGain > 1E-45f && !this.pawn.psychicEntropy.TryAddEntropy(this.def.EntropyGain, null, true, false))
			{
				return false;
			}
			float psyfocusCost = this.def.PsyfocusCost;
			if (psyfocusCost > 1E-45f)
			{
				this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-psyfocusCost);
			}
			return base.Activate(target);
		}

		// Token: 0x0600807E RID: 32894 RVA: 0x002DD2DC File Offset: 0x002DB4DC
		protected override void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.CanApplyPsycastTo(target))
			{
				using (IEnumerator<CompAbilityEffect> enumerator = effects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompAbilityEffect compAbilityEffect = enumerator.Current;
						compAbilityEffect.Apply(target, dest);
					}
					return;
				}
			}
			MoteMaker.ThrowText(target.CenterVector3, this.pawn.Map, "TextMote_Immune".Translate(), -1f);
		}

		// Token: 0x0600807F RID: 32895 RVA: 0x002DD358 File Offset: 0x002DB558
		public bool CanApplyPsycastTo(LocalTargetInfo target)
		{
			if (!base.EffectComps.Any((CompAbilityEffect e) => e.Props.psychic))
			{
				return true;
			}
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) < 1E-45f)
				{
					return false;
				}
				if (pawn.Faction != null && pawn.Faction == Faction.OfMechanoids)
				{
					if (base.EffectComps.Any((CompAbilityEffect e) => !e.Props.applicableToMechs))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008080 RID: 32896 RVA: 0x002DD3F8 File Offset: 0x002DB5F8
		public override bool GizmoDisabled(out string reason)
		{
			if (this.pawn.psychicEntropy.PsychicSensitivity < 1E-45f)
			{
				reason = "CommandPsycastZeroPsychicSensitivity".Translate();
				return true;
			}
			float num = PsycastUtility.TotalPsyfocusCostOfQueuedPsycasts(this.pawn);
			if (this.def.level > 0 && this.pawn.GetPsylinkLevel() < this.def.level)
			{
				reason = "CommandPsycastHigherLevelPsylinkRequired".Translate(this.def.level);
				return true;
			}
			if (this.def.PsyfocusCost + num > this.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
			{
				reason = "CommandPsycastNotEnoughPsyfocus".Translate(this.def.PsyfocusCostPercent, (this.pawn.psychicEntropy.CurrentPsyfocus - num).ToStringPercent("0.#"), this.def.label.Named("PSYCASTNAME"), this.pawn.Named("CASTERNAME"));
				return true;
			}
			if (this.def.level > this.pawn.psychicEntropy.MaxAbilityLevel)
			{
				reason = "CommandPsycastLowPsyfocus".Translate(Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[this.def.RequiredPsyfocusBand].ToStringPercent());
				return true;
			}
			if (this.def.EntropyGain > 1E-45f && this.pawn.psychicEntropy.WouldOverflowEntropy(this.def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(this.pawn)))
			{
				reason = "CommandPsycastWouldExceedEntropy".Translate(this.def.label);
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		// Token: 0x06008081 RID: 32897 RVA: 0x002DD5C8 File Offset: 0x002DB7C8
		public override void AbilityTick()
		{
			base.AbilityTick();
			if (this.pawn.Spawned && base.Casting)
			{
				if (this.moteCast == null || this.moteCast.Destroyed)
				{
					this.moteCast = MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_CastPsycast, Psycast.MoteCastOffset, Psycast.MoteCastScale, base.verb.verbProps.warmupTime - Psycast.MoteCastFadeTime);
					return;
				}
				this.moteCast.Maintain();
			}
		}

		// Token: 0x04005028 RID: 20520
		private Mote moteCast;

		// Token: 0x04005029 RID: 20521
		private static float MoteCastFadeTime = 0.4f;

		// Token: 0x0400502A RID: 20522
		private static float MoteCastScale = 1f;

		// Token: 0x0400502B RID: 20523
		private static Vector3 MoteCastOffset = new Vector3(0f, 0f, 0.48f);
	}
}
