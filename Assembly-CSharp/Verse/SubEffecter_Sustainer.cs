using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020006D6 RID: 1750
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x06003729 RID: 14121 RVA: 0x00161D9D File Offset: 0x0015FF9D
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x00161DA8 File Offset: 0x0015FFA8
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.age++;
			if (this.age > this.def.ticksBeforeSustainerStart)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					SoundInfo info = SoundInfo.InMap(A, MaintenanceType.PerTick);
					this.sustainer = this.def.soundDef.TrySpawnSustainer(info);
					return;
				}
				this.sustainer.Maintain();
			}
		}

		// Token: 0x04002440 RID: 9280
		private int age;

		// Token: 0x04002441 RID: 9281
		private Sustainer sustainer;
	}
}
