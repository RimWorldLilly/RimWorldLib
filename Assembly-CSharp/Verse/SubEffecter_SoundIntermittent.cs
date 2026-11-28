using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020006CE RID: 1742
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x06003713 RID: 14099 RVA: 0x00160ECE File Offset: 0x0015F0CE
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x00160EEC File Offset: 0x0015F0EC
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				this.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = this.def.intermittentSoundInterval.RandomInRange;
			}
		}

		// Token: 0x04002437 RID: 9271
		protected int ticksUntilSound;
	}
}
