using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001169 RID: 4457
	public class IncidentWorker_AnimalInsanityMass : IncidentWorker
	{
		// Token: 0x06006FF8 RID: 28664 RVA: 0x00272E88 File Offset: 0x00271088
		public static bool AnimalUsable(Pawn p)
		{
			return p.Spawned && !p.Position.Fogged(p.Map) && (!p.InMentalState || !p.MentalStateDef.IsAggro) && !p.Downed && p.Faction == null;
		}

		// Token: 0x06006FF9 RID: 28665 RVA: 0x00272ED8 File Offset: 0x002710D8
		public static void DriveInsane(Pawn p)
		{
			p.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, true, false, null, false, false, false);
		}

		// Token: 0x06006FFA RID: 28666 RVA: 0x00272F04 File Offset: 0x00271104
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.points <= 0f)
			{
				Log.Error("AnimalInsanity running without points.");
				parms.points = (float)((int)(map.strengthWatcher.StrengthRating * 50f));
			}
			float adjustedPoints = parms.points;
			if (adjustedPoints > 250f)
			{
				adjustedPoints -= 250f;
				adjustedPoints *= 0.5f;
				adjustedPoints += 250f;
			}
			PawnKindDef animalDef;
			if (!(from def in DefDatabase<PawnKindDef>.AllDefs
			where def.RaceProps.Animal && def.combatPower <= adjustedPoints && (from p in map.mapPawns.AllPawnsSpawned
			where p.kindDef == def && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).Count<Pawn>() >= 3
			select def).TryRandomElement(out animalDef))
			{
				return false;
			}
			List<Pawn> list = (from p in map.mapPawns.AllPawnsSpawned
			where p.kindDef == animalDef && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).ToList<Pawn>();
			float combatPower = animalDef.combatPower;
			float num = 0f;
			List<Pawn> list2 = new List<Pawn>();
			list.Shuffle<Pawn>();
			foreach (Pawn pawn in list)
			{
				if (num + combatPower > adjustedPoints)
				{
					break;
				}
				IncidentWorker_AnimalInsanityMass.DriveInsane(pawn);
				num += combatPower;
				list2.Add(pawn);
			}
			if (num == 0f)
			{
				return false;
			}
			string str;
			string str2;
			LetterDef baseLetterDef;
			if (list2.Count == 1)
			{
				Pawn pawn2 = list2.First<Pawn>();
				str = "LetterLabelAnimalInsanitySingle".Translate(pawn2.LabelShort, pawn2.Named("ANIMAL")).CapitalizeFirst();
				str2 = "AnimalInsanitySingle".Translate(pawn2.LabelShort, pawn2.Named("ANIMAL")).CapitalizeFirst();
				baseLetterDef = LetterDefOf.ThreatSmall;
			}
			else
			{
				str = "LetterLabelAnimalInsanityMultiple".Translate(animalDef.GetLabelPlural(-1)).CapitalizeFirst();
				str2 = "AnimalInsanityMultiple".Translate(animalDef.GetLabelPlural(-1)).CapitalizeFirst();
				baseLetterDef = LetterDefOf.ThreatBig;
			}
			base.SendStandardLetter(str, str2, baseLetterDef, parms, list2, Array.Empty<NamedArgument>());
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(map);
			if (map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(1f);
			}
			return true;
		}
	}
}
