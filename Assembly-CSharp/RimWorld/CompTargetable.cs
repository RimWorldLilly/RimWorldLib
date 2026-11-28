using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A03 RID: 6659
	public abstract class CompTargetable : CompUseEffect, ITargetingSource
	{
		// Token: 0x17001D57 RID: 7511
		// (get) Token: 0x0600A7A2 RID: 42914 RVA: 0x003BB31A File Offset: 0x003B951A
		public CompProperties_Targetable Props
		{
			get
			{
				return (CompProperties_Targetable)this.props;
			}
		}

		// Token: 0x17001D58 RID: 7512
		// (get) Token: 0x0600A7A3 RID: 42915
		protected abstract bool PlayerChoosesTarget { get; }

		// Token: 0x0600A7A4 RID: 42916 RVA: 0x003BB327 File Offset: 0x003B9527
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.selectedTarget, "selectedTarget", false);
		}

		// Token: 0x0600A7A5 RID: 42917 RVA: 0x003BB340 File Offset: 0x003B9540
		public override bool SelectedUseOption(Pawn p)
		{
			if (this.PlayerChoosesTarget)
			{
				this.caster = p;
				Find.Targeter.BeginTargeting(this, null, true, null, null, true);
				return true;
			}
			this.selectedTarget = null;
			return false;
		}

		// Token: 0x0600A7A6 RID: 42918 RVA: 0x003BB36C File Offset: 0x003B956C
		public override void DoEffect(Pawn usedBy)
		{
			if (this.PlayerChoosesTarget && this.selectedTarget == null)
			{
				return;
			}
			if (this.selectedTarget != null && !this.GetTargetingParameters().CanTarget(this.selectedTarget, null))
			{
				return;
			}
			base.DoEffect(usedBy);
			foreach (Thing thing in this.GetTargets(this.selectedTarget))
			{
				foreach (CompTargetEffect compTargetEffect in this.parent.GetComps<CompTargetEffect>())
				{
					compTargetEffect.DoEffectOn(usedBy, thing);
				}
				if (this.Props.moteOnTarget != null)
				{
					MoteMaker.MakeAttachedOverlay(thing, this.Props.moteOnTarget, Vector3.zero, 1f, -1f);
				}
				if (this.Props.fleckOnTarget != null)
				{
					FleckMaker.AttachedOverlay(thing, this.Props.fleckOnTarget, Vector3.zero, 1f, -1f);
				}
				if (this.Props.moteConnecting != null)
				{
					MoteMaker.MakeConnectingLine(usedBy.DrawPos, thing.DrawPos, this.Props.moteConnecting, usedBy.Map, 1f);
				}
				if (this.Props.fleckConnecting != null)
				{
					FleckMaker.ConnectingLine(usedBy.DrawPos, thing.DrawPos, this.Props.fleckConnecting, usedBy.Map, 1f);
				}
			}
			this.selectedTarget = null;
		}

		// Token: 0x0600A7A7 RID: 42919
		protected abstract TargetingParameters GetTargetingParameters();

		// Token: 0x0600A7A8 RID: 42920
		public abstract IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null);

		// Token: 0x17001D59 RID: 7513
		// (get) Token: 0x0600A7A9 RID: 42921 RVA: 0x003BB51C File Offset: 0x003B971C
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D5A RID: 7514
		// (get) Token: 0x0600A7AA RID: 42922 RVA: 0x003BB51F File Offset: 0x003B971F
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D5B RID: 7515
		// (get) Token: 0x0600A7AB RID: 42923 RVA: 0x003BB522 File Offset: 0x003B9722
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D5C RID: 7516
		// (get) Token: 0x0600A7AC RID: 42924 RVA: 0x003BB525 File Offset: 0x003B9725
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D5D RID: 7517
		// (get) Token: 0x0600A7AD RID: 42925 RVA: 0x003BB528 File Offset: 0x003B9728
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D5E RID: 7518
		// (get) Token: 0x0600A7AE RID: 42926 RVA: 0x003BB52B File Offset: 0x003B972B
		public Thing Caster
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17001D5F RID: 7519
		// (get) Token: 0x0600A7AF RID: 42927 RVA: 0x003BB533 File Offset: 0x003B9733
		public Pawn CasterPawn
		{
			get
			{
				return this.caster;
			}
		}

		// Token: 0x17001D60 RID: 7520
		// (get) Token: 0x0600A7B0 RID: 42928 RVA: 0x003BB53B File Offset: 0x003B973B
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001D61 RID: 7521
		// (get) Token: 0x0600A7B1 RID: 42929 RVA: 0x003BB53E File Offset: 0x003B973E
		public TargetingParameters targetParams
		{
			get
			{
				return this.GetTargetingParameters();
			}
		}

		// Token: 0x17001D62 RID: 7522
		// (get) Token: 0x0600A7B2 RID: 42930 RVA: 0x003BB546 File Offset: 0x003B9746
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001D63 RID: 7523
		// (get) Token: 0x0600A7B3 RID: 42931 RVA: 0x003BB549 File Offset: 0x003B9749
		public Texture2D UIIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600A7B4 RID: 42932 RVA: 0x003BB54C File Offset: 0x003B974C
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return this.ValidateTarget(target, false);
		}

		// Token: 0x0600A7B5 RID: 42933 RVA: 0x003BB556 File Offset: 0x003B9756
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x0600A7B6 RID: 42934 RVA: 0x003BB568 File Offset: 0x003B9768
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.selectedTarget = target.Thing;
			CompUsable compUsable;
			if (this.parent.TryGetComp(out compUsable))
			{
				compUsable.TryStartUseJob(this.caster, target, compUsable.Props.ignoreOtherReservations);
			}
			this.caster = null;
		}

		// Token: 0x0600A7B7 RID: 42935 RVA: 0x003BB5B0 File Offset: 0x003B97B0
		public void OnGUI(LocalTargetInfo target)
		{
			Widgets.MouseAttachedLabel("TargetGizmoMouse".Translate(), 0f, 0f, null);
			if (this.ValidateTarget(target, false))
			{
				GenUI.DrawMouseAttachment(TexCommand.Attack);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x0600A7B8 RID: 42936 RVA: 0x003BB600 File Offset: 0x003B9800
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Thing thing = target.Thing;
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (this.Props.psychicSensitiveTargetsOnly && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) <= 0f)
				{
					return false;
				}
				if (this.Props.ignoreQuestLodgerPawns && pawn.IsQuestLodger())
				{
					return false;
				}
				if (this.Props.ignorePlayerFactionPawns && pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
				if (this.Props.cannotHaveHediff != null && pawn.health.hediffSet.HasHediff(this.Props.cannotHaveHediff, false))
				{
					if (showMessages)
					{
						Messages.Message("CannotUseDueToHediff".Translate(this.parent.Named("TARGETABLE"), pawn.Named("TARGET"), this.Props.cannotHaveHediff.label), MessageTypeDefOf.RejectInput, true);
					}
					return false;
				}
			}
			if (this.Props.fleshCorpsesOnly)
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && !corpse.InnerPawn.RaceProps.IsFlesh)
				{
					return false;
				}
			}
			if (this.Props.nonDessicatedCorpsesOnly)
			{
				Corpse corpse2 = thing as Corpse;
				if (corpse2 != null && corpse2.GetRotStage() == RotStage.Dessicated)
				{
					return false;
				}
			}
			if (ModsConfig.AnomalyActive && this.Props.mutantFilter != null)
			{
				Corpse corpse3 = thing as Corpse;
				if (corpse3 == null || !corpse3.InnerPawn.IsMutant || corpse3.InnerPawn.mutant.Def != this.Props.mutantFilter)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040062B0 RID: 25264
		private Thing selectedTarget;

		// Token: 0x040062B1 RID: 25265
		private Pawn caster;
	}
}
