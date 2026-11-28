using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023DE RID: 9182
	public class TransportersArrivalAction_VisitSpace : TransportersArrivalAction
	{
		// Token: 0x17002844 RID: 10308
		// (get) Token: 0x0600E4DF RID: 58591 RVA: 0x00550290 File Offset: 0x0054E490
		public override bool GeneratesMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E4E0 RID: 58592 RVA: 0x00550293 File Offset: 0x0054E493
		public TransportersArrivalAction_VisitSpace()
		{
		}

		// Token: 0x0600E4E1 RID: 58593 RVA: 0x0055029B File Offset: 0x0054E49B
		public TransportersArrivalAction_VisitSpace(MapParent parent)
		{
			this.parent = parent;
		}

		// Token: 0x0600E4E2 RID: 58594 RVA: 0x005502AA File Offset: 0x0054E4AA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x0600E4E3 RID: 58595 RVA: 0x005502C4 File Offset: 0x0054E4C4
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.parent != null && this.parent.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_VisitSpace.CanVisit(pods, this.parent);
		}

		// Token: 0x0600E4E4 RID: 58596 RVA: 0x00550312 File Offset: 0x0054E512
		public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return !this.parent.HasMap;
		}

		// Token: 0x0600E4E5 RID: 58597 RVA: 0x00550324 File Offset: 0x0054E524
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
			IntVec3 size = Find.World.info.initialMapSize;
			if (this.parent.def.overrideMapSize != null)
			{
				size = this.parent.def.overrideMapSize.Value;
			}
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.parent.Tile, size, this.parent.def, null, false);
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(orGenerateMap, false, true);
			if (transporters.IsShuttle())
			{
				TransportersArrivalActionUtility.DropShuttle(transporters[0], orGenerateMap, near, null, null);
				Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
				return;
			}
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, near, orGenerateMap);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x0600E4E6 RID: 58598 RVA: 0x00550408 File Offset: 0x0054E608
		private static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, MapParent parent)
		{
			if (parent == null || !parent.Spawned)
			{
				return false;
			}
			if (!TransportersArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600E4E7 RID: 58599 RVA: 0x00550431 File Offset: 0x0054E631
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, MapParent parent)
		{
			TransportersArrivalAction_VisitSpace.<GetFloatMenuOptions>d__10 <GetFloatMenuOptions>d__ = new TransportersArrivalAction_VisitSpace.<GetFloatMenuOptions>d__10(-2);
			<GetFloatMenuOptions>d__.<>3__launchAction = launchAction;
			<GetFloatMenuOptions>d__.<>3__pods = pods;
			<GetFloatMenuOptions>d__.<>3__parent = parent;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x04009174 RID: 37236
		private MapParent parent;
	}
}
