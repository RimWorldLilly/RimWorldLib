using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x0200111A RID: 4378
	public interface IIncidentMakerQuestPart
	{
		// Token: 0x06006D7E RID: 28030
		IEnumerable<FiringIncident> MakeIntervalIncidents();
	}
}
