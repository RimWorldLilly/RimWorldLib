using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001160 RID: 4448
	public class IncidentWorker_Ambush_ManhunterPack : IncidentWorker_Ambush
	{
		// Token: 0x06006FBF RID: 28607 RVA: 0x00270F94 File Offset: 0x0026F194
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			PawnKindDef pawnKindDef;
			return base.CanFireNowSub(parms) && AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(this.AdjustedPoints(parms.points), PlanetTile.Invalid, out pawnKindDef);
		}

		// Token: 0x06006FC0 RID: 28608 RVA: 0x00270FC4 File Offset: 0x0026F1C4
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			PawnKindDef animalKind;
			if (!AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(this.AdjustedPoints(parms.points), parms.target.Tile, out animalKind) && !AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(this.AdjustedPoints(parms.points), PlanetTile.Invalid, out animalKind))
			{
				string str = "Could not find any valid animal kind for ";
				IncidentDef def = this.def;
				Log.Error(str + ((def != null) ? def.ToString() : null) + " incident.");
				return new List<Pawn>();
			}
			return AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, parms.target.Tile, this.AdjustedPoints(parms.points), 0);
		}

		// Token: 0x06006FC1 RID: 28609 RVA: 0x00271058 File Offset: 0x0026F258
		protected override void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
			for (int i = 0; i < generatedPawns.Count; i++)
			{
				generatedPawns[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
				generatedPawns[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, false, null, false, false, false);
			}
		}

		// Token: 0x06006FC2 RID: 28610 RVA: 0x002710BC File Offset: 0x0026F2BC
		private float AdjustedPoints(float basePoints)
		{
			return basePoints * 0.75f;
		}

		// Token: 0x06006FC3 RID: 28611 RVA: 0x002710C8 File Offset: 0x0026F2C8
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return this.def.letterText.Formatted((caravan != null) ? caravan.Name : "yourCaravan".TranslateSimple(), anyPawn.GetKindLabelPlural(-1)).CapitalizeFirst();
		}

		// Token: 0x04004776 RID: 18294
		private const float ManhunterAmbushPointsFactor = 0.75f;
	}
}
