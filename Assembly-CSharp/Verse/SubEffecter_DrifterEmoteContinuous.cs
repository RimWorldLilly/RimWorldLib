using System;

namespace Verse
{
	// Token: 0x020000D8 RID: 216
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x06000844 RID: 2116 RVA: 0x0002FE9B File Offset: 0x0002E09B
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002FEA5 File Offset: 0x0002E0A5
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, -1);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x040003B9 RID: 953
		private int ticksUntilMote;
	}
}
