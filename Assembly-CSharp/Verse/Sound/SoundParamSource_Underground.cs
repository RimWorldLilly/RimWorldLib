using System;

namespace Verse.Sound
{
	// Token: 0x020007CE RID: 1998
	public class SoundParamSource_Underground : SoundParamSource
	{
		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06003FCC RID: 16332 RVA: 0x00197E7C File Offset: 0x0019607C
		public override string Label
		{
			get
			{
				return "Underground";
			}
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x00197E84 File Offset: 0x00196084
		public override float ValueFor(Sample samp)
		{
			Map currentMap = Find.CurrentMap;
			bool flag;
			if (currentMap == null)
			{
				flag = false;
			}
			else
			{
				MapGeneratorDef generatorDef = currentMap.generatorDef;
				flag = ((generatorDef != null) ? new bool?(generatorDef.isUnderground) : null).GetValueOrDefault();
			}
			if (!flag)
			{
				return 0f;
			}
			return 1f;
		}
	}
}
