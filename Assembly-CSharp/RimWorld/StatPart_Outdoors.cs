using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F97 RID: 8087
	public class StatPart_Outdoors : StatPart
	{
		// Token: 0x0600CD88 RID: 52616 RVA: 0x004B8C22 File Offset: 0x004B6E22
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		// Token: 0x0600CD89 RID: 52617 RVA: 0x004B8C30 File Offset: 0x004B6E30
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing.GetRoom(RegionType.Set_All) != null)
			{
				string str;
				if (this.ConsideredOutdoors(req))
				{
					str = "Outdoors".Translate().CapitalizeFirst();
				}
				else
				{
					str = "Indoors".Translate();
				}
				return str + ": x" + this.OutdoorsFactor(req).ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD8A RID: 52618 RVA: 0x004B8CA2 File Offset: 0x004B6EA2
		private float OutdoorsFactor(StatRequest req)
		{
			if (this.ConsideredOutdoors(req))
			{
				return this.factorOutdoors;
			}
			return this.factorIndoors;
		}

		// Token: 0x0600CD8B RID: 52619 RVA: 0x004B8CBC File Offset: 0x004B6EBC
		private bool ConsideredOutdoors(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					return room.OutdoorsForWork || (req.HasThing && req.Thing.Spawned && !req.Thing.Map.roofGrid.Roofed(req.Thing.Position));
				}
			}
			return false;
		}

		// Token: 0x0400830F RID: 33551
		private float factorIndoors = 1f;

		// Token: 0x04008310 RID: 33552
		private float factorOutdoors = 1f;
	}
}
