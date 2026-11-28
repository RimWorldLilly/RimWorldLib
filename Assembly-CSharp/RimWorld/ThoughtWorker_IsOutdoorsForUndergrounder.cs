using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E38 RID: 3640
	public class ThoughtWorker_IsOutdoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x06006166 RID: 24934 RVA: 0x00232B0C File Offset: 0x00230D0C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (p.Position.UsesOutdoorTemperature(p.Map) || !p.Position.Roofed(p.Map));
		}
	}
}
