using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C85 RID: 7301
	public class Designator_ZoneAdd_Fishing_Expand : Designator_ZoneAdd_Fishing
	{
		// Token: 0x170021D4 RID: 8660
		// (get) Token: 0x0600BA67 RID: 47719 RVA: 0x0041B4C2 File Offset: 0x004196C2
		protected override bool ShowRightClickHideOptions
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BA68 RID: 47720 RVA: 0x0041B4C5 File Offset: 0x004196C5
		public Designator_ZoneAdd_Fishing_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
