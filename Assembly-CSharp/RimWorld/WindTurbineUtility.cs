using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001363 RID: 4963
	public static class WindTurbineUtility
	{
		// Token: 0x06007AFD RID: 31485 RVA: 0x002BC98E File Offset: 0x002BAB8E
		public static IEnumerable<IntVec3> CalculateWindCells(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			WindTurbineUtility.<CalculateWindCells>d__0 <CalculateWindCells>d__ = new WindTurbineUtility.<CalculateWindCells>d__0(-2);
			<CalculateWindCells>d__.<>3__center = center;
			<CalculateWindCells>d__.<>3__rot = rot;
			return <CalculateWindCells>d__;
		}
	}
}
