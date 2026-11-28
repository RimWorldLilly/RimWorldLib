using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200235C RID: 9052
	public class DebugArena : WorldObjectComp
	{
		// Token: 0x0600E05B RID: 57435 RVA: 0x00537120 File Offset: 0x00535320
		public DebugArena()
		{
			this.tickCreated = Find.TickManager.TicksGame;
		}

		// Token: 0x0600E05C RID: 57436 RVA: 0x00537138 File Offset: 0x00535338
		public override void CompTickInterval(int delta)
		{
			if (this.lhs == null || this.rhs == null)
			{
				Log.ErrorOnce("DebugArena improperly set up", 73785616);
				return;
			}
			if ((this.tickFightStarted == 0 && Find.TickManager.TicksGame - this.tickCreated > 10000) || (this.tickFightStarted != 0 && Find.TickManager.TicksGame - this.tickFightStarted > 60000))
			{
				Log.Message("Fight timed out");
				ArenaUtility.ArenaResult obj = default(ArenaUtility.ArenaResult);
				obj.tickDuration = Find.TickManager.TicksGame - this.tickCreated;
				obj.winner = ArenaUtility.ArenaResult.Winner.Other;
				this.callback(obj);
				this.parent.Destroy();
				return;
			}
			if (this.tickFightStarted == 0)
			{
				foreach (Pawn pawn3 in this.lhs.Concat(this.rhs))
				{
					if (pawn3.records.GetValue(RecordDefOf.ShotsFired) > 0f || (pawn3.CurJob != null && pawn3.CurJob.def == JobDefOf.AttackMelee && pawn3.Position.DistanceTo(pawn3.CurJob.targetA.Thing.Position) <= 2f))
					{
						this.tickFightStarted = Find.TickManager.TicksGame;
						break;
					}
				}
			}
			if (this.tickFightStarted != 0)
			{
				bool flag = !this.lhs.Any((Pawn pawn) => !pawn.Dead && !pawn.Downed && pawn.Spawned);
				bool flag2 = !this.rhs.Any((Pawn pawn) => !pawn.Dead && !pawn.Downed && pawn.Spawned);
				if (flag || flag2)
				{
					ArenaUtility.ArenaResult obj2 = default(ArenaUtility.ArenaResult);
					obj2.tickDuration = Find.TickManager.TicksGame - this.tickFightStarted;
					if (flag && !flag2)
					{
						obj2.winner = ArenaUtility.ArenaResult.Winner.Rhs;
					}
					else if (!flag && flag2)
					{
						obj2.winner = ArenaUtility.ArenaResult.Winner.Lhs;
					}
					else
					{
						obj2.winner = ArenaUtility.ArenaResult.Winner.Other;
					}
					this.callback(obj2);
					foreach (Pawn pawn2 in this.lhs.Concat(this.rhs))
					{
						if (!pawn2.Destroyed)
						{
							pawn2.Destroy(DestroyMode.Vanish);
						}
					}
					this.parent.Destroy();
				}
			}
		}

		// Token: 0x04008FFC RID: 36860
		public List<Pawn> lhs;

		// Token: 0x04008FFD RID: 36861
		public List<Pawn> rhs;

		// Token: 0x04008FFE RID: 36862
		public Action<ArenaUtility.ArenaResult> callback;

		// Token: 0x04008FFF RID: 36863
		private int tickCreated;

		// Token: 0x04009000 RID: 36864
		private int tickFightStarted;
	}
}
