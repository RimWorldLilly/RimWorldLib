using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E16 RID: 7702
	public class Instruction_UndraftAll : Lesson_Instruction
	{
		// Token: 0x170024F0 RID: 9456
		// (get) Token: 0x0600C816 RID: 51222 RVA: 0x00496320 File Offset: 0x00494520
		protected override float ProgressPercent
		{
			get
			{
				return 1f - (float)this.DraftedPawns().Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount;
			}
		}

		// Token: 0x0600C817 RID: 51223 RVA: 0x00496346 File Offset: 0x00494546
		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		// Token: 0x0600C818 RID: 51224 RVA: 0x0049637C File Offset: 0x0049457C
		public override void LessonUpdate()
		{
			foreach (Pawn pawn in this.DraftedPawns())
			{
				GenDraw.DrawArrowPointingAt(pawn.DrawPos, false);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
