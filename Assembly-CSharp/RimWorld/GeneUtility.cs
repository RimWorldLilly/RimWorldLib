using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Grammar;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020020A6 RID: 8358
	[StaticConstructorOnStartup]
	public static class GeneUtility
	{
		// Token: 0x170025D4 RID: 9684
		// (get) Token: 0x0600D2A3 RID: 53923 RVA: 0x004D6F10 File Offset: 0x004D5110
		public static List<GeneDef> GenesInOrder
		{
			get
			{
				if (GeneUtility.cachedGeneDefsInOrder == null)
				{
					GeneUtility.cachedGeneDefsInOrder = new List<GeneDef>();
					foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefs)
					{
						if (geneDef.endogeneCategory != EndogeneCategory.Melanin)
						{
							GeneUtility.cachedGeneDefsInOrder.Add(geneDef);
						}
					}
					GeneUtility.cachedGeneDefsInOrder.SortBy((GeneDef x) => -x.displayCategory.displayPriorityInXenotype, (GeneDef x) => x.displayCategory.label, (GeneDef x) => x.displayOrderInCategory);
				}
				return GeneUtility.cachedGeneDefsInOrder;
			}
		}

		// Token: 0x0600D2A4 RID: 53924 RVA: 0x004D6FEC File Offset: 0x004D51EC
		public static GeneSet GenerateGeneSet(int? seed = null)
		{
			if (!ModLister.CheckBiotech("geneset generation"))
			{
				return null;
			}
			GeneSet geneSet = new GeneSet();
			if (seed != null)
			{
				Rand.PushState(seed.Value);
			}
			GeneUtility.GeneCount geneCount = GeneUtility.GeneCountProbabilities.RandomElementByWeight((GeneUtility.GeneCount x) => x.chance);
			Func<GeneDef, bool> <>9__1;
			for (int i = 0; i < geneCount.architeCount; i++)
			{
				IEnumerable<GeneDef> allDefs = DefDatabase<GeneDef>.AllDefs;
				Func<GeneDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((GeneDef x) => x.biostatArc > 0 && geneSet.CanAddGeneDuringGeneration(x)));
				}
				GeneDef gene;
				if (allDefs.Where(predicate).TryRandomElementByWeight((GeneDef x) => x.selectionWeight, out gene))
				{
					geneSet.AddGene(gene);
				}
			}
			Func<GeneDef, bool> <>9__3;
			for (int j = 0; j < geneCount.nonArchiteCount; j++)
			{
				IEnumerable<GeneDef> allDefs2 = DefDatabase<GeneDef>.AllDefs;
				Func<GeneDef, bool> predicate2;
				if ((predicate2 = <>9__3) == null)
				{
					predicate2 = (<>9__3 = ((GeneDef x) => x.biostatArc == 0 && geneSet.CanAddGeneDuringGeneration(x)));
				}
				GeneDef gene2;
				if (allDefs2.Where(predicate2).TryRandomElementByWeight((GeneDef x) => x.selectionWeight, out gene2))
				{
					geneSet.AddGene(gene2);
				}
			}
			geneSet.GenerateName();
			if (seed != null)
			{
				Rand.PopState();
			}
			if (geneSet.Empty)
			{
				Log.Error("Generated gene pack with no genes.");
			}
			geneSet.SortGenes();
			return geneSet;
		}

		// Token: 0x0600D2A5 RID: 53925 RVA: 0x004D717D File Offset: 0x004D537D
		public static bool PawnWouldDieFromReimplanting(Pawn pawn)
		{
			return ModsConfig.BiotechActive && pawn.health.hediffSet.HasHediff(HediffDefOf.XenogermReplicating, false);
		}

		// Token: 0x0600D2A6 RID: 53926 RVA: 0x004D71A0 File Offset: 0x004D53A0
		public static void ReimplantXenogerm(Pawn caster, Pawn recipient)
		{
			if (!ModLister.CheckBiotech("xenogerm reimplantation"))
			{
				return;
			}
			QuestUtility.SendQuestTargetSignals(caster.questTags, "XenogermReimplanted", caster.Named("SUBJECT"));
			recipient.genes.SetXenotype(caster.genes.Xenotype);
			recipient.genes.xenotypeName = caster.genes.xenotypeName;
			recipient.genes.xenotypeName = caster.genes.xenotypeName;
			recipient.genes.iconDef = caster.genes.iconDef;
			recipient.genes.ClearXenogenes();
			foreach (Gene gene in caster.genes.Xenogenes)
			{
				recipient.genes.AddGene(gene.def, true);
			}
			if (!caster.genes.Xenotype.soundDefOnImplant.NullOrUndefined())
			{
				caster.genes.Xenotype.soundDefOnImplant.PlayOneShot(SoundInfo.InMap(recipient, MaintenanceType.None));
			}
			recipient.health.AddHediff(HediffDefOf.XenogerminationComa, null, null, null);
			GeneUtility.ExtractXenogerm(caster, -1);
			GeneUtility.UpdateXenogermReplication(recipient);
		}

		// Token: 0x0600D2A7 RID: 53927 RVA: 0x004D72F0 File Offset: 0x004D54F0
		public static void ExtractXenogerm(Pawn pawn, int overrideDurationTicks = -1)
		{
			if (!ModLister.CheckBiotech("xenogerm extraction"))
			{
				return;
			}
			pawn.health.AddHediff(HediffDefOf.XenogermLossShock, null, null, null);
			if (GeneUtility.PawnWouldDieFromReimplanting(pawn))
			{
				pawn.genes.SetXenotype(XenotypeDefOf.Baseliner);
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.XenogermReplicating, pawn, null);
			if (overrideDurationTicks > 0)
			{
				hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = overrideDurationTicks;
			}
			pawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x0600D2A8 RID: 53928 RVA: 0x004D7374 File Offset: 0x004D5574
		public static void ImplantXenogermItem(Pawn pawn, Xenogerm xenogerm)
		{
			if (!ModLister.CheckBiotech("xenogerm implantation"))
			{
				return;
			}
			GeneUtility.UpdateXenogermReplication(pawn);
			if (xenogerm.GeneSet != null && pawn.genes != null)
			{
				pawn.genes.SetXenotype(XenotypeDefOf.Baseliner);
				pawn.genes.xenotypeName = xenogerm.xenotypeName;
				pawn.genes.iconDef = xenogerm.iconDef;
				foreach (GeneDef geneDef in xenogerm.GeneSet.GenesListForReading)
				{
					pawn.genes.AddGene(geneDef, true);
				}
			}
		}

		// Token: 0x0600D2A9 RID: 53929 RVA: 0x004D7428 File Offset: 0x004D5628
		public static void UpdateXenogermReplication(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.XenogermReplicating, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			pawn.health.AddHediff(HediffDefOf.XenogermReplicating, null, null, null);
		}

		// Token: 0x0600D2AA RID: 53930 RVA: 0x004D747F File Offset: 0x004D567F
		public static bool IsBloodfeeder(this Pawn pawn)
		{
			return ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Bloodfeeder);
		}

		// Token: 0x0600D2AB RID: 53931 RVA: 0x004D74A2 File Offset: 0x004D56A2
		public static bool CanDeathrest(this Pawn pawn)
		{
			return ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.GetFirstGeneOfType<Gene_Deathrest>() != null;
		}

		// Token: 0x0600D2AC RID: 53932 RVA: 0x004D74C4 File Offset: 0x004D56C4
		public static bool SterileGenes(this Pawn pawn)
		{
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return false;
			}
			List<Gene> genesListForReading = pawn.genes.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i].Active && genesListForReading[i].def.sterilize)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D2AD RID: 53933 RVA: 0x004D7524 File Offset: 0x004D5724
		public static BodyTypeDef ToBodyType(this GeneticBodyType bodyType, Pawn pawn)
		{
			switch (bodyType)
			{
			case GeneticBodyType.Fat:
				return BodyTypeDefOf.Fat;
			case GeneticBodyType.Thin:
				return BodyTypeDefOf.Thin;
			case GeneticBodyType.Hulk:
				return BodyTypeDefOf.Hulk;
			case GeneticBodyType.Standard:
				if (pawn.gender != Gender.Female)
				{
					return BodyTypeDefOf.Male;
				}
				return BodyTypeDefOf.Female;
			default:
				Log.Error("Undefined bodyType");
				return null;
			}
		}

		// Token: 0x0600D2AE RID: 53934 RVA: 0x004D757C File Offset: 0x004D577C
		public static void OffsetHemogen(Pawn pawn, float offset, bool applyStatFactor = true)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			if (offset > 0f && applyStatFactor)
			{
				offset *= pawn.GetStatValue(StatDefOf.HemogenGainFactor, true, -1);
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_HemogenDrain gene_HemogenDrain = (genes != null) ? genes.GetFirstGeneOfType<Gene_HemogenDrain>() : null;
			if (gene_HemogenDrain != null)
			{
				GeneResourceDrainUtility.OffsetResource(gene_HemogenDrain, offset);
				return;
			}
			Pawn_GeneTracker genes2 = pawn.genes;
			Gene_Hemogen gene_Hemogen = (genes2 != null) ? genes2.GetFirstGeneOfType<Gene_Hemogen>() : null;
			if (gene_Hemogen != null)
			{
				gene_Hemogen.Value += offset;
			}
		}

		// Token: 0x0600D2AF RID: 53935 RVA: 0x004D75F0 File Offset: 0x004D57F0
		public static string GenerateXenotypeNameFromGenes(List<GeneDef> genes)
		{
			GeneUtility.tmpSymbolPacks.Clear();
			string text = string.Empty;
			if (genes.Count == 0)
			{
				return text;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.NamerXenotype);
			for (int i = 0; i < genes.Count; i++)
			{
				if (genes[i].symbolPack != null)
				{
					GeneUtility.tmpSymbolPacks.Add(genes[i].symbolPack);
				}
			}
			for (int j = 0; j < 150; j++)
			{
				GeneSymbolPack geneSymbolPack;
				(from x in GeneUtility.tmpSymbolPacks
				where !x.wholeNameSymbols.NullOrEmpty<GeneSymbolPack.WeightedSymbol>()
				select x).TryRandomElementByWeight((GeneSymbolPack x) => x.wholeNameSymbols.Sum((GeneSymbolPack.WeightedSymbol y) => y.weight), out geneSymbolPack);
				GeneSymbolPack geneSymbolPack2;
				(from x in GeneUtility.tmpSymbolPacks
				where !x.prefixSymbols.NullOrEmpty<GeneSymbolPack.WeightedSymbol>()
				select x).TryRandomElementByWeight((GeneSymbolPack x) => x.prefixSymbols.Sum((GeneSymbolPack.WeightedSymbol y) => y.weight), out geneSymbolPack2);
				GeneSymbolPack geneSymbolPack3;
				(from x in GeneUtility.tmpSymbolPacks
				where !x.suffixSymbols.NullOrEmpty<GeneSymbolPack.WeightedSymbol>()
				select x).Except(geneSymbolPack2).TryRandomElementByWeight((GeneSymbolPack x) => x.suffixSymbols.Sum((GeneSymbolPack.WeightedSymbol y) => y.weight), out geneSymbolPack3);
				GeneSymbolPack.WeightedSymbol prefix = null;
				GeneSymbolPack.WeightedSymbol suffix = null;
				GeneSymbolPack.WeightedSymbol weightedSymbol = null;
				if (geneSymbolPack != null && geneSymbolPack.wholeNameSymbols.TryRandomElementByWeight((GeneSymbolPack.WeightedSymbol x) => base.<GenerateXenotypeNameFromGenes>g__SymbolWeight|6(x), out weightedSymbol))
				{
					request.Rules.Add(new Rule_String("wholeName", weightedSymbol.symbol));
				}
				if (geneSymbolPack2 != null && geneSymbolPack2.prefixSymbols.TryRandomElementByWeight((GeneSymbolPack.WeightedSymbol x) => base.<GenerateXenotypeNameFromGenes>g__SymbolWeight|6(x), out prefix))
				{
					request.Rules.Add(new Rule_String("genePrefix", prefix.symbol));
				}
				if (geneSymbolPack3 != null && geneSymbolPack3.suffixSymbols.TryRandomElementByWeight((GeneSymbolPack.WeightedSymbol x) => base.<GenerateXenotypeNameFromGenes>g__SymbolWeight|6(x), out suffix))
				{
					request.Rules.Add(new Rule_String("geneSuffix", suffix.symbol));
				}
				string result = GrammarResolver.Resolve("r_name", request, null, false, null, null, null, false);
				if (!NameUseChecker.XenotypeNameIsUsed(text))
				{
					return result;
				}
			}
			Log.Error("Failed to generate a unique xenotype name.");
			if (text.NullOrEmpty())
			{
				text = "ERR";
			}
			return text;
		}

		// Token: 0x0600D2B0 RID: 53936 RVA: 0x004D7894 File Offset: 0x004D5A94
		public static void SatisfyChemicalGenes(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return;
			}
			foreach (Gene gene in pawn.genes.GenesListForReading)
			{
				Gene_ChemicalDependency gene_ChemicalDependency = gene as Gene_ChemicalDependency;
				if (gene_ChemicalDependency != null)
				{
					gene_ChemicalDependency.Reset();
				}
			}
		}

		// Token: 0x0600D2B1 RID: 53937 RVA: 0x004D7904 File Offset: 0x004D5B04
		public static void SortGeneDefs(this List<GeneDef> geneDefs)
		{
			geneDefs.SortBy((GeneDef x) => -x.displayCategory.displayPriorityInXenotype, (GeneDef x) => x.displayOrderInCategory, (GeneDef y) => y.label);
		}

		// Token: 0x0600D2B2 RID: 53938 RVA: 0x004D7974 File Offset: 0x004D5B74
		public static void SortGenes(this List<Gene> genes)
		{
			genes.SortBy((Gene x) => !x.Active, (Gene x) => -x.def.displayCategory.displayPriorityInXenotype, (Gene y) => y.def.displayOrderInCategory);
		}

		// Token: 0x0600D2B3 RID: 53939 RVA: 0x004D79E4 File Offset: 0x004D5BE4
		public static void SortGenepacks(this List<Genepack> genepacks)
		{
			genepacks.SortBy((Genepack x) => -x.GeneSet.GenesListForReading[0].displayCategory.displayPriorityInXenotype, (Genepack x) => x.GeneSet.GenesListForReading[0].displayCategory.label, (Genepack x) => x.GeneSet.GenesListForReading[0].displayOrderInCategory);
		}

		// Token: 0x0600D2B4 RID: 53940 RVA: 0x004D7A54 File Offset: 0x004D5C54
		public static List<GeneDef> NonOverriddenGenes(this List<GeneDefWithType> geneDefWithTypes)
		{
			GeneUtility.tmpGenes.Clear();
			GeneUtility.tmpOverriddenGenes.Clear();
			if (!ModsConfig.BiotechActive)
			{
				return GeneUtility.tmpGenes;
			}
			foreach (GeneDefWithType geneDefWithType in geneDefWithTypes)
			{
				GeneUtility.tmpGenes.Add(geneDefWithType.geneDef);
			}
			for (int i = 0; i < geneDefWithTypes.Count; i++)
			{
				if (!geneDefWithTypes[i].RandomChosen)
				{
					for (int j = i + 1; j < geneDefWithTypes.Count; j++)
					{
						if (!geneDefWithTypes[j].RandomChosen && geneDefWithTypes[i].ConflictsWith(geneDefWithTypes[j]))
						{
							if (geneDefWithTypes[i].Overrides(geneDefWithTypes[j]))
							{
								GeneUtility.tmpOverriddenGenes.Add(geneDefWithTypes[j]);
							}
							else
							{
								GeneUtility.tmpOverriddenGenes.Add(geneDefWithTypes[i]);
							}
						}
					}
				}
			}
			foreach (GeneDefWithType geneDefWithType2 in GeneUtility.tmpOverriddenGenes)
			{
				GeneUtility.tmpGenes.Remove(geneDefWithType2.geneDef);
			}
			GeneUtility.tmpOverriddenGenes.Clear();
			return GeneUtility.tmpGenes;
		}

		// Token: 0x0600D2B5 RID: 53941 RVA: 0x004D7BD8 File Offset: 0x004D5DD8
		public static List<GeneDef> NonOverriddenGenes(this List<GeneDef> geneDefs, bool xenogene)
		{
			GeneUtility.tmpGeneDefsWithType.Clear();
			foreach (GeneDef geneDef in geneDefs)
			{
				GeneUtility.tmpGeneDefsWithType.Add(new GeneDefWithType(geneDef, xenogene));
			}
			return GeneUtility.tmpGeneDefsWithType.NonOverriddenGenes();
		}

		// Token: 0x0600D2B6 RID: 53942 RVA: 0x004D7C44 File Offset: 0x004D5E44
		public static bool Overrides(this GeneDef gene, GeneDef other, bool isXenogene, bool otherIsXenogene)
		{
			if (gene.RandomChosen || other.RandomChosen || !gene.ConflictsWith(other))
			{
				return false;
			}
			if (isXenogene == otherIsXenogene)
			{
				return GeneUtility.GenesInOrder.IndexOf(gene) <= GeneUtility.GenesInOrder.IndexOf(other);
			}
			return isXenogene && !otherIsXenogene;
		}

		// Token: 0x0600D2B7 RID: 53943 RVA: 0x004D7C98 File Offset: 0x004D5E98
		public static bool IsBaseliner(this Pawn pawn)
		{
			if (ModsConfig.BiotechActive)
			{
				Pawn_GeneTracker genes = pawn.genes;
				if (((genes != null) ? genes.Xenotype : null) != null)
				{
					return pawn.genes.Xenotype == XenotypeDefOf.Baseliner && pawn.genes.CustomXenotype == null;
				}
			}
			return true;
		}

		// Token: 0x0600D2B8 RID: 53944 RVA: 0x004D7CE4 File Offset: 0x004D5EE4
		public static bool SameHeritableXenotype(Pawn pawn1, Pawn pawn2)
		{
			if (((pawn1 != null) ? pawn1.genes : null) == null || ((pawn2 != null) ? pawn2.genes : null) == null)
			{
				return false;
			}
			if (pawn1.genes.UniqueXenotype || pawn2.genes.UniqueXenotype)
			{
				for (int i = 0; i < pawn1.genes.Endogenes.Count; i++)
				{
					Gene gene = pawn1.genes.Endogenes[i];
					if (gene.def != GeneDefOf.Inbred && gene.def.endogeneCategory != EndogeneCategory.Melanin && gene.def.endogeneCategory != EndogeneCategory.HairColor && !pawn2.genes.Endogenes.Any((Gene x) => x.def == gene.def))
					{
						return false;
					}
				}
				for (int j = 0; j < pawn2.genes.Endogenes.Count; j++)
				{
					Gene gene = pawn2.genes.Endogenes[j];
					if (gene.def != GeneDefOf.Inbred && gene.def.endogeneCategory != EndogeneCategory.Melanin && gene.def.endogeneCategory != EndogeneCategory.HairColor && !pawn1.genes.Endogenes.Any((Gene x) => x.def == gene.def))
					{
						return false;
					}
				}
				return true;
			}
			return pawn1.genes.Xenotype == pawn2.genes.Xenotype && pawn1.genes.Xenotype.inheritable;
		}

		// Token: 0x0600D2B9 RID: 53945 RVA: 0x004D7E80 File Offset: 0x004D6080
		public static bool SameXenotype(Pawn pawn1, Pawn pawn2)
		{
			Pawn pawn3 = pawn1;
			if (((pawn3 != null) ? pawn3.genes : null) != null)
			{
				Pawn pawn4 = pawn2;
				if (((pawn4 != null) ? pawn4.genes : null) != null)
				{
					if (pawn1.genes.UniqueXenotype || pawn2.genes.UniqueXenotype)
					{
						int j;
						int k;
						for (k = 0; k < pawn1.genes.Xenogenes.Count; k = j + 1)
						{
							if (!pawn2.genes.Xenogenes.Any((Gene x) => x.def == pawn1.genes.Xenogenes[k].def))
							{
								return false;
							}
							j = k;
						}
						int i;
						for (i = 0; i < pawn2.genes.Xenogenes.Count; i = j + 1)
						{
							if (!pawn1.genes.Xenogenes.Any((Gene x) => x.def == pawn2.genes.Xenogenes[i].def))
							{
								return false;
							}
							j = i;
						}
						CustomXenotype customXenotype = pawn1.genes.CustomXenotype;
						CustomXenotype customXenotype2 = pawn2.genes.CustomXenotype;
						return customXenotype == customXenotype2;
					}
					return pawn1.genes.Xenotype == pawn2.genes.Xenotype;
				}
			}
			return false;
		}

		// Token: 0x0600D2BA RID: 53946 RVA: 0x004D802C File Offset: 0x004D622C
		public static bool PawnIsCustomXenotype(Pawn pawn, CustomXenotype custom)
		{
			if (!ModsConfig.BiotechActive || pawn.genes == null)
			{
				return false;
			}
			List<Gene> list = custom.inheritable ? pawn.genes.Endogenes : pawn.genes.Xenogenes;
			int i;
			Predicate<Gene> <>9__0;
			int k;
			for (i = 0; i < custom.genes.Count; i = k + 1)
			{
				if (custom.genes[i].passOnDirectly)
				{
					List<Gene> list2 = list;
					Predicate<Gene> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((Gene x) => x.def == custom.genes[i]));
					}
					if (!list2.Any(predicate))
					{
						return false;
					}
				}
				k = i;
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].def.passOnDirectly && !custom.genes.Contains(list[j].def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D2BB RID: 53947 RVA: 0x004D8140 File Offset: 0x004D6340
		public static int MetabolismAfterImplanting(Pawn pawn, GeneSet geneSet)
		{
			GeneUtility.tmpTypedGenes.Clear();
			foreach (Gene gene in pawn.genes.Endogenes)
			{
				GeneUtility.tmpTypedGenes.Add(new GeneDefWithType(gene.def, false));
			}
			foreach (GeneDef geneDef in geneSet.GenesListForReading)
			{
				GeneUtility.tmpTypedGenes.Add(new GeneDefWithType(geneDef, true));
			}
			int result = GeneUtility.tmpTypedGenes.NonOverriddenGenes().Sum((GeneDef x) => x.biostatMet);
			GeneUtility.tmpTypedGenes.Clear();
			return result;
		}

		// Token: 0x0600D2BC RID: 53948 RVA: 0x004D8238 File Offset: 0x004D6438
		public static void GiveReimplantJob(Pawn pawn, Pawn targPawn)
		{
			pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.AbsorbXenogerm, targPawn), new JobTag?(JobTag.Misc), false);
			if (targPawn.HomeFaction != null && !targPawn.HomeFaction.Hidden && targPawn.HomeFaction != pawn.Faction && !targPawn.HomeFaction.HostileTo(Faction.OfPlayer))
			{
				Messages.Message("MessageAbsorbingXenogermWillAngerFaction".Translate(targPawn.HomeFaction, targPawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0600D2BD RID: 53949 RVA: 0x004D82D4 File Offset: 0x004D64D4
		public static bool CanAbsorbXenogerm(Pawn pawn)
		{
			return ((pawn != null) ? pawn.genes : null) != null && pawn.genes.HasActiveGene(GeneDefOf.XenogermReimplanter) && ((pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure) || (pawn.Downed && pawn.genes.Xenogenes.Any<Gene>()));
		}

		// Token: 0x0600D2BE RID: 53950 RVA: 0x004D833C File Offset: 0x004D653C
		public static int AddedAndImplantedPartsWithXenogenesCount(Pawn pawn)
		{
			int num = pawn.health.hediffSet.CountAddedAndImplantedParts();
			if (ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.Xenogenes.Any<Gene>())
			{
				num++;
			}
			return num;
		}

		// Token: 0x04008591 RID: 34193
		private static List<GeneDef> cachedGeneDefsInOrder = null;

		// Token: 0x04008592 RID: 34194
		private const int MaxTriesToGenerateSingleXenotypeName = 150;

		// Token: 0x04008593 RID: 34195
		private const string BiostatsTexPath = "UI/Icons/Biostats/";

		// Token: 0x04008594 RID: 34196
		private const string PrefixKeyword = "genePrefix";

		// Token: 0x04008595 RID: 34197
		private const string SuffixKeyword = "geneSuffix";

		// Token: 0x04008596 RID: 34198
		private const string WholeNameKeyword = "wholeName";

		// Token: 0x04008597 RID: 34199
		private const string RootKeyword = "r_name";

		// Token: 0x04008598 RID: 34200
		public static readonly CachedTexture UniqueXenotypeTex = new CachedTexture("UI/Icons/Xenotypes/Other");

		// Token: 0x04008599 RID: 34201
		public static readonly CachedTexture GCXTex = new CachedTexture("UI/Icons/Biostats/Complexity");

		// Token: 0x0400859A RID: 34202
		public static readonly CachedTexture METTex = new CachedTexture("UI/Icons/Biostats/Metabolism");

		// Token: 0x0400859B RID: 34203
		public static readonly CachedTexture ARCTex = new CachedTexture("UI/Icons/Biostats/ArchiteCapsuleRequired");

		// Token: 0x0400859C RID: 34204
		private static readonly GeneUtility.GeneCount[] GeneCountProbabilities = new GeneUtility.GeneCount[]
		{
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 1,
				chance = 0.66f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 2,
				chance = 0.2f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 3,
				chance = 0.05f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 4,
				chance = 0.02f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 1,
				architeCount = 1,
				chance = 0.05f
			},
			new GeneUtility.GeneCount
			{
				nonArchiteCount = 2,
				architeCount = 1,
				chance = 0.02f
			}
		};

		// Token: 0x0400859D RID: 34205
		public static readonly Color GCXColor = new ColorInt(105, 213, 240).ToColor;

		// Token: 0x0400859E RID: 34206
		public static readonly Color METColor = new ColorInt(247, 72, 160).ToColor;

		// Token: 0x0400859F RID: 34207
		public static readonly Color ARCColor = new ColorInt(214, 219, 61).ToColor;

		// Token: 0x040085A0 RID: 34208
		private static List<GeneSymbolPack> tmpSymbolPacks = new List<GeneSymbolPack>();

		// Token: 0x040085A1 RID: 34209
		private static List<GeneDef> tmpGenes = new List<GeneDef>();

		// Token: 0x040085A2 RID: 34210
		private static HashSet<GeneDefWithType> tmpOverriddenGenes = new HashSet<GeneDefWithType>();

		// Token: 0x040085A3 RID: 34211
		private static List<GeneDefWithType> tmpGeneDefsWithType = new List<GeneDefWithType>();

		// Token: 0x040085A4 RID: 34212
		private static List<GeneDefWithType> tmpTypedGenes = new List<GeneDefWithType>();

		// Token: 0x02003C68 RID: 15464
		private struct GeneCount
		{
			// Token: 0x0400EF69 RID: 61289
			public int nonArchiteCount;

			// Token: 0x0400EF6A RID: 61290
			public int architeCount;

			// Token: 0x0400EF6B RID: 61291
			public float chance;
		}
	}
}
