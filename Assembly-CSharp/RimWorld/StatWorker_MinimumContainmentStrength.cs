using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC9 RID: 8137
	public class StatWorker_MinimumContainmentStrength : StatWorker
	{
		// Token: 0x0600CE99 RID: 52889 RVA: 0x004C0E22 File Offset: 0x004BF022
		public override bool ShouldShowFor(StatRequest req)
		{
			return ContainmentUtility.ShowContainmentStats(req.Thing);
		}
	}
}
