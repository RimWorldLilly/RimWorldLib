using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F7B RID: 8059
	public class StatPart_CorpseCasket : StatPart
	{
		// Token: 0x0600CD0C RID: 52492 RVA: 0x004B6EE1 File Offset: 0x004B50E1
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ApplyTo(req))
			{
				val += (float)this.offsetOccupied;
			}
		}

		// Token: 0x0600CD0D RID: 52493 RVA: 0x004B6EF8 File Offset: 0x004B50F8
		public override string ExplanationPart(StatRequest req)
		{
			if (!this.ApplyTo(req))
			{
				return null;
			}
			return "StatsReport_OccupiedCorpseCasket".Translate() + ": " + this.offsetOccupied.ToString();
		}

		// Token: 0x0600CD0E RID: 52494 RVA: 0x004B6F30 File Offset: 0x004B5130
		private bool ApplyTo(StatRequest req)
		{
			Building_CorpseCasket building_CorpseCasket = req.Thing as Building_CorpseCasket;
			return building_CorpseCasket != null && building_CorpseCasket.HasCorpse && this.offsetOccupied != 0 && (this.thingDefs == null || this.thingDefs.Contains(req.Thing.def));
		}

		// Token: 0x040082F1 RID: 33521
		public int offsetOccupied;

		// Token: 0x040082F2 RID: 33522
		public List<ThingDef> thingDefs;
	}
}
