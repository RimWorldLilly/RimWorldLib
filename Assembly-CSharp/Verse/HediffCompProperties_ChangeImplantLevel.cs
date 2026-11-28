using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000413 RID: 1043
	public class HediffCompProperties_ChangeImplantLevel : HediffCompProperties
	{
		// Token: 0x06001EBD RID: 7869 RVA: 0x000BC914 File Offset: 0x000BAB14
		public HediffCompProperties_ChangeImplantLevel()
		{
			this.compClass = typeof(HediffComp_ChangeImplantLevel);
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000BC92C File Offset: 0x000BAB2C
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			HediffCompProperties_ChangeImplantLevel.<ConfigErrors>d__4 <ConfigErrors>d__ = new HediffCompProperties_ChangeImplantLevel.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04001824 RID: 6180
		public HediffDef implant;

		// Token: 0x04001825 RID: 6181
		public int levelOffset;

		// Token: 0x04001826 RID: 6182
		public List<ChangeImplantLevel_Probability> probabilityPerStage;
	}
}
