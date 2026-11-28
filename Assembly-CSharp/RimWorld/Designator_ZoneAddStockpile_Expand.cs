using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C81 RID: 7297
	public class Designator_ZoneAddStockpile_Expand : Designator_ZoneAddStockpile_Resources
	{
		// Token: 0x170021CB RID: 8651
		// (get) Token: 0x0600BA54 RID: 47700 RVA: 0x0041AF37 File Offset: 0x00419137
		protected override bool ShowRightClickHideOptions
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BA55 RID: 47701 RVA: 0x0041AF3A File Offset: 0x0041913A
		public Designator_ZoneAddStockpile_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
