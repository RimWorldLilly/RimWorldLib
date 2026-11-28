using System;
using RimWorld.Planet;

namespace RimWorld.QuestGen
{
	// Token: 0x0200219E RID: 8606
	public class QuestNode_AddShipJob_FlyAway : QuestNode_AddShipJob
	{
		// Token: 0x170025FF RID: 9727
		// (get) Token: 0x0600D6AE RID: 54958 RVA: 0x004FB7AA File Offset: 0x004F99AA
		protected override ShipJobDef DefaultShipJobDef
		{
			get
			{
				return ShipJobDefOf.FlyAway;
			}
		}

		// Token: 0x0600D6AF RID: 54959 RVA: 0x004FB7B4 File Offset: 0x004F99B4
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_FlyAway shipJob_FlyAway = shipJob as ShipJob_FlyAway;
			if (shipJob_FlyAway != null)
			{
				MapParent value = this.destination.GetValue(slate);
				if (value != null)
				{
					shipJob_FlyAway.destinationTile = value.Tile;
				}
				shipJob_FlyAway.dropMode = this.unsatisfiedDropMode.GetValue(slate).GetValueOrDefault(TransportShipDropMode.NonRequired);
			}
		}

		// Token: 0x040087FB RID: 34811
		public SlateRef<MapParent> destination;

		// Token: 0x040087FC RID: 34812
		public SlateRef<TransportShipDropMode?> unsatisfiedDropMode;
	}
}
