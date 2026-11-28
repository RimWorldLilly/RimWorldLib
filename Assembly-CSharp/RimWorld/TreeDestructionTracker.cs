using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200139F RID: 5023
	public class TreeDestructionTracker : IExposable
	{
		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x06007C81 RID: 31873 RVA: 0x002C7D66 File Offset: 0x002C5F66
		public int PlayerResponsibleTreeDestructionCount
		{
			get
			{
				this.DiscardOldEvents();
				return this.playerTreeDestructionTicks.Count;
			}
		}

		// Token: 0x06007C82 RID: 31874 RVA: 0x002C7D79 File Offset: 0x002C5F79
		public TreeDestructionTracker(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007C83 RID: 31875 RVA: 0x002C7D93 File Offset: 0x002C5F93
		public void Notify_TreeDestroyed(DamageInfo dInfo)
		{
			if (dInfo.Instigator != null && dInfo.Instigator.Faction == Faction.OfPlayer)
			{
				this.playerTreeDestructionTicks.Add(Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06007C84 RID: 31876 RVA: 0x002C7DC6 File Offset: 0x002C5FC6
		public void Notify_TreeCut(Pawn by)
		{
			if (by.Faction == Faction.OfPlayer)
			{
				this.playerTreeDestructionTicks.Add(Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06007C85 RID: 31877 RVA: 0x002C7DEC File Offset: 0x002C5FEC
		private void DiscardOldEvents()
		{
			int num = 0;
			while (num < this.playerTreeDestructionTicks.Count && Find.TickManager.TicksGame >= this.playerTreeDestructionTicks[num] + 900000)
			{
				this.playerTreeDestructionTicks.RemoveAt(num);
				num--;
				num++;
			}
		}

		// Token: 0x06007C86 RID: 31878 RVA: 0x002C7E3D File Offset: 0x002C603D
		public void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.playerTreeDestructionTicks, "playerTreeDestructionTicks", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x04004E62 RID: 20066
		private Map map;

		// Token: 0x04004E63 RID: 20067
		private List<int> playerTreeDestructionTicks = new List<int>();

		// Token: 0x04004E64 RID: 20068
		private const int DestructionsExpireInTicks = 900000;
	}
}
