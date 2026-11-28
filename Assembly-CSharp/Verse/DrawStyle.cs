using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000253 RID: 595
	public abstract class DrawStyle
	{
		// Token: 0x06001180 RID: 4480
		public abstract void Update(IntVec3 origin, IntVec3 target, List<IntVec3> buffer);

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x000647A2 File Offset: 0x000629A2
		public virtual bool CanHaveDuplicates
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x000647A5 File Offset: 0x000629A5
		public virtual bool SingleCell
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x000647A8 File Offset: 0x000629A8
		public virtual void Draw()
		{
		}
	}
}
