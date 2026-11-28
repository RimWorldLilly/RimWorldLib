using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD2 RID: 3538
	public class ThoughtWorker_TreesDesired : ThoughtWorker_Precept
	{
		// Token: 0x06006077 RID: 24695 RVA: 0x0022F6C6 File Offset: 0x0022D8C6
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.surroundings == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(this.ThoughtStageIndex(p));
		}

		// Token: 0x06006078 RID: 24696 RVA: 0x0022F6E4 File Offset: 0x0022D8E4
		private int ThoughtStageIndex(Pawn p)
		{
			ThoughtWorker_TreesDesired.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.p = p;
			int num = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Super, 2500, ref CS$<>8__locals1);
			if (num > 1)
			{
				return 0;
			}
			if (num == 1)
			{
				return 1;
			}
			int num2 = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Super, 15000, ref CS$<>8__locals1);
			int num3 = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Full, 15000, ref CS$<>8__locals1);
			if (num2 >= 1 && num3 >= 3)
			{
				return 2;
			}
			if (num2 >= 1)
			{
				return 3;
			}
			if (num3 >= 5)
			{
				return 4;
			}
			if (num3 >= 3)
			{
				return 5;
			}
			int num4 = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Mini, 30000, ref CS$<>8__locals1);
			if (num3 >= 1 && num4 >= 1)
			{
				return 6;
			}
			if (num3 == 1)
			{
				return 7;
			}
			if (num4 >= 5)
			{
				return 8;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Mini, 60000, ref CS$<>8__locals1) >= 1)
			{
				return 9;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|1_1(120000, ref CS$<>8__locals1))
			{
				return 10;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|1_1(180000, ref CS$<>8__locals1))
			{
				return 11;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|1_1(240000, ref CS$<>8__locals1))
			{
				return 12;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|1_1(300000, ref CS$<>8__locals1))
			{
				return 13;
			}
			return 14;
		}

		// Token: 0x0600607A RID: 24698 RVA: 0x0022F7D3 File Offset: 0x0022D9D3
		[CompilerGenerated]
		internal static int <ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory category, int ticks, ref ThoughtWorker_TreesDesired.<>c__DisplayClass1_0 A_2)
		{
			return A_2.p.surroundings.NumSightingsInRange(category, ticks);
		}

		// Token: 0x0600607B RID: 24699 RVA: 0x0022F7E7 File Offset: 0x0022D9E7
		[CompilerGenerated]
		internal static bool <ThoughtStageIndex>g__AnySightingsInRange|1_1(int ticks, ref ThoughtWorker_TreesDesired.<>c__DisplayClass1_0 A_1)
		{
			return ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Super, ticks, ref A_1) > 0 || ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Full, ticks, ref A_1) > 0 || ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Mini, ticks, ref A_1) > 0;
		}
	}
}
