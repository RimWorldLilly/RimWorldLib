using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC5 RID: 4037
	public struct PreceptThingChance
	{
		// Token: 0x0600667F RID: 26239 RVA: 0x002461E4 File Offset: 0x002443E4
		public static implicit operator PreceptThingChance(PreceptThingChanceClass c)
		{
			return new PreceptThingChance
			{
				chance = c.chance,
				def = c.def
			};
		}

		// Token: 0x04003F09 RID: 16137
		public ThingDef def;

		// Token: 0x04003F0A RID: 16138
		public float chance;
	}
}
