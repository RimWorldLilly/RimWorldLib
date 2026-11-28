using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118B RID: 4491
	public class IncidentWorker_OrbitalDebris : IncidentWorker
	{
		// Token: 0x06007086 RID: 28806 RVA: 0x002762D9 File Offset: 0x002744D9
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ModsConfig.OdysseyActive && base.CanFireNowSub(parms);
		}

		// Token: 0x06007087 RID: 28807 RVA: 0x002762EC File Offset: 0x002744EC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			GenSpawn.Spawn(ThingDefOf.OrbitalDebrisSpawner, map.Center, map, WipeMode.Vanish);
			base.SendStandardLetter(parms, null, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
