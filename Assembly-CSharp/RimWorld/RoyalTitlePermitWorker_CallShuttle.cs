using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200150B RID: 5387
	[StaticConstructorOnStartup]
	public class RoyalTitlePermitWorker_CallShuttle : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x060083F5 RID: 33781 RVA: 0x002F705C File Offset: 0x002F525C
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.CanHitTarget(target))
			{
				if (target.IsValid && showMessages)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map, null, null);
			if (!acceptanceReport.Accepted)
			{
				Messages.Message(acceptanceReport.Reason, new LookTargets(target.Cell, this.map), MessageTypeDefOf.RejectInput, false);
			}
			return acceptanceReport.Accepted;
		}

		// Token: 0x060083F6 RID: 33782 RVA: 0x002F70FE File Offset: 0x002F52FE
		public override void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, base.RangeClamped, Color.white, null);
			RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(target, this.map, ThingDefOf.Shuttle, ThingDefOf.Shuttle.defaultPlacingRot);
		}

		// Token: 0x060083F7 RID: 33783 RVA: 0x002F7137 File Offset: 0x002F5337
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallShuttle(target.Cell);
		}

		// Token: 0x060083F8 RID: 33784 RVA: 0x002F7148 File Offset: 0x002F5348
		public override void OnGUI(LocalTargetInfo target)
		{
			if (!target.IsValid || !RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map, null, null).Accepted)
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
		}

		// Token: 0x060083F9 RID: 33785 RVA: 0x002F7188 File Offset: 0x002F5388
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			RoyalTitlePermitWorker_CallShuttle.<GetRoyalAidOptions>d__6 <GetRoyalAidOptions>d__ = new RoyalTitlePermitWorker_CallShuttle.<GetRoyalAidOptions>d__6(-2);
			<GetRoyalAidOptions>d__.<>4__this = this;
			<GetRoyalAidOptions>d__.<>3__map = map;
			<GetRoyalAidOptions>d__.<>3__pawn = pawn;
			<GetRoyalAidOptions>d__.<>3__faction = faction;
			return <GetRoyalAidOptions>d__;
		}

		// Token: 0x060083FA RID: 33786 RVA: 0x002F71AD File Offset: 0x002F53AD
		public override IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
		{
			RoyalTitlePermitWorker_CallShuttle.<GetCaravanGizmos>d__7 <GetCaravanGizmos>d__ = new RoyalTitlePermitWorker_CallShuttle.<GetCaravanGizmos>d__7(-2);
			<GetCaravanGizmos>d__.<>4__this = this;
			<GetCaravanGizmos>d__.<>3__pawn = pawn;
			<GetCaravanGizmos>d__.<>3__faction = faction;
			return <GetCaravanGizmos>d__;
		}

		// Token: 0x060083FB RID: 33787 RVA: 0x002F71CC File Offset: 0x002F53CC
		private void BeginCallShuttle(Pawn caller, Map map, Faction faction, bool free)
		{
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetSelf = false;
			this.targetingParameters.canTargetPawns = false;
			this.targetingParameters.canTargetFires = false;
			this.targetingParameters.canTargetBuildings = true;
			this.targetingParameters.canTargetItems = true;
			this.caller = caller;
			this.map = map;
			this.calledFaction = faction;
			this.free = free;
			float rangeActual = base.RangeClamped;
			this.targetingParameters.validator = ((TargetInfo target) => rangeActual <= 0f || target.Cell.DistanceTo(caller.Position) <= rangeActual);
			Find.Targeter.BeginTargeting(this, null, false, null, null, true);
		}

		// Token: 0x060083FC RID: 33788 RVA: 0x002F7290 File Offset: 0x002F5490
		private void CallShuttle(IntVec3 landingCell)
		{
			if (this.caller.Spawned)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
				compShuttle.permitShuttle = true;
				compShuttle.acceptChildren = true;
				TransportShip transportShip = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, null, thing);
				transportShip.ArriveAt(landingCell, this.map.Parent);
				transportShip.AddJobs(new ShipJobDef[]
				{
					ShipJobDefOf.WaitForever,
					ShipJobDefOf.Unload_Destination,
					ShipJobDefOf.FlyAway
				});
				this.caller.royalty.GetPermit(this.def, this.calledFaction).Notify_Used();
				if (!this.free)
				{
					this.caller.royalty.TryRemoveFavor(this.calledFaction, this.def.royalAid.favorCost);
				}
			}
		}

		// Token: 0x060083FD RID: 33789 RVA: 0x002F7360 File Offset: 0x002F5560
		private void CallShuttleToCaravan(Pawn caller, Faction faction, bool free)
		{
			RoyalTitlePermitWorker_CallShuttle.<>c__DisplayClass10_0 CS$<>8__locals1 = new RoyalTitlePermitWorker_CallShuttle.<>c__DisplayClass10_0();
			CS$<>8__locals1.caller = caller;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.faction = faction;
			CS$<>8__locals1.free = free;
			CS$<>8__locals1.caravan = CS$<>8__locals1.caller.GetCaravan();
			CS$<>8__locals1.maxLaunchDistance = TransportShipDefOf.Ship_Shuttle.maxLaunchDistance;
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(CS$<>8__locals1.caravan), CameraJumper.MovementMode.Pan);
			Find.WorldSelector.ClearSelection();
			CS$<>8__locals1.caravanTile = CS$<>8__locals1.caravan.Tile;
			Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(CS$<>8__locals1.<CallShuttleToCaravan>g__ChoseWorldTarget|1), true, CompLaunchable.TargeterMouseAttachment, false, delegate
			{
				if (CS$<>8__locals1.maxLaunchDistance > 0)
				{
					PlanetTile center = CS$<>8__locals1.caravanTile;
					if (CS$<>8__locals1.caravanTile.Layer != Find.WorldSelector.SelectedLayer)
					{
						center = Find.WorldSelector.SelectedLayer.GetClosestTile_NewTemp(CS$<>8__locals1.caravanTile, false);
					}
					GenDraw.DrawWorldRadiusRing(center, CS$<>8__locals1.maxLaunchDistance, null);
				}
			}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, CS$<>8__locals1.caravanTile, CS$<>8__locals1.maxLaunchDistance, Gen.YieldSingle<Caravan>(CS$<>8__locals1.caravan), new Action<PlanetTile, TransportersArrivalAction>(base.<CallShuttleToCaravan>g__Launch|0), null, null), null, null, false);
		}

		// Token: 0x060083FE RID: 33790 RVA: 0x002F7424 File Offset: 0x002F5624
		public static void DrawShuttleGhost(LocalTargetInfo target, Map map, ThingDef shuttleDef, Rot4 rot)
		{
			Color ghostCol = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, map, shuttleDef, new Rot4?(rot)).Accepted ? Designator_Place.CanPlaceColor : Designator_Place.CannotPlaceColor;
			GhostDrawer.DrawGhostThing(target.Cell, rot, shuttleDef, shuttleDef.graphic, ghostCol, AltitudeLayer.Blueprint, null, true, null);
			Vector3 position = ThingUtility.InteractionCellWhenAt(shuttleDef, target.Cell, rot, map).ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
		}

		// Token: 0x060083FF RID: 33791 RVA: 0x002F74A0 File Offset: 0x002F56A0
		public static AcceptanceReport ShuttleCanLandHere(LocalTargetInfo target, Map map, ThingDef shuttleDef = null, Rot4? rot = null)
		{
			TaggedString t = "ShuttleCannotLand".Translate() + ": ";
			if (!target.IsValid)
			{
				return new AcceptanceReport(t + "MessageTransportPodsDestinationIsInvalid".Translate().CapitalizeFirst());
			}
			if (shuttleDef == null)
			{
				shuttleDef = ThingDefOf.Shuttle;
			}
			Rot4 value = rot.GetValueOrDefault();
			if (rot == null)
			{
				value = shuttleDef.defaultPlacingRot;
				rot = new Rot4?(value);
			}
			foreach (IntVec3 cell in GenAdj.OccupiedRect(target.Cell, rot.Value, shuttleDef.size))
			{
				string reportFromCell = RoyalTitlePermitWorker_CallShuttle.GetReportFromCell(cell, map, false, shuttleDef);
				if (reportFromCell != null)
				{
					return new AcceptanceReport(t + reportFromCell);
				}
			}
			string reportFromCell2 = RoyalTitlePermitWorker_CallShuttle.GetReportFromCell(ThingUtility.InteractionCellWhenAt(shuttleDef, target.Cell, rot.Value, map), map, true, shuttleDef);
			if (reportFromCell2 != null)
			{
				return new AcceptanceReport(t + reportFromCell2);
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06008400 RID: 33792 RVA: 0x002F75D0 File Offset: 0x002F57D0
		private static string GetReportFromCell(IntVec3 cell, Map map, bool interactionSpot, ThingDef shuttleDef)
		{
			if (!cell.InBounds(map))
			{
				return "OutOfBounds".Translate().CapitalizeFirst();
			}
			if (cell.Fogged(map))
			{
				return "ShuttleCannotLand_Fogged".Translate().CapitalizeFirst();
			}
			if (!cell.Walkable(map))
			{
				return "ShuttleCannotLand_Unwalkable".Translate().CapitalizeFirst();
			}
			if (!cell.GetAffordances(map).Contains(ThingDefOf.Shuttle.terrainAffordanceNeeded))
			{
				return "ShuttleCannotLand_Unaffordable".Translate(shuttleDef.label).CapitalizeFirst();
			}
			RoofDef roof = cell.GetRoof(map);
			if (roof != null && (roof.isNatural || roof.isThickRoof))
			{
				return "MessageTransportPodsDestinationIsInvalid".Translate().CapitalizeFirst();
			}
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is IActiveTransporter || thing is Skyfaller || (thing.def.category == ThingCategory.Building && !thing.def.building.isPowerConduit))
				{
					return "BlockedBy".Translate(thing).CapitalizeFirst();
				}
				PlantProperties plant = thing.def.plant;
				if (plant != null && plant.IsTree)
				{
					return "BlockedBy".Translate(thing).CapitalizeFirst();
				}
			}
			return null;
		}

		// Token: 0x04005182 RID: 20866
		private Faction calledFaction;

		// Token: 0x04005183 RID: 20867
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallShuttle", true);
	}
}
