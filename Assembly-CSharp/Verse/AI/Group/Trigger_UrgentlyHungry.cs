using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200097F RID: 2431
	public class Trigger_UrgentlyHungry : Trigger
	{
		// Token: 0x060049AA RID: 18858 RVA: 0x001CB084 File Offset: 0x001C9284
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn_NeedsTracker needs = lord.ownedPawns[i].needs;
					bool flag;
					if (needs == null)
					{
						flag = false;
					}
					else
					{
						Need_Food food = needs.food;
						HungerCategory? hungerCategory = (food != null) ? new HungerCategory?(food.CurCategory) : null;
						HungerCategory hungerCategory2 = HungerCategory.UrgentlyHungry;
						flag = (hungerCategory.GetValueOrDefault() >= hungerCategory2 & hungerCategory != null);
					}
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
