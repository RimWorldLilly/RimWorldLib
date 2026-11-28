using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150C RID: 5388
	[StaticConstructorOnStartup]
	public class RoyalTitlePermitWorker_DropResources : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x06008403 RID: 33795 RVA: 0x002F777E File Offset: 0x002F597E
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallResources(target.Cell);
		}

		// Token: 0x06008404 RID: 33796 RVA: 0x002F778D File Offset: 0x002F598D
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			RoyalTitlePermitWorker_DropResources.<GetRoyalAidOptions>d__3 <GetRoyalAidOptions>d__ = new RoyalTitlePermitWorker_DropResources.<GetRoyalAidOptions>d__3(-2);
			<GetRoyalAidOptions>d__.<>4__this = this;
			<GetRoyalAidOptions>d__.<>3__map = map;
			<GetRoyalAidOptions>d__.<>3__pawn = pawn;
			<GetRoyalAidOptions>d__.<>3__faction = faction;
			return <GetRoyalAidOptions>d__;
		}

		// Token: 0x06008405 RID: 33797 RVA: 0x002F77B2 File Offset: 0x002F59B2
		public override IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
		{
			RoyalTitlePermitWorker_DropResources.<GetCaravanGizmos>d__4 <GetCaravanGizmos>d__ = new RoyalTitlePermitWorker_DropResources.<GetCaravanGizmos>d__4(-2);
			<GetCaravanGizmos>d__.<>4__this = this;
			<GetCaravanGizmos>d__.<>3__pawn = pawn;
			<GetCaravanGizmos>d__.<>3__faction = faction;
			return <GetCaravanGizmos>d__;
		}

		// Token: 0x06008406 RID: 33798 RVA: 0x002F77D0 File Offset: 0x002F59D0
		private void BeginCallResources(Pawn caller, Faction faction, Map map, bool free)
		{
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetBuildings = false;
			this.targetingParameters.canTargetPawns = false;
			this.caller = caller;
			this.map = map;
			this.faction = faction;
			this.free = free;
			float rangeActual = base.RangeClamped;
			this.targetingParameters.validator = ((TargetInfo target) => (rangeActual <= 0f || target.Cell.DistanceTo(caller.Position) <= rangeActual) && !target.Cell.Fogged(map) && DropCellFinder.CanPhysicallyDropInto(target.Cell, map, true, true));
			Find.Targeter.BeginTargeting(this, null, false, null, null, true);
		}

		// Token: 0x06008407 RID: 33799 RVA: 0x002F787C File Offset: 0x002F5A7C
		private void CallResources(IntVec3 cell)
		{
			List<Thing> list = new List<Thing>();
			for (int i = 0; i < this.def.royalAid.itemsToDrop.Count; i++)
			{
				Thing thing = ThingMaker.MakeThing(this.def.royalAid.itemsToDrop[i].thingDef, null);
				thing.stackCount = this.def.royalAid.itemsToDrop[i].count;
				list.Add(thing);
			}
			if (list.Any<Thing>())
			{
				ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
				activeTransporterInfo.innerContainer.TryAddRangeOrTransfer(list, true, false);
				DropPodUtility.MakeDropPodAt(cell, this.map, activeTransporterInfo, null);
				Messages.Message("MessagePermitTransportDrop".Translate(this.faction.Named("FACTION")), new LookTargets(cell, this.map), MessageTypeDefOf.NeutralEvent, true);
				this.caller.royalty.GetPermit(this.def, this.faction).Notify_Used();
				if (!this.free)
				{
					this.caller.royalty.TryRemoveFavor(this.faction, this.def.royalAid.favorCost);
				}
			}
		}

		// Token: 0x06008408 RID: 33800 RVA: 0x002F79AC File Offset: 0x002F5BAC
		private void CallResourcesToCaravan(Pawn caller, Faction faction, bool free)
		{
			Caravan caravan = caller.GetCaravan();
			for (int i = 0; i < this.def.royalAid.itemsToDrop.Count; i++)
			{
				Thing thing = ThingMaker.MakeThing(this.def.royalAid.itemsToDrop[i].thingDef, null);
				thing.stackCount = this.def.royalAid.itemsToDrop[i].count;
				CaravanInventoryUtility.GiveThing(caravan, thing);
			}
			Messages.Message("MessagePermitTransportDropCaravan".Translate(faction.Named("FACTION"), caller.Named("PAWN")), caravan, MessageTypeDefOf.NeutralEvent, true);
			caller.royalty.GetPermit(this.def, faction).Notify_Used();
			if (!free)
			{
				caller.royalty.TryRemoveFavor(faction, this.def.royalAid.favorCost);
			}
		}

		// Token: 0x04005184 RID: 20868
		private Faction faction;

		// Token: 0x04005185 RID: 20869
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallAid", true);
	}
}
