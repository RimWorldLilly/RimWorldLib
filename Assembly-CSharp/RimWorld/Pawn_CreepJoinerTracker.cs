using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001595 RID: 5525
	public class Pawn_CreepJoinerTracker : IExposable
	{
		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x060087AA RID: 34730 RVA: 0x00310A31 File Offset: 0x0030EC31
		private BaseCreepJoinerWorker DownsideWorker
		{
			get
			{
				return this.GetWorker<BaseCreepJoinerWorker>(this.downside.workerType, ref this.downsideWorkerInt);
			}
		}

		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x060087AB RID: 34731 RVA: 0x00310A4A File Offset: 0x0030EC4A
		private BaseCreepJoinerWorker RejectionWorker
		{
			get
			{
				return this.GetWorker<BaseCreepJoinerWorker>(this.rejection.workerType, ref this.rejectionWorkerInt);
			}
		}

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x060087AC RID: 34732 RVA: 0x00310A63 File Offset: 0x0030EC63
		private BaseCreepJoinerWorker AggressiveWorker
		{
			get
			{
				return this.GetWorker<BaseCreepJoinerWorker>(this.aggressive.workerType, ref this.aggressiveWorkerInt);
			}
		}

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x060087AD RID: 34733 RVA: 0x00310A7C File Offset: 0x0030EC7C
		public bool IsOnEntryLord
		{
			get
			{
				if (this.entryLordEnded)
				{
					return false;
				}
				Lord lord = this.Pawn.GetLord();
				return lord != null && lord.LordJob is LordJob_CreepJoiner;
			}
		}

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x060087AE RID: 34734 RVA: 0x00310AB2 File Offset: 0x0030ECB2
		public bool Disabled
		{
			get
			{
				return this.duplicated || this.Pawn.everLostEgo || !this.Pawn.SpawnedOrAnyParentSpawned;
			}
		}

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x060087AF RID: 34735 RVA: 0x00310AD9 File Offset: 0x0030ECD9
		public Pawn Pawn { get; }

		// Token: 0x060087B0 RID: 34736 RVA: 0x00310AE1 File Offset: 0x0030ECE1
		public Pawn_CreepJoinerTracker()
		{
		}

		// Token: 0x060087B1 RID: 34737 RVA: 0x00310AE9 File Offset: 0x0030ECE9
		public Pawn_CreepJoinerTracker(Pawn pawn)
		{
			this.Pawn = pawn;
		}

		// Token: 0x060087B2 RID: 34738 RVA: 0x00310AF8 File Offset: 0x0030ECF8
		public void Notify_Created()
		{
			this.ResolveGraphics();
			BaseCreepJoinerWorker downsideWorker = this.DownsideWorker;
			if (downsideWorker != null)
			{
				downsideWorker.OnCreated();
			}
			BaseCreepJoinerWorker aggressiveWorker = this.AggressiveWorker;
			if (aggressiveWorker != null)
			{
				aggressiveWorker.OnCreated();
			}
			BaseCreepJoinerWorker rejectionWorker = this.RejectionWorker;
			if (rejectionWorker == null)
			{
				return;
			}
			rejectionWorker.OnCreated();
		}

		// Token: 0x060087B3 RID: 34739 RVA: 0x00310B32 File Offset: 0x0030ED32
		public void TickInterval(int delta)
		{
			if (this.Disabled)
			{
				return;
			}
			if (this.Pawn.IsColonist)
			{
				this.ColonistTickInterval(delta);
				return;
			}
			this.CheckTriggersTickInterval(delta);
		}

		// Token: 0x060087B4 RID: 34740 RVA: 0x00310B5C File Offset: 0x0030ED5C
		private void CheckTriggersTickInterval(int delta)
		{
			if (this.triggerPrisonerAggressiveAt != 0 && GenTicks.TicksGame >= this.triggerPrisonerAggressiveAt && !this.triggeredAggressive)
			{
				this.DoAggressive();
			}
			else if (this.triggerPrisonerRejectionAt != 0 && GenTicks.TicksGame >= this.triggerPrisonerRejectionAt && !this.triggeredRejection)
			{
				this.DoRejection();
			}
			if (this.triggerPrisonerAggressiveAt == 0 && this.triggerPrisonerRejectionAt == 0 && this.downside.canOccurWhenImprisoned && this.Pawn.IsPrisonerOfColony && this.Pawn.IsHashIntervalTick(15000, delta))
			{
				this.CheckDownsideOccurs();
			}
		}

		// Token: 0x060087B5 RID: 34741 RVA: 0x00310BF4 File Offset: 0x0030EDF4
		private void ColonistTickInterval(int delta)
		{
			if (this.Pawn.IsHashIntervalTick(15000, delta))
			{
				this.CheckDownsideOccurs();
			}
		}

		// Token: 0x060087B6 RID: 34742 RVA: 0x00310C10 File Offset: 0x0030EE10
		private void CheckDownsideOccurs()
		{
			if (this.downside == null)
			{
				return;
			}
			if (!this.downside.repeats && this.triggeredDownside)
			{
				return;
			}
			if (!this.downside.canOccurWhenImprisoned && (this.Pawn.IsPrisoner || this.Pawn.IsSlave))
			{
				return;
			}
			if (this.downside.triggerMinDays != FloatRange.Zero && GenTicks.TicksGame < this.canTriggerDownsideAfter)
			{
				return;
			}
			if (!this.downside.canOccurWhileDowned && this.Pawn.Downed)
			{
				return;
			}
			if (!this.Pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (this.downside.mustBeConscious && !this.Pawn.health.capacities.CanBeAwake)
			{
				return;
			}
			bool flag = this.downside.triggerMtbDays != 0f && Rand.MTBEventOccurs(this.downside.triggerMtbDays, 60000f, 15000f);
			bool flag2 = this.downsideTriggersAt != 0 && GenTicks.TicksGame >= this.downsideTriggersAt;
			if (flag || flag2)
			{
				this.DoDownside();
			}
		}

		// Token: 0x060087B7 RID: 34743 RVA: 0x00310D2C File Offset: 0x0030EF2C
		public void Notify_ChangedFaction()
		{
			if (this.Pawn.IsColonist)
			{
				this.ClearLord();
				this.joinedTick = GenTicks.TicksGame;
				if (this.downside.triggersAfterDays != FloatRange.Zero)
				{
					this.downsideTriggersAt = GenTicks.TicksGame + (int)(this.downside.triggersAfterDays.RandomInRange * 60000f);
				}
				if (this.downside.triggerMinDays != FloatRange.Zero)
				{
					this.canTriggerDownsideAfter = GenTicks.TicksGame + (int)(this.downside.triggerMinDays.RandomInRange * 60000f);
				}
			}
		}

		// Token: 0x060087B8 RID: 34744 RVA: 0x00310DCE File Offset: 0x0030EFCE
		public void Notify_DuplicatedFrom(Pawn _)
		{
			this.duplicated = true;
		}

		// Token: 0x060087B9 RID: 34745 RVA: 0x00310DD7 File Offset: 0x0030EFD7
		public void Notify_Arrested(bool succeeded)
		{
			if (this.duplicated)
			{
				return;
			}
			if (!succeeded)
			{
				this.DoAggressive();
				return;
			}
			this.ClearLord();
		}

		// Token: 0x060087BA RID: 34746 RVA: 0x00310DF4 File Offset: 0x0030EFF4
		public void Notify_Released()
		{
			if (this.duplicated)
			{
				return;
			}
			this.triggerPrisonerRejectionAt = GenTicks.TicksGame + Pawn_CreepJoinerTracker.ReleaseRejectionDelayTicks.RandomInRange;
		}

		// Token: 0x060087BB RID: 34747 RVA: 0x00310E24 File Offset: 0x0030F024
		public void Notify_PrisonBreakout()
		{
			if (this.duplicated)
			{
				return;
			}
			this.triggerPrisonerAggressiveAt = GenTicks.TicksGame + Pawn_CreepJoinerTracker.ReleaseRejectionDelayTicks.RandomInRange;
		}

		// Token: 0x060087BC RID: 34748 RVA: 0x00310E53 File Offset: 0x0030F053
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_CreepJoinerTracker.<GetGizmos>d__50 <GetGizmos>d__ = new Pawn_CreepJoinerTracker.<GetGizmos>d__50(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x060087BD RID: 34749 RVA: 0x00310E64 File Offset: 0x0030F064
		public string GetInspectString()
		{
			if (!DebugSettings.godMode || this.Disabled)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string text = "DEV Downside: " + this.downside.label;
			if (this.canTriggerDownsideAfter != 0 && !this.triggeredDownside && GenTicks.TicksGame < this.canTriggerDownsideAfter && (this.downside.canOccurWhenImprisoned || !this.Pawn.IsPrisoner))
			{
				text = text + " (can after: " + (this.canTriggerDownsideAfter - GenTicks.TicksGame).ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			else if (this.downsideTriggersAt != 0 && !this.triggeredDownside && (this.downside.canOccurWhenImprisoned || !this.Pawn.IsPrisoner))
			{
				text = text + " (triggers: " + (this.downsideTriggersAt - GenTicks.TicksGame).ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			string str = "DEV Benefit: ";
			CreepJoinerBenefitDef creepJoinerBenefitDef = this.benefit;
			stringBuilder.AppendLine(str + ((creepJoinerBenefitDef != null) ? creepJoinerBenefitDef.label : null));
			stringBuilder.AppendLine(text);
			string str2 = "DEV Rejection: ";
			CreepJoinerRejectionDef creepJoinerRejectionDef = this.rejection;
			stringBuilder.AppendLine(str2 + ((creepJoinerRejectionDef != null) ? creepJoinerRejectionDef.label : null));
			string str3 = "DEV Aggressive: ";
			CreepJoinerAggressiveDef creepJoinerAggressiveDef = this.aggressive;
			stringBuilder.Append(str3 + ((creepJoinerAggressiveDef != null) ? creepJoinerAggressiveDef.label : null));
			return stringBuilder.ToString();
		}

		// Token: 0x060087BE RID: 34750 RVA: 0x00310FC4 File Offset: 0x0030F1C4
		public void Notify_CreepJoinerSpokenTo(Pawn speaker)
		{
			if (this.Disabled)
			{
				return;
			}
			this.speaker = speaker;
			Find.SignalManager.SendSignal(new Signal(this.spokeToSignal, false));
		}

		// Token: 0x060087BF RID: 34751 RVA: 0x00310FEC File Offset: 0x0030F1EC
		public void Notify_CreepJoinerAttacked(Pawn instigatorPawn)
		{
			if (this.Disabled)
			{
				return;
			}
			if (instigatorPawn == null)
			{
				return;
			}
			if (instigatorPawn.CurJobDef == JobDefOf.SocialFight)
			{
				return;
			}
			this.DoAggressive();
		}

		// Token: 0x060087C0 RID: 34752 RVA: 0x0031100F File Offset: 0x0030F20F
		public void Notify_CreepJoinerKilled()
		{
			if (this.Disabled)
			{
				return;
			}
			if (this.AggressiveWorker.CanOccurOnDeath)
			{
				this.DoAggressive();
			}
		}

		// Token: 0x060087C1 RID: 34753 RVA: 0x0031102D File Offset: 0x0030F22D
		public void Notify_CreepJoinerRejected()
		{
			this.DoRejection();
		}

		// Token: 0x060087C2 RID: 34754 RVA: 0x00311038 File Offset: 0x0030F238
		public void DoDownside()
		{
			if (this.Disabled)
			{
				return;
			}
			if (this.DownsideWorker != null && !this.DownsideWorker.CanDoResponse())
			{
				return;
			}
			this.ClearLord();
			this.triggeredDownside = true;
			if (this.downside.triggersAfterDays != FloatRange.Zero)
			{
				this.downsideTriggersAt = GenTicks.TicksGame + (int)(this.downside.triggersAfterDays.RandomInRange * 60000f);
			}
			if (this.downside.triggerMinDays != FloatRange.Zero)
			{
				this.canTriggerDownsideAfter = GenTicks.TicksGame + (int)(this.downside.triggerMinDays.RandomInRange * 60000f);
			}
			List<TargetInfo> list = new List<TargetInfo>
			{
				this.Pawn
			};
			List<NamedArgument> list2 = new List<NamedArgument>
			{
				this.Pawn.Named("PAWN")
			};
			foreach (HediffDef def in this.downside.hediffs)
			{
				Hediff hd;
				HediffComp_ReplaceHediff hediffComp_ReplaceHediff;
				if (this.Pawn.health.hediffSet.TryGetHediff(def, out hd) && hd.TryGetComp(out hediffComp_ReplaceHediff))
				{
					hediffComp_ReplaceHediff.Trigger();
				}
			}
			BaseCreepJoinerWorker downsideWorker = this.DownsideWorker;
			if (downsideWorker != null)
			{
				downsideWorker.DoResponse(list, list2);
			}
			if (this.downside.hasLetter)
			{
				TaggedString label = this.downside.letterLabel.Formatted(list2);
				TaggedString text = this.downside.letterDesc.Formatted(list2);
				Find.LetterStack.ReceiveLetter(label, text, this.downside.letterDef, list, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060087C3 RID: 34755 RVA: 0x003111F4 File Offset: 0x0030F3F4
		public void DoRejection()
		{
			if (this.Disabled || this.triggeredRejection || this.triggeredAggressive || this.hasLeft)
			{
				return;
			}
			if (this.RejectionWorker != null && !this.RejectionWorker.CanDoResponse())
			{
				return;
			}
			this.triggeredRejection = true;
			this.ClearLord();
			List<TargetInfo> list = new List<TargetInfo>
			{
				this.Pawn
			};
			List<NamedArgument> list2 = new List<NamedArgument>
			{
				this.Pawn.Named("PAWN")
			};
			BaseCreepJoinerWorker rejectionWorker = this.RejectionWorker;
			if (rejectionWorker != null)
			{
				rejectionWorker.DoResponse(list, list2);
			}
			if (this.rejection.hasLetter)
			{
				TaggedString label = this.rejection.letterLabel.Formatted(list2);
				TaggedString text = this.rejection.letterDesc.Formatted(list2);
				Find.LetterStack.ReceiveLetter(label, text, this.rejection.letterDef, list, null, null, null, null, 0, true);
			}
		}

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x060087C4 RID: 34756 RVA: 0x003112DF File Offset: 0x0030F4DF
		public bool CanTriggerAggressive
		{
			get
			{
				return !this.Disabled && !this.triggeredAggressive && (this.AggressiveWorker == null || this.AggressiveWorker.CanDoResponse());
			}
		}

		// Token: 0x060087C5 RID: 34757 RVA: 0x00311308 File Offset: 0x0030F508
		public void DoAggressive()
		{
			if (!this.CanTriggerAggressive)
			{
				return;
			}
			this.triggeredAggressive = true;
			this.ClearLord();
			List<TargetInfo> list = new List<TargetInfo>
			{
				this.Pawn
			};
			List<NamedArgument> list2 = new List<NamedArgument>
			{
				this.Pawn.Named("PAWN")
			};
			BaseCreepJoinerWorker aggressiveWorker = this.AggressiveWorker;
			if (aggressiveWorker != null)
			{
				aggressiveWorker.DoResponse(list, list2);
			}
			if (this.aggressive.hasMessage)
			{
				Messages.Message(this.aggressive.message.Formatted(list2), list, MessageTypeDefOf.NegativeEvent, true);
			}
			if (this.aggressive.hasLetter)
			{
				TaggedString label = this.aggressive.letterLabel.Formatted(list2);
				TaggedString text = this.aggressive.letterDesc.Formatted(list2);
				Find.LetterStack.ReceiveLetter(label, text, this.aggressive.letterDef, list, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060087C6 RID: 34758 RVA: 0x003113FC File Offset: 0x0030F5FC
		public void DoLeave()
		{
			if (this.Disabled || this.hasLeft)
			{
				return;
			}
			this.ClearLord();
			if (this.Pawn.Faction != null && this.Pawn.Faction.IsPlayer)
			{
				this.Pawn.SetFaction(null, null);
			}
			LordMaker.MakeNewLord(this.Pawn.Faction, new LordJob_ExitMapBest(LocomotionUrgency.Jog, false, false), this.Pawn.Map, null).AddPawn(this.Pawn);
			this.hasLeft = true;
		}

		// Token: 0x060087C7 RID: 34759 RVA: 0x00311484 File Offset: 0x0030F684
		public bool DoSurgicalInspection(Pawn surgeon, StringBuilder sb)
		{
			bool result = false;
			if (!this.benefit.surgicalInspectionLetterExtra.NullOrEmpty())
			{
				sb.Append("\n\n" + this.benefit.surgicalInspectionLetterExtra.Formatted(this.Pawn.Named("PAWN"), surgeon.Named("SURGEON")));
				result = true;
			}
			if (!this.downside.surgicalInspectionLetterExtra.NullOrEmpty())
			{
				sb.Append("\n\n" + this.downside.surgicalInspectionLetterExtra.Formatted(this.Pawn.Named("PAWN"), surgeon.Named("SURGEON")));
				result = true;
			}
			if (!this.aggressive.surgicalInspectionLetterExtra.NullOrEmpty())
			{
				sb.Append("\n\n" + this.aggressive.surgicalInspectionLetterExtra.Formatted(this.Pawn.Named("PAWN"), surgeon.Named("SURGEON")));
				result = true;
			}
			if (!this.rejection.surgicalInspectionLetterExtra.NullOrEmpty())
			{
				sb.Append("\n\n" + this.rejection.surgicalInspectionLetterExtra.Formatted(this.Pawn.Named("PAWN"), surgeon.Named("SURGEON")));
				result = true;
			}
			return result;
		}

		// Token: 0x060087C8 RID: 34760 RVA: 0x003115E8 File Offset: 0x0030F7E8
		public IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Pawn_CreepJoinerTracker.<GetFloatMenuOptions>d__63 <GetFloatMenuOptions>d__ = new Pawn_CreepJoinerTracker.<GetFloatMenuOptions>d__63(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x060087C9 RID: 34761 RVA: 0x00311600 File Offset: 0x0030F800
		private void ResolveGraphics()
		{
			if (!this.form.forcedHeadTypes.NullOrEmpty<HeadTypeDef>())
			{
				this.Pawn.story.TryGetRandomHeadFromSet(this.form.forcedHeadTypes);
			}
			if (this.form.hairTagFilter != null)
			{
				this.Pawn.story.hairDef = PawnStyleItemChooser.RandomHairFor(this.Pawn);
			}
			if (this.form.beardTagFilter != null)
			{
				this.Pawn.style.beardDef = PawnStyleItemChooser.RandomBeardFor(this.Pawn);
			}
			if (this.form.hairColorOverride != null)
			{
				this.Pawn.story.HairColor = this.form.hairColorOverride.Value;
			}
			this.Pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x060087CA RID: 34762 RVA: 0x003116D4 File Offset: 0x0030F8D4
		private void ClearLord()
		{
			if (this.IsOnEntryLord)
			{
				this.entryLordEnded = true;
			}
			Lord lord = this.Pawn.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_PawnLost(this.Pawn, PawnLostCondition.Undefined, null);
		}

		// Token: 0x060087CB RID: 34763 RVA: 0x00311718 File Offset: 0x0030F918
		private T GetWorker<T>(Type type, ref T worker) where T : BaseCreepJoinerWorker
		{
			if (worker != null)
			{
				return worker;
			}
			if (type == null)
			{
				return default(T);
			}
			worker = (T)((object)Activator.CreateInstance(type));
			worker.Tracker = this;
			return worker;
		}

		// Token: 0x060087CC RID: 34764 RVA: 0x00311770 File Offset: 0x0030F970
		public void ExposeData()
		{
			Scribe_Defs.Look<CreepJoinerFormKindDef>(ref this.form, "form");
			Scribe_Defs.Look<CreepJoinerBenefitDef>(ref this.benefit, "benefit");
			Scribe_Defs.Look<CreepJoinerDownsideDef>(ref this.downside, "downside");
			Scribe_Defs.Look<CreepJoinerRejectionDef>(ref this.rejection, "rejection");
			Scribe_Defs.Look<CreepJoinerAggressiveDef>(ref this.aggressive, "aggressive");
			Scribe_Values.Look<int>(ref this.timeoutAt, "timeoutAt", 0, false);
			Scribe_Values.Look<bool>(ref this.duplicated, "duplicated", false, false);
			Scribe_Values.Look<int>(ref this.joinedTick, "joinedTick", 0, false);
			Scribe_Values.Look<string>(ref this.spokeToSignal, "spokeToSignal", null, false);
			Scribe_Values.Look<bool>(ref this.triggeredDownside, "triggeredDownside", false, false);
			Scribe_Values.Look<bool>(ref this.triggeredAggressive, "triggeredAggressive", false, false);
			Scribe_Values.Look<bool>(ref this.triggeredRejection, "triggeredRejection", false, false);
			Scribe_Values.Look<bool>(ref this.hasLeft, "hasLeft", false, false);
			Scribe_Values.Look<int>(ref this.canTriggerDownsideAfter, "canTriggerDownsideAfter", 0, false);
			Scribe_Values.Look<int>(ref this.downsideTriggersAt, "downsideTriggersAt", 0, false);
			Scribe_Values.Look<bool>(ref this.entryLordEnded, "entryLordEnded", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_References.Look<Pawn>(ref this.speaker, "speaker", false);
		}

		// Token: 0x040053BD RID: 21437
		private static readonly IntRange ReleaseRejectionDelayTicks = new IntRange(60, 180);

		// Token: 0x040053BE RID: 21438
		private const int CheckDownsideInterval = 15000;

		// Token: 0x040053BF RID: 21439
		public CreepJoinerFormKindDef form;

		// Token: 0x040053C0 RID: 21440
		public CreepJoinerBenefitDef benefit;

		// Token: 0x040053C1 RID: 21441
		public CreepJoinerDownsideDef downside;

		// Token: 0x040053C2 RID: 21442
		public CreepJoinerAggressiveDef aggressive;

		// Token: 0x040053C3 RID: 21443
		public CreepJoinerRejectionDef rejection;

		// Token: 0x040053C4 RID: 21444
		public Pawn speaker;

		// Token: 0x040053C5 RID: 21445
		public Quest quest;

		// Token: 0x040053C6 RID: 21446
		public string spokeToSignal;

		// Token: 0x040053C7 RID: 21447
		public int timeoutAt;

		// Token: 0x040053C8 RID: 21448
		public int joinedTick;

		// Token: 0x040053C9 RID: 21449
		private int downsideTriggersAt;

		// Token: 0x040053CA RID: 21450
		private int canTriggerDownsideAfter;

		// Token: 0x040053CB RID: 21451
		private bool triggeredDownside;

		// Token: 0x040053CC RID: 21452
		private bool triggeredAggressive;

		// Token: 0x040053CD RID: 21453
		private bool triggeredRejection;

		// Token: 0x040053CE RID: 21454
		private bool hasLeft;

		// Token: 0x040053CF RID: 21455
		private bool entryLordEnded;

		// Token: 0x040053D0 RID: 21456
		private int triggerPrisonerRejectionAt;

		// Token: 0x040053D1 RID: 21457
		private int triggerPrisonerAggressiveAt;

		// Token: 0x040053D2 RID: 21458
		private bool duplicated;

		// Token: 0x040053D3 RID: 21459
		private BaseCreepJoinerWorker downsideWorkerInt;

		// Token: 0x040053D4 RID: 21460
		private BaseCreepJoinerWorker rejectionWorkerInt;

		// Token: 0x040053D5 RID: 21461
		private BaseCreepJoinerWorker aggressiveWorkerInt;
	}
}
