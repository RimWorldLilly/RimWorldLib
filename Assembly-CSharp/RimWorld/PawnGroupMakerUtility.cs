using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DC RID: 5340
	public class PawnGroupMakerUtility
	{
		// Token: 0x0600828E RID: 33422 RVA: 0x002ED99C File Offset: 0x002EBB9C
		public static IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool warnOnZeroResults = true)
		{
			PawnGroupMakerUtility.<GeneratePawns>d__2 <GeneratePawns>d__ = new PawnGroupMakerUtility.<GeneratePawns>d__2(-2);
			<GeneratePawns>d__.<>3__parms = parms;
			<GeneratePawns>d__.<>3__warnOnZeroResults = warnOnZeroResults;
			return <GeneratePawns>d__;
		}

		// Token: 0x0600828F RID: 33423 RVA: 0x002ED9B3 File Offset: 0x002EBBB3
		public static IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			PawnGroupMakerUtility.<GeneratePawnKindsExample>d__3 <GeneratePawnKindsExample>d__ = new PawnGroupMakerUtility.<GeneratePawnKindsExample>d__3(-2);
			<GeneratePawnKindsExample>d__.<>3__parms = parms;
			return <GeneratePawnKindsExample>d__;
		}

		// Token: 0x06008290 RID: 33424 RVA: 0x002ED9C4 File Offset: 0x002EBBC4
		public static bool TryGetRandomPawnGroupMaker(PawnGroupMakerParms parms, out PawnGroupMaker pawnGroupMaker, bool ignoreCommonality = false)
		{
			if (parms.seed != null)
			{
				Rand.PushState(parms.seed.Value);
			}
			IEnumerable<PawnGroupMaker> source = from gm in parms.faction.def.pawnGroupMakers
			where gm.kindDef == parms.groupKind && gm.CanGenerateFrom(parms)
			select gm;
			bool result;
			if (ignoreCommonality)
			{
				result = source.TryRandomElement(out pawnGroupMaker);
			}
			else
			{
				result = source.TryRandomElementByWeight((PawnGroupMaker gm) => gm.commonality, out pawnGroupMaker);
			}
			if (parms.seed != null)
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x06008291 RID: 33425 RVA: 0x002EDA78 File Offset: 0x002EBC78
		public static bool PawnGenOptionValid(PawnGenOption o, PawnGroupMakerParms groupParms, List<PawnGenOptionWithXenotype> chosenOptions = null)
		{
			PawnGroupMakerUtility.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.chosenOptions = chosenOptions;
			if (groupParms != null)
			{
				if (groupParms.generateFightersOnly && !o.kind.isFighter)
				{
					return false;
				}
				if (groupParms.dontUseSingleUseRocketLaunchers && o.kind.weaponTags != null && o.kind.weaponTags.Contains("GunSingleUse"))
				{
					return false;
				}
				if (groupParms.raidStrategy != null && !groupParms.raidStrategy.Worker.CanUsePawnGenOption(groupParms.points, o, CS$<>8__locals1.chosenOptions, groupParms.faction))
				{
					return false;
				}
				if (groupParms.raidAgeRestriction != null && groupParms.raidAgeRestriction.Worker.ShouldApplyToKind(o.kind) && !groupParms.raidAgeRestriction.Worker.CanUseKind(o.kind))
				{
					return false;
				}
			}
			return (!ModsConfig.BiotechActive || !Find.BossgroupManager.ReservedByBossgroup(o.kind)) && (!ModsConfig.AnomalyActive || !(o.kind is CreepJoinerFormKindDef)) && (o.kind.maxPerGroup >= int.MaxValue || PawnGroupMakerUtility.<PawnGenOptionValid>g__ChosenKindCount|5_0(o.kind, ref CS$<>8__locals1) < o.kind.maxPerGroup);
		}

		// Token: 0x06008292 RID: 33426 RVA: 0x002EDBA4 File Offset: 0x002EBDA4
		public static List<PawnGenOptionWithXenotype> GetOptions(PawnGroupMakerParms groupParms, FactionDef faction, List<PawnGenOption> options, float pointsTotal, float pointsLeft, float? maxCost, List<PawnGenOptionWithXenotype> chosenOptions = null, bool leaderChosen = false)
		{
			List<PawnGenOptionWithXenotype> list = new List<PawnGenOptionWithXenotype>();
			bool flag = ModsConfig.BiotechActive && (faction == null || faction.humanlikeFaction);
			float num = maxCost ?? PawnGroupMakerUtility.MaxPawnCost((groupParms != null) ? groupParms.faction : null, pointsTotal, (groupParms != null) ? groupParms.raidStrategy : null, (groupParms != null) ? groupParms.groupKind : null);
			int i = 0;
			while (i < options.Count)
			{
				PawnGenOption pawnGenOption = options[i];
				if (flag)
				{
					using (Dictionary<XenotypeDef, float>.Enumerator enumerator = PawnGenerator.XenotypesAvailableFor(pawnGenOption.kind, faction, (groupParms != null) ? groupParms.faction : null).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<XenotypeDef, float> keyValuePair = enumerator.Current;
							if (PawnGroupMakerUtility.CanUseOption(pawnGenOption, keyValuePair.Key, groupParms, chosenOptions, pointsLeft, num * keyValuePair.Key.combatPowerFactor, leaderChosen))
							{
								list.Add(new PawnGenOptionWithXenotype(pawnGenOption, keyValuePair.Key, pawnGenOption.selectionWeight * keyValuePair.Value));
							}
						}
						goto IL_128;
					}
					goto IL_101;
				}
				goto IL_101;
				IL_128:
				i++;
				continue;
				IL_101:
				if (PawnGroupMakerUtility.CanUseOption(pawnGenOption, null, groupParms, chosenOptions, pointsLeft, num, leaderChosen))
				{
					list.Add(new PawnGenOptionWithXenotype(pawnGenOption, null, pawnGenOption.selectionWeight));
					goto IL_128;
				}
				goto IL_128;
			}
			return list;
		}

		// Token: 0x06008293 RID: 33427 RVA: 0x002EDD00 File Offset: 0x002EBF00
		public static bool AnyOptions(PawnGroupMakerParms groupParms, FactionDef faction, List<PawnGenOption> options, float points)
		{
			bool flag = ModsConfig.BiotechActive && (faction == null || faction.humanlikeFaction);
			int i = 0;
			while (i < options.Count)
			{
				PawnGenOption pawnGenOption = options[i];
				if (flag)
				{
					using (Dictionary<XenotypeDef, float>.Enumerator enumerator = PawnGenerator.XenotypesAvailableFor(pawnGenOption.kind, faction, (groupParms != null) ? groupParms.faction : null).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<XenotypeDef, float> keyValuePair = enumerator.Current;
							if (PawnGroupMakerUtility.CanUseOption(pawnGenOption, keyValuePair.Key, groupParms, null, points, points, false))
							{
								return true;
							}
						}
						goto IL_91;
					}
					goto IL_81;
				}
				goto IL_81;
				IL_91:
				i++;
				continue;
				IL_81:
				if (PawnGroupMakerUtility.CanUseOption(pawnGenOption, null, groupParms, null, points, points, false))
				{
					return true;
				}
				goto IL_91;
			}
			return false;
		}

		// Token: 0x06008294 RID: 33428 RVA: 0x002EDDC4 File Offset: 0x002EBFC4
		private static bool CanUseOption(PawnGenOption o, XenotypeDef xenotype, PawnGroupMakerParms groupParms, List<PawnGenOptionWithXenotype> chosenOptions, float pointsLeft, float maxOptionCost, bool leaderChosen)
		{
			float num = o.Cost;
			if (xenotype != null)
			{
				num *= xenotype.combatPowerFactor;
			}
			return num <= pointsLeft && num <= maxOptionCost && (!leaderChosen || !o.kind.factionLeader) && PawnGroupMakerUtility.PawnGenOptionValid(o, groupParms, chosenOptions) && (Find.Storyteller.difficulty.ChildRaidersAllowed || o.kind.pawnGroupDevelopmentStage == null || o.kind.pawnGroupDevelopmentStage.Value == DevelopmentalStage.Adult);
		}

		// Token: 0x06008295 RID: 33429 RVA: 0x002EDE4C File Offset: 0x002EC04C
		public static IEnumerable<PawnGenOptionWithXenotype> ChoosePawnGenOptionsByPoints(float pointsTotal, List<PawnGenOption> options, PawnGroupMakerParms groupParms)
		{
			if (groupParms.seed != null)
			{
				Rand.PushState(groupParms.seed.Value);
			}
			List<PawnGenOptionWithXenotype> list = new List<PawnGenOptionWithXenotype>();
			List<PawnGenOptionWithXenotype> chosenOptions = new List<PawnGenOptionWithXenotype>();
			float num = pointsTotal;
			bool leaderChosen = false;
			float highestCost = -1f;
			Func<PawnGenOptionWithXenotype, float> <>9__0;
			for (;;)
			{
				list.Clear();
				foreach (PawnGenOptionWithXenotype item in PawnGroupMakerUtility.GetOptions(groupParms, groupParms.faction.def, options, pointsTotal, num, null, chosenOptions, leaderChosen))
				{
					if (item.Cost <= num)
					{
						if (item.Cost > highestCost)
						{
							highestCost = item.Cost;
						}
						list.Add(item);
					}
				}
				Func<PawnGenOptionWithXenotype, float> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = delegate(PawnGenOptionWithXenotype gr)
					{
						if (!PawnGroupMakerUtility.PawnGenOptionValid(gr.Option, groupParms, chosenOptions))
						{
							return 0f;
						}
						return gr.SelectionWeight * PawnGroupMakerUtility.PawnWeightFactorByMostExpensivePawnCostFractionCurve.Evaluate(gr.Cost / highestCost);
					});
				}
				Func<PawnGenOptionWithXenotype, float> weightSelector = func;
				PawnGenOptionWithXenotype item2;
				if (!list.TryRandomElementByWeight(weightSelector, out item2))
				{
					break;
				}
				chosenOptions.Add(item2);
				num -= item2.Cost;
				if (item2.Option.kind.factionLeader)
				{
					leaderChosen = true;
				}
			}
			list.Clear();
			if (chosenOptions.Count == 1 && num > pointsTotal / 2f)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Used only ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(pointsTotal - num);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(pointsTotal);
				defaultInterpolatedStringHandler.AppendLiteral(" points generating for ");
				defaultInterpolatedStringHandler.AppendFormatted<Faction>(groupParms.faction);
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (groupParms.seed != null)
			{
				Rand.PopState();
			}
			return chosenOptions;
		}

		// Token: 0x06008296 RID: 33430 RVA: 0x002EE048 File Offset: 0x002EC248
		public static float MaxPawnCost(Faction faction, float totalPoints, RaidStrategyDef raidStrategy, PawnGroupKindDef groupKind)
		{
			if (faction == null)
			{
				return totalPoints;
			}
			float num = faction.def.maxPawnCostPerTotalPointsCurve.Evaluate(totalPoints);
			if (raidStrategy != null)
			{
				num = Mathf.Min(num, totalPoints / raidStrategy.minPawns);
			}
			num = Mathf.Max(num, faction.def.MinPointsToGeneratePawnGroup(groupKind, null) * 1.2f);
			if (raidStrategy != null)
			{
				num = Mathf.Max(num, raidStrategy.Worker.MinMaxAllowedPawnGenOptionCost(faction, groupKind) * 1.2f);
			}
			return num;
		}

		// Token: 0x06008297 RID: 33431 RVA: 0x002EE0B8 File Offset: 0x002EC2B8
		public static bool CanGenerateAnyNormalGroup(Faction faction, float points)
		{
			if (faction.def.pawnGroupMakers == null)
			{
				return false;
			}
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.faction = faction;
			pawnGroupMakerParms.points = points;
			for (int i = 0; i < faction.def.pawnGroupMakers.Count; i++)
			{
				PawnGroupMaker pawnGroupMaker = faction.def.pawnGroupMakers[i];
				if (pawnGroupMaker.kindDef == PawnGroupKindDefOf.Combat && pawnGroupMaker.CanGenerateFrom(pawnGroupMakerParms))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008298 RID: 33432 RVA: 0x002EE130 File Offset: 0x002EC330
		[DebugOutput]
		public static void PawnGroupsMade()
		{
			Dialog_DebugOptionListLister.ShowSimpleDebugMenu<Faction>(from fac in Find.FactionManager.AllFactions
			where !fac.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()
			select fac, (Faction fac) => fac.Name + " (" + fac.def.defName + ")", delegate(Faction fac)
			{
				StringBuilder sb = new StringBuilder();
				StringBuilder sb5 = sb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 3);
				defaultInterpolatedStringHandler.AppendLiteral("FACTION: ");
				defaultInterpolatedStringHandler.AppendFormatted(fac.Name);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted(fac.def.defName);
				defaultInterpolatedStringHandler.AppendLiteral(") min=");
				defaultInterpolatedStringHandler.AppendFormatted<float>(fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
				sb5.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				Action<float> action = delegate(float points)
				{
					if (points < fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null))
					{
						return;
					}
					PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
					pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
					pawnGroupMakerParms.tile = Find.CurrentMap.Tile;
					pawnGroupMakerParms.points = points;
					pawnGroupMakerParms.faction = fac;
					StringBuilder sb2 = sb;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(38, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("Group with ");
					defaultInterpolatedStringHandler2.AppendFormatted<float>(pawnGroupMakerParms.points);
					defaultInterpolatedStringHandler2.AppendLiteral(" points (max option cost: ");
					defaultInterpolatedStringHandler2.AppendFormatted<float>(PawnGroupMakerUtility.MaxPawnCost(fac, points, RaidStrategyDefOf.ImmediateAttack, PawnGroupKindDefOf.Combat));
					defaultInterpolatedStringHandler2.AppendLiteral(")");
					sb2.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
					float num = 0f;
					foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, false).OrderBy((Pawn pa) => pa.kindDef.combatPower))
					{
						string value;
						if (pawn.equipment.Primary != null)
						{
							value = pawn.equipment.Primary.Label;
						}
						else
						{
							value = "no-equipment";
						}
						Apparel apparel = pawn.apparel.FirstApparelOnBodyPartGroup(BodyPartGroupDefOf.Torso);
						string value2;
						if (apparel != null)
						{
							value2 = apparel.LabelCap;
						}
						else
						{
							value2 = "shirtless";
						}
						StringBuilder sb3 = sb;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(6, 4);
						defaultInterpolatedStringHandler3.AppendLiteral("  ");
						defaultInterpolatedStringHandler3.AppendFormatted(pawn.kindDef.combatPower.ToString("F0").PadRight(6));
						defaultInterpolatedStringHandler3.AppendFormatted(pawn.kindDef.defName);
						defaultInterpolatedStringHandler3.AppendLiteral(", ");
						defaultInterpolatedStringHandler3.AppendFormatted(value);
						defaultInterpolatedStringHandler3.AppendLiteral(", ");
						defaultInterpolatedStringHandler3.AppendFormatted(value2);
						sb3.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
						num += pawn.kindDef.combatPower;
					}
					StringBuilder sb4 = sb;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(19, 1);
					defaultInterpolatedStringHandler4.AppendLiteral("         totalCost ");
					defaultInterpolatedStringHandler4.AppendFormatted<float>(num);
					sb4.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
					sb.AppendLine();
				};
				foreach (float obj in DebugActionsUtility.PointsOptions(false))
				{
					action(obj);
				}
				Log.Message(sb.ToString());
			});
		}

		// Token: 0x06008299 RID: 33433 RVA: 0x002EE1B0 File Offset: 0x002EC3B0
		private static List<Faction> UsableFactions(float points, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			return Find.FactionManager.AllFactions.Where(delegate(Faction f)
			{
				if ((allowHidden || !f.Hidden) && !f.temporary && (allowDefeated || !f.defeated) && (allowNonHumanlike || f.def.humanlikeFaction) && (allowNonHostileToPlayer || f.HostileTo(Faction.OfPlayer)) && f.def.pawnGroupMakers != null)
				{
					if (f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat) && !f.def.raidsForbidden && (validator == null || validator(f)))
					{
						return points >= f.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null);
					}
				}
				return false;
			}).ToList<Faction>();
		}

		// Token: 0x0600829A RID: 33434 RVA: 0x002EE210 File Offset: 0x002EC410
		public static bool TryGetRandomFactionForCombatPawnGroup(float points, out Faction faction, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			return PawnGroupMakerUtility.UsableFactions(points, validator, allowNonHostileToPlayer, allowHidden, allowDefeated, allowNonHumanlike).TryRandomElementByWeight((Faction f) => f.def.RaidCommonalityFromPoints(points), out faction);
		}

		// Token: 0x0600829B RID: 33435 RVA: 0x002EE250 File Offset: 0x002EC450
		public static bool TryGetRandomFactionForCombatPawnGroupWeighted(IncidentParms parms, out Faction faction, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			float maxPoints = parms.points;
			if (maxPoints <= 0f)
			{
				maxPoints = 999999f;
			}
			return PawnGroupMakerUtility.UsableFactions(maxPoints, validator, allowNonHostileToPlayer, allowHidden, allowDefeated, allowNonHumanlike).TryRandomElementByWeight(delegate(Faction f)
			{
				float num = 1f;
				if (parms.target != null && f == parms.target.StoryState.lastRaidFaction)
				{
					num = 0.4f;
				}
				return f.def.RaidCommonalityFromPoints(maxPoints) * num;
			}, out faction);
		}

		// Token: 0x0600829E RID: 33438 RVA: 0x002EE31C File Offset: 0x002EC51C
		[CompilerGenerated]
		internal static int <PawnGenOptionValid>g__ChosenKindCount|5_0(PawnKindDef d, ref PawnGroupMakerUtility.<>c__DisplayClass5_0 A_1)
		{
			int num = 0;
			if (A_1.chosenOptions.NullOrEmpty<PawnGenOptionWithXenotype>())
			{
				return num;
			}
			for (int i = 0; i < A_1.chosenOptions.Count; i++)
			{
				if (A_1.chosenOptions[i].Option.kind == d)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x040050E9 RID: 20713
		private static readonly SimpleCurve PawnWeightFactorByMostExpensivePawnCostFractionCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0.01f),
				true
			},
			{
				new CurvePoint(0.3f, 0.3f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			}
		};

		// Token: 0x040050EA RID: 20714
		private const float FactionSelectionWeightFactor_RecentlyRaided = 0.4f;
	}
}
