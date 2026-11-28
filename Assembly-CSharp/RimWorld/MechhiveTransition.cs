using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CE RID: 6094
	public class MechhiveTransition : OrbitalTransition
	{
		// Token: 0x06009818 RID: 38936 RVA: 0x003725F4 File Offset: 0x003707F4
		public override bool IsTransitionSatisfied()
		{
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.listerThings.AnyThingWithDef(ThingDefOf.CerebrexCore))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
