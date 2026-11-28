using System;

namespace Verse
{
	// Token: 0x0200038E RID: 910
	public static class PawnGraphicUtils
	{
		// Token: 0x06001BA4 RID: 7076 RVA: 0x000AA85C File Offset: 0x000A8A5C
		public static bool TryGetAlternate(this Pawn pawn, out AlternateGraphic ag, out int index)
		{
			ag = null;
			index = -1;
			Rand.PushState(pawn.thingIDNumber ^ 46101);
			if (Rand.Chance(pawn.kindDef.alternateGraphicChance))
			{
				if (pawn.kindDef.alternateGraphics.TryRandomElementByWeight((AlternateGraphic x) => x.Weight, out ag))
				{
					index = pawn.kindDef.alternateGraphics.IndexOf(ag);
				}
			}
			Rand.PopState();
			return ag != null;
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x000AA8E4 File Offset: 0x000A8AE4
		public static int GetGraphicIndex(this Pawn pawn)
		{
			AlternateGraphic alternateGraphic;
			int result;
			if (pawn.TryGetAlternate(out alternateGraphic, out result))
			{
				return result;
			}
			return -1;
		}
	}
}
