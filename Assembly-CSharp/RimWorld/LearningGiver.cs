using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B8E RID: 2958
	public class LearningGiver
	{
		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x060055CE RID: 21966 RVA: 0x001FF1FD File Offset: 0x001FD3FD
		public virtual bool CanGiveDesire
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060055CF RID: 21967 RVA: 0x001FF200 File Offset: 0x001FD400
		public virtual bool CanDo(Pawn pawn)
		{
			return ModsConfig.BiotechActive && (pawn.DevelopmentalStage.Child() && !pawn.Downed && !PawnUtility.WillSoonHaveBasicNeed(pawn, 0.1f) && pawn.needs.learning != null) && pawn.needs.learning.CurLevel < 0.9f;
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x001FF25E File Offset: 0x001FD45E
		public virtual Job TryGiveJob(Pawn pawn)
		{
			return JobMaker.MakeJob(this.def.jobDef);
		}

		// Token: 0x040032F5 RID: 13045
		public const float NeedThresholdOffsetStart = 0.1f;

		// Token: 0x040032F6 RID: 13046
		public const float NeedThresholdOffsetStop = -0.05f;

		// Token: 0x040032F7 RID: 13047
		public LearningDesireDef def;
	}
}
