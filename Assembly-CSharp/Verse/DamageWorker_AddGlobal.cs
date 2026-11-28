using System;

namespace Verse
{
	// Token: 0x020003F3 RID: 1011
	public class DamageWorker_AddGlobal : DamageWorker
	{
		// Token: 0x06001D88 RID: 7560 RVA: 0x000B4EB8 File Offset: 0x000B30B8
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn, null);
				hediff.Severity = dinfo.Amount;
				pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
			}
			return new DamageWorker.DamageResult();
		}
	}
}
