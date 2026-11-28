using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020017A5 RID: 6053
	public class ScenPart_DisableIncident : ScenPart_IncidentBase
	{
		// Token: 0x170019AF RID: 6575
		// (get) Token: 0x060096EA RID: 38634 RVA: 0x0036C160 File Offset: 0x0036A360
		protected override string IncidentTag
		{
			get
			{
				return "DisableIncident";
			}
		}

		// Token: 0x060096EB RID: 38635 RVA: 0x0036C167 File Offset: 0x0036A367
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return new ScenPart_DisableIncident.<RandomizableIncidents>d__2(-2);
		}
	}
}
