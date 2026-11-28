using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F91 RID: 3985
	public class RoomPartDef : Def
	{
		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x060065C0 RID: 26048 RVA: 0x0024384C File Offset: 0x00241A4C
		public RoomPartWorker Worker
		{
			get
			{
				RoomPartWorker result;
				if ((result = this.cachedWorker) == null)
				{
					result = (this.cachedWorker = (RoomPartWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					}));
				}
				return result;
			}
		}

		// Token: 0x060065C1 RID: 26049 RVA: 0x00243886 File Offset: 0x00241A86
		public override IEnumerable<string> ConfigErrors()
		{
			RoomPartDef.<ConfigErrors>d__4 <ConfigErrors>d__ = new RoomPartDef.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003CD2 RID: 15570
		protected Type workerClass;

		// Token: 0x04003CD3 RID: 15571
		private RoomPartWorker cachedWorker;
	}
}
