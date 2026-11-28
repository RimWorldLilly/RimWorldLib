using System;

namespace Verse.AI
{
	// Token: 0x020008B7 RID: 2231
	public abstract class ThinkNode_ChancePerHour : ThinkNode_Priority
	{
		// Token: 0x06004587 RID: 17799 RVA: 0x001B6F88 File Offset: 0x001B5188
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (Find.TickManager.TicksGame < this.GetLastTryTick(pawn) + 2500)
			{
				return ThinkResult.NoJob;
			}
			this.SetLastTryTick(pawn, Find.TickManager.TicksGame);
			float num = this.MtbHours(pawn);
			if (num <= 0f)
			{
				return ThinkResult.NoJob;
			}
			Rand.PushState();
			int salt = Gen.HashCombineInt(base.UniqueSaveKey, 26504059);
			Rand.Seed = pawn.RandSeedForHour(salt);
			bool flag = Rand.MTBEventOccurs(num, 2500f, 2500f);
			Rand.PopState();
			if (flag)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x06004588 RID: 17800
		protected abstract float MtbHours(Pawn pawn);

		// Token: 0x06004589 RID: 17801 RVA: 0x001B7024 File Offset: 0x001B5224
		private int GetLastTryTick(Pawn pawn)
		{
			int result;
			if (pawn.mindState.thinkData.TryGetValue(base.UniqueSaveKey, out result))
			{
				return result;
			}
			return -99999;
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x001B7052 File Offset: 0x001B5252
		private void SetLastTryTick(Pawn pawn, int val)
		{
			pawn.mindState.thinkData[base.UniqueSaveKey] = val;
		}
	}
}
