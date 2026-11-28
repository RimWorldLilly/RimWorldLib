using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E36 RID: 3638
	public class ThoughtWorker_IsIndoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x06006161 RID: 24929 RVA: 0x00232A68 File Offset: 0x00230C68
		public static bool IsAwakeAndIndoors(Pawn p, out bool isNaturalRoof)
		{
			isNaturalRoof = false;
			if (!p.Awake())
			{
				return false;
			}
			if (p.Position.UsesOutdoorTemperature(p.Map))
			{
				return false;
			}
			RoofDef roofDef = p.Map.roofGrid.RoofAt(p.Position);
			if (roofDef == null)
			{
				return false;
			}
			isNaturalRoof = roofDef.isNatural;
			return true;
		}

		// Token: 0x06006162 RID: 24930 RVA: 0x00232ABC File Offset: 0x00230CBC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && !flag;
		}
	}
}
