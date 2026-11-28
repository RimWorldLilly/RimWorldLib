using System;

namespace Verse
{
	// Token: 0x020006D5 RID: 1749
	public class SubEffecter_SprayerTriggeredDelayed : SubEffecter_SprayerTriggered
	{
		// Token: 0x06003726 RID: 14118 RVA: 0x00161D2E File Offset: 0x0015FF2E
		public SubEffecter_SprayerTriggeredDelayed(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x00161D3F File Offset: 0x0015FF3F
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			this.ticksLeft = this.def.initialDelayTicks;
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x00161D54 File Offset: 0x0015FF54
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			bool flag = this.ticksLeft > 0;
			if (flag)
			{
				this.ticksLeft--;
			}
			if (this.ticksLeft <= 0 && flag)
			{
				base.MakeMote(A, B, -1);
			}
			base.SubEffectTick(A, B);
		}

		// Token: 0x0400243F RID: 9279
		private int ticksLeft = -1;
	}
}
