using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023B4 RID: 9140
	public class AbandonedSettlement : WorldObject
	{
		// Token: 0x0600E3A9 RID: 58281 RVA: 0x0054A670 File Offset: 0x00548870
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Faction != Faction.OfPlayer)
			{
				return text;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "Abandoned".Translate() + ": " + GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(this.creationGameTicks), Find.WorldGrid.LongLatOf(base.Tile));
			text += " (";
			text += "TimeAgo".Translate((Find.TickManager.TicksGame - this.creationGameTicks).ToStringTicksToPeriodVague(true, true));
			return text + ")";
		}
	}
}
