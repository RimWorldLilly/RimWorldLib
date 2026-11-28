using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E8 RID: 5608
	public static class TrainableUtility
	{
		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x06008B76 RID: 35702 RVA: 0x00329568 File Offset: 0x00327768
		public static List<TrainableDef> TrainableDefsInListOrder
		{
			get
			{
				return TrainableUtility.defsInListOrder;
			}
		}

		// Token: 0x06008B77 RID: 35703 RVA: 0x00329570 File Offset: 0x00327770
		public static void Reset()
		{
			TrainableUtility.defsInListOrder.Clear();
			TrainableUtility.defsInListOrder.AddRange(from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td);
			bool flag;
			do
			{
				flag = false;
				for (int i = 0; i < TrainableUtility.defsInListOrder.Count; i++)
				{
					TrainableDef trainableDef = TrainableUtility.defsInListOrder[i];
					if (trainableDef.prerequisites != null)
					{
						for (int j = 0; j < trainableDef.prerequisites.Count; j++)
						{
							if (trainableDef.indent <= trainableDef.prerequisites[j].indent)
							{
								trainableDef.indent = trainableDef.prerequisites[j].indent + 1;
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			while (flag);
		}

		// Token: 0x06008B78 RID: 35704 RVA: 0x00329637 File Offset: 0x00327837
		public static string MasterString(Pawn pawn)
		{
			if (pawn.playerSettings.Master == null)
			{
				return "(" + "NoneLower".TranslateSimple() + ")";
			}
			return RelationsUtility.LabelWithBondInfo(pawn.playerSettings.Master, pawn);
		}

		// Token: 0x06008B79 RID: 35705 RVA: 0x00329671 File Offset: 0x00327871
		public static int MinimumHandlingSkill(Pawn p)
		{
			return Mathf.RoundToInt(p.GetStatValue(StatDefOf.MinimumHandlingSkill, true, -1));
		}

		// Token: 0x06008B7A RID: 35706 RVA: 0x00329688 File Offset: 0x00327888
		public static void MasterSelectButton(Rect rect, Pawn pawn, bool paintable)
		{
			Widgets.Dropdown<Pawn, Pawn>(rect, pawn, new Func<Pawn, Pawn>(TrainableUtility.MasterSelectButton_GetMaster), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>>(TrainableUtility.MasterSelectButton_GenerateMenu), TrainableUtility.MasterString(pawn).Truncate(rect.width, null), null, TrainableUtility.MasterString(pawn), null, null, paintable);
		}

		// Token: 0x06008B7B RID: 35707 RVA: 0x003296D1 File Offset: 0x003278D1
		private static Pawn MasterSelectButton_GetMaster(Pawn pet)
		{
			return pet.playerSettings.Master;
		}

		// Token: 0x06008B7C RID: 35708 RVA: 0x003296DE File Offset: 0x003278DE
		private static IEnumerable<Widgets.DropdownMenuElement<Pawn>> MasterSelectButton_GenerateMenu(Pawn p)
		{
			TrainableUtility.<MasterSelectButton_GenerateMenu>d__10 <MasterSelectButton_GenerateMenu>d__ = new TrainableUtility.<MasterSelectButton_GenerateMenu>d__10(-2);
			<MasterSelectButton_GenerateMenu>d__.<>3__p = p;
			return <MasterSelectButton_GenerateMenu>d__;
		}

		// Token: 0x06008B7D RID: 35709 RVA: 0x003296F0 File Offset: 0x003278F0
		public static bool CanBeMaster(Pawn master, Pawn animal, bool checkSpawned = true)
		{
			if ((checkSpawned && !master.Spawned) || master.IsPrisoner)
			{
				return false;
			}
			if (ModsConfig.IdeologyActive && animal.RaceProps.animalType == AnimalType.Dryad && animal.connections != null)
			{
				foreach (Thing thing in animal.connections.ConnectedThings)
				{
					CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
					if (compTreeConnection != null && compTreeConnection.ConnectedPawn == master)
					{
						return true;
					}
				}
				return false;
			}
			if (master.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				return true;
			}
			int level = master.skills.GetSkill(SkillDefOf.Animals).Level;
			int num = TrainableUtility.MinimumHandlingSkill(animal);
			return level >= num;
		}

		// Token: 0x06008B7E RID: 35710 RVA: 0x003297C4 File Offset: 0x003279C4
		public static string GetIconTooltipText(Pawn pawn)
		{
			string text = "";
			if (pawn.playerSettings != null && pawn.playerSettings.Master != null)
			{
				text += string.Format("{0}: {1}\n", "Master".Translate(), pawn.playerSettings.Master.LabelShort);
			}
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			if (allColonistBondsFor.Any<Pawn>())
			{
				text += string.Format("{0}: {1}\n", "BondedTo".Translate(), (from bond in allColonistBondsFor
				select bond.LabelShort).ToCommaList(true, false));
			}
			return text.TrimEndNewlines();
		}

		// Token: 0x06008B7F RID: 35711 RVA: 0x00329880 File Offset: 0x00327A80
		public static IEnumerable<Pawn> GetAllColonistBondsFor(Pawn pet)
		{
			return from bond in pet.relations.DirectRelations
			where bond.def == PawnRelationDefOf.Bond && bond.otherPawn != null && bond.otherPawn.IsColonist
			select bond.otherPawn;
		}

		// Token: 0x06008B80 RID: 35712 RVA: 0x003298E0 File Offset: 0x00327AE0
		public static int DegradationPeriodTicks(ThingDef def)
		{
			return Mathf.RoundToInt(TrainableUtility.DecayIntervalDaysFromWildnessCurve.Evaluate(def.GetStatValueAbstract(StatDefOf.Wildness, null)) * 60000f);
		}

		// Token: 0x06008B81 RID: 35713 RVA: 0x00329903 File Offset: 0x00327B03
		public static int DegradationPeriodTicks(Pawn pawn)
		{
			if (pawn == null)
			{
				return 0;
			}
			return Mathf.RoundToInt(TrainableUtility.DecayIntervalDaysFromWildnessCurve.Evaluate(pawn.GetStatValue(StatDefOf.Wildness, true, 1)) * 60000f);
		}

		// Token: 0x06008B82 RID: 35714 RVA: 0x0032992C File Offset: 0x00327B2C
		public static bool TamenessCanDecay(ThingDef def)
		{
			return !def.race.FenceBlocked && def.GetStatValueAbstract(StatDefOf.Wildness, null) > 0.101f;
		}

		// Token: 0x06008B83 RID: 35715 RVA: 0x00329950 File Offset: 0x00327B50
		public static bool TamenessCanDecay(Pawn pawn)
		{
			return !pawn.FenceBlocked && pawn.GetStatValue(StatDefOf.Wildness, true, 1) > 0.101f;
		}

		// Token: 0x06008B84 RID: 35716 RVA: 0x00329970 File Offset: 0x00327B70
		public static bool TrainedTooRecently(Pawn animal)
		{
			return Find.TickManager.TicksGame < animal.mindState.lastAssignedInteractTime + 15000;
		}

		// Token: 0x06008B85 RID: 35717 RVA: 0x00329990 File Offset: 0x00327B90
		public static bool IsReleasedToAttack(this Pawn pawn)
		{
			bool flag;
			if (pawn == null)
			{
				flag = (null != null);
			}
			else
			{
				Pawn_PlayerSettings playerSettings = pawn.playerSettings;
				if (playerSettings == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn master = playerSettings.Master;
					flag = (((master != null) ? master.playerSettings : null) != null);
				}
			}
			return flag && pawn.training != null && pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release);
		}

		// Token: 0x06008B86 RID: 35718 RVA: 0x003299F8 File Offset: 0x00327BF8
		public static TrainabilityDef GetTrainability(Pawn pawn)
		{
			if (pawn == null)
			{
				return null;
			}
			if (ModsConfig.OdysseyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.SentienceCatalyst, false))
			{
				return TrainableUtility.GetNextHighestTrainability(pawn.RaceProps.trainability);
			}
			return pawn.RaceProps.trainability;
		}

		// Token: 0x06008B87 RID: 35719 RVA: 0x00329A48 File Offset: 0x00327C48
		private static TrainabilityDef GetNextHighestTrainability(TrainabilityDef racePropsTrainability)
		{
			if (TrainableUtility.cachedTrainabilityDefs == null)
			{
				TrainableUtility.cachedTrainabilityDefs = DefDatabase<TrainabilityDef>.AllDefsListForReading.ToList<TrainabilityDef>();
				TrainableUtility.cachedTrainabilityDefs.SortBy((TrainabilityDef td) => td.intelligenceOrder);
			}
			for (int i = 0; i < TrainableUtility.cachedTrainabilityDefs.Count; i++)
			{
				if (TrainableUtility.cachedTrainabilityDefs[i] == racePropsTrainability)
				{
					return TrainableUtility.cachedTrainabilityDefs[Mathf.Min(TrainableUtility.cachedTrainabilityDefs.Count - 1, i + 1)];
				}
			}
			return null;
		}

		// Token: 0x040055A4 RID: 21924
		private static List<TrainableDef> defsInListOrder = new List<TrainableDef>();

		// Token: 0x040055A5 RID: 21925
		public const int MinTrainInterval = 15000;

		// Token: 0x040055A6 RID: 21926
		private static readonly SimpleCurve DecayIntervalDaysFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 12f),
				true
			},
			{
				new CurvePoint(1f, 6f),
				true
			}
		};

		// Token: 0x040055A7 RID: 21927
		private static List<TrainabilityDef> cachedTrainabilityDefs;
	}
}
