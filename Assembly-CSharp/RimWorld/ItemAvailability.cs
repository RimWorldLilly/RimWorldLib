using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC5 RID: 3013
	public class ItemAvailability
	{
		// Token: 0x060056CC RID: 22220 RVA: 0x002041A2 File Offset: 0x002023A2
		public ItemAvailability(Map map)
		{
			this.map = map;
		}

		// Token: 0x060056CD RID: 22221 RVA: 0x002041BC File Offset: 0x002023BC
		public void Tick()
		{
			this.cachedResults.Clear();
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x002041CC File Offset: 0x002023CC
		public bool ThingsAvailableAnywhere(ThingDef need, int amount, Pawn pawn)
		{
			int key = Gen.HashCombine<Faction>(need.GetHashCode(), pawn.Faction);
			bool flag;
			if (!this.cachedResults.TryGetValue(key, out flag))
			{
				List<Thing> list = this.map.listerThings.ThingsOfDef(need);
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].IsForbidden(pawn))
					{
						num += list[i].stackCount;
						if (num >= amount)
						{
							break;
						}
					}
				}
				flag = (num >= amount);
				this.cachedResults.Add(key, flag);
			}
			return flag;
		}

		// Token: 0x04003328 RID: 13096
		private readonly Map map;

		// Token: 0x04003329 RID: 13097
		private readonly Dictionary<int, bool> cachedResults = new Dictionary<int, bool>();
	}
}
