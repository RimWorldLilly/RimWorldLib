using System;

namespace Verse
{
	// Token: 0x020003EB RID: 1003
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x06001D71 RID: 7537 RVA: 0x000B46E6 File Offset: 0x000B28E6
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
