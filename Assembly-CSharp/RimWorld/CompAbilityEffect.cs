using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013D3 RID: 5075
	public abstract class CompAbilityEffect : AbilityComp
	{
		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x06007E48 RID: 32328 RVA: 0x002D25FA File Offset: 0x002D07FA
		public CompProperties_AbilityEffect Props
		{
			get
			{
				return (CompProperties_AbilityEffect)this.props;
			}
		}

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x06007E49 RID: 32329 RVA: 0x002D2607 File Offset: 0x002D0807
		public virtual bool HideTargetPawnTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x06007E4A RID: 32330 RVA: 0x002D260A File Offset: 0x002D080A
		public virtual bool ShouldHideGizmo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x06007E4B RID: 32331 RVA: 0x002D260D File Offset: 0x002D080D
		protected bool SendLetter
		{
			get
			{
				return this.Props.sendLetter && !this.Props.customLetterText.NullOrEmpty() && !this.Props.customLetterLabel.NullOrEmpty();
			}
		}

		// Token: 0x06007E4C RID: 32332 RVA: 0x002D2648 File Offset: 0x002D0848
		public virtual void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.Props.screenShakeIntensity > 1E-45f)
			{
				Find.CameraDriver.shaker.DoShake(this.Props.screenShakeIntensity);
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			if (pawn2 != null && !pawn2.IsSlaveOfColony)
			{
				Faction homeFaction = pawn2.HomeFaction;
				if (this.Props.goodwillImpact != 0 && pawn.Faction == Faction.OfPlayer && homeFaction != null && !homeFaction.HostileTo(pawn.Faction) && (this.Props.applyGoodwillImpactToLodgers || !pawn2.IsQuestLodger()) && !pawn2.IsQuestHelper())
				{
					Faction.OfPlayer.TryAffectGoodwillWith(homeFaction, this.Props.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			if (this.Props.clamorType != null)
			{
				GenClamor.DoClamor(this.parent.pawn, target.Cell, (float)this.Props.clamorRadius, this.Props.clamorType);
			}
			Gender gender = pawn.gender;
			SoundDef soundDef;
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					soundDef = this.Props.sound;
				}
				else
				{
					soundDef = (this.Props.soundFemale ?? this.Props.sound);
				}
			}
			else
			{
				soundDef = (this.Props.soundMale ?? this.Props.sound);
			}
			if (soundDef != null)
			{
				soundDef.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
			if (!this.Props.message.NullOrEmpty())
			{
				Messages.Message(this.Props.message, this.parent.pawn, this.Props.messageType ?? MessageTypeDefOf.SilentInput, true);
			}
		}

		// Token: 0x06007E4D RID: 32333 RVA: 0x002D281E File Offset: 0x002D0A1E
		public virtual void Apply(GlobalTargetInfo target)
		{
		}

		// Token: 0x06007E4E RID: 32334 RVA: 0x002D2820 File Offset: 0x002D0A20
		public virtual bool AICanTargetNow(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x06007E4F RID: 32335 RVA: 0x002D2824 File Offset: 0x002D0A24
		public virtual bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Props.availableWhenTargetIsWounded || (target.Pawn.health.hediffSet.BleedRateTotal <= 0f && !target.Pawn.health.HasHediffsNeedingTend(false));
		}

		// Token: 0x06007E50 RID: 32336 RVA: 0x002D2872 File Offset: 0x002D0A72
		public virtual bool CanApplyOn(GlobalTargetInfo target)
		{
			return true;
		}

		// Token: 0x06007E51 RID: 32337 RVA: 0x002D2875 File Offset: 0x002D0A75
		public virtual void PostApplied(List<LocalTargetInfo> targets, Map map)
		{
		}

		// Token: 0x06007E52 RID: 32338 RVA: 0x002D2877 File Offset: 0x002D0A77
		public virtual IEnumerable<PreCastAction> GetPreCastActions()
		{
			return Enumerable.Empty<PreCastAction>();
		}

		// Token: 0x06007E53 RID: 32339 RVA: 0x002D287E File Offset: 0x002D0A7E
		public virtual IEnumerable<Mote> CustomWarmupMotes(LocalTargetInfo target)
		{
			return Enumerable.Empty<Mote>();
		}

		// Token: 0x06007E54 RID: 32340 RVA: 0x002D2885 File Offset: 0x002D0A85
		public virtual void DrawEffectPreview(LocalTargetInfo target)
		{
		}

		// Token: 0x06007E55 RID: 32341 RVA: 0x002D2888 File Offset: 0x002D0A88
		public virtual bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!this.Props.canTargetBaby && !AbilityUtility.ValidateMustNotBeBaby(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (!this.Props.canTargetBosses && pawn.kindDef.isBoss)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007E56 RID: 32342 RVA: 0x002D28DB File Offset: 0x002D0ADB
		public virtual bool Valid(GlobalTargetInfo target, bool throwMessages = false)
		{
			return true;
		}

		// Token: 0x06007E57 RID: 32343 RVA: 0x002D28DE File Offset: 0x002D0ADE
		public virtual string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			return null;
		}

		// Token: 0x06007E58 RID: 32344 RVA: 0x002D28E1 File Offset: 0x002D0AE1
		public virtual string WorldMapExtraLabel(GlobalTargetInfo target)
		{
			return null;
		}

		// Token: 0x06007E59 RID: 32345 RVA: 0x002D28E4 File Offset: 0x002D0AE4
		public virtual Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			return null;
		}

		// Token: 0x06007E5A RID: 32346 RVA: 0x002D28E7 File Offset: 0x002D0AE7
		public virtual Window ConfirmationDialog(GlobalTargetInfo target, Action confirmAction)
		{
			return null;
		}

		// Token: 0x06007E5B RID: 32347 RVA: 0x002D28EA File Offset: 0x002D0AEA
		public virtual string ExtraTooltipPart()
		{
			return null;
		}

		// Token: 0x06007E5C RID: 32348 RVA: 0x002D28ED File Offset: 0x002D0AED
		public virtual void OnGizmoUpdate()
		{
		}
	}
}
