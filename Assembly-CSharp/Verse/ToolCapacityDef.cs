using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020001D5 RID: 469
	public class ToolCapacityDef : Def
	{
		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x00043E0E File Offset: 0x0004200E
		public IEnumerable<ManeuverDef> Maneuvers
		{
			get
			{
				return from x in DefDatabase<ManeuverDef>.AllDefsListForReading
				where x.requiredCapacity == this
				select x;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00043E26 File Offset: 0x00042026
		public IEnumerable<VerbProperties> VerbsProperties
		{
			get
			{
				return from x in this.Maneuvers
				select x.verb;
			}
		}
	}
}
