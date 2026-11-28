using System;

namespace Verse
{
	// Token: 0x020003EF RID: 1007
	public class DamageWorker_Stun : DamageWorker
	{
		// Token: 0x06001D7B RID: 7547 RVA: 0x000B4AB5 File Offset: 0x000B2CB5
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			damageResult.stunned = true;
			return damageResult;
		}
	}
}
