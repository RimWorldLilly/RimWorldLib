using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200207B RID: 8315
	public class Gene_Deathrest : Gene
	{
		// Token: 0x17002582 RID: 9602
		// (get) Token: 0x0600D179 RID: 53625 RVA: 0x004D0258 File Offset: 0x004CE458
		public Need_Deathrest DeathrestNeed
		{
			get
			{
				if (this.cachedDeathrestNeed == null)
				{
					this.pawn.needs.TryGetNeed<Need_Deathrest>(out this.cachedDeathrestNeed);
				}
				return this.cachedDeathrestNeed;
			}
		}

		// Token: 0x17002583 RID: 9603
		// (get) Token: 0x0600D17A RID: 53626 RVA: 0x004D027F File Offset: 0x004CE47F
		public List<Thing> BoundBuildings
		{
			get
			{
				return this.boundBuildings;
			}
		}

		// Token: 0x17002584 RID: 9604
		// (get) Token: 0x0600D17B RID: 53627 RVA: 0x004D0288 File Offset: 0x004CE488
		public List<CompDeathrestBindable> BoundComps
		{
			get
			{
				if (this.cachedBoundComps == null)
				{
					this.cachedBoundComps = new List<CompDeathrestBindable>();
					foreach (Thing thing in this.boundBuildings)
					{
						if (this.CanUseBindableNow(thing))
						{
							this.cachedBoundComps.Add(thing.TryGetComp<CompDeathrestBindable>());
						}
					}
				}
				return this.cachedBoundComps;
			}
		}

		// Token: 0x17002585 RID: 9605
		// (get) Token: 0x0600D17C RID: 53628 RVA: 0x004D0308 File Offset: 0x004CE508
		public float DeathrestEfficiency
		{
			get
			{
				float num = 1f;
				foreach (CompDeathrestBindable compDeathrestBindable in this.BoundComps)
				{
					if (compDeathrestBindable.Props.deathrestEffectivenessFactor > 0f && compDeathrestBindable.CanIncreasePresence)
					{
						num *= compDeathrestBindable.Props.deathrestEffectivenessFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x17002586 RID: 9606
		// (get) Token: 0x0600D17D RID: 53629 RVA: 0x004D0384 File Offset: 0x004CE584
		public int MinDeathrestTicks
		{
			get
			{
				return Mathf.RoundToInt(240000f / this.DeathrestEfficiency);
			}
		}

		// Token: 0x17002587 RID: 9607
		// (get) Token: 0x0600D17E RID: 53630 RVA: 0x004D0397 File Offset: 0x004CE597
		public int DeathrestCapacity
		{
			get
			{
				return this.deathrestCapacity;
			}
		}

		// Token: 0x17002588 RID: 9608
		// (get) Token: 0x0600D17F RID: 53631 RVA: 0x004D03A0 File Offset: 0x004CE5A0
		public int CurrentCapacity
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.boundBuildings.Count; i++)
				{
					CompDeathrestBindable compDeathrestBindable = this.boundBuildings[i].TryGetComp<CompDeathrestBindable>();
					if (compDeathrestBindable != null && compDeathrestBindable.Props.countsTowardsBuildingLimit)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17002589 RID: 9609
		// (get) Token: 0x0600D180 RID: 53632 RVA: 0x004D03EC File Offset: 0x004CE5EC
		public bool AtBuildingCapacityLimit
		{
			get
			{
				return this.CurrentCapacity >= this.deathrestCapacity;
			}
		}

		// Token: 0x1700258A RID: 9610
		// (get) Token: 0x0600D181 RID: 53633 RVA: 0x004D03FF File Offset: 0x004CE5FF
		public float DeathrestPercent
		{
			get
			{
				return Mathf.Clamp01((float)this.deathrestTicks / (float)this.MinDeathrestTicks);
			}
		}

		// Token: 0x1700258B RID: 9611
		// (get) Token: 0x0600D182 RID: 53634 RVA: 0x004D0415 File Offset: 0x004CE615
		public bool ShowWakeAlert
		{
			get
			{
				return this.DeathrestPercent >= 1f && !this.autoWake;
			}
		}

		// Token: 0x0600D183 RID: 53635 RVA: 0x004D042F File Offset: 0x004CE62F
		public override void PostAdd()
		{
			if (!ModLister.CheckBiotech("Deathrest"))
			{
				return;
			}
			base.PostAdd();
			this.Reset();
		}

		// Token: 0x0600D184 RID: 53636 RVA: 0x004D044C File Offset: 0x004CE64C
		public override void PostRemove()
		{
			base.PostRemove();
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Deathrest, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff firstHediffOfDef2 = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DeathrestExhaustion, false);
			if (firstHediffOfDef2 != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef2);
			}
			this.RemoveOldDeathrestBonuses();
			this.Reset();
		}

		// Token: 0x0600D185 RID: 53637 RVA: 0x004D04CB File Offset: 0x004CE6CB
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.pawn.IsHashIntervalTick(200, delta) && this.pawn.IsColonistPlayerControlled)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.Deathrest, OpportunityType.Important);
			}
		}

		// Token: 0x0600D186 RID: 53638 RVA: 0x004D0500 File Offset: 0x004CE700
		public void OffsetCapacity(int offset, bool sendNotification = true)
		{
			int num = this.deathrestCapacity;
			this.deathrestCapacity += offset;
			if (sendNotification && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageDeathrestCapacityChanged".Translate(this.pawn.Named("PAWN"), num.Named("OLD"), this.deathrestCapacity.Named("NEW")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x0600D187 RID: 53639 RVA: 0x004D058C File Offset: 0x004CE78C
		public void TickDeathresting(bool paused, int delta)
		{
			if (paused)
			{
				return;
			}
			if (this.DeathrestNeed != null)
			{
				this.DeathrestNeed.lastDeathrestTick = Find.TickManager.TicksGame;
			}
			this.deathrestTicks += delta;
			this.adjustedDeathrestTicks += this.DeathrestEfficiency * (float)delta;
			foreach (CompDeathrestBindable compDeathrestBindable in this.BoundComps)
			{
				compDeathrestBindable.TryIncreasePresence(delta);
			}
			if (this.DeathrestPercent >= 1f && !this.notifiedWakeOK)
			{
				this.notifiedWakeOK = true;
				if (this.autoWake)
				{
					this.Wake();
					return;
				}
				if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageDeathrestingPawnCanWakeSafely".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			if (this.pawn.Spawned && this.pawn.IsHashIntervalTick(150, delta) && this.PawnOrBedTouchingSunlight())
			{
				if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessagePawnWokenFromSunlight".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				}
				this.Wake();
			}
		}

		// Token: 0x0600D188 RID: 53640 RVA: 0x004D0700 File Offset: 0x004CE900
		private bool PawnOrBedTouchingSunlight()
		{
			Building_Bed building_Bed = this.pawn.CurrentBed();
			if (building_Bed != null)
			{
				using (CellRect.Enumerator enumerator = building_Bed.OccupiedRect().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.InSunlight(building_Bed.Map))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600D189 RID: 53641 RVA: 0x004D0774 File Offset: 0x004CE974
		public void Notify_DeathrestStarted()
		{
			this.RemoveOldDeathrestBonuses();
			this.TryLinkToNearbyDeathrestBuildings();
			this.notifiedWakeOK = false;
		}

		// Token: 0x0600D18A RID: 53642 RVA: 0x004D078C File Offset: 0x004CE98C
		public void Notify_DeathrestEnded()
		{
			foreach (Thing thing in this.boundBuildings)
			{
				thing.TryGetComp<CompDeathrestBindable>().Notify_DeathrestEnded();
			}
		}

		// Token: 0x0600D18B RID: 53643 RVA: 0x004D07E4 File Offset: 0x004CE9E4
		public void Notify_BoundBuildingDeSpawned(Thing building)
		{
			this.boundBuildings.Remove(building);
			this.cachedBoundComps = null;
		}

		// Token: 0x0600D18C RID: 53644 RVA: 0x004D07FC File Offset: 0x004CE9FC
		public void TryLinkToNearbyDeathrestBuildings()
		{
			if (!ModsConfig.BiotechActive || !this.pawn.Spawned)
			{
				return;
			}
			this.cachedBoundComps = null;
			Room room = this.pawn.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return;
			}
			foreach (Region region in room.Regions)
			{
				foreach (Thing thing in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
				{
					CompDeathrestBindable bindComp = thing.TryGetComp<CompDeathrestBindable>();
					if (this.CanBindToBindable(bindComp))
					{
						this.BindTo(bindComp);
					}
				}
			}
		}

		// Token: 0x0600D18D RID: 53645 RVA: 0x004D08CC File Offset: 0x004CEACC
		public void RemoveOldDeathrestBonuses()
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = hediffs.Count - 1; i >= 0; i--)
			{
				if (hediffs[i].def.removeOnDeathrestStart)
				{
					this.pawn.health.RemoveHediff(hediffs[i]);
				}
			}
			Gene_Hemogen firstGeneOfType = this.pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
			if (firstGeneOfType == null)
			{
				return;
			}
			firstGeneOfType.ResetMax();
		}

		// Token: 0x0600D18E RID: 53646 RVA: 0x004D0948 File Offset: 0x004CEB48
		private void ApplyDeathrestBuildingBonuses()
		{
			if (this.deathrestTicks == 0)
			{
				return;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.InterruptedDeathrest, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			foreach (CompDeathrestBindable compDeathrestBindable in this.BoundComps)
			{
				if ((float)compDeathrestBindable.presenceTicks / (float)this.deathrestTicks >= 0.75f)
				{
					compDeathrestBindable.Apply();
				}
			}
		}

		// Token: 0x0600D18F RID: 53647 RVA: 0x004D09EC File Offset: 0x004CEBEC
		public bool CanBindToBindable(CompDeathrestBindable bindComp)
		{
			return ModsConfig.BiotechActive && bindComp != null && bindComp.parent.Faction == this.pawn.Faction && !this.boundBuildings.Contains(bindComp.parent) && (!bindComp.Props.countsTowardsBuildingLimit || !this.AtBuildingCapacityLimit) && bindComp.CanBindTo(this.pawn) && this.CanUseBindableNow(bindComp.parent) && !this.BindingWillExceedStackLimit(bindComp);
		}

		// Token: 0x0600D190 RID: 53648 RVA: 0x004D0A7C File Offset: 0x004CEC7C
		public bool BindingWillExceedStackLimit(CompDeathrestBindable bindComp)
		{
			if (this.boundBuildings.Contains(bindComp.parent))
			{
				return false;
			}
			int stackLimit = bindComp.Props.stackLimit;
			if (stackLimit > 0)
			{
				int num = 0;
				for (int i = 0; i < this.boundBuildings.Count; i++)
				{
					if (this.boundBuildings[i].def == bindComp.parent.def)
					{
						num++;
						if (num >= stackLimit)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600D191 RID: 53649 RVA: 0x004D0AF0 File Offset: 0x004CECF0
		private bool CanUseBindableNow(Thing building)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			CompDeathrestBindable compDeathrestBindable = building.TryGetComp<CompDeathrestBindable>();
			if (compDeathrestBindable == null)
			{
				return false;
			}
			Building_Bed building_Bed = this.pawn.CurrentBed();
			if (building_Bed == null)
			{
				return false;
			}
			if (compDeathrestBindable.Props.mustBeLayingInToBind)
			{
				if (building_Bed != building)
				{
					return false;
				}
			}
			else if (!building_Bed.def.HasComp(typeof(CompDeathrestBindable)))
			{
				return false;
			}
			return building.Spawned && building.Map == this.pawn.MapHeld && building.GetRoom(RegionType.Set_All) == this.pawn.GetRoom(RegionType.Set_All) && GenSight.LineOfSight(this.pawn.Position, building.OccupiedRect().CenterCell, this.pawn.Map);
		}

		// Token: 0x0600D192 RID: 53650 RVA: 0x004D0BB4 File Offset: 0x004CEDB4
		public void BindTo(CompDeathrestBindable bindComp)
		{
			bindComp.BindTo(this.pawn);
			this.boundBuildings.Add(bindComp.parent);
			this.cachedBoundComps = null;
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) && bindComp.Props.countsTowardsBuildingLimit)
			{
				Messages.Message("MessageDeathrestBuildingBound".Translate(bindComp.parent.Named("BUILDING"), this.pawn.Named("PAWN"), this.CurrentCapacity.Named("CUR"), this.DeathrestCapacity.Named("MAX")), new LookTargets(new TargetInfo[]
				{
					bindComp.parent,
					this.pawn
				}), MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x0600D193 RID: 53651 RVA: 0x004D0C98 File Offset: 0x004CEE98
		public override void Reset()
		{
			this.deathrestCapacity = 1;
			foreach (CompDeathrestBindable compDeathrestBindable in this.BoundComps)
			{
				compDeathrestBindable.Notify_DeathrestGeneRemoved();
			}
			this.cachedBoundComps = null;
		}

		// Token: 0x0600D194 RID: 53652 RVA: 0x004D0CF8 File Offset: 0x004CEEF8
		public override void Notify_NewColony()
		{
			if (this.pawn.BeingTransportedOnGravship)
			{
				return;
			}
			this.boundBuildings.Clear();
			this.cachedBoundComps = null;
		}

		// Token: 0x0600D195 RID: 53653 RVA: 0x004D0D1A File Offset: 0x004CEF1A
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			this.boundBuildings.Clear();
			this.cachedBoundComps = null;
			this.cachedDeathrestNeed = null;
		}

		// Token: 0x0600D196 RID: 53654 RVA: 0x004D0D40 File Offset: 0x004CEF40
		public void Wake()
		{
			if (this.DeathrestPercent < 1f)
			{
				this.pawn.health.AddHediff(HediffDefOf.InterruptedDeathrest, null, null, null);
			}
			else
			{
				this.ApplyDeathrestBuildingBonuses();
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Deathrest, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			this.deathrestTicks = 0;
			this.adjustedDeathrestTicks = 0f;
		}

		// Token: 0x0600D197 RID: 53655 RVA: 0x004D0DC5 File Offset: 0x004CEFC5
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Gene_Deathrest.<GetGizmos>d__58 <GetGizmos>d__ = new Gene_Deathrest.<GetGizmos>d__58(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600D198 RID: 53656 RVA: 0x004D0DD5 File Offset: 0x004CEFD5
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Gene_Deathrest.<SpecialDisplayStats>d__59 <SpecialDisplayStats>d__ = new Gene_Deathrest.<SpecialDisplayStats>d__59(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600D199 RID: 53657 RVA: 0x004D0DE8 File Offset: 0x004CEFE8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.deathrestCapacity, "deathrestCapacity", 1, false);
			Scribe_Values.Look<float>(ref this.adjustedDeathrestTicks, "adjustedDeathrestTicks", 0f, false);
			Scribe_Values.Look<int>(ref this.deathrestTicks, "deathrestTicks", 0, false);
			Scribe_Values.Look<bool>(ref this.autoWake, "autoWake", false, false);
			Scribe_Values.Look<int>(ref this.chamberThoughtIndex, "chamberThoughtIndex", -1, false);
			Scribe_Values.Look<bool>(ref this.notifiedWakeOK, "notifiedWakeOK", false, false);
			Scribe_Collections.Look<Thing>(ref this.boundBuildings, "boundBuildings", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.boundBuildings.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040084ED RID: 34029
		private int deathrestCapacity;

		// Token: 0x040084EE RID: 34030
		private List<Thing> boundBuildings = new List<Thing>();

		// Token: 0x040084EF RID: 34031
		public float adjustedDeathrestTicks;

		// Token: 0x040084F0 RID: 34032
		public int deathrestTicks;

		// Token: 0x040084F1 RID: 34033
		public bool autoWake;

		// Token: 0x040084F2 RID: 34034
		public int chamberThoughtIndex = -1;

		// Token: 0x040084F3 RID: 34035
		private bool notifiedWakeOK;

		// Token: 0x040084F4 RID: 34036
		[Unsaved(false)]
		private Gene_Hemogen cachedHemogenGene;

		// Token: 0x040084F5 RID: 34037
		[Unsaved(false)]
		private Need_Deathrest cachedDeathrestNeed;

		// Token: 0x040084F6 RID: 34038
		[Unsaved(false)]
		private List<CompDeathrestBindable> cachedBoundComps;

		// Token: 0x040084F7 RID: 34039
		[Unsaved(false)]
		private GeneGizmo_DeathrestCapacity gizmo;

		// Token: 0x040084F8 RID: 34040
		private const int InitialDeathrestCapacity = 1;

		// Token: 0x040084F9 RID: 34041
		public const int BaseDeathrestTicksWithoutInterruptedHediff = 240000;

		// Token: 0x040084FA RID: 34042
		public const float PresencePercentRequiredToApply = 0.75f;

		// Token: 0x040084FB RID: 34043
		private static readonly CachedTexture WakeCommandTex = new CachedTexture("UI/Gizmos/Wake");

		// Token: 0x040084FC RID: 34044
		private static readonly CachedTexture AutoWakeCommandTex = new CachedTexture("UI/Gizmos/DeathrestAutoWake");

		// Token: 0x040084FD RID: 34045
		private const int LessonDeathrestTicks = 200;

		// Token: 0x040084FE RID: 34046
		private const int SunlightCheckInterval = 150;
	}
}
