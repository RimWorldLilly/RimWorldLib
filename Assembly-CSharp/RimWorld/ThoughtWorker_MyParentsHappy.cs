using System;

namespace RimWorld
{
	// Token: 0x02000E84 RID: 3716
	public class ThoughtWorker_MyParentsHappy : ThoughtWorker_MyParentHappy
	{
		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x0600624B RID: 25163 RVA: 0x00235670 File Offset: 0x00233870
		protected override int RequiredParentCount
		{
			get
			{
				return 2;
			}
		}
	}
}
