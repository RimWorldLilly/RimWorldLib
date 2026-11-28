using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119B RID: 4507
	public class IncidentWorker_ShortCircuit : IncidentWorker
	{
		// Token: 0x060070C9 RID: 28873 RVA: 0x002778D8 File Offset: 0x00275AD8
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).Any<Building>();
		}

		// Token: 0x060070CA RID: 28874 RVA: 0x002778F0 File Offset: 0x00275AF0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Building culprit;
			if (!ShortCircuitUtility.GetShortCircuitablePowerConduits((Map)parms.target).TryRandomElement(out culprit))
			{
				return false;
			}
			ShortCircuitUtility.DoShortCircuit(culprit);
			return true;
		}
	}
}
