using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200219B RID: 8603
	public class QuestNode_AddShipJob_Arrive : QuestNode_AddShipJob
	{
		// Token: 0x170025FD RID: 9725
		// (get) Token: 0x0600D6A6 RID: 54950 RVA: 0x004FB644 File Offset: 0x004F9844
		protected override ShipJobDef DefaultShipJobDef
		{
			get
			{
				return ShipJobDefOf.Arrive;
			}
		}

		// Token: 0x0600D6A7 RID: 54951 RVA: 0x004FB64C File Offset: 0x004F984C
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_Arrive shipJob_Arrive = shipJob as ShipJob_Arrive;
			if (shipJob_Arrive != null)
			{
				List<Pawn> list;
				if (this.forPawns.TryGetValue(slate, out list) && !list.NullOrEmpty<Pawn>())
				{
					shipJob_Arrive.mapOfPawn = list.First<Pawn>();
				}
				else
				{
					Map map = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
					shipJob_Arrive.mapParent = map.Parent;
				}
				IntVec3? value = this.landingCell.GetValue(slate);
				IntVec3? intVec = (value != null) ? value : slate.Get<IntVec3?>("landingCell", null, false);
				if (intVec != null)
				{
					shipJob_Arrive.cell = intVec.Value;
				}
			}
		}

		// Token: 0x040087F4 RID: 34804
		public SlateRef<Map> map;

		// Token: 0x040087F5 RID: 34805
		public SlateRef<IntVec3?> landingCell;

		// Token: 0x040087F6 RID: 34806
		public SlateRef<List<Pawn>> forPawns;
	}
}
