using System;

namespace Verse
{
	// Token: 0x020000D9 RID: 217
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x06000846 RID: 2118 RVA: 0x0002FED7 File Offset: 0x0002E0D7
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0002FEE1 File Offset: 0x0002E0E1
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (Rand.Chance(this.def.chancePerTick))
			{
				base.MakeMote(A, -1);
			}
		}
	}
}
