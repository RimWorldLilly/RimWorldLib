using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F8B RID: 8075
	public class StatPart_GrowthVatSpeedFactor : StatPart
	{
		// Token: 0x0600CD56 RID: 52566 RVA: 0x004B8398 File Offset: 0x004B6598
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600CD57 RID: 52567 RVA: 0x004B83B8 File Offset: 0x004B65B8
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetFactor(req, out f))
			{
				return "StatsReport_GrowthVatSpeedFactor".Translate() + ": " + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD58 RID: 52568 RVA: 0x004B83F8 File Offset: 0x004B65F8
		private bool TryGetFactor(StatRequest req, out float value)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Ideo != null)
				{
					value = 1f;
					foreach (Precept precept in pawn.Ideo.PreceptsListForReading)
					{
						value *= precept.def.growthVatSpeedFactor;
					}
					return true;
				}
			}
			value = 0f;
			return false;
		}
	}
}
