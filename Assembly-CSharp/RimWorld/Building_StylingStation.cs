using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A4B RID: 6731
	public class Building_StylingStation : Building
	{
		// Token: 0x0600A899 RID: 43161 RVA: 0x003BE0A5 File Offset: 0x003BC2A5
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Building_StylingStation.<GetFloatMenuOptions>d__0 <GetFloatMenuOptions>d__ = new Building_StylingStation.<GetFloatMenuOptions>d__0(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}
	}
}
