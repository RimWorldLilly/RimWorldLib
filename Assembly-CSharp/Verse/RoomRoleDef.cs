using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001B1 RID: 433
	public class RoomRoleDef : Def
	{
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x000402EC File Offset: 0x0003E4EC
		public RoomRoleWorker Worker
		{
			get
			{
				RoomRoleWorker result;
				if ((result = this.workerInt) == null)
				{
					result = (this.workerInt = (RoomRoleWorker)Activator.CreateInstance(this.workerClass));
				}
				return result;
			}
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0004031C File Offset: 0x0003E51C
		public string PostProcessedLabel(Room room)
		{
			return this.Worker.PostProcessedLabel(this.label, room);
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00040330 File Offset: 0x0003E530
		public string PostProcessedLabelCap(Room room)
		{
			return this.PostProcessedLabel(room).CapitalizeFirst();
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00040340 File Offset: 0x0003E540
		public bool IsStatRelated(RoomStatDef def)
		{
			if (this.relatedStats == null)
			{
				return false;
			}
			for (int i = 0; i < this.relatedStats.Count; i++)
			{
				if (this.relatedStats[i] == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000B6A RID: 2922
		public Type workerClass;

		// Token: 0x04000B6B RID: 2923
		public bool avoidViewingArtIfUnowned;

		// Token: 0x04000B6C RID: 2924
		private List<RoomStatDef> relatedStats;

		// Token: 0x04000B6D RID: 2925
		private RoomRoleWorker workerInt;
	}
}
