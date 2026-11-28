using System;

namespace Verse
{
	// Token: 0x020006D3 RID: 1747
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06003722 RID: 14114 RVA: 0x00161CE2 File Offset: 0x0015FEE2
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x00161CEC File Offset: 0x0015FEEC
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			base.MakeMote(A, B, overrideSpawnTick);
		}
	}
}
