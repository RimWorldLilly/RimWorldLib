using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FB RID: 4347
	public class QuestPart_SpawnThing : QuestPart
	{
		// Token: 0x17001288 RID: 4744
		// (get) Token: 0x06006C75 RID: 27765 RVA: 0x002607B0 File Offset: 0x0025E9B0
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_SpawnThing.<get_QuestLookTargets>d__14 <get_QuestLookTargets>d__ = new QuestPart_SpawnThing.<get_QuestLookTargets>d__14(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x17001289 RID: 4745
		// (get) Token: 0x06006C76 RID: 27766 RVA: 0x002607C0 File Offset: 0x0025E9C0
		public override bool IncreasesPopulation
		{
			get
			{
				Pawn pawn = this.thing as Pawn;
				return pawn != null && PawnsArriveQuestPartUtility.IncreasesPopulation(Gen.YieldSingle<Pawn>(pawn), false, false);
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06006C77 RID: 27767 RVA: 0x002607EB File Offset: 0x0025E9EB
		public MapParent MapParent
		{
			get
			{
				if (this.mapParentOfPawn == null)
				{
					return this.mapParent;
				}
				Map mapHeld = this.mapParentOfPawn.MapHeld;
				if (mapHeld == null)
				{
					return null;
				}
				return mapHeld.Parent;
			}
		}

		// Token: 0x06006C78 RID: 27768 RVA: 0x00260814 File Offset: 0x0025EA14
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.inSignal)
			{
				return;
			}
			if (this.MapParent == null || this.MapParent.Destroyed)
			{
				this.mapParentOfPawn = null;
				this.mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
				this.cell = IntVec3.Invalid;
			}
			if (!this.MapParent.HasMap)
			{
				return;
			}
			IntVec3 center = IntVec3.Invalid;
			if (this.cell.IsValid)
			{
				center = this.cell;
			}
			else
			{
				Thing thing;
				if (this.tryLandInShipLandingZone && !DropCellFinder.TryFindShipLandingArea(this.MapParent.Map, out center, out thing))
				{
					if (thing != null)
					{
						Messages.Message("ShuttleBlocked".Translate("BlockedBy".Translate(thing).CapitalizeFirst()), thing, MessageTypeDefOf.NeutralEvent, true);
					}
					center = DropCellFinder.TryFindSafeLandingSpotCloseToColony(this.MapParent.Map, this.thing.def.Size, this.factionForFindingSpot, 2);
				}
				if (!center.IsValid && this.tryLandNearThing != null)
				{
					DropCellFinder.FindSafeLandingSpotNearAvoidingHostiles(this.tryLandNearThing, this.MapParent.Map, out center, 35, 15, 25, new IntVec2?(this.thing.def.size), null);
				}
				if (!center.IsValid && (!this.lookForSafeSpot || !DropCellFinder.FindSafeLandingSpot(out center, this.factionForFindingSpot, this.MapParent.Map, 35, 15, 25, new IntVec2?(this.thing.def.size), null)))
				{
					IntVec3 intVec = DropCellFinder.RandomDropSpot(this.MapParent.Map, true);
					if (!DropCellFinder.TryFindDropSpotNear(intVec, this.MapParent.Map, out center, false, false, false, new IntVec2?(this.thing.def.size), true))
					{
						center = intVec;
					}
				}
			}
			GenPlace.TryPlaceThing(this.thing, center, this.MapParent.Map, ThingPlaceMode.Near, null, null, null, 1);
			this.spawned = true;
			if (this.spawnEffecter != null)
			{
				this.spawnEffecter.SpawnMaintained(this.thing, this.thing.Map, 1f);
			}
			Skyfaller skyfaller = this.thing as Skyfaller;
			if (skyfaller != null && skyfaller.innerContainer.Count == 1)
			{
				this.innerSkyfallerThing = skyfaller.innerContainer.First<Thing>();
				return;
			}
			this.innerSkyfallerThing = null;
		}

		// Token: 0x06006C79 RID: 27769 RVA: 0x00260A9B File Offset: 0x0025EC9B
		public override bool QuestPartReserves(Pawn p)
		{
			return p == this.thing || (this.thing is Skyfaller && ((Skyfaller)this.thing).innerContainer.Contains(p));
		}

		// Token: 0x06006C7A RID: 27770 RVA: 0x00260AD0 File Offset: 0x0025ECD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			if (!this.spawned && (this.thing == null || !(this.thing is Pawn)))
			{
				Scribe_Deep.Look<Thing>(ref this.thing, "thing", Array.Empty<object>());
			}
			else
			{
				Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			}
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.lookForSafeSpot, "lookForSafeSpot", false, false);
			Scribe_References.Look<Faction>(ref this.factionForFindingSpot, "factionForFindingSpot", false);
			Scribe_Values.Look<bool>(ref this.questLookTarget, "questLookTarget", true, false);
			Scribe_References.Look<Thing>(ref this.innerSkyfallerThing, "innerSkyfallerThing", false);
			Scribe_Values.Look<bool>(ref this.tryLandInShipLandingZone, "tryLandInShipLandingZone", false, false);
			Scribe_References.Look<Thing>(ref this.tryLandNearThing, "tryLandNearThing", false);
			Scribe_References.Look<Pawn>(ref this.mapParentOfPawn, "mapParentOfPawn", false);
			Scribe_Defs.Look<EffecterDef>(ref this.spawnEffecter, "spawnEffecter");
		}

		// Token: 0x06006C7B RID: 27771 RVA: 0x00260C04 File Offset: 0x0025EE04
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.thing = ThingMaker.MakeThing(ThingDefOf.Silver, null);
			}
		}

		// Token: 0x04004505 RID: 17669
		public string inSignal;

		// Token: 0x04004506 RID: 17670
		public Thing thing;

		// Token: 0x04004507 RID: 17671
		public Faction factionForFindingSpot;

		// Token: 0x04004508 RID: 17672
		public MapParent mapParent;

		// Token: 0x04004509 RID: 17673
		public IntVec3 cell = IntVec3.Invalid;

		// Token: 0x0400450A RID: 17674
		public bool questLookTarget = true;

		// Token: 0x0400450B RID: 17675
		public bool lookForSafeSpot;

		// Token: 0x0400450C RID: 17676
		public bool tryLandInShipLandingZone;

		// Token: 0x0400450D RID: 17677
		public Thing tryLandNearThing;

		// Token: 0x0400450E RID: 17678
		public Pawn mapParentOfPawn;

		// Token: 0x0400450F RID: 17679
		public EffecterDef spawnEffecter;

		// Token: 0x04004510 RID: 17680
		private Thing innerSkyfallerThing;

		// Token: 0x04004511 RID: 17681
		private bool spawned;
	}
}
