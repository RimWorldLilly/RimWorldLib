using System;

namespace Verse
{
	// Token: 0x020006D4 RID: 1748
	public class SubEffecter_SprayerTriggeredChance : SubEffecter_Sprayer
	{
		// Token: 0x06003724 RID: 14116 RVA: 0x00161CF7 File Offset: 0x0015FEF7
		public SubEffecter_SprayerTriggeredChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x00161D04 File Offset: 0x0015FF04
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			float effectiveChancePerTick = base.EffectiveChancePerTick;
			if (Rand.Value < effectiveChancePerTick || force)
			{
				base.MakeMote(A, B, overrideSpawnTick);
			}
		}
	}
}
