using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001168 RID: 4456
	public class IncidentWorker_AnimalInsanitySingle : IncidentWorker
	{
		// Token: 0x06006FF4 RID: 28660 RVA: 0x00272D60 File Offset: 0x00270F60
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			Pawn pawn;
			return this.TryFindRandomAnimal(map, out pawn);
		}

		// Token: 0x06006FF5 RID: 28661 RVA: 0x00272D90 File Offset: 0x00270F90
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn;
			if (!this.TryFindRandomAnimal(map, out pawn))
			{
				return false;
			}
			IncidentWorker_AnimalInsanityMass.DriveInsane(pawn);
			string str = "AnimalInsanitySingle".Translate(pawn.Label, pawn.Named("ANIMAL")).CapitalizeFirst();
			base.SendStandardLetter("LetterLabelAnimalInsanitySingle".Translate(pawn.Label, pawn.Named("ANIMAL")).CapitalizeFirst(), str, LetterDefOf.ThreatSmall, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06006FF6 RID: 28662 RVA: 0x00272E30 File Offset: 0x00271030
		private bool TryFindRandomAnimal(Map map, out Pawn animal)
		{
			int maxPoints = 150;
			if (GenDate.DaysPassedSinceSettle < 7)
			{
				maxPoints = 40;
			}
			return (from p in map.mapPawns.AllPawnsSpawned
			where p.IsAnimal && p.kindDef.combatPower <= (float)maxPoints && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).TryRandomElement(out animal);
		}

		// Token: 0x0400478E RID: 18318
		private const int FixedPoints = 30;
	}
}
