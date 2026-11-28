using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F71 RID: 8049
	public class StatPart_BiosculptingSpeedFactor : StatPart
	{
		// Token: 0x0600CCEB RID: 52459 RVA: 0x004B6854 File Offset: 0x004B4A54
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600CCEC RID: 52460 RVA: 0x004B6874 File Offset: 0x004B4A74
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetFactor(req, out f))
			{
				return "StatsReport_BiosculptingSpeedFactor".Translate() + ": " + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CCED RID: 52461 RVA: 0x004B68B4 File Offset: 0x004B4AB4
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
						value *= precept.def.biosculpterPodCycleSpeedFactor;
					}
					return true;
				}
			}
			value = 0f;
			return false;
		}
	}
}
