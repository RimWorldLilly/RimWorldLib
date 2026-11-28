using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FBC RID: 8124
	public class StatWorker_ColdContainmentBonus : StatWorker
	{
		// Token: 0x0600CE5E RID: 52830 RVA: 0x004BED7C File Offset: 0x004BCF7C
		public override bool ShouldShowFor(StatRequest req)
		{
			float num;
			return ContainmentUtility.ShowContainmentStats(req.Thing) && ContainmentUtility.TryGetColdContainmentBonus(req.Thing, out num);
		}

		// Token: 0x0600CE5F RID: 52831 RVA: 0x004BEDA8 File Offset: 0x004BCFA8
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			float num;
			ContainmentUtility.TryGetColdContainmentBonus(req.Thing, out num);
			return num - 1f;
		}
	}
}
