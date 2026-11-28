using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002084 RID: 8324
	public static class AddictionUtility
	{
		// Token: 0x0600D1F2 RID: 53746 RVA: 0x004D20A8 File Offset: 0x004D02A8
		public static bool HasChemicalDependency(Pawn pawn, Thing drug)
		{
			return AddictionUtility.HasChemicalDependency(pawn, drug.def);
		}

		// Token: 0x0600D1F3 RID: 53747 RVA: 0x004D20B8 File Offset: 0x004D02B8
		public static bool HasChemicalDependency(Pawn pawn, ThingDef drug)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			ChemicalDef chemicalDef = (compProperties != null) ? compProperties.chemical : null;
			if (chemicalDef == null)
			{
				return false;
			}
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				Hediff_ChemicalDependency hediff_ChemicalDependency = hediff as Hediff_ChemicalDependency;
				if (hediff_ChemicalDependency != null && hediff_ChemicalDependency.chemical == chemicalDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D1F4 RID: 53748 RVA: 0x004D2148 File Offset: 0x004D0348
		public static bool IsAddicted(Pawn pawn, Thing drug)
		{
			return AddictionUtility.FindAddictionHediff(pawn, drug) != null;
		}

		// Token: 0x0600D1F5 RID: 53749 RVA: 0x004D2154 File Offset: 0x004D0354
		public static bool IsAddicted(Pawn pawn, ChemicalDef chemical)
		{
			return AddictionUtility.FindAddictionHediff(pawn, chemical) != null;
		}

		// Token: 0x0600D1F6 RID: 53750 RVA: 0x004D2160 File Offset: 0x004D0360
		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return null;
			}
			CompDrug compDrug = drug.TryGetComp<CompDrug>();
			if (!compDrug.Props.Addictive)
			{
				return null;
			}
			return AddictionUtility.FindAddictionHediff(pawn, compDrug.Props.chemical);
		}

		// Token: 0x0600D1F7 RID: 53751 RVA: 0x004D21A4 File Offset: 0x004D03A4
		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, ChemicalDef chemical)
		{
			return (Hediff_Addiction)pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.addictionHediff);
		}

		// Token: 0x0600D1F8 RID: 53752 RVA: 0x004D21E4 File Offset: 0x004D03E4
		public static Hediff FindToleranceHediff(Pawn pawn, ChemicalDef chemical)
		{
			if (chemical.toleranceHediff == null)
			{
				return null;
			}
			return pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.toleranceHediff);
		}

		// Token: 0x0600D1F9 RID: 53753 RVA: 0x004D2230 File Offset: 0x004D0430
		public static void ModifyChemicalEffectForToleranceAndBodySize(Pawn pawn, ChemicalDef chemicalDef, ref float effect, bool applyGeneToleranceFactor, bool divideByBodySize = true)
		{
			if (chemicalDef != null)
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					hediffs[i].ModifyChemicalEffect(chemicalDef, ref effect);
				}
			}
			if (applyGeneToleranceFactor && ModsConfig.BiotechActive && pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					if (gene.Active && gene.def.chemical == chemicalDef)
					{
						effect *= gene.def.toleranceBuildupFactor;
					}
				}
			}
			if (divideByBodySize)
			{
				effect /= pawn.BodySize;
			}
		}

		// Token: 0x0600D1FA RID: 53754 RVA: 0x004D22FC File Offset: 0x004D04FC
		public static void CheckDrugAddictionTeachOpportunity(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh || !pawn.Spawned)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return;
			}
			if (!AddictionUtility.AddictedToAnything(pawn))
			{
				return;
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugAddiction, pawn, OpportunityType.Important);
		}

		// Token: 0x0600D1FB RID: 53755 RVA: 0x004D2350 File Offset: 0x004D0550
		public static bool AddictedToAnything(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i] is Hediff_Addiction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D1FC RID: 53756 RVA: 0x004D2390 File Offset: 0x004D0590
		public static bool CanBingeOnNow(Pawn pawn, ChemicalDef chemical, DrugCategory drugCategory)
		{
			if (!chemical.canBinge)
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				return false;
			}
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Drug);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].Position.Fogged(list[i].Map) && (drugCategory == DrugCategory.Any || list[i].def.ingestible.drugCategory == drugCategory) && list[i].TryGetComp<CompDrug>().Props.chemical == chemical && (list[i].Position.Roofed(list[i].Map) || list[i].Position.InHorDistOf(pawn.Position, 45f)) && pawn.CanReach(list[i], PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
