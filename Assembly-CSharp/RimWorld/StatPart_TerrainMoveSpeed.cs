using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FAA RID: 8106
	public class StatPart_TerrainMoveSpeed : StatPart
	{
		// Token: 0x0600CDD8 RID: 52696 RVA: 0x004B9FFE File Offset: 0x004B81FE
		public override void TransformValue(StatRequest req, ref float val)
		{
		}

		// Token: 0x0600CDD9 RID: 52697 RVA: 0x004BA000 File Offset: 0x004B8200
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					if (pawn.kindDef.moveSpeedFactorByTerrainTag == null)
					{
						return null;
					}
					StringBuilder stringBuilder = new StringBuilder();
					foreach (KeyValuePair<string, float> keyValuePair in pawn.kindDef.moveSpeedFactorByTerrainTag)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						TaggedString value = ("TerrainTag" + keyValuePair.Key).Translate();
						stringBuilder.AppendLine("StatsReport_TerrainSpeedMultiplier".Translate(value) + ": x" + keyValuePair.Value.ToStringPercent());
					}
					return stringBuilder.ToString();
				}
			}
			return null;
		}
	}
}
