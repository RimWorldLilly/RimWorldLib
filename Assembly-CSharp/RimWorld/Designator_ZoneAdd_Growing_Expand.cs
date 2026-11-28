using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C83 RID: 7299
	public class Designator_ZoneAdd_Growing_Expand : Designator_ZoneAdd_Growing
	{
		// Token: 0x170021CF RID: 8655
		// (get) Token: 0x0600BA5C RID: 47708 RVA: 0x0041B0AD File Offset: 0x004192AD
		protected override bool ShowRightClickHideOptions
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BA5D RID: 47709 RVA: 0x0041B0B0 File Offset: 0x004192B0
		public Designator_ZoneAdd_Growing_Expand()
		{
			this.defaultLabel = "DesignatorZoneExpand".Translate();
			this.hotKey = KeyBindingDefOf.Misc6;
		}
	}
}
