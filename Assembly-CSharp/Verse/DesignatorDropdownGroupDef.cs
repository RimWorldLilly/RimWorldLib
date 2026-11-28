using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000164 RID: 356
	public class DesignatorDropdownGroupDef : Def
	{
		// Token: 0x06000AC0 RID: 2752 RVA: 0x0003B855 File Offset: 0x00039A55
		public IEnumerable<BuildableDef> BuildablesWithoutDefaultDesignators()
		{
			return from x in DefDatabase<ThingDef>.AllDefs.Concat(DefDatabase<TerrainDef>.AllDefs)
			where x.designatorDropdown == this && !x.canGenerateDefaultDesignator
			select x;
		}

		// Token: 0x040007F6 RID: 2038
		public DesignatorDropdownGroupDef.IconSource iconSource;

		// Token: 0x040007F7 RID: 2039
		public bool useGridMenu;

		// Token: 0x040007F8 RID: 2040
		public bool includeEyeDropperTool;

		// Token: 0x02002539 RID: 9529
		public enum IconSource : byte
		{
			// Token: 0x04009863 RID: 39011
			Cost,
			// Token: 0x04009864 RID: 39012
			Placed
		}
	}
}
