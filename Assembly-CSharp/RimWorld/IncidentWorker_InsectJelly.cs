using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001183 RID: 4483
	public class IncidentWorker_InsectJelly : IncidentWorker
	{
		// Token: 0x0600706B RID: 28779 RVA: 0x00275C54 File Offset: 0x00273E54
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && Faction.OfInsects != null && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x0600706C RID: 28780 RVA: 0x00275C88 File Offset: 0x00273E88
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Thing t = InfestationUtility.SpawnJellyTunnels(Rand.Range(2, 3), (int)parms.points / 8, map);
			base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x040047AE RID: 18350
		public const int JellyPointsCost = 8;
	}
}
