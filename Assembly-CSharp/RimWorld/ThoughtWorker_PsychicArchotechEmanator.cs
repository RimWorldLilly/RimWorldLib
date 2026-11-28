using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5F RID: 3679
	public abstract class ThoughtWorker_PsychicArchotechEmanator : ThoughtWorker
	{
		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x060061D2 RID: 25042
		protected abstract ThingDef EmanatorDef { get; }

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x060061D3 RID: 25043
		protected abstract int InnerRadius { get; }

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x060061D4 RID: 25044
		protected abstract int OuterRadius { get; }

		// Token: 0x060061D5 RID: 25045 RVA: 0x002340E8 File Offset: 0x002322E8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsOfDef(this.EmanatorDef);
			for (int i = 0; i < list.Count; i++)
			{
				if (p.Position.InHorDistOf(list[i].Position, (float)this.InnerRadius))
				{
					return ThoughtState.ActiveAtStage(0);
				}
				if (p.Position.InHorDistOf(list[i].Position, (float)this.OuterRadius))
				{
					return ThoughtState.ActiveAtStage(1);
				}
			}
			return false;
		}
	}
}
