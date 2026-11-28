using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200182E RID: 6190
	[StaticConstructorOnStartup]
	public class CompBiosculpterPod : ThingComp, ISuspendableThingHolder, IThingHolder, IThingHolderWithDrawnPawn, IStoreSettingsParent, INotifyHauledTo, ISearchableContents
	{
		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x06009ACA RID: 39626 RVA: 0x0037D120 File Offset: 0x0037B320
		public CompProperties_BiosculpterPod Props
		{
			get
			{
				return this.props as CompProperties_BiosculpterPod;
			}
		}

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x06009ACB RID: 39627 RVA: 0x0037D12D File Offset: 0x0037B32D
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x06009ACC RID: 39628 RVA: 0x0037D135 File Offset: 0x0037B335
		public bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x06009ACD RID: 39629 RVA: 0x0037D138 File Offset: 0x0037B338
		public float RequiredNutritionRemaining
		{
			get
			{
				return Mathf.Max(5f - this.liquifiedNutrition, 0f);
			}
		}

		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x06009ACE RID: 39630 RVA: 0x0037D150 File Offset: 0x0037B350
		public bool NutritionLoaded
		{
			get
			{
				return this.RequiredNutritionRemaining <= 0f;
			}
		}

		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x06009ACF RID: 39631 RVA: 0x0037D162 File Offset: 0x0037B362
		public bool AutoAgeReversal
		{
			get
			{
				return this.autoAgeReversal;
			}
		}

		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x06009AD0 RID: 39632 RVA: 0x0037D16A File Offset: 0x0037B36A
		private Texture2D AutoAgeReversalIcon
		{
			get
			{
				if (this.cachedAutoAgeReverseIcon == null)
				{
					this.cachedAutoAgeReverseIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/BiosculpterAutoAgeReversal", true);
				}
				return this.cachedAutoAgeReverseIcon;
			}
		}

		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x06009AD1 RID: 39633 RVA: 0x0037D191 File Offset: 0x0037B391
		public BiosculpterPodState State
		{
			get
			{
				if (this.Occupant != null)
				{
					return BiosculpterPodState.Occupied;
				}
				if (this.NutritionLoaded)
				{
					return BiosculpterPodState.SelectingCycle;
				}
				return BiosculpterPodState.LoadingNutrition;
			}
		}

		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x06009AD2 RID: 39634 RVA: 0x0037D1A8 File Offset: 0x0037B3A8
		public Pawn Occupant
		{
			get
			{
				if (this.pawnEnteringBiosculpter != null)
				{
					return this.pawnEnteringBiosculpter;
				}
				if (this.currentCycleKey == null)
				{
					return null;
				}
				if (this.innerContainer.Count != 1)
				{
					return null;
				}
				return this.innerContainer[0] as Pawn;
			}
		}

		// Token: 0x17001A6B RID: 6763
		// (get) Token: 0x06009AD3 RID: 39635 RVA: 0x0037D1E4 File Offset: 0x0037B3E4
		public CompBiosculpterPod_Cycle CurrentCycle
		{
			get
			{
				if (this.currentCycleKey == null)
				{
					return null;
				}
				foreach (CompBiosculpterPod_Cycle compBiosculpterPod_Cycle in this.AvailableCycles)
				{
					if (compBiosculpterPod_Cycle.Props.key == this.currentCycleKey)
					{
						return compBiosculpterPod_Cycle;
					}
				}
				return null;
			}
		}

		// Token: 0x17001A6C RID: 6764
		// (get) Token: 0x06009AD4 RID: 39636 RVA: 0x0037D25C File Offset: 0x0037B45C
		public List<CompBiosculpterPod_Cycle> AvailableCycles
		{
			get
			{
				if (this.cachedAvailableCycles == null)
				{
					this.SetupCycleCaches();
				}
				return this.cachedAvailableCycles;
			}
		}

		// Token: 0x17001A6D RID: 6765
		// (get) Token: 0x06009AD5 RID: 39637 RVA: 0x0037D272 File Offset: 0x0037B472
		public string AgeReversalCycleKey
		{
			get
			{
				if (CompBiosculpterPod.cachedAgeReversalCycleKey == null)
				{
					this.SetupCycleCaches();
				}
				return CompBiosculpterPod.cachedAgeReversalCycleKey;
			}
		}

		// Token: 0x17001A6E RID: 6766
		// (get) Token: 0x06009AD6 RID: 39638 RVA: 0x0037D286 File Offset: 0x0037B486
		private float CycleSpeedFactorNoPawn
		{
			get
			{
				return this.CleanlinessSpeedFactor * this.BiotunedSpeedFactor;
			}
		}

		// Token: 0x17001A6F RID: 6767
		// (get) Token: 0x06009AD7 RID: 39639 RVA: 0x0037D295 File Offset: 0x0037B495
		public float CycleSpeedFactor
		{
			get
			{
				if (this.Occupant == null)
				{
					return Mathf.Max(0.1f, this.CycleSpeedFactorNoPawn);
				}
				return this.GetCycleSpeedFactorForPawn(this.Occupant);
			}
		}

		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x06009AD8 RID: 39640 RVA: 0x0037D2BC File Offset: 0x0037B4BC
		private float CleanlinessSpeedFactor
		{
			get
			{
				return this.parent.GetStatValue(StatDefOf.BiosculpterPodSpeedFactor, true, -1);
			}
		}

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x06009AD9 RID: 39641 RVA: 0x0037D2D0 File Offset: 0x0037B4D0
		private float BiotunedSpeedFactor
		{
			get
			{
				if (this.biotunedTo == null)
				{
					return 1f;
				}
				return this.Props.biotunedCycleSpeedFactor;
			}
		}

		// Token: 0x17001A72 RID: 6770
		// (get) Token: 0x06009ADA RID: 39642 RVA: 0x0037D2EB File Offset: 0x0037B4EB
		public bool PowerOn
		{
			get
			{
				return this.parent.TryGetComp<CompPowerTrader>().PowerOn;
			}
		}

		// Token: 0x17001A73 RID: 6771
		// (get) Token: 0x06009ADB RID: 39643 RVA: 0x0037D2FD File Offset: 0x0037B4FD
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.parent.DrawPos.y - 0.03658537f;
			}
		}

		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x06009ADC RID: 39644 RVA: 0x0037D318 File Offset: 0x0037B518
		public float HeldPawnBodyAngle
		{
			get
			{
				return this.parent.Rotation.Opposite.AsAngle;
			}
		}

		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x06009ADD RID: 39645 RVA: 0x0037D340 File Offset: 0x0037B540
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x06009ADE RID: 39646 RVA: 0x0037D344 File Offset: 0x0037B544
		public CompBiosculpterPod()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06009ADF RID: 39647 RVA: 0x0037D3B8 File Offset: 0x0037B5B8
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowedNutritionSettings = new StorageSettings(this);
			if (this.parent.def.building.defaultStorageSettings != null)
			{
				this.allowedNutritionSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06009AE0 RID: 39648 RVA: 0x0037D40F File Offset: 0x0037B60F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Biosculpter pod comp"))
			{
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06009AE1 RID: 39649 RVA: 0x0037D428 File Offset: 0x0037B628
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<string>(ref this.currentCycleKey, "currentCycleKey", null, false);
			Scribe_Values.Look<float>(ref this.currentCycleTicksRemaining, "currentCycleTicksRemaining", 0f, false);
			Scribe_Values.Look<int>(ref this.currentCyclePowerCutTicks, "currentCyclePowerCutTicks", 0, false);
			Scribe_References.Look<Pawn>(ref this.biotunedTo, "biotunedTo", false);
			Scribe_Values.Look<int>(ref this.biotunedCountdownTicks, "biotunedCountdownTicks", 0, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedNutritionSettings, "allowedNutritionSettings", Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.liquifiedNutrition, "liquifiedNutrition", 0f, false);
			Scribe_Values.Look<bool>(ref this.autoLoadNutrition, "autoLoadNutrition", false, false);
			Scribe_Values.Look<bool>(ref this.devFillPodLatch, "devFillPodLatch", false, false);
			Scribe_Values.Look<bool>(ref this.autoAgeReversal, "autoAgeReversal", false, false);
			Scribe_Values.Look<int>(ref this.tickEntered, "tickEntered", 0, false);
			Scribe_References.Look<Job>(ref this.queuedEnterJob, "queuedEnterJob", false);
			Scribe_References.Look<Pawn>(ref this.queuedPawn, "queuedPawn", false);
			if (this.allowedNutritionSettings == null)
			{
				this.allowedNutritionSettings = new StorageSettings(this);
				if (this.parent.def.building.defaultStorageSettings != null)
				{
					this.allowedNutritionSettings.CopyFrom(this.parent.def.building.defaultStorageSettings);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.currentCycleKey == "healing")
				{
					this.currentCycleKey = "medic";
				}
				if (this.biotunedTo != null)
				{
					this.SetBiotuned(this.biotunedTo);
				}
				this.LiquifyNutrition();
			}
		}

		// Token: 0x06009AE2 RID: 39650 RVA: 0x0037D5D1 File Offset: 0x0037B7D1
		public CompBiosculpterPod_Cycle GetCycle(string key)
		{
			if (this.cycleLookup == null)
			{
				this.SetupCycleCaches();
			}
			return this.cycleLookup[key];
		}

		// Token: 0x06009AE3 RID: 39651 RVA: 0x0037D5ED File Offset: 0x0037B7ED
		public float GetCycleSpeedFactorForPawn(Pawn p)
		{
			return Mathf.Max(0.1f, this.CycleSpeedFactorNoPawn * p.GetStatValue(StatDefOf.BiosculpterOccupantSpeed, true, -1));
		}

		// Token: 0x06009AE4 RID: 39652 RVA: 0x0037D610 File Offset: 0x0037B810
		private void SetupCycleCaches()
		{
			this.cachedAvailableCycles = new List<CompBiosculpterPod_Cycle>();
			this.cachedAvailableCycles.AddRange(this.parent.AllComps.OfType<CompBiosculpterPod_Cycle>());
			this.cycleLookup = new Dictionary<string, CompBiosculpterPod_Cycle>();
			foreach (CompBiosculpterPod_Cycle compBiosculpterPod_Cycle in this.cachedAvailableCycles)
			{
				CompBiosculpterPod_AgeReversalCycle compBiosculpterPod_AgeReversalCycle = compBiosculpterPod_Cycle as CompBiosculpterPod_AgeReversalCycle;
				if (compBiosculpterPod_AgeReversalCycle != null)
				{
					CompBiosculpterPod.cachedAgeReversalCycleKey = compBiosculpterPod_AgeReversalCycle.Props.key;
				}
				this.cycleLookup[compBiosculpterPod_Cycle.Props.key] = compBiosculpterPod_Cycle;
			}
		}

		// Token: 0x06009AE5 RID: 39653 RVA: 0x0037D6C0 File Offset: 0x0037B8C0
		public void SetBiotuned(Pawn newBiotunedTo)
		{
			if (newBiotunedTo != this.biotunedTo)
			{
				this.autoAgeReversal = false;
			}
			if (this.biotunedTo != null && CompBiosculpterPod.cachedBiotunedPods.ContainsKey(this.biotunedTo))
			{
				CompBiosculpterPod.cachedBiotunedPods[this.biotunedTo].Remove(this);
			}
			if (newBiotunedTo != null && !CompBiosculpterPod.cachedBiotunedPods.ContainsKey(newBiotunedTo))
			{
				CompBiosculpterPod.cachedBiotunedPods[newBiotunedTo] = new List<CompBiosculpterPod>();
			}
			if (newBiotunedTo != null && !CompBiosculpterPod.cachedBiotunedPods[newBiotunedTo].Contains(this))
			{
				CompBiosculpterPod.cachedBiotunedPods[newBiotunedTo].Add(this);
			}
			if (newBiotunedTo != null && newBiotunedTo != this.biotunedTo)
			{
				this.biotunedCountdownTicks = 4800000;
			}
			this.biotunedTo = newBiotunedTo;
		}

		// Token: 0x06009AE6 RID: 39654 RVA: 0x0037D773 File Offset: 0x0037B973
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.SetBiotuned(null);
			if (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize)
			{
				this.EjectContents(true, false, previousMap);
			}
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			base.PostDestroy(mode, previousMap);
		}

		// Token: 0x06009AE7 RID: 39655 RVA: 0x0037D7A4 File Offset: 0x0037B9A4
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.EjectContents(true, false, map);
				this.currentCycleKey = null;
			}
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBarEffecter = null;
			Effecter effecter2 = this.operatingEffecter;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.operatingEffecter = null;
			Effecter effecter3 = this.readyEffecter;
			if (effecter3 != null)
			{
				effecter3.Cleanup();
			}
			this.readyEffecter = null;
		}

		// Token: 0x06009AE8 RID: 39656 RVA: 0x0037D810 File Offset: 0x0037BA10
		public override void DrawGUIOverlay()
		{
			base.DrawGUIOverlay();
			if (Find.ScreenshotModeHandler.Active)
			{
				return;
			}
			if (this.biotunedTo != null || this.Occupant != null)
			{
				Thing parent = this.parent;
				Pawn pawn = this.biotunedTo;
				GenMapUI.DrawThingLabel(parent, ((pawn != null) ? pawn.LabelShort : null) ?? this.Occupant.LabelShort, GenMapUI.DefaultThingLabelColor);
			}
		}

		// Token: 0x06009AE9 RID: 39657 RVA: 0x0037D874 File Offset: 0x0037BA74
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			BiosculpterPodState state = this.State;
			if (this.parent.Spawned)
			{
				CompBiosculpterPod_Cycle currentCycle = this.CurrentCycle;
				if (currentCycle != null)
				{
					stringBuilder.AppendLineIfNotEmpty().Append("BiosculpterPodCycleLabel".Translate()).Append(": ").Append(currentCycle.Props.LabelCap);
					if (this.biotunedTo == null)
					{
						stringBuilder.Append(" " + "BiosculpterPodCycleWillBiotune".Translate());
					}
				}
				else if (state == BiosculpterPodState.SelectingCycle)
				{
					if (this.PowerOn)
					{
						if (this.queuedEnterJob != null && !this.queuedEnterJob.biosculpterCycleKey.NullOrEmpty())
						{
							stringBuilder.Append("BiosculpterPodCycleStandby".Translate(this.GetCycle(this.queuedEnterJob.biosculpterCycleKey).Props.label.Named("CYCLE"), this.queuedPawn.Named("PAWN")));
						}
						else
						{
							stringBuilder.Append("BiosculpterPodCycleSelection".Translate().CapitalizeFirst());
						}
					}
					else
					{
						stringBuilder.Append("BiosculpterPodCycleSelectionNoPower".Translate().CapitalizeFirst());
					}
				}
				if (state == BiosculpterPodState.LoadingNutrition)
				{
					stringBuilder.Append("BiosculpterPodCycleLabelLoading".Translate().CapitalizeFirst());
					stringBuilder.AppendLineIfNotEmpty().Append("Nutrition".Translate()).Append(": ").Append(this.liquifiedNutrition.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Absolute)).Append(" / ").Append(5f);
				}
				if (state == BiosculpterPodState.Occupied)
				{
					float num = this.currentCycleTicksRemaining / this.CycleSpeedFactor;
					stringBuilder.AppendLineIfNotEmpty().Append("Contains".Translate()).Append(": ").Append(this.Occupant.NameShortColored.Resolve());
					if (!this.PowerOn)
					{
						stringBuilder.AppendLine().Append("BiosculpterCycleNoPowerInterrupt".Translate((60000 - this.currentCyclePowerCutTicks).ToStringTicksToPeriod(true, false, true, true, false).Named("TIME")).Colorize(ColorLibrary.RedReadable));
					}
					stringBuilder.AppendLine().Append("BiosculpterCycleTimeRemaining".Translate()).Append(": ").Append(((int)num).ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor));
					Ideo ideo = this.Occupant.Ideo;
					if (ideo != null && ideo.HasPrecept(PreceptDefOf.Biosculpting_Accelerated))
					{
						stringBuilder.Append(" (" + "BiosculpterCycleAccelerated".Translate() + ")");
					}
					if (this.biotunedTo != null)
					{
						stringBuilder.AppendLine().Append("BiosculpterBiotunedSpeedFactor".Translate()).Append(": ").Append(this.BiotunedSpeedFactor.ToStringPercent());
					}
					stringBuilder.AppendLine().Append("BiosculpterCleanlinessSpeedFactor".Translate()).Append(": ").Append(this.CleanlinessSpeedFactor.ToStringPercent());
				}
			}
			if (this.biotunedTo != null && state != BiosculpterPodState.Occupied)
			{
				stringBuilder.AppendLineIfNotEmpty().Append("BiosculpterBiotunedTo".Translate()).Append(": ").Append(this.biotunedTo.LabelShort).Append(" (").Append(this.biotunedCountdownTicks.ToStringTicksToPeriod(true, false, true, true, false)).Append(")");
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06009AEA RID: 39658 RVA: 0x0037DC41 File Offset: 0x0037BE41
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompBiosculpterPod.<CompGetGizmosExtra>d__91 <CompGetGizmosExtra>d__ = new CompBiosculpterPod.<CompGetGizmosExtra>d__91(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009AEB RID: 39659 RVA: 0x0037DC54 File Offset: 0x0037BE54
		private string IngredientsDescription(CompBiosculpterPod_Cycle cycle)
		{
			this.tmpIngredientsStrings.Clear();
			if (!cycle.Props.extraRequiredIngredients.NullOrEmpty<ThingDefCountClass>() && !this.devFillPodLatch)
			{
				for (int i = 0; i < cycle.Props.extraRequiredIngredients.Count; i++)
				{
					this.tmpIngredientsStrings.Add(cycle.Props.extraRequiredIngredients[i].Summary);
				}
			}
			return this.tmpIngredientsStrings.ToCommaList(true, false);
		}

		// Token: 0x06009AEC RID: 39660 RVA: 0x0037DCD0 File Offset: 0x0037BED0
		private string CycleDescription(CompBiosculpterPod_Cycle cycle)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(cycle.Description(this.biotunedTo));
			float num = cycle.Props.durationDays / this.CycleSpeedFactor;
			float num2 = num / PreceptDefOf.Biosculpting_Accelerated.biosculpterPodCycleSpeedFactor;
			stringBuilder.AppendLine("\n\n" + "BiosculpterPodCycleDuration".Translate() + ": " + ((int)(num * 60000f)).ToStringTicksToDays("F1"));
			if (!Find.IdeoManager.classicMode)
			{
				stringBuilder.Append("BiosculpterPodCycleDurationTranshumanists".Translate() + ": " + ((int)(num2 * 60000f)).ToStringTicksToDays("F1"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06009AED RID: 39661 RVA: 0x0037DDA0 File Offset: 0x0037BFA0
		public bool PawnCanUseNow(Pawn pawn, CompBiosculpterPod_Cycle cycle)
		{
			string text;
			if ((text = this.CannotUseNowReason()) == null && (text = this.CannotUseNowPawnReason(pawn)) == null)
			{
				text = (this.CannotUseNowCycleReason(cycle) ?? this.CannotUseNowPawnCycleReason(pawn, cycle, true));
			}
			return text == null;
		}

		// Token: 0x06009AEE RID: 39662 RVA: 0x0037DDCE File Offset: 0x0037BFCE
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompBiosculpterPod.<CompFloatMenuOptions>d__96 <CompFloatMenuOptions>d__ = new CompBiosculpterPod.<CompFloatMenuOptions>d__96(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x06009AEF RID: 39663 RVA: 0x0037DDE5 File Offset: 0x0037BFE5
		public static bool CanAgeReverse(Pawn biosculptee)
		{
			return biosculptee.ageTracker.Adult;
		}

		// Token: 0x06009AF0 RID: 39664 RVA: 0x0037DDF2 File Offset: 0x0037BFF2
		public static List<CompBiosculpterPod> BiotunedPods(Pawn pawn)
		{
			return CompBiosculpterPod.cachedBiotunedPods.TryGetValue(pawn, null);
		}

		// Token: 0x06009AF1 RID: 39665 RVA: 0x0037DE00 File Offset: 0x0037C000
		public static bool HasBiotunedAutoAgeReversePod(Pawn pawn)
		{
			List<CompBiosculpterPod> list = CompBiosculpterPod.cachedBiotunedPods.TryGetValue(pawn, null);
			if (list == null)
			{
				return false;
			}
			using (List<CompBiosculpterPod>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.AutoAgeReversal)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06009AF2 RID: 39666 RVA: 0x0037DE68 File Offset: 0x0037C068
		public static string CannotStartText(CompBiosculpterPod_Cycle cycle, string translatedReason)
		{
			return "BiosculpterCannotStartCycle".Translate(cycle.Props.label) + ": " + translatedReason.CapitalizeFirst();
		}

		// Token: 0x06009AF3 RID: 39667 RVA: 0x0037DEA0 File Offset: 0x0037C0A0
		public string CannotUseNowCycleReason(CompBiosculpterPod_Cycle cycle)
		{
			List<string> list = cycle.MissingResearchLabels();
			if (list.Any<string>())
			{
				return "MissingRequiredResearch".Translate() + " " + list.ToCommaList(false, false);
			}
			return null;
		}

		// Token: 0x06009AF4 RID: 39668 RVA: 0x0037DEE4 File Offset: 0x0037C0E4
		public string CannotUseNowPawnCycleReason(Pawn p, CompBiosculpterPod_Cycle cycle, bool checkIngredients = true)
		{
			return this.CannotUseNowPawnCycleReason(p, p, cycle, checkIngredients);
		}

		// Token: 0x06009AF5 RID: 39669 RVA: 0x0037DEF0 File Offset: 0x0037C0F0
		private bool CanReachOrHasIngredients(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, bool useCache = false)
		{
			return this.PawnCarryingExtraCycleIngredients(biosculptee, cycle, false) || (biosculptee != hauler && this.PawnCarryingExtraCycleIngredients(hauler, cycle, false)) || this.CanReachRequiredIngredients(hauler, cycle, useCache);
		}

		// Token: 0x06009AF6 RID: 39670 RVA: 0x0037DF18 File Offset: 0x0037C118
		public string CannotUseNowPawnCycleReason(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, bool checkIngredients = true)
		{
			if (this.AgeReversalCycleKey != null && cycle.Props.key == this.AgeReversalCycleKey && !CompBiosculpterPod.CanAgeReverse(biosculptee))
			{
				return "UnderMinBiosculpterAgeReversalAge".Translate(biosculptee.ageTracker.AdultMinAge.Named("ADULTAGE")).CapitalizeFirst();
			}
			if (checkIngredients && !this.CanReachOrHasIngredients(hauler, biosculptee, cycle, true))
			{
				return "BiosculpterMissingIngredients".Translate(this.IngredientsDescription(cycle).Named("INGREDIENTS")).CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x06009AF7 RID: 39671 RVA: 0x0037DFBC File Offset: 0x0037C1BC
		public string CannotUseNowPawnReason(Pawn p)
		{
			if (this.biotunedTo != null && this.biotunedTo != p)
			{
				return "BiosculpterBiotunedToAnother".Translate().CapitalizeFirst();
			}
			if (!p.CanReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate().CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x06009AF8 RID: 39672 RVA: 0x0037E024 File Offset: 0x0037C224
		public string CannotUseNowReason()
		{
			if (!this.PowerOn)
			{
				return "NoPower".Translate().CapitalizeFirst();
			}
			if (this.State == BiosculpterPodState.LoadingNutrition)
			{
				return "BiosculpterNutritionNotLoaded".Translate().CapitalizeFirst();
			}
			if (this.State == BiosculpterPodState.Occupied)
			{
				return "BiosculpterOccupied".Translate().CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x06009AF9 RID: 39673 RVA: 0x0037E094 File Offset: 0x0037C294
		private List<IngredientCount> RequiredIngredients(CompBiosculpterPod_Cycle cycle)
		{
			List<ThingDefCountClass> extraRequiredIngredients = cycle.Props.extraRequiredIngredients;
			if (extraRequiredIngredients == null || this.devFillPodLatch)
			{
				return null;
			}
			if (!this.cachedExtraIngredients.ContainsKey(cycle))
			{
				this.cachedExtraIngredients[cycle] = (from tc in extraRequiredIngredients
				select tc.ToIngredientCount()).ToList<IngredientCount>();
			}
			return this.cachedExtraIngredients[cycle];
		}

		// Token: 0x06009AFA RID: 39674 RVA: 0x0037E10C File Offset: 0x0037C30C
		private bool CanReachRequiredIngredients(Pawn pawn, CompBiosculpterPod_Cycle cycle, bool useCache = false)
		{
			this.chosenExtraItems.Clear();
			if (cycle.Props.extraRequiredIngredients == null || this.devFillPodLatch)
			{
				return true;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (useCache && this.cacheReachIngredientsPawn == pawn && this.cacheReachIngredientsCycle == cycle && realtimeSinceStartup < this.cacheReachIngredientsTime + 2f)
			{
				return this.cacheReachIngredientsResult;
			}
			this.cacheReachIngredientsPawn = pawn;
			this.cacheReachIngredientsCycle = cycle;
			this.cacheReachIngredientsTime = realtimeSinceStartup;
			this.cacheReachIngredientsResult = WorkGiver_DoBill.TryFindBestFixedIngredients(this.RequiredIngredients(cycle), pawn, this.parent, this.chosenExtraItems, 999f);
			return this.cacheReachIngredientsResult;
		}

		// Token: 0x06009AFB RID: 39675 RVA: 0x0037E1AC File Offset: 0x0037C3AC
		private bool SelectPawnCycleOption(Pawn pawn, CompBiosculpterPod_Cycle cycle, out FloatMenuOption option)
		{
			string text = this.CannotUseNowPawnReason(pawn) ?? this.CannotUseNowPawnCycleReason(pawn, cycle, false);
			string label = pawn.Label + ((text == null) ? "" : (": " + text));
			Action action = null;
			if (text == null)
			{
				action = delegate()
				{
					this.PrepareCycleJob(pawn, pawn, cycle, this.EnterBiosculpterJob());
				};
			}
			option = new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			return text == null;
		}

		// Token: 0x06009AFC RID: 39676 RVA: 0x0037E24C File Offset: 0x0037C44C
		private bool SelectPawnsForCycleOptions(CompBiosculpterPod_Cycle cycle, out List<FloatMenuOption> options, bool shortCircuit = false)
		{
			this.cycleEligiblePawnOptions.Clear();
			options = this.cycleEligiblePawnOptions;
			if (!this.cachedAnyPawnEligible.ContainsKey(cycle))
			{
				this.cachedAnyPawnEligible[cycle] = new CacheAnyPawnEligibleCycle
				{
					gameTime = float.MinValue
				};
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (shortCircuit && (float)ticksGame < this.cachedAnyPawnEligible[cycle].gameTime + 2f)
			{
				return this.cachedAnyPawnEligible[cycle].anyEligible;
			}
			this.cachedAnyPawnEligible[cycle].gameTime = (float)ticksGame;
			if (this.biotunedTo != null)
			{
				if (this.biotunedTo.Dead || !this.biotunedTo.Spawned || this.biotunedTo.Map != this.parent.Map)
				{
					this.cachedAnyPawnEligible[cycle].anyEligible = false;
					return this.cachedAnyPawnEligible[cycle].anyEligible;
				}
				FloatMenuOption item;
				if (this.SelectPawnCycleOption(this.biotunedTo, cycle, out item) && shortCircuit)
				{
					this.cachedAnyPawnEligible[cycle].anyEligible = true;
					return this.cachedAnyPawnEligible[cycle].anyEligible;
				}
				this.cycleEligiblePawnOptions.Add(item);
			}
			else
			{
				foreach (Pawn pawn in this.parent.Map.mapPawns.FreeColonistsSpawned)
				{
					if (!pawn.IsQuestLodger())
					{
						FloatMenuOption item2;
						if (this.SelectPawnCycleOption(pawn, cycle, out item2) && shortCircuit)
						{
							this.cachedAnyPawnEligible[cycle].anyEligible = true;
							return this.cachedAnyPawnEligible[cycle].anyEligible;
						}
						this.cycleEligiblePawnOptions.Add(item2);
					}
				}
			}
			this.cachedAnyPawnEligible[cycle].anyEligible = (this.cycleEligiblePawnOptions.Count > 0);
			return this.cachedAnyPawnEligible[cycle].anyEligible;
		}

		// Token: 0x06009AFD RID: 39677 RVA: 0x0037E458 File Offset: 0x0037C658
		public Job EnterBiosculpterJob()
		{
			return JobMaker.MakeJob(JobDefOf.EnterBiosculpterPod, this.parent);
		}

		// Token: 0x06009AFE RID: 39678 RVA: 0x0037E46F File Offset: 0x0037C66F
		public Job MakeCarryToBiosculpterJob(Pawn willBeCarried)
		{
			return JobMaker.MakeJob(JobDefOf.CarryToBiosculpterPod, willBeCarried, LocalTargetInfo.Invalid, this.parent);
		}

		// Token: 0x06009AFF RID: 39679 RVA: 0x0037E494 File Offset: 0x0037C694
		public void ConfigureJobForCycle(Job job, CompBiosculpterPod_Cycle cycle, List<ThingCount> extraIngredients)
		{
			if (!extraIngredients.NullOrEmpty<ThingCount>())
			{
				job.targetQueueB = new List<LocalTargetInfo>(extraIngredients.Count);
				job.countQueue = new List<int>(extraIngredients.Count);
				foreach (ThingCount thingCount in extraIngredients)
				{
					job.targetQueueB.Add(thingCount.Thing);
					job.countQueue.Add(thingCount.Count);
				}
			}
			job.haulMode = HaulMode.ToCellNonStorage;
			job.biosculpterCycleKey = cycle.Props.key;
		}

		// Token: 0x06009B00 RID: 39680 RVA: 0x0037E548 File Offset: 0x0037C748
		public void PrepareCycleJob(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, Job job)
		{
			CompBiosculpterPod.OrderToPod(cycle, biosculptee, delegate
			{
				this.chosenExtraItems.Clear();
				if (!this.CanReachOrHasIngredients(hauler, biosculptee, cycle, false))
				{
					Messages.Message("BiosculpterMissingIngredients".Translate(this.IngredientsDescription(cycle).Named("INGREDIENTS")).CapitalizeFirst(), this.parent, MessageTypeDefOf.NegativeEvent, false);
					return;
				}
				this.ConfigureJobForCycle(job, cycle, this.chosenExtraItems);
				if (cycle.Props.extraRequiredIngredients != null && !this.devFillPodLatch)
				{
					if (job.def == JobDefOf.CarryToBiosculpterPod)
					{
						Messages.Message("BiosculpterCarryStartedMessage".Translate(hauler.Named("PAWN"), this.IngredientsDescription(cycle).Named("INGREDIENTS"), biosculptee.Named("DOWNED"), cycle.Props.label.Named("CYCLE")), this.parent, MessageTypeDefOf.SilentInput, false);
					}
					else
					{
						Messages.Message("BiosculpterLoadingStartedMessage".Translate(hauler.Named("PAWN"), this.IngredientsDescription(cycle).Named("INGREDIENTS"), cycle.Props.label.Named("CYCLE")), this.parent, MessageTypeDefOf.SilentInput, false);
					}
				}
				if (hauler.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false))
				{
					this.SetQueuedInformation(job, biosculptee);
				}
			});
		}

		// Token: 0x06009B01 RID: 39681 RVA: 0x0037E59C File Offset: 0x0037C79C
		public void ClearQueuedInformation()
		{
			this.SetQueuedInformation(null, null);
		}

		// Token: 0x06009B02 RID: 39682 RVA: 0x0037E5A6 File Offset: 0x0037C7A6
		public void SetQueuedInformation(Job job, Pawn biosculptee)
		{
			this.queuedEnterJob = job;
			this.queuedPawn = biosculptee;
		}

		// Token: 0x06009B03 RID: 39683 RVA: 0x0037E5B6 File Offset: 0x0037C7B6
		public bool CanAcceptNutrition(Thing thing)
		{
			return this.allowedNutritionSettings.AllowedToAccept(thing);
		}

		// Token: 0x06009B04 RID: 39684 RVA: 0x0037E5C4 File Offset: 0x0037C7C4
		public bool CanAcceptOnceCycleChosen(Pawn pawn)
		{
			return this.State == BiosculpterPodState.SelectingCycle && this.PowerOn && (this.biotunedTo == null || this.biotunedTo == pawn);
		}

		// Token: 0x06009B05 RID: 39685 RVA: 0x0037E5ED File Offset: 0x0037C7ED
		public bool PawnCarryingExtraCycleIngredients(Pawn pawn, string cycleKey, bool remove = false)
		{
			return this.PawnCarryingExtraCycleIngredients(pawn, this.GetCycle(cycleKey), remove);
		}

		// Token: 0x06009B06 RID: 39686 RVA: 0x0037E600 File Offset: 0x0037C800
		public bool PawnCarryingExtraCycleIngredients(Pawn pawn, CompBiosculpterPod_Cycle cycle, bool remove = false)
		{
			if (cycle.Props.extraRequiredIngredients.NullOrEmpty<ThingDefCountClass>() || this.devFillPodLatch)
			{
				return true;
			}
			foreach (ThingDefCountClass thingDefCountClass in cycle.Props.extraRequiredIngredients)
			{
				if (pawn.inventory.Count(thingDefCountClass.thingDef) < thingDefCountClass.count)
				{
					return false;
				}
			}
			if (remove)
			{
				foreach (ThingDefCountClass thingDefCountClass2 in cycle.Props.extraRequiredIngredients)
				{
					pawn.inventory.RemoveCount(thingDefCountClass2.thingDef, thingDefCountClass2.count, true);
				}
			}
			return true;
		}

		// Token: 0x06009B07 RID: 39687 RVA: 0x0037E6E8 File Offset: 0x0037C8E8
		public bool TryAcceptPawn(Pawn pawn, string cycleKey)
		{
			return this.TryAcceptPawn(pawn, this.GetCycle(cycleKey));
		}

		// Token: 0x06009B08 RID: 39688 RVA: 0x0037E6F8 File Offset: 0x0037C8F8
		public bool TryAcceptPawn(Pawn pawn, CompBiosculpterPod_Cycle cycle)
		{
			if (!this.CanAcceptOnceCycleChosen(pawn))
			{
				return false;
			}
			if (!this.PawnCarryingExtraCycleIngredients(pawn, cycle, true))
			{
				return false;
			}
			this.currentCycleKey = cycle.Props.key;
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			this.pawnEnteringBiosculpter = pawn;
			bool flag = pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
			if (pawn.holdingOwner != null)
			{
				pawn.holdingOwner.TryTransferToContainer(pawn, this.innerContainer, true);
			}
			else
			{
				this.innerContainer.TryAdd(pawn, true);
			}
			if (flag)
			{
				Find.Selector.Select(pawn, false, false);
			}
			this.pawnEnteringBiosculpter = null;
			this.currentCycleTicksRemaining = cycle.Props.durationDays * 60000f;
			this.liquifiedNutrition = 0f;
			this.devFillPodLatch = false;
			this.ClearQueuedInformation();
			this.tickEntered = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x06009B09 RID: 39689 RVA: 0x0037E7CC File Offset: 0x0037C9CC
		public void EjectContents(bool interrupted, bool playSounds, Map destMap = null)
		{
			if (destMap == null)
			{
				destMap = this.parent.Map;
			}
			Pawn occupant = this.Occupant;
			this.currentCycleKey = null;
			this.currentCycleTicksRemaining = 0f;
			this.currentCyclePowerCutTicks = 0;
			this.liquifiedNutrition = 0f;
			this.devFillPodLatch = false;
			this.innerContainer.TryDropAll(this.parent.InteractionCell, destMap, ThingPlaceMode.Near, null, null, true);
			if (occupant != null)
			{
				FilthMaker.TryMakeFilth(this.parent.InteractionCell, destMap, ThingDefOf.Filth_PodSlime, new IntRange(3, 6).RandomInRange, FilthSourceFlags.None, true);
				if (interrupted)
				{
					Pawn_NeedsTracker needs = occupant.needs;
					if (needs != null)
					{
						needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoakingWet, null, null);
					}
					Pawn_HealthTracker health = occupant.health;
					if (health != null)
					{
						health.AddHediff(HediffDefOf.BiosculptingSickness, null, null, null);
					}
				}
			}
			if (playSounds)
			{
				SoundDef exitSound = this.Props.exitSound;
				if (exitSound == null)
				{
					return;
				}
				exitSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(this.parent.Position, this.parent.Map, false), MaintenanceType.None));
			}
		}

		// Token: 0x06009B0A RID: 39690 RVA: 0x0037E8E8 File Offset: 0x0037CAE8
		private void CycleCompleted()
		{
			Pawn occupant = this.Occupant;
			CompBiosculpterPod_Cycle currentCycle = this.CurrentCycle;
			this.SetBiotuned(occupant);
			currentCycle.CycleCompleted(occupant);
			this.EjectContents(false, true, null);
			if (occupant != null)
			{
				Pawn_NeedsTracker needs = occupant.needs;
				Need_Food need_Food = (needs != null) ? needs.food : null;
				if (need_Food != null)
				{
					need_Food.CurLevelPercentage = 1f;
				}
				Pawn_NeedsTracker needs2 = occupant.needs;
				Need_Rest need_Rest = (needs2 != null) ? needs2.rest : null;
				if (need_Rest != null)
				{
					need_Rest.CurLevelPercentage = 1f;
				}
				if (currentCycle.Props.gainThoughtOnCompletion != null)
				{
					Pawn_NeedsTracker needs3 = occupant.needs;
					if (needs3 != null)
					{
						Need_Mood mood = needs3.mood;
						if (mood != null)
						{
							mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AgeReversalReceived, null, null);
						}
					}
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedBiosculpterPod, occupant.Named(HistoryEventArgsNames.Doer)), true);
			}
			if (this.tickEntered > 0)
			{
				occupant.drugs.Notify_LeftSuspension(Find.TickManager.TicksGame - this.tickEntered);
			}
		}

		// Token: 0x06009B0B RID: 39691 RVA: 0x0037E9E4 File Offset: 0x0037CBE4
		private void LiquifyNutrition()
		{
			CompBiosculpterPod.tmpItems.AddRange(this.innerContainer);
			foreach (Thing thing in CompBiosculpterPod.tmpItems)
			{
				float num = thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)thing.stackCount;
				if (num > 0f && !(thing is Pawn))
				{
					this.liquifiedNutrition = Mathf.Min(5f, this.liquifiedNutrition + num);
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			CompBiosculpterPod.tmpItems.Clear();
		}

		// Token: 0x06009B0C RID: 39692 RVA: 0x0037EA90 File Offset: 0x0037CC90
		public override void CompTick()
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
			{
				return;
			}
			base.CompTick();
			if (this.State != BiosculpterPodState.SelectingCycle || !this.PowerOn)
			{
				Effecter effecter = this.readyEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.readyEffecter = null;
			}
			else if (this.Props.readyEffecter != null)
			{
				if (this.readyEffecter == null)
				{
					this.readyEffecter = this.Props.readyEffecter.Spawn();
					this.ColorizeEffecter(this.readyEffecter, this.Props.selectCycleColor);
					this.readyEffecter.Trigger(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false), -1);
				}
				this.readyEffecter.EffectTick(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
			}
			if (this.State != BiosculpterPodState.Occupied)
			{
				Effecter effecter2 = this.progressBarEffecter;
				if (effecter2 != null)
				{
					effecter2.Cleanup();
				}
				this.progressBarEffecter = null;
				Effecter effecter3 = this.operatingEffecter;
				if (effecter3 != null)
				{
					effecter3.Cleanup();
				}
				this.operatingEffecter = null;
			}
			else
			{
				Pawn occupant = this.Occupant;
				this.biotunedCountdownTicks = 4800000;
				if (this.PowerOn)
				{
					int num = 1;
					this.currentCycleTicksRemaining -= (float)num * this.CycleSpeedFactor;
					if (this.currentCycleTicksRemaining <= 0f)
					{
						this.CycleCompleted();
					}
				}
				else
				{
					this.currentCyclePowerCutTicks++;
					if (this.currentCyclePowerCutTicks >= 60000)
					{
						this.EjectContents(true, true, null);
						Messages.Message("BiosculpterNoPowerEjectedMessage".Translate(occupant.Named("PAWN")), occupant, MessageTypeDefOf.NegativeEvent, false);
					}
				}
				if (this.currentCycleTicksRemaining > 0f)
				{
					if (this.progressBarEffecter == null)
					{
						this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
					}
					this.progressBarEffecter.EffectTick(this.parent, TargetInfo.Invalid);
					SubEffecter_ProgressBar subEffecter_ProgressBar = this.progressBarEffecter.children[0] as SubEffecter_ProgressBar;
					MoteProgressBar moteProgressBar = (subEffecter_ProgressBar != null) ? subEffecter_ProgressBar.mote : null;
					if (moteProgressBar != null)
					{
						float num2 = this.CurrentCycle.Props.durationDays * 60000f;
						moteProgressBar.progress = 1f - Mathf.Clamp01(this.currentCycleTicksRemaining / num2);
						int num3 = (this.parent.RotatedSize.z - 1) / 2;
						moteProgressBar.offsetZ = -((float)num3 + 0.5f);
					}
					if (this.Props.operatingEffecter != null)
					{
						if (!this.PowerOn)
						{
							Effecter effecter4 = this.operatingEffecter;
							if (effecter4 != null)
							{
								effecter4.Cleanup();
							}
							this.operatingEffecter = null;
						}
						else
						{
							if (this.operatingEffecter == null)
							{
								this.operatingEffecter = this.Props.operatingEffecter.Spawn();
								this.ColorizeEffecter(this.operatingEffecter, this.CurrentCycle.Props.operatingColor);
								this.operatingEffecter.Trigger(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false), -1);
							}
							this.operatingEffecter.EffectTick(this.parent, new TargetInfo(this.parent.InteractionCell, this.parent.Map, false));
						}
					}
				}
			}
			if (this.PowerOn && this.biotunedCountdownTicks > 0)
			{
				this.biotunedCountdownTicks--;
			}
			if (this.biotunedCountdownTicks <= 0)
			{
				this.SetBiotuned(null);
			}
			this.SetPower();
			Pawn pawn = this.biotunedTo;
			if (((pawn != null) ? pawn.Ideo : null) != null && !this.biotunedTo.Ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded))
			{
				this.autoAgeReversal = false;
			}
		}

		// Token: 0x06009B0D RID: 39693 RVA: 0x0037EE58 File Offset: 0x0037D058
		private void SetPower()
		{
			if (this.powerTraderComp == null)
			{
				this.powerTraderComp = this.parent.TryGetComp<CompPowerTrader>();
			}
			if (this.powerComp == null)
			{
				this.powerComp = this.parent.TryGetComp<CompPower>();
			}
			if (this.State == BiosculpterPodState.Occupied)
			{
				this.powerTraderComp.PowerOutput = -this.powerComp.Props.PowerConsumption;
				return;
			}
			this.powerTraderComp.PowerOutput = -this.powerComp.Props.idlePowerDraw;
		}

		// Token: 0x06009B0E RID: 39694 RVA: 0x0037EEDC File Offset: 0x0037D0DC
		private void ColorizeEffecter(Effecter effecter, Color color)
		{
			foreach (SubEffecter subEffecter in effecter.children)
			{
				SubEffecter_Sprayer subEffecter_Sprayer = subEffecter as SubEffecter_Sprayer;
				if (subEffecter_Sprayer != null)
				{
					subEffecter_Sprayer.colorOverride = new Color?(color * subEffecter.def.color);
				}
			}
		}

		// Token: 0x06009B0F RID: 39695 RVA: 0x0037EF50 File Offset: 0x0037D150
		public override void PostDraw()
		{
			base.PostDraw();
			Rot4 rotation = this.parent.Rotation;
			Vector3 s = new Vector3(this.parent.def.graphicData.drawSize.x * 0.8f, 1f, this.parent.def.graphicData.drawSize.y * 0.8f);
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y -= 0.07317074f;
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(drawPos, rotation.AsQuat, s), CompBiosculpterPod.BackgroundMat, 0);
			if (this.State == BiosculpterPodState.Occupied)
			{
				Pawn occupant = this.Occupant;
				Vector3 drawLoc = this.parent.DrawPos + CompBiosculpterPod.FloatingOffset(this.currentCycleTicksRemaining + (float)this.currentCyclePowerCutTicks);
				Rot4 rotation2 = this.parent.Rotation;
				if (rotation2 == Rot4.East || rotation2 == Rot4.West)
				{
					drawLoc.z += 0.2f;
				}
				occupant.Drawer.renderer.RenderPawnAt(drawLoc, null, true);
			}
		}

		// Token: 0x06009B10 RID: 39696 RVA: 0x0037F07C File Offset: 0x0037D27C
		public static Vector3 FloatingOffset(float tickOffset)
		{
			float num = tickOffset % 500f / 500f;
			float num2 = Mathf.Sin(3.1415927f * num);
			float z = num2 * num2 * 0.04f;
			return new Vector3(0f, 0f, z);
		}

		// Token: 0x06009B11 RID: 39697 RVA: 0x0037F0BD File Offset: 0x0037D2BD
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06009B12 RID: 39698 RVA: 0x0037F0CB File Offset: 0x0037D2CB
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06009B13 RID: 39699 RVA: 0x0037F0D3 File Offset: 0x0037D2D3
		public StorageSettings GetStoreSettings()
		{
			return this.allowedNutritionSettings;
		}

		// Token: 0x06009B14 RID: 39700 RVA: 0x0037F0DB File Offset: 0x0037D2DB
		public StorageSettings GetParentStoreSettings()
		{
			return this.parent.def.building.fixedStorageSettings;
		}

		// Token: 0x06009B15 RID: 39701 RVA: 0x0037F0F2 File Offset: 0x0037D2F2
		public void Notify_SettingsChanged()
		{
		}

		// Token: 0x06009B16 RID: 39702 RVA: 0x0037F0F4 File Offset: 0x0037D2F4
		private static void OrderToPod(CompBiosculpterPod_Cycle cycle, Pawn pawn, Action giveJobAct)
		{
			CompBiosculpterPod_HealingCycle compBiosculpterPod_HealingCycle = cycle as CompBiosculpterPod_HealingCycle;
			if (compBiosculpterPod_HealingCycle != null)
			{
				string healingDescriptionForPawn = compBiosculpterPod_HealingCycle.GetHealingDescriptionForPawn(pawn);
				string str = healingDescriptionForPawn.NullOrEmpty() ? "BiosculpterNoCoditionsToHeal".Translate(pawn.Named("PAWN"), compBiosculpterPod_HealingCycle.Props.label.Named("CYCLE")).Resolve() : ("OnCompletionOfCycle".Translate(compBiosculpterPod_HealingCycle.Props.label.Named("CYCLE")).Resolve() + ":\n\n" + healingDescriptionForPawn);
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str, giveJobAct, healingDescriptionForPawn.NullOrEmpty(), null, WindowLayer.Dialog));
				return;
			}
			giveJobAct();
		}

		// Token: 0x17001A76 RID: 6774
		// (get) Token: 0x06009B17 RID: 39703 RVA: 0x0037F1AA File Offset: 0x0037D3AA
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009B18 RID: 39704 RVA: 0x0037F1B0 File Offset: 0x0037D3B0
		public static Thing FindPodFor(Pawn pawn, Pawn traveller, bool biotuned)
		{
			CompBiosculpterPod.<>c__DisplayClass143_0 CS$<>8__locals1 = new CompBiosculpterPod.<>c__DisplayClass143_0();
			CS$<>8__locals1.biotuned = biotuned;
			CS$<>8__locals1.traveller = traveller;
			if (CompBiosculpterPod.cachedPodDefs.NullOrEmpty<ThingDef>())
			{
				CompBiosculpterPod.cachedPodDefs = (from def in DefDatabase<ThingDef>.AllDefs
				where def.GetCompProperties<CompProperties_BiosculpterPod>() != null
				select def).ToList<ThingDef>();
			}
			foreach (ThingDef singleDef in CompBiosculpterPod.cachedPodDefs)
			{
				Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.traveller.Position, pawn.Map, ThingRequest.ForDef(singleDef), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<FindPodFor>g__Validator|1), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06009B19 RID: 39705 RVA: 0x0037F2A0 File Offset: 0x0037D4A0
		public static bool WasLoadingCanceled(Thing thing)
		{
			CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
			return compBiosculpterPod != null && compBiosculpterPod.State != BiosculpterPodState.LoadingNutrition;
		}

		// Token: 0x06009B1A RID: 39706 RVA: 0x0037F2C2 File Offset: 0x0037D4C2
		public void ClearCycle()
		{
			this.currentCycleKey = null;
		}

		// Token: 0x06009B1B RID: 39707 RVA: 0x0037F2CB File Offset: 0x0037D4CB
		public void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			this.LiquifyNutrition();
			SoundDefOf.Standard_Drop.PlayOneShot(this.parent);
		}

		// Token: 0x04005C14 RID: 23572
		private const int NoPowerEjectCumulativeTicks = 60000;

		// Token: 0x04005C15 RID: 23573
		private const int BiotunedDuration = 4800000;

		// Token: 0x04005C16 RID: 23574
		private const float NutritionRequired = 5f;

		// Token: 0x04005C17 RID: 23575
		private const float CacheForSecs = 2f;

		// Token: 0x04005C18 RID: 23576
		private static readonly Texture2D InterruptCycleIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04005C19 RID: 23577
		private static readonly Material BackgroundMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.082f, 0.078f, 0.063f), ShaderDatabase.SolidColorBehind);

		// Token: 0x04005C1A RID: 23578
		private const float BackgroundRect_YOff = 0.07317074f;

		// Token: 0x04005C1B RID: 23579
		private const float Pawn_YOff = 0.03658537f;

		// Token: 0x04005C1C RID: 23580
		private string currentCycleKey;

		// Token: 0x04005C1D RID: 23581
		private float currentCycleTicksRemaining;

		// Token: 0x04005C1E RID: 23582
		private int currentCyclePowerCutTicks;

		// Token: 0x04005C1F RID: 23583
		private ThingOwner innerContainer;

		// Token: 0x04005C20 RID: 23584
		private Pawn biotunedTo;

		// Token: 0x04005C21 RID: 23585
		private int biotunedCountdownTicks;

		// Token: 0x04005C22 RID: 23586
		private StorageSettings allowedNutritionSettings;

		// Token: 0x04005C23 RID: 23587
		private float liquifiedNutrition;

		// Token: 0x04005C24 RID: 23588
		public bool autoLoadNutrition = true;

		// Token: 0x04005C25 RID: 23589
		public bool devFillPodLatch;

		// Token: 0x04005C26 RID: 23590
		private bool autoAgeReversal;

		// Token: 0x04005C27 RID: 23591
		private int tickEntered = -99999;

		// Token: 0x04005C28 RID: 23592
		public Job queuedEnterJob;

		// Token: 0x04005C29 RID: 23593
		public Pawn queuedPawn;

		// Token: 0x04005C2A RID: 23594
		private List<ThingCount> chosenExtraItems = new List<ThingCount>();

		// Token: 0x04005C2B RID: 23595
		private List<FloatMenuOption> cycleEligiblePawnOptions = new List<FloatMenuOption>();

		// Token: 0x04005C2C RID: 23596
		private Pawn pawnEnteringBiosculpter;

		// Token: 0x04005C2D RID: 23597
		private Dictionary<CompBiosculpterPod_Cycle, List<IngredientCount>> cachedExtraIngredients = new Dictionary<CompBiosculpterPod_Cycle, List<IngredientCount>>();

		// Token: 0x04005C2E RID: 23598
		private Dictionary<CompBiosculpterPod_Cycle, CacheAnyPawnEligibleCycle> cachedAnyPawnEligible = new Dictionary<CompBiosculpterPod_Cycle, CacheAnyPawnEligibleCycle>();

		// Token: 0x04005C2F RID: 23599
		private static Dictionary<Pawn, List<CompBiosculpterPod>> cachedBiotunedPods = new Dictionary<Pawn, List<CompBiosculpterPod>>();

		// Token: 0x04005C30 RID: 23600
		private Pawn cacheReachIngredientsPawn;

		// Token: 0x04005C31 RID: 23601
		private CompBiosculpterPod_Cycle cacheReachIngredientsCycle;

		// Token: 0x04005C32 RID: 23602
		private float cacheReachIngredientsTime = float.MinValue;

		// Token: 0x04005C33 RID: 23603
		private bool cacheReachIngredientsResult;

		// Token: 0x04005C34 RID: 23604
		private Effecter progressBarEffecter;

		// Token: 0x04005C35 RID: 23605
		private Effecter operatingEffecter;

		// Token: 0x04005C36 RID: 23606
		private Effecter readyEffecter;

		// Token: 0x04005C37 RID: 23607
		private Texture2D cachedAutoAgeReverseIcon;

		// Token: 0x04005C38 RID: 23608
		private List<CompBiosculpterPod_Cycle> cachedAvailableCycles;

		// Token: 0x04005C39 RID: 23609
		private Dictionary<string, CompBiosculpterPod_Cycle> cycleLookup;

		// Token: 0x04005C3A RID: 23610
		private static string cachedAgeReversalCycleKey = null;

		// Token: 0x04005C3B RID: 23611
		private List<string> tmpIngredientsStrings = new List<string>();

		// Token: 0x04005C3C RID: 23612
		private static readonly List<Thing> tmpItems = new List<Thing>();

		// Token: 0x04005C3D RID: 23613
		private CompPowerTrader powerTraderComp;

		// Token: 0x04005C3E RID: 23614
		private CompPower powerComp;

		// Token: 0x04005C3F RID: 23615
		private static List<ThingDef> cachedPodDefs;
	}
}
