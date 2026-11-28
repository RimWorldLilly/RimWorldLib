using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200054D RID: 1357
	public class Corpse : ThingWithComps, IThingHolder, IThingHolderTickable, IStrippable, IBillGiver, IObservedThoughtGiver
	{
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x000F87AE File Offset: 0x000F69AE
		// (set) Token: 0x06002830 RID: 10288 RVA: 0x000F87CC File Offset: 0x000F69CC
		public Pawn InnerPawn
		{
			get
			{
				if (this.innerContainer.Count <= 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
			set
			{
				if (value == null)
				{
					this.innerContainer.Clear();
					return;
				}
				if (this.innerContainer.Count > 0)
				{
					Log.Error("Setting InnerPawn in corpse that already has one.");
					this.innerContainer.Clear();
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x000F8819 File Offset: 0x000F6A19
		// (set) Token: 0x06002832 RID: 10290 RVA: 0x000F882C File Offset: 0x000F6A2C
		public int Age
		{
			get
			{
				return Find.TickManager.TicksGame - this.timeOfDeath;
			}
			set
			{
				this.timeOfDeath = Find.TickManager.TicksGame - value;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002833 RID: 10291 RVA: 0x000F8840 File Offset: 0x000F6A40
		public override string LabelNoCount
		{
			get
			{
				if (this.cachedLabel == null)
				{
					if (this.Bugged)
					{
						Log.Error("LabelNoCount on Corpse while Bugged.");
						return string.Empty;
					}
					this.cachedLabel = "DeadLabel".Translate(this.InnerPawn.Label, this.InnerPawn);
				}
				return this.cachedLabel;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002834 RID: 10292 RVA: 0x000F88A3 File Offset: 0x000F6AA3
		public override bool IngestibleNow
		{
			get
			{
				if (this.Bugged)
				{
					Log.Error("IngestibleNow on Corpse while Bugged.");
					return false;
				}
				return base.IngestibleNow && this.InnerPawn.RaceProps.IsFlesh && this.GetRotStage() == RotStage.Fresh;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002835 RID: 10293 RVA: 0x000F88E4 File Offset: 0x000F6AE4
		public RotDrawMode CurRotDrawMode
		{
			get
			{
				CompRottable comp = base.GetComp<CompRottable>();
				if (comp != null)
				{
					switch (comp.Stage)
					{
					case RotStage.Rotting:
						return RotDrawMode.Rotting;
					case RotStage.Dessicated:
						return RotDrawMode.Dessicated;
					}
					return RotDrawMode.Fresh;
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002836 RID: 10294 RVA: 0x000F8920 File Offset: 0x000F6B20
		private bool ShouldVanish
		{
			get
			{
				return this.InnerPawn.RaceProps.Animal && this.vanishAfterTimestamp > 0 && this.Age >= this.vanishAfterTimestamp && base.Spawned && this.GetRoom(RegionType.Set_All) != null && this.GetRoom(RegionType.Set_All).TouchesMapEdge && !base.Map.roofGrid.Roofed(base.Position);
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002837 RID: 10295 RVA: 0x000F8992 File Offset: 0x000F6B92
		public BillStack BillStack
		{
			get
			{
				return this.operationsBillStack;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002838 RID: 10296 RVA: 0x000F899A File Offset: 0x000F6B9A
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				Corpse.<get_IngredientStackCells>d__25 <get_IngredientStackCells>d__ = new Corpse.<get_IngredientStackCells>d__25(-2);
				<get_IngredientStackCells>d__.<>4__this = this;
				return <get_IngredientStackCells>d__;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002839 RID: 10297 RVA: 0x000F89AA File Offset: 0x000F6BAA
		public bool ShouldTickContents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x0600283A RID: 10298 RVA: 0x000F89AD File Offset: 0x000F6BAD
		public bool Bugged
		{
			get
			{
				if (this.innerContainer.Count != 0)
				{
					Pawn pawn = this.innerContainer[0];
					if (((pawn != null) ? pawn.def : null) != null)
					{
						return this.innerContainer[0].kindDef == null;
					}
				}
				return true;
			}
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000F89EC File Offset: 0x000F6BEC
		public Corpse()
		{
			this.operationsBillStack = new BillStack(this);
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Reference, false);
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000F8A20 File Offset: 0x000F6C20
		public bool CurrentlyUsableForBills()
		{
			return this.InteractionCell.IsValid;
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x000F8A3B File Offset: 0x000F6C3B
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000F8A43 File Offset: 0x000F6C43
		public bool AnythingToStrip()
		{
			return this.InnerPawn.AnythingToStrip();
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x000F8A50 File Offset: 0x000F6C50
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x000F8A58 File Offset: 0x000F6C58
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x000F8A66 File Offset: 0x000F6C66
		public override void PostMake()
		{
			base.PostMake();
			this.timeOfDeath = Find.TickManager.TicksGame;
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000F8A80 File Offset: 0x000F6C80
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Bugged)
			{
				Log.Error(((this != null) ? this.ToString() : null) + " spawned in bugged state.");
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.InnerPawn.Rotation = Rot4.South;
			List<Hediff> hediffs = this.InnerPawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				hediffs[i].Notify_PawnCorpseSpawned();
			}
			this.NotifyColonistBar();
		}

		// Token: 0x06002843 RID: 10307 RVA: 0x000F8B04 File Offset: 0x000F6D04
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (dinfo != null && dinfo.Value.Def == DamageDefOf.Bomb)
			{
				CompRottable comp = base.GetComp<CompRottable>();
				ThingDef thingDef;
				if (comp != null && comp.Stage == RotStage.Rotting)
				{
					thingDef = ThingDefOf.Filth_CorpseBile;
				}
				else
				{
					thingDef = this.InnerPawn.RaceProps.BloodDef;
				}
				if (thingDef != null)
				{
					int randomInRange = Corpse.ExplodeFilthCountRange.RandomInRange;
					for (int i = 0; i < randomInRange; i++)
					{
						FilthMaker.TryMakeFilth(base.PositionHeld, base.MapHeld, thingDef, 1, FilthSourceFlags.None, true);
					}
				}
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x06002844 RID: 10308 RVA: 0x000F8B9C File Offset: 0x000F6D9C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			if (!this.Bugged)
			{
				this.NotifyColonistBar();
			}
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000F8BB4 File Offset: 0x000F6DB4
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = null;
			if (!this.Bugged)
			{
				pawn = this.InnerPawn;
				this.NotifyColonistBar();
				this.innerContainer.Clear();
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_CorpseLost(this, null);
			}
			base.Destroy(mode);
			if (pawn != null)
			{
				Corpse.PostCorpseDestroy(pawn, false);
			}
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x000F8C10 File Offset: 0x000F6E10
		public static void PostCorpseDestroy(Pawn pawn, bool discarded = false)
		{
			Pawn_Ownership ownership = pawn.ownership;
			if (ownership != null)
			{
				ownership.UnclaimAll();
			}
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (equipment != null)
			{
				equipment.DestroyAllEquipment(DestroyMode.Vanish);
			}
			pawn.inventory.DestroyAll(DestroyMode.Vanish);
			pawn.health.Notify_PawnCorpseDestroyed();
			Pawn_ApparelTracker apparel = pawn.apparel;
			if (apparel != null)
			{
				apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (!PawnGenerator.IsBeingGenerated(pawn) && !discarded)
			{
				Ideo ideo = pawn.Ideo;
				if (ideo == null)
				{
					return;
				}
				ideo.Notify_MemberCorpseDestroyed(pawn);
			}
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x000F8C88 File Offset: 0x000F6E88
		public override void TickRare()
		{
			this.TickRareInt();
			if (base.Destroyed)
			{
				return;
			}
			if (this.Bugged)
			{
				Log.Error(((this != null) ? this.ToString() : null) + " has null innerPawn. Destroying.");
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Hediff_DeathRefusal firstHediff = this.InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			if (firstHediff != null)
			{
				firstHediff.TickRare();
				if (base.Destroyed)
				{
					return;
				}
			}
			if (ModsConfig.AnomalyActive && this.InnerPawn.kindDef.IsFleshBeast() && this.GetRotStage() == RotStage.Dessicated)
			{
				FilthMaker.TryMakeFilth(base.PositionHeld, base.MapHeld, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.ShouldVanish)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x000F8D4C File Offset: 0x000F6F4C
		protected void TickRareInt()
		{
			if (base.AllComps != null)
			{
				int i = 0;
				int count = base.AllComps.Count;
				while (i < count)
				{
					base.AllComps[i].CompTickRare();
					i++;
				}
			}
			if (base.Destroyed)
			{
				return;
			}
			this.InnerPawn.TickRare();
			GasUtility.CorpseGasEffectsTickRare(this);
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x000F8DA4 File Offset: 0x000F6FA4
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			BodyPartRecord bodyPartRecord = this.GetBestBodyPartToEat(nutritionWanted);
			if (bodyPartRecord == null)
			{
				Log.Error(((ingester != null) ? ingester.ToString() : null) + " ate " + ((this != null) ? this.ToString() : null) + " but no body part was found. Replacing with core part.");
				bodyPartRecord = this.InnerPawn.RaceProps.body.corePart;
			}
			float bodyPartNutrition = FoodUtility.GetBodyPartNutrition(this, bodyPartRecord);
			if (bodyPartRecord == this.InnerPawn.RaceProps.body.corePart)
			{
				if (ingester != null && PawnUtility.ShouldSendNotificationAbout(this.InnerPawn) && this.InnerPawn.RaceProps.Humanlike)
				{
					Messages.Message("MessageEatenByPredator".Translate(this.InnerPawn.LabelShort, ingester.Named("PREDATOR"), this.InnerPawn.Named("EATEN")).CapitalizeFirst(), ingester, MessageTypeDefOf.NegativeEvent, true);
				}
				numTaken = 1;
			}
			else
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.InnerPawn, bodyPartRecord);
				if (ingester != null)
				{
					hediff_MissingPart.lastInjury = HediffDefOf.Bite;
				}
				hediff_MissingPart.IsFresh = true;
				this.InnerPawn.health.AddHediff(hediff_MissingPart, null, null, null);
				numTaken = 0;
			}
			nutritionIngested = bodyPartNutrition;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000F8EEA File Offset: 0x000F70EA
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			Corpse.<ButcherProducts>d__45 <ButcherProducts>d__ = new Corpse.<ButcherProducts>d__45(-2);
			<ButcherProducts>d__.<>4__this = this;
			<ButcherProducts>d__.<>3__butcher = butcher;
			<ButcherProducts>d__.<>3__efficiency = efficiency;
			return <ButcherProducts>d__;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000F8F08 File Offset: 0x000F7108
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.timeOfDeath, "timeOfDeath", 0, false);
			Scribe_Values.Look<int>(ref this.vanishAfterTimestamp, "vanishAfterTimestamp", 0, false);
			Scribe_Values.Look<bool>(ref this.everBuriedInSarcophagus, "everBuriedInSarcophagus", false, false);
			Scribe_Deep.Look<BillStack>(ref this.operationsBillStack, "operationsBillStack", new object[]
			{
				this
			});
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.innerContainer.removeContentsIfDestroyed)
			{
				this.innerContainer.removeContentsIfDestroyed = false;
			}
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000F8FA6 File Offset: 0x000F71A6
		public void Strip(bool notifyFaction = true)
		{
			this.InnerPawn.Strip(notifyFaction);
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000F8FB4 File Offset: 0x000F71B4
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			this.InnerPawn.DynamicDrawPhaseAt(phase, drawLoc.WithYOffset(this.InnerPawn.Drawer.SeededYOffset), false);
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000F8FD9 File Offset: 0x000F71D9
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			return null;
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000F8FDC File Offset: 0x000F71DC
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			if (!this.InnerPawn.RaceProps.Humanlike)
			{
				return null;
			}
			if (this.InnerPawn.health.killedByRitual && Find.TickManager.TicksGame - this.timeOfDeath < 60000)
			{
				return null;
			}
			if (this.StoringThing() != null)
			{
				return null;
			}
			if (this.IsNotFresh())
			{
				return HistoryEventDefOf.ObservedLayingRottingCorpse;
			}
			return HistoryEventDefOf.ObservedLayingCorpse;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000F9048 File Offset: 0x000F7248
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.InnerPawn.Faction != null && !this.InnerPawn.Faction.Hidden)
			{
				stringBuilder.AppendLineTagged("Faction".Translate() + ": " + this.InnerPawn.Faction.NameColored);
			}
			stringBuilder.AppendLine("DeadTime".Translate(this.Age.ToStringTicksToPeriodVague(true, false)));
			float num = 1f - this.InnerPawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(this.InnerPawn.RaceProps.body.corePart);
			if (num >= 0.01f)
			{
				stringBuilder.AppendLine("CorpsePercentMissing".Translate() + ": " + num.ToStringPercent());
			}
			Hediff_DeathRefusal firstHediff = this.InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			if (firstHediff != null && firstHediff.InProgress)
			{
				stringBuilder.AppendLine("SelfResurrecting".Translate());
			}
			stringBuilder.AppendLine(base.GetInspectString());
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000F9185 File Offset: 0x000F7385
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Corpse.<SpecialDisplayStats>d__52 <SpecialDisplayStats>d__ = new Corpse.<SpecialDisplayStats>d__52(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000F9195 File Offset: 0x000F7395
		public void RotStageChanged()
		{
			this.InnerPawn.Drawer.renderer.SetAllGraphicsDirty();
			this.InnerPawn.Drawer.renderer.WoundOverlays.ClearCache();
			this.NotifyColonistBar();
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x000F91CC File Offset: 0x000F73CC
		public BodyPartRecord GetBestBodyPartToEat(float nutritionWanted)
		{
			IEnumerable<BodyPartRecord> source = from x in this.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside && FoodUtility.GetBodyPartNutrition(this, x) > 0.001f
			select x;
			if (!source.Any<BodyPartRecord>())
			{
				return null;
			}
			return source.MinBy((BodyPartRecord x) => Mathf.Abs(FoodUtility.GetBodyPartNutrition(this, x) - nutritionWanted));
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x000F9234 File Offset: 0x000F7434
		private void NotifyColonistBar()
		{
			if (this.InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x000F925A File Offset: 0x000F745A
		public void Notify_BillDeleted(Bill bill)
		{
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000F925C File Offset: 0x000F745C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Corpse.<GetGizmos>d__57 <GetGizmos>d__ = new Corpse.<GetGizmos>d__57(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x04001C63 RID: 7267
		protected ThingOwner<Pawn> innerContainer;

		// Token: 0x04001C64 RID: 7268
		public int timeOfDeath = -1;

		// Token: 0x04001C65 RID: 7269
		private int vanishAfterTimestamp = -1;

		// Token: 0x04001C66 RID: 7270
		private BillStack operationsBillStack;

		// Token: 0x04001C67 RID: 7271
		public bool everBuriedInSarcophagus;

		// Token: 0x04001C68 RID: 7272
		[Unsaved(false)]
		private string cachedLabel;

		// Token: 0x04001C69 RID: 7273
		private const int DontCauseObservedCorpseThoughtAfterRitualExecutionTicks = 60000;

		// Token: 0x04001C6A RID: 7274
		private static readonly IntRange ExplodeFilthCountRange = new IntRange(2, 5);
	}
}
