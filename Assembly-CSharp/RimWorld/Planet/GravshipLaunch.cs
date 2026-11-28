using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023AE RID: 9134
	public class GravshipLaunch : WorldObject
	{
		// Token: 0x0600E364 RID: 58212 RVA: 0x00549370 File Offset: 0x00547570
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
			text += "LaunchDate".Translate() + ": " + GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(this.creationGameTicks), Find.WorldGrid.LongLatOf(base.Tile));
			text += " (";
			text += "TimeAgo".Translate((Find.TickManager.TicksGame - this.creationGameTicks).ToStringTicksToPeriodVague(true, true));
			return text + ")";
		}
	}
}
