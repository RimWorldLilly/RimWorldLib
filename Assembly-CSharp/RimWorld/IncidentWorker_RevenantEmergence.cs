using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001194 RID: 4500
	public class IncidentWorker_RevenantEmergence : IncidentWorker
	{
		// Token: 0x060070A7 RID: 28839 RVA: 0x00276D1C File Offset: 0x00274F1C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Caravan caravan = parms.target as Caravan;
			if (caravan != null)
			{
				Thing thing;
				Pawn pawn;
				return CaravanInventoryUtility.TryGetThingOfDef(caravan, ThingDefOf.RevenantSpine, out thing, out pawn);
			}
			Map map = parms.target as Map;
			return map != null && map.listerThings.ThingsOfDef(ThingDefOf.RevenantSpine).Count > 0;
		}

		// Token: 0x060070A8 RID: 28840 RVA: 0x00276D7C File Offset: 0x00274F7C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			if (caravan != null)
			{
				Thing thing;
				Pawn pawn;
				CaravanInventoryUtility.TryGetThingOfDef(caravan, ThingDefOf.RevenantSpine, out thing, out pawn);
				if (thing == null)
				{
					return false;
				}
				CompSpawnsRevenant compSpawnsRevenant = thing.TryGetComp<CompSpawnsRevenant>();
				if (compSpawnsRevenant.spawnTick > 0)
				{
					return false;
				}
				compSpawnsRevenant.spawnTick = Find.TickManager.TicksGame + IncidentWorker_RevenantEmergence.EmergenceDelay.RandomInRange;
				IncidentWorker.SendIncidentLetter(this.def.letterLabel, this.def.letterText, LetterDefOf.NegativeEvent, parms, caravan, this.def, Array.Empty<NamedArgument>());
				return true;
			}
			else
			{
				Map map = parms.target as Map;
				if (map == null)
				{
					return false;
				}
				Thing thing2;
				(from s in map.listerThings.ThingsOfDef(ThingDefOf.RevenantSpine)
				where s.TryGetComp<CompSpawnsRevenant>().spawnTick < 0
				select s).TryRandomElement(out thing2);
				if (thing2 == null)
				{
					return false;
				}
				thing2.TryGetComp<CompSpawnsRevenant>().spawnTick = Find.TickManager.TicksGame + IncidentWorker_RevenantEmergence.EmergenceDelay.RandomInRange;
				IncidentWorker.SendIncidentLetter(this.def.letterLabel, this.def.letterText, LetterDefOf.NegativeEvent, parms, thing2, this.def, Array.Empty<NamedArgument>());
				return true;
			}
		}

		// Token: 0x040047B7 RID: 18359
		private static readonly IntRange EmergenceDelay = new IntRange(7500, 12500);
	}
}
