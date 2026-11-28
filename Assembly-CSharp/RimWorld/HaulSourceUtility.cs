using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A81 RID: 6785
	public static class HaulSourceUtility
	{
		// Token: 0x0600AB35 RID: 43829 RVA: 0x003C8D01 File Offset: 0x003C6F01
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(IHaulSource source, Pawn pawn)
		{
			HaulSourceUtility.<GetFloatMenuOptions>d__0 <GetFloatMenuOptions>d__ = new HaulSourceUtility.<GetFloatMenuOptions>d__0(-2);
			<GetFloatMenuOptions>d__.<>3__source = source;
			<GetFloatMenuOptions>d__.<>3__pawn = pawn;
			return <GetFloatMenuOptions>d__;
		}
	}
}
