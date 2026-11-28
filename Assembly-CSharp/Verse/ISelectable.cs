using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000654 RID: 1620
	public interface ISelectable
	{
		// Token: 0x060032C5 RID: 12997
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x060032C6 RID: 12998
		string GetInspectString();

		// Token: 0x060032C7 RID: 12999
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}
