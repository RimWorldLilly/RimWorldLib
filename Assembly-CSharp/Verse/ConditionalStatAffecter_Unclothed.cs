using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000E1 RID: 225
	public class ConditionalStatAffecter_Unclothed : ConditionalStatAffecter
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x00030148 File Offset: 0x0002E348
		public override string Label
		{
			get
			{
				return "StatsReport_Unclothed".Translate();
			}
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0003015C File Offset: 0x0002E35C
		public override bool Applies(StatRequest req)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.apparel != null)
				{
					using (List<Apparel>.Enumerator enumerator = pawn.apparel.WornApparel.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def.apparel.countsAsClothingForNudity)
							{
								return false;
							}
						}
					}
					return true;
				}
			}
			return true;
		}
	}
}
