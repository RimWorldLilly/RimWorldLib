using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000C9 RID: 201
	public class DeathActionProperties_Divide : DeathActionProperties
	{
		// Token: 0x060007B0 RID: 1968 RVA: 0x0002C63E File Offset: 0x0002A83E
		public DeathActionProperties_Divide()
		{
			this.workerClass = typeof(DeathActionWorker_Divide);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0002C66C File Offset: 0x0002A86C
		public override IEnumerable<string> ConfigErrors()
		{
			DeathActionProperties_Divide.<ConfigErrors>d__5 <ConfigErrors>d__ = new DeathActionProperties_Divide.<ConfigErrors>d__5(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04000367 RID: 871
		public List<PawnKindDef> dividePawnKindOptions = new List<PawnKindDef>();

		// Token: 0x04000368 RID: 872
		public int dividePawnCount;

		// Token: 0x04000369 RID: 873
		public List<PawnKindDef> dividePawnKindAdditionalForced = new List<PawnKindDef>();

		// Token: 0x0400036A RID: 874
		public IntRange divideBloodFilthCountRange;
	}
}
