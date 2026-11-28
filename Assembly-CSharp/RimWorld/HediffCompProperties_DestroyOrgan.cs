using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002086 RID: 8326
	public class HediffCompProperties_DestroyOrgan : HediffCompProperties
	{
		// Token: 0x0600D207 RID: 53767 RVA: 0x004D28B3 File Offset: 0x004D0AB3
		public HediffCompProperties_DestroyOrgan()
		{
			this.compClass = typeof(HediffComp_DestroyOrgan);
		}

		// Token: 0x04008513 RID: 34067
		public string messageText;

		// Token: 0x04008514 RID: 34068
		public DamageDef damageType;
	}
}
