using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE7 RID: 3559
	public class ThoughtWorker_Precept_ResettledRecently : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x060060B0 RID: 24752 RVA: 0x002301C9 File Offset: 0x0022E3C9
		public int TicksSinceLastResettle
		{
			get
			{
				return GenTicks.TicksGame - Find.IdeoManager.lastResettledTick;
			}
		}

		// Token: 0x060060B1 RID: 24753 RVA: 0x002301DB File Offset: 0x0022E3DB
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (!p.IsColonist)
			{
				return false;
			}
			return Find.TickManager.TicksGame > Find.IdeoManager.lastResettledTick;
		}

		// Token: 0x060060B2 RID: 24754 RVA: 0x00230218 File Offset: 0x0022E418
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			float num = this.TicksSinceLastResettle.TicksToDays();
			foreach (ValueTuple<int, int> valueTuple in ThoughtWorker_Precept_ResettledRecently.Levels)
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (num < (float)item)
				{
					return (item2 < 0) ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(item2);
				}
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x060060B3 RID: 24755 RVA: 0x002302B8 File Offset: 0x0022E4B8
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			ThoughtWorker_Precept_ResettledRecently.<GetDescriptionArgs>d__5 <GetDescriptionArgs>d__ = new ThoughtWorker_Precept_ResettledRecently.<GetDescriptionArgs>d__5(-2);
			<GetDescriptionArgs>d__.<>4__this = this;
			return <GetDescriptionArgs>d__;
		}

		// Token: 0x040035FC RID: 13820
		[TupleElementNames(new string[]
		{
			"day",
			"index"
		})]
		private static readonly List<ValueTuple<int, int>> Levels = new List<ValueTuple<int, int>>
		{
			new ValueTuple<int, int>(5, 0),
			new ValueTuple<int, int>(10, 1),
			new ValueTuple<int, int>(20, -1),
			new ValueTuple<int, int>(30, 2),
			new ValueTuple<int, int>(int.MaxValue, 3)
		};
	}
}
