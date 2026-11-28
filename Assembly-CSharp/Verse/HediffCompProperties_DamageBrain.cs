using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000419 RID: 1049
	public class HediffCompProperties_DamageBrain : HediffCompProperties
	{
		// Token: 0x06001ED5 RID: 7893 RVA: 0x000BCD2B File Offset: 0x000BAF2B
		public HediffCompProperties_DamageBrain()
		{
			this.compClass = typeof(HediffComp_DamageBrain);
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000BCD4E File Offset: 0x000BAF4E
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties_DamageBrain.<ConfigErrors>d__3 <ConfigErrors>d__ = new HediffCompProperties_DamageBrain.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04001831 RID: 6193
		public IntRange damageAmount = IntRange.Zero;

		// Token: 0x04001832 RID: 6194
		public List<float> mtbDaysPerStage;
	}
}
