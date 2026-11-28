using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5C RID: 3676
	public abstract class ThoughtWorker_WearingColor : ThoughtWorker
	{
		// Token: 0x060061CB RID: 25035
		protected abstract Color? Color(Pawn p);

		// Token: 0x060061CC RID: 25036 RVA: 0x00233FA8 File Offset: 0x002321A8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Color? color = this.Color(p);
			if (color == null)
			{
				return false;
			}
			int num = 0;
			foreach (Apparel thing in p.apparel.WornApparel)
			{
				CompColorable compColorable = thing.TryGetComp<CompColorable>();
				if (compColorable != null && compColorable.Active && compColorable.Color.IndistinguishableFrom(color.Value))
				{
					num++;
				}
			}
			return (float)num / (float)p.apparel.WornApparelCount >= 0.6f;
		}

		// Token: 0x04003609 RID: 13833
		public const float RequiredMinPercentage = 0.6f;
	}
}
