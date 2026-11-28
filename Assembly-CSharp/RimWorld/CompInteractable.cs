using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020018CE RID: 6350
	public abstract class CompInteractable : ThingComp, ITargetingSource
	{
		// Token: 0x17001B4A RID: 6986
		// (get) Token: 0x06009F15 RID: 40725 RVA: 0x003911AA File Offset: 0x0038F3AA
		public CompProperties_Interactable Props
		{
			get
			{
				return (CompProperties_Interactable)this.props;
			}
		}

		// Token: 0x17001B4B RID: 6987
		// (get) Token: 0x06009F16 RID: 40726 RVA: 0x003911B7 File Offset: 0x0038F3B7
		public bool OnCooldown
		{
			get
			{
				return this.cooldownTicks > 0;
			}
		}

		// Token: 0x17001B4C RID: 6988
		// (get) Token: 0x06009F17 RID: 40727 RVA: 0x003911C2 File Offset: 0x0038F3C2
		public virtual bool Active
		{
			get
			{
				return this.activeTicks > 0;
			}
		}

		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x06009F18 RID: 40728 RVA: 0x003911CD File Offset: 0x0038F3CD
		public virtual bool CanCooldown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B4E RID: 6990
		// (get) Token: 0x06009F19 RID: 40729 RVA: 0x003911D0 File Offset: 0x0038F3D0
		public virtual int TicksToActivate
		{
			get
			{
				return this.Props.ticksToActivate;
			}
		}

		// Token: 0x17001B4F RID: 6991
		// (get) Token: 0x06009F1A RID: 40730 RVA: 0x003911E0 File Offset: 0x0038F3E0
		public virtual NamedArgument? ExtraNamedArg
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B50 RID: 6992
		// (get) Token: 0x06009F1B RID: 40731 RVA: 0x003911F6 File Offset: 0x0038F3F6
		public virtual string ExposeKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B51 RID: 6993
		// (get) Token: 0x06009F1C RID: 40732 RVA: 0x003911F9 File Offset: 0x0038F3F9
		public virtual bool HideInteraction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B52 RID: 6994
		// (get) Token: 0x06009F1D RID: 40733 RVA: 0x003911FC File Offset: 0x0038F3FC
		protected virtual string ActivateOptionLabel
		{
			get
			{
				return this.Props.jobString.CapitalizeFirst();
			}
		}

		// Token: 0x17001B53 RID: 6995
		// (get) Token: 0x06009F1E RID: 40734 RVA: 0x0039120E File Offset: 0x0038F40E
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B54 RID: 6996
		// (get) Token: 0x06009F1F RID: 40735 RVA: 0x00391211 File Offset: 0x0038F411
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B55 RID: 6997
		// (get) Token: 0x06009F20 RID: 40736 RVA: 0x00391214 File Offset: 0x0038F414
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B56 RID: 6998
		// (get) Token: 0x06009F21 RID: 40737 RVA: 0x00391217 File Offset: 0x0038F417
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B57 RID: 6999
		// (get) Token: 0x06009F22 RID: 40738 RVA: 0x0039121A File Offset: 0x0038F41A
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B58 RID: 7000
		// (get) Token: 0x06009F23 RID: 40739 RVA: 0x0039121D File Offset: 0x0038F41D
		public Thing Caster
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17001B59 RID: 7001
		// (get) Token: 0x06009F24 RID: 40740 RVA: 0x00391225 File Offset: 0x0038F425
		public Pawn CasterPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B5A RID: 7002
		// (get) Token: 0x06009F25 RID: 40741 RVA: 0x00391228 File Offset: 0x0038F428
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B5B RID: 7003
		// (get) Token: 0x06009F26 RID: 40742 RVA: 0x0039122B File Offset: 0x0038F42B
		public TargetingParameters targetParams
		{
			get
			{
				return this.Props.targetingParameters;
			}
		}

		// Token: 0x17001B5C RID: 7004
		// (get) Token: 0x06009F27 RID: 40743 RVA: 0x00391238 File Offset: 0x0038F438
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B5D RID: 7005
		// (get) Token: 0x06009F28 RID: 40744 RVA: 0x0039123C File Offset: 0x0038F43C
		public Texture2D UIIcon
		{
			get
			{
				if (!(this.activateTex != null))
				{
					return this.activateTex = ContentFinder<Texture2D>.Get(this.Props.activateTexPath, true);
				}
				return this.activateTex;
			}
		}

		// Token: 0x06009F29 RID: 40745 RVA: 0x00391278 File Offset: 0x0038F478
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.refuelable = this.parent.TryGetComp<CompRefuelable>();
			this.power = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06009F2A RID: 40746 RVA: 0x0039129C File Offset: 0x0038F49C
		public override void CompTick()
		{
			if (this.Active)
			{
				if (this.TryInteractTick())
				{
					this.Deactivate();
				}
				else
				{
					if (this.ShouldDeactivate())
					{
						this.SendDeactivateMessage();
						this.Deactivate();
						return;
					}
					this.activeTicks--;
				}
				if (this.activeTicks <= 0)
				{
					this.StartCooldown();
				}
			}
			else if (this.OnCooldown)
			{
				if (this.CanCooldown)
				{
					this.cooldownTicks--;
				}
				if (this.parent.Spawned)
				{
					if (this.progressBarEffecter == null)
					{
						this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
					}
					this.progressBarEffecter.EffectTick(this.parent, TargetInfo.Invalid);
					MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
					mote.progress = 1f - (float)this.cooldownTicks / (float)this.Props.cooldownTicks;
					mote.offsetZ = -0.8f;
				}
				else
				{
					Effecter effecter = this.progressBarEffecter;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.progressBarEffecter = null;
				}
				if (this.Props.cooldownFleck != null && this.parent.IsHashIntervalTick(this.Props.cooldownFleckSpawnIntervalTicks))
				{
					FleckCreationData dataStatic = FleckMaker.GetDataStatic(this.parent.DrawPos, this.parent.Map, this.Props.cooldownFleck, this.Props.cooldownFleckScale);
					this.parent.Map.flecks.CreateFleck(dataStatic);
				}
				if (this.cooldownTicks <= 0)
				{
					this.CooldownEnded(true);
				}
			}
			Effecter effecter2 = this.interactionEffecter;
			if (effecter2 == null)
			{
				return;
			}
			effecter2.EffectTick(this.parent, this.parent);
		}

		// Token: 0x06009F2B RID: 40747 RVA: 0x0039145E File Offset: 0x0038F65E
		public void ResetCooldown(bool sendMessage = true)
		{
			if (this.Active)
			{
				this.Deactivate();
			}
			if (this.OnCooldown)
			{
				this.cooldownTicks = 0;
				this.CooldownEnded(sendMessage);
			}
		}

		// Token: 0x06009F2C RID: 40748 RVA: 0x00391484 File Offset: 0x0038F684
		protected virtual void SendDeactivateMessage()
		{
			Messages.Message("MessageActivationCanceled".Translate(this.parent), this.parent, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06009F2D RID: 40749 RVA: 0x003914B8 File Offset: 0x0038F6B8
		public virtual AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (this.Active)
			{
				return "AlreadyActive".Translate();
			}
			if (this.OnCooldown)
			{
				return this.Props.onCooldownString + " (" + "DurationLeft".Translate(this.cooldownTicks.ToStringTicksToPeriod(true, false, true, true, false)) + ")";
			}
			if (this.refuelable != null && !this.refuelable.HasFuel)
			{
				return this.refuelable.Props.outOfFuelMessage ?? "NoFuel".Translate();
			}
			if (this.Props.requiresPower && this.power != null && !this.power.PowerOn)
			{
				return "NoPower".Translate().CapitalizeFirst();
			}
			if (activateBy != null)
			{
				if (activateBy.Dead)
				{
					return "PawnIsDead".Translate(activateBy);
				}
				if (this.Props.activateStat != null && (this.Props.activateStat.Worker.IsDisabledFor(activateBy) || this.Props.activateStat.statFactors.Any((StatDef s) => s.Worker.IsDisabledFor(activateBy))))
				{
					return "Incapable".Translate().CapitalizeFirst();
				}
				if (this.parent.PositionHeld.IsForbidden(activateBy) && !activateBy.Drafted && !this.Props.ignoreForbidden)
				{
					return "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + activateBy.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label;
				}
				if (this.parent.IsForbidden(activateBy) && !activateBy.Drafted && !this.Props.ignoreForbidden)
				{
					return "CannotPrioritizeCellForbidden".Translate();
				}
				if (activateBy.Downed)
				{
					return "MessageRitualPawnDowned".Translate(activateBy);
				}
				if (activateBy.Deathresting)
				{
					return "IsDeathresting".Translate(activateBy.Named("PAWN"));
				}
				if (!activateBy.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					return "MessageIncapableOfManipulation".Translate(activateBy);
				}
				if (!activateBy.CanReach(this.parent.SpawnedParentOrMe, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return "CannotReach".Translate();
				}
			}
			return true;
		}

		// Token: 0x06009F2E RID: 40750 RVA: 0x003917B4 File Offset: 0x0038F9B4
		public void Interact(Pawn caster, bool force = false)
		{
			if (this.CanInteract(caster, false).Accepted || force)
			{
				this.activeTicks = this.Props.activeTicks;
				if (!this.Props.soundActivate.NullOrUndefined())
				{
					this.Props.soundActivate.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				if (this.Props.fleckOnUsed != null)
				{
					FleckMaker.AttachedOverlay(caster, this.Props.fleckOnUsed, Vector3.zero, this.Props.fleckOnUsedScale, -1f);
				}
				this.OnInteracted(caster);
				this.parent.BroadcastCompSignal("Interacted");
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "Activated", this.parent.Named("SUBJECT"), caster.Named("ACTIVATOR"));
				if (!string.IsNullOrEmpty(this.Props.messageCompletedString))
				{
					Messages.Message(this.Props.messageCompletedString.Formatted(caster.Named("PAWN")), caster, MessageTypeDefOf.NeutralEvent, false);
				}
			}
		}

		// Token: 0x06009F2F RID: 40751 RVA: 0x003918D9 File Offset: 0x0038FAD9
		protected virtual void OnInteracted(Pawn caster)
		{
		}

		// Token: 0x06009F30 RID: 40752 RVA: 0x003918DB File Offset: 0x0038FADB
		protected virtual bool TryInteractTick()
		{
			return true;
		}

		// Token: 0x06009F31 RID: 40753 RVA: 0x003918DE File Offset: 0x0038FADE
		protected virtual bool ShouldDeactivate()
		{
			return false;
		}

		// Token: 0x06009F32 RID: 40754 RVA: 0x003918E1 File Offset: 0x0038FAE1
		protected virtual void Deactivate()
		{
			this.activeTicks = 0;
		}

		// Token: 0x06009F33 RID: 40755 RVA: 0x003918EA File Offset: 0x0038FAEA
		protected virtual void StartCooldown()
		{
			this.cooldownTicks = this.Props.cooldownTicks;
		}

		// Token: 0x06009F34 RID: 40756 RVA: 0x00391900 File Offset: 0x0038FB00
		protected virtual void CooldownEnded(bool sendMessage = true)
		{
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBarEffecter = null;
			if (sendMessage && !string.IsNullOrEmpty(this.Props.messageCooldownEnded))
			{
				Messages.Message(this.Props.messageCooldownEnded, this.parent, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06009F35 RID: 40757 RVA: 0x0039195B File Offset: 0x0038FB5B
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBarEffecter = null;
			base.PostDestroy(mode, previousMap);
		}

		// Token: 0x06009F36 RID: 40758 RVA: 0x0039197D File Offset: 0x0038FB7D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompInteractable.<CompGetGizmosExtra>d__62 <CompGetGizmosExtra>d__ = new CompInteractable.<CompGetGizmosExtra>d__62(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009F37 RID: 40759 RVA: 0x00391990 File Offset: 0x0038FB90
		public override string CompInspectStringExtra()
		{
			TaggedString taggedString = base.CompInspectStringExtra();
			if (!taggedString.NullOrEmpty())
			{
				taggedString += "\n";
			}
			if (this.OnCooldown)
			{
				taggedString += this.Props.onCooldownString.CapitalizeFirst() + ": " + "DurationLeft".Translate(this.cooldownTicks.ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst() + ".";
				if (!this.CanCooldown)
				{
					TaggedString t = taggedString;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Paused".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(")");
					taggedString = t + defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			if (!this.Props.inspectString.NullOrEmpty())
			{
				if (!taggedString.NullOrEmpty())
				{
					taggedString += "\n";
				}
				taggedString += this.Props.inspectString;
			}
			else if (this.Props.showMustBeActivatedByColonist)
			{
				if (!taggedString.NullOrEmpty())
				{
					taggedString += "\n";
				}
				taggedString += "MustBeActivatedByColonist".Translate();
			}
			if (!this.Active && this.Props.maintainProgress && this.progress > 0f)
			{
				if (this.Props.remainingSecondsInInspectString)
				{
					taggedString += "\n" + "Activation".Translate() + ": " + Mathf.FloorToInt((float)this.TicksToActivate * (1f - this.progress)).ToStringSecondsFromTicks("F0");
				}
				else
				{
					taggedString += "\n" + "Activation".Translate() + ": " + this.progress.ToStringPercent();
				}
			}
			return taggedString.Resolve();
		}

		// Token: 0x06009F38 RID: 40760 RVA: 0x00391B99 File Offset: 0x0038FD99
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompInteractable.<CompFloatMenuOptions>d__64 <CompFloatMenuOptions>d__ = new CompInteractable.<CompFloatMenuOptions>d__64(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x06009F39 RID: 40761 RVA: 0x00391BB0 File Offset: 0x0038FDB0
		public override void PostExposeData()
		{
			if (string.IsNullOrEmpty(this.ExposeKey))
			{
				Scribe_Values.Look<int>(ref this.cooldownTicks, "cooldownTicks", 0, false);
				Scribe_Values.Look<int>(ref this.activeTicks, "activeTicks", 0, false);
				Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
				return;
			}
			Scribe_Values.Look<int>(ref this.cooldownTicks, this.ExposeKey + "_cooldownTicks", 0, false);
			Scribe_Values.Look<int>(ref this.activeTicks, this.ExposeKey + "_activeTicks", 0, false);
			Scribe_Values.Look<float>(ref this.progress, this.ExposeKey + "_progress", 0f, false);
		}

		// Token: 0x06009F3A RID: 40762 RVA: 0x00391C60 File Offset: 0x0038FE60
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return this.ValidateTarget(target, false);
		}

		// Token: 0x06009F3B RID: 40763 RVA: 0x00391C6C File Offset: 0x0038FE6C
		public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!target.IsValid || target.Pawn == null)
			{
				return false;
			}
			Pawn pawn = target.Pawn;
			AcceptanceReport acceptanceReport = this.CanInteract(pawn, true);
			if (!acceptanceReport.Accepted)
			{
				if (showMessages && !acceptanceReport.Reason.NullOrEmpty())
				{
					Messages.Message("CannotGenericWorkCustom".Translate(this.Props.jobString) + ": " + acceptanceReport.Reason.CapitalizeFirst(), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06009F3C RID: 40764 RVA: 0x00391D06 File Offset: 0x0038FF06
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x06009F3D RID: 40765 RVA: 0x00391D18 File Offset: 0x0038FF18
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
			if (this.ValidateTarget(target, false))
			{
				Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent);
				target.Pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}
		}

		// Token: 0x06009F3E RID: 40766 RVA: 0x00391D60 File Offset: 0x0038FF60
		public void OnGUI(LocalTargetInfo target)
		{
			string label;
			if (string.IsNullOrEmpty(this.Props.guiLabelString))
			{
				label = "ChooseWhoShouldActivate".Translate();
			}
			else
			{
				label = this.Props.guiLabelString;
			}
			Widgets.MouseAttachedLabel(label, 0f, 0f, null);
			if (this.ValidateTarget(target, false) && this.Props.targetingParameters.CanTarget(target.Pawn, this))
			{
				GenUI.DrawMouseAttachment(this.UIIcon);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x06009F3F RID: 40767 RVA: 0x00391DF5 File Offset: 0x0038FFF5
		public void Notify_InteractionStarted()
		{
			if (this.Props.interactionEffecter != null)
			{
				this.interactionEffecter = this.Props.interactionEffecter.Spawn(this.parent, this.parent.Map, 1f);
			}
		}

		// Token: 0x06009F40 RID: 40768 RVA: 0x00391E30 File Offset: 0x00390030
		public void Notify_InteractionEnded()
		{
			if (this.Props.interactionEffecter != null)
			{
				Effecter effecter = this.interactionEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.interactionEffecter = null;
			}
		}

		// Token: 0x04005E3A RID: 24122
		protected int cooldownTicks;

		// Token: 0x04005E3B RID: 24123
		private int activeTicks;

		// Token: 0x04005E3C RID: 24124
		public float progress;

		// Token: 0x04005E3D RID: 24125
		[Unsaved(false)]
		public Effecter interactionEffecter;

		// Token: 0x04005E3E RID: 24126
		[Unsaved(false)]
		private Effecter progressBarEffecter;

		// Token: 0x04005E3F RID: 24127
		[Unsaved(false)]
		private Texture2D activateTex;

		// Token: 0x04005E40 RID: 24128
		[Unsaved(false)]
		private CompRefuelable refuelable;

		// Token: 0x04005E41 RID: 24129
		[Unsaved(false)]
		protected CompPowerTrader power;

		// Token: 0x04005E42 RID: 24130
		public const string InteractedSignal = "Interacted";
	}
}
