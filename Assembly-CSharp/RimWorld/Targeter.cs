using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D8E RID: 7566
	public class Targeter
	{
		// Token: 0x17002475 RID: 9333
		// (get) Token: 0x0600C44B RID: 50251 RVA: 0x0047D350 File Offset: 0x0047B550
		public bool IsTargeting
		{
			get
			{
				return this.targetingSource != null || this.action != null;
			}
		}

		// Token: 0x0600C44C RID: 50252 RVA: 0x0047D368 File Offset: 0x0047B568
		public void BeginTargeting(ITargetingSource source, ITargetingSource parent = null, bool allowNonSelectedTargetingSource = false, Func<LocalTargetInfo, ITargetingSource> extraSourceGetter = null, Action actionWhenFinished = null, bool requiresAvailableVerb = true)
		{
			if (source.Targetable)
			{
				this.targetingSource = source;
				this.targetingSourceAdditionalPawns = new List<Pawn>();
			}
			else
			{
				Verb getVerb = source.GetVerb;
				if (getVerb.verbProps.nonInterruptingSelfCast)
				{
					getVerb.TryStartCastOn(getVerb.Caster, false, true, false, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThing, getVerb.Caster);
				job.verbToUse = getVerb;
				source.CasterPawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
			this.action = null;
			this.caster = null;
			this.targetParams = null;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = null;
			this.targetingSourceParent = parent;
			this.targetingSourceGetter = extraSourceGetter;
			this.requiresAvailableVerb = requiresAvailableVerb;
			this.requiresCastedSelected = true;
			this.needsStopTargetingCall = false;
			this.allowNonSelectedTargetingSource = allowNonSelectedTargetingSource;
		}

		// Token: 0x0600C44D RID: 50253 RVA: 0x0047D458 File Offset: 0x0047B658
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Pawn caster = null, Action actionWhenFinished = null, Texture2D mouseAttachment = null, bool requiresCastedSelected = true)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.targetParams = targetParams;
			this.caster = caster;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = mouseAttachment;
			this.highlightAction = null;
			this.targetValidator = null;
			this.onGuiAction = null;
			this.onUpdateAction = null;
			this.requiresCastedSelected = requiresCastedSelected;
			this.needsStopTargetingCall = false;
			this.playSoundOnAction = true;
		}

		// Token: 0x0600C44E RID: 50254 RVA: 0x0047D4D4 File Offset: 0x0047B6D4
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Action<LocalTargetInfo> onGuiAction)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.targetParams = targetParams;
			this.caster = null;
			this.actionWhenFinished = null;
			this.mouseAttachment = null;
			this.highlightAction = null;
			this.targetValidator = null;
			this.onGuiAction = onGuiAction;
			this.onUpdateAction = null;
			this.requiresCastedSelected = true;
			this.needsStopTargetingCall = false;
			this.playSoundOnAction = true;
		}

		// Token: 0x0600C44F RID: 50255 RVA: 0x0047D54C File Offset: 0x0047B74C
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Action<LocalTargetInfo> highlightAction, Func<LocalTargetInfo, bool> targetValidator, Pawn caster = null, Action actionWhenFinished = null, Texture2D mouseAttachment = null, bool playSoundOnAction = true, Action<LocalTargetInfo> onGuiAction = null, Action<LocalTargetInfo> onUpdateAction = null)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = new List<Pawn>();
			this.action = action;
			this.targetParams = targetParams;
			this.caster = caster;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = mouseAttachment;
			this.highlightAction = highlightAction;
			this.targetValidator = targetValidator;
			this.playSoundOnAction = playSoundOnAction;
			this.onGuiAction = onGuiAction;
			this.onUpdateAction = onUpdateAction;
			this.requiresCastedSelected = true;
			this.needsStopTargetingCall = false;
		}

		// Token: 0x0600C450 RID: 50256 RVA: 0x0047D5D0 File Offset: 0x0047B7D0
		public void BeginTargeting(TargetingParameters targetParams, ITargetingSource ability, Action<LocalTargetInfo> action, Action actionWhenFinished = null, Texture2D mouseAttachment = null)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.actionWhenFinished = actionWhenFinished;
			this.caster = null;
			this.targetParams = targetParams;
			this.mouseAttachment = mouseAttachment;
			this.targetingSource = ability;
			this.highlightAction = null;
			this.targetValidator = null;
			this.onGuiAction = null;
			this.onUpdateAction = null;
			this.requiresCastedSelected = true;
			this.needsStopTargetingCall = false;
			this.playSoundOnAction = true;
		}

		// Token: 0x0600C451 RID: 50257 RVA: 0x0047D650 File Offset: 0x0047B850
		public void StopTargeting()
		{
			if (this.actionWhenFinished != null)
			{
				Action action = this.actionWhenFinished;
				this.actionWhenFinished = null;
				action();
			}
			this.targetingSource = null;
			this.action = null;
			this.targetParams = null;
			this.highlightAction = null;
			this.targetValidator = null;
			this.targetingSourceGetter = null;
			this.onUpdateAction = null;
			this.onGuiAction = null;
			this.requiresCastedSelected = true;
		}

		// Token: 0x0600C452 RID: 50258 RVA: 0x0047D6B8 File Offset: 0x0047B8B8
		public void ProcessInputEvents()
		{
			this.UpdateTargetingSource();
			this.ConfirmStillValid();
			if (this.IsTargeting)
			{
				if (Event.current.type == EventType.mouseDown && Event.current.button == 0)
				{
					LocalTargetInfo localTargetInfo = this.CurrentTargetUnderMouse(false);
					this.needsStopTargetingCall = true;
					if (this.targetingSource != null)
					{
						if (!this.targetingSource.ValidateTarget(localTargetInfo, true))
						{
							Event.current.Use();
							return;
						}
						this.OrderVerbForceTarget();
					}
					else if (this.action != null)
					{
						if (this.targetValidator != null)
						{
							if (this.targetValidator(localTargetInfo))
							{
								this.action(localTargetInfo);
							}
							else
							{
								this.needsStopTargetingCall = false;
							}
						}
						else if (localTargetInfo.IsValid)
						{
							this.action(localTargetInfo);
						}
					}
					if (this.playSoundOnAction)
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					if (this.targetingSource != null)
					{
						if (this.targetingSource.DestinationSelector != null)
						{
							this.BeginTargeting(this.targetingSource.DestinationSelector, this.targetingSource, false, this.targetingSourceGetter, null, true);
						}
						else if (this.targetingSource.MultiSelect && Event.current.shift)
						{
							this.BeginTargeting(this.targetingSource, null, false, this.targetingSourceGetter, null, true);
						}
						else if (this.targetingSourceParent != null && this.targetingSourceParent.MultiSelect && Event.current.shift)
						{
							this.BeginTargeting(this.targetingSourceParent, null, false, this.targetingSourceGetter, null, true);
						}
					}
					if (this.needsStopTargetingCall)
					{
						this.StopTargeting();
					}
					Event.current.Use();
				}
				if ((Event.current.type == EventType.mouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
		}

		// Token: 0x0600C453 RID: 50259 RVA: 0x0047D88C File Offset: 0x0047BA8C
		public void TargeterOnGUI()
		{
			if (this.targetingSource != null)
			{
				LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
				this.targetingSource.OnGUI(target);
			}
			if (this.action != null)
			{
				GenUI.DrawMouseAttachment(this.mouseAttachment ?? TexCommand.Attack);
			}
			if (this.onGuiAction != null)
			{
				LocalTargetInfo obj = this.CurrentTargetUnderMouse(true);
				this.onGuiAction(obj);
			}
			ITargetingSource targetingSource = this.targetingSource;
			bool flag;
			if (targetingSource == null)
			{
				flag = (null != null);
			}
			else
			{
				Verb getVerb = targetingSource.GetVerb;
				if (getVerb == null)
				{
					flag = (null != null);
				}
				else
				{
					VerbProperties verbProps = getVerb.verbProps;
					flag = (((verbProps != null) ? verbProps.mouseTargetingText : null) != null);
				}
			}
			if (flag)
			{
				Widgets.MouseAttachedLabel(this.targetingSource.GetVerb.verbProps.mouseTargetingText, 0f, 0f, null);
			}
		}

		// Token: 0x0600C454 RID: 50260 RVA: 0x0047D948 File Offset: 0x0047BB48
		public void TargeterUpdate()
		{
			this.UpdateTargetingSource();
			if (this.targetingSource != null)
			{
				this.targetingSource.DrawHighlight(this.CurrentTargetUnderMouse(true));
			}
			if (this.action != null)
			{
				LocalTargetInfo localTargetInfo = this.CurrentTargetUnderMouse(false);
				if (this.highlightAction != null)
				{
					this.highlightAction(localTargetInfo);
				}
				else if (localTargetInfo.IsValid)
				{
					GenDraw.DrawTargetHighlight(localTargetInfo);
				}
			}
			if (this.onUpdateAction != null)
			{
				LocalTargetInfo obj = this.CurrentTargetUnderMouse(true);
				this.onUpdateAction(obj);
			}
		}

		// Token: 0x0600C455 RID: 50261 RVA: 0x0047D9C8 File Offset: 0x0047BBC8
		public bool IsPawnTargeting(Pawn p)
		{
			if (this.caster == p)
			{
				return true;
			}
			if (this.targetingSource != null && this.targetingSource.CasterIsPawn)
			{
				if (this.targetingSource.CasterPawn == p)
				{
					return true;
				}
				for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
				{
					if (this.targetingSourceAdditionalPawns[i] == p)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600C456 RID: 50262 RVA: 0x0047DA30 File Offset: 0x0047BC30
		private void UpdateTargetingSource()
		{
			if (this.targetingSourceGetter != null)
			{
				ITargetingSource targetingSource = this.targetingSourceGetter(this.CurrentTargetUnderMouse(true));
				if (targetingSource != null)
				{
					this.targetingSource = targetingSource;
				}
			}
		}

		// Token: 0x0600C457 RID: 50263 RVA: 0x0047DA64 File Offset: 0x0047BC64
		private void ConfirmStillValid()
		{
			if (this.caster != null && (this.caster.Map != Find.CurrentMap || this.caster.Destroyed || (this.requiresCastedSelected && !Find.Selector.IsSelected(this.caster))))
			{
				this.StopTargeting();
			}
			if (this.targetingSource != null)
			{
				Selector selector = Find.Selector;
				if (this.targetingSource.Caster.MapHeld == Find.CurrentMap && !this.targetingSource.Caster.Destroyed && (this.allowNonSelectedTargetingSource || selector.IsSelected(this.targetingSource.Caster)) && (this.targetingSource.GetVerb == null || !this.requiresAvailableVerb || this.targetingSource.GetVerb.Available()))
				{
					CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.targetingSource as CompAbilityEffect_WithDest;
					if (compAbilityEffect_WithDest == null || !compAbilityEffect_WithDest.SelectedTargetInvalidated())
					{
						Verb_CastAbility verb_CastAbility = this.targetingSource as Verb_CastAbility;
						if (verb_CastAbility == null || verb_CastAbility.ability.CanQueueCast)
						{
							for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
							{
								if (this.targetingSourceAdditionalPawns[i].Destroyed || !selector.IsSelected(this.targetingSourceAdditionalPawns[i]))
								{
									this.StopTargeting();
									return;
								}
							}
							return;
						}
					}
				}
				this.StopTargeting();
				return;
			}
		}

		// Token: 0x0600C458 RID: 50264 RVA: 0x0047DBB8 File Offset: 0x0047BDB8
		private void OrderVerbForceTarget()
		{
			if (this.targetingSource.CasterIsPawn)
			{
				this.OrderPawnForceTarget(this.targetingSource);
				if (this.targetingSourceAdditionalPawns != null)
				{
					for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
					{
						Verb targetingVerb = this.GetTargetingVerb(this.targetingSourceAdditionalPawns[i]);
						if (targetingVerb != null)
						{
							this.OrderPawnForceTarget(targetingVerb);
						}
					}
					return;
				}
			}
			else
			{
				int numSelected = Find.Selector.NumSelected;
				List<object> selectedObjects = Find.Selector.SelectedObjects;
				for (int j = 0; j < numSelected; j++)
				{
					Building_Turret building_Turret = selectedObjects[j] as Building_Turret;
					if (building_Turret != null && building_Turret.Map == Find.CurrentMap)
					{
						LocalTargetInfo targ = this.CurrentTargetUnderMouse(true);
						building_Turret.OrderAttack(targ);
					}
				}
				if (this.targetingSource != null)
				{
					Building_Turret building_Turret2 = this.targetingSource.Caster as Building_Turret;
					if (building_Turret2 != null && building_Turret2 != null && building_Turret2.Map == Find.CurrentMap)
					{
						LocalTargetInfo targ2 = this.CurrentTargetUnderMouse(true);
						building_Turret2.OrderAttack(targ2);
					}
				}
			}
		}

		// Token: 0x0600C459 RID: 50265 RVA: 0x0047DCB8 File Offset: 0x0047BEB8
		public void OrderPawnForceTarget(ITargetingSource targetingSource)
		{
			LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
			if (!target.IsValid)
			{
				return;
			}
			targetingSource.OrderForceTarget(target);
		}

		// Token: 0x0600C45A RID: 50266 RVA: 0x0047DCE0 File Offset: 0x0047BEE0
		private LocalTargetInfo CurrentTargetUnderMouse(bool mustBeHittableNowIfNotMelee)
		{
			if (!this.IsTargeting)
			{
				return LocalTargetInfo.Invalid;
			}
			TargetingParameters targetingParameters = (this.targetingSource != null) ? this.targetingSource.targetParams : this.targetParams;
			ColonistBar.Entry entry;
			LocalTargetInfo localTargetInfo;
			if (Find.ColonistBar.TryGetEntryAt(UI.MousePositionOnUIInverted, out entry) && entry.pawn != null && targetingParameters.CanTarget(entry.pawn, null))
			{
				if (entry.pawn.Dead)
				{
					localTargetInfo = entry.pawn.Corpse;
				}
				else
				{
					localTargetInfo = entry.pawn;
				}
			}
			else
			{
				localTargetInfo = GenUI.TargetsAtMouse(targetingParameters, false, this.targetingSource).FirstOrFallback(LocalTargetInfo.Invalid);
			}
			if (localTargetInfo.IsValid && this.targetingSource != null)
			{
				if (mustBeHittableNowIfNotMelee && !(localTargetInfo.Thing is Pawn) && !this.targetingSource.IsMeleeAttack)
				{
					if (this.targetingSourceAdditionalPawns != null && this.targetingSourceAdditionalPawns.Any<Pawn>())
					{
						bool flag = false;
						for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
						{
							Verb targetingVerb = this.GetTargetingVerb(this.targetingSourceAdditionalPawns[i]);
							if (targetingVerb != null && targetingVerb.CanHitTarget(localTargetInfo))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							localTargetInfo = LocalTargetInfo.Invalid;
						}
					}
					else if (!this.targetingSource.CanHitTarget(localTargetInfo))
					{
						localTargetInfo = LocalTargetInfo.Invalid;
					}
				}
				if (localTargetInfo == this.targetingSource.Caster && !targetingParameters.canTargetSelf)
				{
					localTargetInfo = LocalTargetInfo.Invalid;
				}
			}
			return localTargetInfo;
		}

		// Token: 0x0600C45B RID: 50267 RVA: 0x0047DE68 File Offset: 0x0047C068
		private Verb GetTargetingVerb(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			Verb verb = (equipment != null) ? equipment.AllEquipmentVerbs.FirstOrDefault(new Func<Verb, bool>(this.<GetTargetingVerb>g__SameVerb|35_0)) : null;
			if (verb != null)
			{
				return verb;
			}
			Pawn_ApparelTracker apparel = pawn.apparel;
			if (apparel == null)
			{
				return null;
			}
			return apparel.AllApparelVerbs.FirstOrDefault(new Func<Verb, bool>(this.<GetTargetingVerb>g__SameVerb|35_0));
		}

		// Token: 0x0600C45D RID: 50269 RVA: 0x0047DEDD File Offset: 0x0047C0DD
		[CompilerGenerated]
		private bool <GetTargetingVerb>g__SameVerb|35_0(Verb x)
		{
			return x.verbProps == this.targetingSource.GetVerb.verbProps;
		}

		// Token: 0x0400701B RID: 28699
		public ITargetingSource targetingSource;

		// Token: 0x0400701C RID: 28700
		public ITargetingSource targetingSourceParent;

		// Token: 0x0400701D RID: 28701
		public List<Pawn> targetingSourceAdditionalPawns;

		// Token: 0x0400701E RID: 28702
		public Func<LocalTargetInfo, ITargetingSource> targetingSourceGetter;

		// Token: 0x0400701F RID: 28703
		private Action<LocalTargetInfo> action;

		// Token: 0x04007020 RID: 28704
		private Pawn caster;

		// Token: 0x04007021 RID: 28705
		private TargetingParameters targetParams;

		// Token: 0x04007022 RID: 28706
		private Action actionWhenFinished;

		// Token: 0x04007023 RID: 28707
		private Texture2D mouseAttachment;

		// Token: 0x04007024 RID: 28708
		private Action<LocalTargetInfo> highlightAction;

		// Token: 0x04007025 RID: 28709
		private Func<LocalTargetInfo, bool> targetValidator;

		// Token: 0x04007026 RID: 28710
		private bool playSoundOnAction = true;

		// Token: 0x04007027 RID: 28711
		private bool requiresAvailableVerb = true;

		// Token: 0x04007028 RID: 28712
		private bool requiresCastedSelected = true;

		// Token: 0x04007029 RID: 28713
		private Action<LocalTargetInfo> onGuiAction;

		// Token: 0x0400702A RID: 28714
		private Action<LocalTargetInfo> onUpdateAction;

		// Token: 0x0400702B RID: 28715
		private bool needsStopTargetingCall;

		// Token: 0x0400702C RID: 28716
		private bool allowNonSelectedTargetingSource;
	}
}
