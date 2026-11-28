using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121D RID: 4637
	public class ComplexThreatWorker_SleepingMechanoids : ComplexThreatWorker_SleepingThreat
	{
		// Token: 0x06007305 RID: 29445 RVA: 0x00283CAC File Offset: 0x00281EAC
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && (parms.hostileFaction == null || parms.hostileFaction == Faction.OfMechanoids);
		}

		// Token: 0x06007306 RID: 29446 RVA: 0x00283CD0 File Offset: 0x00281ED0
		private static bool MechKindSuitableForComplex(PawnKindDef def)
		{
			return def.RaceProps.IsMechanoid && !def.isGoodBreacher && def.isFighter && def.allowInMechClusters;
		}

		// Token: 0x06007307 RID: 29447 RVA: 0x00283CF7 File Offset: 0x00281EF7
		protected override IEnumerable<PawnKindDef> GetPawnKindsForPoints(float points)
		{
			return PawnUtility.GetCombatPawnKindsForPoints(new Func<PawnKindDef, bool>(ComplexThreatWorker_SleepingMechanoids.MechKindSuitableForComplex), points, null);
		}
	}
}
