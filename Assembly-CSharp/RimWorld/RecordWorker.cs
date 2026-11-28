using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000FD6 RID: 4054
	public class RecordWorker
	{
		// Token: 0x060066C5 RID: 26309 RVA: 0x002476E8 File Offset: 0x002458E8
		public virtual bool ShouldMeasureTimeNow(Pawn pawn)
		{
			if (this.def.measuredTimeJobs == null)
			{
				return false;
			}
			Job curJob = pawn.CurJob;
			if (curJob == null)
			{
				return false;
			}
			for (int i = 0; i < this.def.measuredTimeJobs.Count; i++)
			{
				if (curJob.def == this.def.measuredTimeJobs[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003FE5 RID: 16357
		public RecordDef def;
	}
}
