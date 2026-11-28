using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200120F RID: 4623
	public class EffecterMaintainer
	{
		// Token: 0x060072CE RID: 29390 RVA: 0x002823E7 File Offset: 0x002805E7
		public EffecterMaintainer(Map map)
		{
			this.map = map;
		}

		// Token: 0x060072CF RID: 29391 RVA: 0x00282404 File Offset: 0x00280604
		public void AddEffecterToMaintain(Effecter eff, IntVec3 pos, int ticks)
		{
			eff.ticksLeft = ticks;
			TargetInfo a = new TargetInfo(pos, this.map, false);
			this.maintainedEffecters.Add(new EffecterMaintainer.MaintainedEffecter(eff, a, TargetInfo.Invalid));
		}

		// Token: 0x060072D0 RID: 29392 RVA: 0x0028243E File Offset: 0x0028063E
		public void AddEffecterToMaintain(Effecter eff, Thing target, int ticks)
		{
			eff.ticksLeft = ticks;
			this.maintainedEffecters.Add(new EffecterMaintainer.MaintainedEffecter(eff, target, TargetInfo.Invalid));
		}

		// Token: 0x060072D1 RID: 29393 RVA: 0x00282463 File Offset: 0x00280663
		public void AddEffecterToMaintain(Effecter eff, TargetInfo A, TargetInfo B, int ticks)
		{
			eff.ticksLeft = ticks;
			this.maintainedEffecters.Add(new EffecterMaintainer.MaintainedEffecter(eff, A, B));
		}

		// Token: 0x060072D2 RID: 29394 RVA: 0x00282480 File Offset: 0x00280680
		public void EffecterMaintainerTick()
		{
			for (int i = this.maintainedEffecters.Count - 1; i >= 0; i--)
			{
				EffecterMaintainer.MaintainedEffecter maintainedEffecter = this.maintainedEffecters[i];
				if (maintainedEffecter.Effecter.ticksLeft > 0)
				{
					maintainedEffecter.Effecter.EffectTick(maintainedEffecter.A, maintainedEffecter.B);
					maintainedEffecter.Effecter.ticksLeft--;
				}
				else
				{
					maintainedEffecter.Effecter.Cleanup();
					this.maintainedEffecters.RemoveAt(i);
				}
			}
		}

		// Token: 0x0400489E RID: 18590
		private Map map;

		// Token: 0x0400489F RID: 18591
		private List<EffecterMaintainer.MaintainedEffecter> maintainedEffecters = new List<EffecterMaintainer.MaintainedEffecter>();

		// Token: 0x02003119 RID: 12569
		public struct MaintainedEffecter
		{
			// Token: 0x060127B8 RID: 75704 RVA: 0x00636810 File Offset: 0x00634A10
			public MaintainedEffecter(Effecter effecter, TargetInfo A, TargetInfo B)
			{
				this.Effecter = effecter;
				this.A = A;
				this.B = B;
			}

			// Token: 0x0400C6FE RID: 50942
			public Effecter Effecter;

			// Token: 0x0400C6FF RID: 50943
			public TargetInfo A;

			// Token: 0x0400C700 RID: 50944
			public TargetInfo B;
		}
	}
}
