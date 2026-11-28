using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006CB RID: 1739
	public class SubEffecter_GroupedChance : SubEffecter
	{
		// Token: 0x0600370A RID: 14090 RVA: 0x00160C08 File Offset: 0x0015EE08
		public SubEffecter_GroupedChance(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
			this.ticksUntilEffect = this.def.initialDelayTicks;
			this.lifespanMaxTicks = Find.TickManager.TicksGame + this.def.lifespanMaxTicks + this.ticksUntilEffect;
			if (this.def.children != null)
			{
				for (int i = 0; i < this.def.children.Count; i++)
				{
					this.children.Add(this.def.children[i].Spawn(parent));
				}
			}
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x00160CA8 File Offset: 0x0015EEA8
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilEffect--;
			if (Find.TickManager.TicksGame >= this.lastEffectTicks + this.def.chancePeriodTicks && Find.TickManager.TicksGame < this.lifespanMaxTicks && this.ticksUntilEffect <= 0 && Rand.Chance(base.EffectiveChancePerTick))
			{
				this.lastEffectTicks = Find.TickManager.TicksGame;
				this.SubTrigger(A, B, -1, true);
			}
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x00160D2C File Offset: 0x0015EF2C
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			if (!this.def.subTriggerOnSpawn && Find.TickManager.TicksGame == this.parent.spawnTick)
			{
				return;
			}
			foreach (SubEffecter subEffecter in this.children)
			{
				subEffecter.SubTrigger(A, B, overrideSpawnTick, force);
			}
		}

		// Token: 0x04002431 RID: 9265
		private int lastEffectTicks;

		// Token: 0x04002432 RID: 9266
		private int ticksUntilEffect;

		// Token: 0x04002433 RID: 9267
		private int lifespanMaxTicks;

		// Token: 0x04002434 RID: 9268
		public List<SubEffecter> children = new List<SubEffecter>();
	}
}
