using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002060 RID: 8288
	[StaticConstructorOnStartup]
	public class PlaceWorker_RitualFocusNoArrow : PlaceWorker_RitualFocus
	{
		// Token: 0x17002581 RID: 9601
		// (get) Token: 0x0600D133 RID: 53555 RVA: 0x004CED6C File Offset: 0x004CCF6C
		protected override bool UseArrow
		{
			get
			{
				return false;
			}
		}
	}
}
