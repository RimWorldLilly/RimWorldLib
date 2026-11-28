using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001477 RID: 5239
	public class Verb_CastAbility : Verb, IAbilityVerb
	{
		// Token: 0x17001609 RID: 5641
		// (get) Token: 0x06008086 RID: 32902 RVA: 0x002DD7E5 File Offset: 0x002DB9E5
		// (set) Token: 0x06008087 RID: 32903 RVA: 0x002DD7ED File Offset: 0x002DB9ED
		public Ability Ability
		{
			get
			{
				return this.ability;
			}
			set
			{
				this.ability = value;
			}
		}

		// Token: 0x1700160A RID: 5642
		// (get) Token: 0x06008088 RID: 32904 RVA: 0x002DD7F6 File Offset: 0x002DB9F6
		public static Color RadiusHighlightColor
		{
			get
			{
				return new Color(0.3f, 0.8f, 1f);
			}
		}

		// Token: 0x1700160B RID: 5643
		// (get) Token: 0x06008089 RID: 32905 RVA: 0x002DD80C File Offset: 0x002DBA0C
		public override string ReportLabel
		{
			get
			{
				return this.ability.def.label;
			}
		}

		// Token: 0x1700160C RID: 5644
		// (get) Token: 0x0600808A RID: 32906 RVA: 0x002DD81E File Offset: 0x002DBA1E
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x0600808B RID: 32907 RVA: 0x002DD824 File Offset: 0x002DBA24
		public override bool HidePawnTooltips
		{
			get
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.ability.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HideTargetPawnTooltip)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x0600808C RID: 32908 RVA: 0x002DD884 File Offset: 0x002DBA84
		public override ITargetingSource DestinationSelector
		{
			get
			{
				CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.ability.CompOfType<CompAbilityEffect_WithDest>();
				if (compAbilityEffect_WithDest != null && compAbilityEffect_WithDest.Props.destination == AbilityEffectDestination.Selected)
				{
					return compAbilityEffect_WithDest;
				}
				return null;
			}
		}

		// Token: 0x0600808D RID: 32909 RVA: 0x002DD8B1 File Offset: 0x002DBAB1
		protected override bool TryCastShot()
		{
			return this.ability.Activate(this.currentTarget, this.currentDestination);
		}

		// Token: 0x0600808E RID: 32910 RVA: 0x002DD8CC File Offset: 0x002DBACC
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.ability.CompOfType<CompAbilityEffect_WithDest>();
			if (compAbilityEffect_WithDest != null && compAbilityEffect_WithDest.Props.destination == AbilityEffectDestination.Selected)
			{
				compAbilityEffect_WithDest.SetTarget(target);
				return;
			}
			this.ability.QueueCastingJob(target, null);
		}

		// Token: 0x0600808F RID: 32911 RVA: 0x002DD910 File Offset: 0x002DBB10
		public virtual bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			return true;
		}

		// Token: 0x06008090 RID: 32912 RVA: 0x002DD914 File Offset: 0x002DBB14
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (this.EffectiveRange > 0f)
			{
				if (!this.CanHitTarget(target))
				{
					if (target.IsValid && showMessages)
					{
						string t = "CannotUseAbility".Translate(this.ability.def.label) + ": ";
						if (this.ability.verb.OutOfRange(this.ability.pawn.Position, target, target.HasThing ? target.Thing.OccupiedRect() : CellRect.SingleCell(target.Cell)))
						{
							Messages.Message(t + "AbilityOutOfRange".Translate(), new LookTargets(new TargetInfo[]
							{
								this.ability.pawn,
								target.ToTargetInfo(this.ability.pawn.Map)
							}), MessageTypeDefOf.RejectInput, false);
						}
						else if (this.ability.pawn.Spawned)
						{
							Messages.Message(t + "AbilityCannotHitTarget".Translate(), new LookTargets(new TargetInfo[]
							{
								this.ability.pawn,
								target.ToTargetInfo(this.ability.pawn.Map)
							}), MessageTypeDefOf.RejectInput, false);
						}
					}
					return false;
				}
			}
			else if (!this.ability.pawn.CanReach(target, PathEndMode.Touch, this.ability.pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				if (target.IsValid && showMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.ability.def.label) + ": " + "AbilityCannotReachTarget".Translate(), new LookTargets(new TargetInfo[]
					{
						this.ability.pawn,
						target.ToTargetInfo(this.ability.pawn.Map)
					}), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.IsApplicableTo(target, showMessages))
			{
				return false;
			}
			for (int i = 0; i < this.ability.EffectComps.Count; i++)
			{
				if (!this.ability.EffectComps[i].Valid(target, showMessages))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008091 RID: 32913 RVA: 0x002DDB9C File Offset: 0x002DBD9C
		public override bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.EffectiveRange <= 0f || base.CanHitTarget(targ);
		}

		// Token: 0x06008092 RID: 32914 RVA: 0x002DDBB4 File Offset: 0x002DBDB4
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false) && this.ValidateTarget(target, false))
			{
				base.OnGUI(target);
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
			this.DrawAttachmentExtraLabel(target);
		}

		// Token: 0x06008093 RID: 32915 RVA: 0x002DDBF0 File Offset: 0x002DBDF0
		protected void DrawAttachmentExtraLabel(LocalTargetInfo target)
		{
			foreach (CompAbilityEffect compAbilityEffect in this.ability.EffectComps)
			{
				string text = compAbilityEffect.ExtraLabelMouseAttachment(target);
				if (!text.NullOrEmpty())
				{
					Widgets.MouseAttachedLabel(text, 0f, 0f, null);
					break;
				}
			}
		}

		// Token: 0x06008094 RID: 32916 RVA: 0x002DDC6C File Offset: 0x002DBE6C
		public override bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			bool flag = base.TryStartCastOn(castTarg, destTarg, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire, nonInterruptingSelfCast);
			if (flag && this.ability.def.stunTargetWhileCasting && this.ability.def.verbProperties.warmupTime > 0f)
			{
				Pawn pawn = castTarg.Thing as Pawn;
				if (pawn != null && pawn != this.ability.pawn)
				{
					pawn.stances.stunner.StunFor(this.ability.def.verbProperties.warmupTime.SecondsToTicks(), this.ability.pawn, false, false, false);
					if (!pawn.Awake())
					{
						RestUtility.WakeUp(pawn, true);
					}
				}
			}
			return flag;
		}

		// Token: 0x06008095 RID: 32917 RVA: 0x002DDD24 File Offset: 0x002DBF24
		public override void DrawHighlight(LocalTargetInfo target)
		{
			AbilityDef def = this.ability.def;
			if (this.EffectiveRange > 0f)
			{
				this.verbProps.DrawRadiusRing(this.caster.Position, this);
			}
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false))
			{
				if (def.HasAreaOfEffect)
				{
					if (target.IsValid)
					{
						GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
						GenDraw.DrawRadiusRing(target.Cell, def.EffectRadius, Verb_CastAbility.RadiusHighlightColor, null);
					}
				}
				else
				{
					GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
				}
			}
			if (target.IsValid)
			{
				this.ability.DrawEffectPreviews(target);
			}
		}

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x06008096 RID: 32918 RVA: 0x002DDDD0 File Offset: 0x002DBFD0
		public override Texture2D UIIcon
		{
			get
			{
				return this.ability.def.uiIcon;
			}
		}

		// Token: 0x06008097 RID: 32919 RVA: 0x002DDDE2 File Offset: 0x002DBFE2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
		}

		// Token: 0x0400502C RID: 20524
		public Ability ability;
	}
}
