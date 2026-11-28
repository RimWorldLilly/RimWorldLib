using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200236F RID: 9071
	public class AbandonedArchotechStructures : WorldObject
	{
		// Token: 0x0600E12B RID: 57643 RVA: 0x00539C50 File Offset: 0x00537E50
		public Settlement GenerateSettlementAndDestroy()
		{
			this.Destroy();
			ArchotechSettlement archotechSettlement;
			if (this.worldObjectDef != null)
			{
				archotechSettlement = (ArchotechSettlement)WorldObjectMaker.MakeWorldObject(this.worldObjectDef);
			}
			else
			{
				archotechSettlement = (ArchotechSettlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement_SecondArchonexusCycle);
				Log.Warning("No archotech settlement configured for abandoned archotech structures, using default settlement def.");
			}
			archotechSettlement.existingBuildings.AddRange(this.archotechStructures);
			return archotechSettlement;
		}

		// Token: 0x0600E12C RID: 57644 RVA: 0x00539CAA File Offset: 0x00537EAA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WorldObjectDef>(ref this.worldObjectDef, "worldObjectDef");
			Scribe_Collections.Look<Building>(ref this.archotechStructures, "archotechStructures", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04009034 RID: 36916
		public WorldObjectDef worldObjectDef;

		// Token: 0x04009035 RID: 36917
		public List<Building> archotechStructures = new List<Building>();
	}
}
