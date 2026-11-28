using System;

namespace Verse
{
	// Token: 0x020006D1 RID: 1745
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x0600371E RID: 14110 RVA: 0x00161B4E File Offset: 0x0015FD4E
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilMote = def.initialDelayTicks;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x00161B64 File Offset: 0x0015FD64
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.moteCount >= this.def.maxMoteCount)
			{
				return;
			}
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, B, -1);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
				this.moteCount++;
			}
		}

		// Token: 0x0400243A RID: 9274
		private int ticksUntilMote;

		// Token: 0x0400243B RID: 9275
		private int moteCount;
	}
}
