using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D99 RID: 3481
	public class ThoughtWorker_Precept_RecentConquest : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x06005FD9 RID: 24537 RVA: 0x0022D86C File Offset: 0x0022BA6C
		private float DaysSinceLastRaid
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - Find.History.lastTickPlayerRaidedSomeone) / 60000f;
			}
		}

		// Token: 0x06005FDA RID: 24538 RVA: 0x0022D88C File Offset: 0x0022BA8C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			if (p.IsSlave)
			{
				return false;
			}
			int num = (this.DaysSinceLastRaid > (float)this.DaysSinceLastRaidThreshold) ? 1 : 0;
			if (num == 1)
			{
				int num2 = Find.TickManager.SettleTick;
				if (p.Ideo.Fluid && p.Ideo.development.lastTickRaidingApproved > num2)
				{
					num2 = p.Ideo.development.lastTickRaidingApproved;
				}
				if ((float)(Find.TickManager.TicksGame - num2) < 1800000f)
				{
					return false;
				}
			}
			return ThoughtState.ActiveAtStage(num);
		}

		// Token: 0x06005FDB RID: 24539 RVA: 0x0022D928 File Offset: 0x0022BB28
		public override float MoodMultiplier(Pawn p)
		{
			return ThoughtWorker_Precept_RecentConquest.MoodMultiplierCurve.Evaluate(this.DaysSinceLastRaid);
		}

		// Token: 0x06005FDC RID: 24540 RVA: 0x0022D93A File Offset: 0x0022BB3A
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			ThoughtWorker_Precept_RecentConquest.<GetDescriptionArgs>d__6 <GetDescriptionArgs>d__ = new ThoughtWorker_Precept_RecentConquest.<GetDescriptionArgs>d__6(-2);
			<GetDescriptionArgs>d__.<>4__this = this;
			return <GetDescriptionArgs>d__;
		}

		// Token: 0x040035DE RID: 13790
		private static readonly SimpleCurve MoodMultiplierCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(25f, 0f),
				true
			},
			{
				new CurvePoint(50f, 1f),
				true
			},
			{
				new CurvePoint(75f, 1.5f),
				true
			}
		};

		// Token: 0x040035DF RID: 13791
		private int DaysSinceLastRaidThreshold = 25;
	}
}
