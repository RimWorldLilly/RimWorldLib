using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001196 RID: 4502
	public class IncidentWorker_SelfTame : IncidentWorker
	{
		// Token: 0x060070AE RID: 28846 RVA: 0x00276FAF File Offset: 0x002751AF
		private IEnumerable<Pawn> Candidates(Map map)
		{
			return from x in map.mapPawns.AllPawnsSpawned
			where x.IsAnimal && x.Faction == null && !x.Position.Fogged(x.Map) && !x.InMentalState && !x.Downed && !x.health.hediffSet.HasHediff(HediffDefOf.Scaria, false) && x.GetStatValue(StatDefOf.Wildness, true, -1) > 0f
			select x;
		}

		// Token: 0x060070AF RID: 28847 RVA: 0x00276FE0 File Offset: 0x002751E0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return this.Candidates(map).Any<Pawn>();
		}

		// Token: 0x060070B0 RID: 28848 RVA: 0x00277008 File Offset: 0x00275208
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn = null;
			if (!this.Candidates(map).TryRandomElement(out pawn))
			{
				return false;
			}
			string value = pawn.LabelIndefinite();
			bool flag = pawn.Name != null;
			pawn.SetFaction(Faction.OfPlayer, null);
			string str;
			if (!flag && pawn.Name != null)
			{
				if (pawn.Name.Numerical)
				{
					str = "LetterAnimalSelfTameAndNameNumerical".Translate(value, pawn.Name.ToStringFull, pawn.Named("ANIMAL")).CapitalizeFirst();
				}
				else
				{
					str = "LetterAnimalSelfTameAndName".Translate(value, pawn.Name.ToStringFull, pawn.Named("ANIMAL")).CapitalizeFirst();
				}
			}
			else
			{
				str = "LetterAnimalSelfTame".Translate(pawn).CapitalizeFirst();
			}
			base.SendStandardLetter("LetterLabelAnimalSelfTame".Translate(pawn.KindLabel, pawn).CapitalizeFirst(), str, LetterDefOf.PositiveEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
