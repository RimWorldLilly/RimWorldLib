using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F85 RID: 8069
	public class StatPart_Food : StatPart
	{
		// Token: 0x0600CD33 RID: 52531 RVA: 0x004B76F4 File Offset: 0x004B58F4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					val *= this.FoodMultiplier(pawn.needs.food.CurCategory);
				}
			}
		}

		// Token: 0x0600CD34 RID: 52532 RVA: 0x004B7744 File Offset: 0x004B5944
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					return pawn.needs.food.CurCategory.GetLabel() + ": x" + this.FoodMultiplier(pawn.needs.food.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600CD35 RID: 52533 RVA: 0x004B77B3 File Offset: 0x004B59B3
		private float FoodMultiplier(HungerCategory hunger)
		{
			switch (hunger)
			{
			case HungerCategory.Fed:
				return this.factorFed;
			case HungerCategory.Hungry:
				return this.factorHungry;
			case HungerCategory.UrgentlyHungry:
				return this.factorUrgentlyHungry;
			case HungerCategory.Starving:
				return this.factorStarving;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x040082FB RID: 33531
		public float factorStarving = 1f;

		// Token: 0x040082FC RID: 33532
		public float factorUrgentlyHungry = 1f;

		// Token: 0x040082FD RID: 33533
		public float factorHungry = 1f;

		// Token: 0x040082FE RID: 33534
		public float factorFed = 1f;
	}
}
