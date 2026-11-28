using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F6E RID: 8046
	public class StatPart_ArtificialBuildingsNearbyOffset : StatPart
	{
		// Token: 0x0600CCE0 RID: 52448 RVA: 0x004B6668 File Offset: 0x004B4868
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetArtificialBuildingOffset(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600CCE1 RID: 52449 RVA: 0x004B6688 File Offset: 0x004B4888
		private bool TryGetArtificialBuildingOffset(StatRequest req, out float offset)
		{
			if (!req.HasThing || !req.Thing.Spawned)
			{
				offset = 0f;
				return false;
			}
			offset = this.curve.Evaluate((float)req.Thing.Map.listerArtificialBuildingsForMeditation.GetForCell(req.Thing.Position, this.radius).Count);
			return true;
		}

		// Token: 0x0600CCE2 RID: 52450 RVA: 0x004B66F4 File Offset: 0x004B48F4
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetArtificialBuildingOffset(req, out f))
			{
				return "StatsReport_NearbyArtificialStructures".Translate().CapitalizeFirst() + ": " + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x040082ED RID: 33517
		public SimpleCurve curve;

		// Token: 0x040082EE RID: 33518
		public float radius = 10f;
	}
}
