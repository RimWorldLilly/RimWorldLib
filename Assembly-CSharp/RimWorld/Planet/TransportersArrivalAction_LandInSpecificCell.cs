using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023DA RID: 9178
	public class TransportersArrivalAction_LandInSpecificCell : TransportersArrivalAction
	{
		// Token: 0x17002842 RID: 10306
		// (get) Token: 0x0600E4C3 RID: 58563 RVA: 0x0054FA6E File Offset: 0x0054DC6E
		public override bool GeneratesMap
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E4C4 RID: 58564 RVA: 0x0054FA71 File Offset: 0x0054DC71
		public TransportersArrivalAction_LandInSpecificCell()
		{
		}

		// Token: 0x0600E4C5 RID: 58565 RVA: 0x0054FA79 File Offset: 0x0054DC79
		public TransportersArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell)
		{
			this.mapParent = mapParent;
			this.cell = cell;
		}

		// Token: 0x0600E4C6 RID: 58566 RVA: 0x0054FA8F File Offset: 0x0054DC8F
		public TransportersArrivalAction_LandInSpecificCell(MapParent mapParent, IntVec3 cell, Rot4 rotation, bool landInShuttle)
		{
			this.mapParent = mapParent;
			this.cell = cell;
			this.rotation = rotation;
			this.landInShuttle = landInShuttle;
		}

		// Token: 0x0600E4C7 RID: 58567 RVA: 0x0054FAB4 File Offset: 0x0054DCB4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<Rot4>(ref this.rotation, "rotation", default(Rot4), false);
			Scribe_Values.Look<bool>(ref this.landInShuttle, "landInShuttle", false, false);
		}

		// Token: 0x0600E4C8 RID: 58568 RVA: 0x0054FB20 File Offset: 0x0054DD20
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, this.mapParent);
		}

		// Token: 0x0600E4C9 RID: 58569 RVA: 0x0054FB74 File Offset: 0x0054DD74
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
			if (this.landInShuttle)
			{
				if (transporters.Count > 1)
				{
					Log.Error("Shuttles can only have one transporter in group");
				}
				TransportersArrivalActionUtility.DropShuttle(transporters.FirstOrDefault<ActiveTransporterInfo>(), this.mapParent.Map, this.cell, new Rot4?(this.rotation), null);
				Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
				return;
			}
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, this.cell, this.mapParent.Map);
			Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x0600E4CA RID: 58570 RVA: 0x0054FC24 File Offset: 0x0054DE24
		public static bool CanLandInSpecificCell(IEnumerable<IThingHolder> pods, MapParent mapParent)
		{
			return mapParent != null && mapParent.Spawned && mapParent.HasMap && (!mapParent.EnterCooldownBlocksEntering() || FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(mapParent.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false))));
		}

		// Token: 0x0400916D RID: 37229
		private MapParent mapParent;

		// Token: 0x0400916E RID: 37230
		private IntVec3 cell;

		// Token: 0x0400916F RID: 37231
		private Rot4 rotation;

		// Token: 0x04009170 RID: 37232
		private bool landInShuttle;
	}
}
