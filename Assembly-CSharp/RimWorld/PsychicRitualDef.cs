using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C94 RID: 3220
	public class PsychicRitualDef : Def
	{
		// Token: 0x06005B2B RID: 23339 RVA: 0x00218888 File Offset: 0x00216A88
		public override void PostLoad()
		{
			if (!string.IsNullOrEmpty(this.iconPath))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x06005B2C RID: 23340 RVA: 0x002188A8 File Offset: 0x00216AA8
		public virtual AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			if (this.allowsDrafting)
			{
				return true;
			}
			return new AcceptanceReport("ParticipatingInPsychicRitual".Translate(pawn, this.label));
		}

		// Token: 0x06005B2D RID: 23341 RVA: 0x002188DE File Offset: 0x00216ADE
		public virtual AcceptanceReport AllowsFloatMenu(Pawn pawn)
		{
			if (this.allowsFloatMenu)
			{
				return true;
			}
			return new AcceptanceReport("ParticipatingInPsychicRitual".Translate(pawn, this.label));
		}

		// Token: 0x06005B2E RID: 23342 RVA: 0x00218914 File Offset: 0x00216B14
		public virtual bool BlocksSocialInteraction(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06005B2F RID: 23343 RVA: 0x00218917 File Offset: 0x00216B17
		public virtual AcceptanceReport AbilityAllowed(Ability ability)
		{
			return new AcceptanceReport("AbilityDisabledInPsychicRitual".Translate(ability.pawn, this.label));
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06005B30 RID: 23344 RVA: 0x00218943 File Offset: 0x00216B43
		public virtual List<PsychicRitualRoleDef> Roles
		{
			get
			{
				this.rolesBackingList.Clear();
				return this.rolesBackingList;
			}
		}

		// Token: 0x06005B31 RID: 23345 RVA: 0x00218956 File Offset: 0x00216B56
		public virtual PsychicRitualGraph CreateGraph()
		{
			return new PsychicRitualGraph();
		}

		// Token: 0x06005B32 RID: 23346 RVA: 0x0021895D File Offset: 0x00216B5D
		public virtual List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			throw new NotImplementedException("You must subclass PsychicRitualDef and override CreateToils() to return a list of toils.");
		}

		// Token: 0x06005B33 RID: 23347 RVA: 0x0021896C File Offset: 0x00216B6C
		public virtual PsychicRitualCandidatePool FindCandidatePool()
		{
			return new PsychicRitualCandidatePool(new List<Pawn>(from p in Find.CurrentMap.mapPawns.FreeColonistsAndPrisonersSpawned
			where !p.IsSubhuman
			select p), new List<Pawn>());
		}

		// Token: 0x06005B34 RID: 23348 RVA: 0x002189BB File Offset: 0x00216BBB
		public virtual PsychicRitualRoleAssignments BuildRoleAssignments(TargetInfo target)
		{
			return new PsychicRitualRoleAssignments(this.Roles, target, null);
		}

		// Token: 0x06005B35 RID: 23349 RVA: 0x002189CC File Offset: 0x00216BCC
		public static bool OfferingReachable(Map map, List<Pawn> pawns, IngredientCount offering, out int reachableCount)
		{
			bool result;
			using (new ProfilerBlock("Offering reachable"))
			{
				reachableCount = 0;
				float num = offering.GetBaseCount();
				if (num <= 0f)
				{
					result = true;
				}
				else
				{
					List<Thing> list;
					using (new ProfilerBlock("ThingsMatchingFilter"))
					{
						list = map.listerThings.ThingsMatchingFilter(offering.filter);
					}
					foreach (Thing thing in list)
					{
						if (num <= 0f)
						{
							break;
						}
						foreach (Pawn pawn in pawns)
						{
							if (pawn.CanReserveAndReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, false) && !thing.IsForbidden(pawn) && !thing.Fogged())
							{
								num -= (float)thing.stackCount;
								reachableCount += thing.stackCount;
								break;
							}
						}
					}
					result = (num <= 0f);
				}
			}
			return result;
		}

		// Token: 0x06005B36 RID: 23350 RVA: 0x00218B64 File Offset: 0x00216D64
		public virtual IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x06005B37 RID: 23351 RVA: 0x00218B6B File Offset: 0x00216D6B
		public virtual TaggedString PsychicRitualBegunMessage(PsychicRitualRoleAssignments assignments)
		{
			return "PsychicRitualBegun".Translate(this.label);
		}

		// Token: 0x06005B38 RID: 23352 RVA: 0x00218B82 File Offset: 0x00216D82
		public virtual TaggedString PsychicRitualCompletedMessage()
		{
			return "PsychicRitualCompleted".Translate(this.label);
		}

		// Token: 0x06005B39 RID: 23353 RVA: 0x00218B99 File Offset: 0x00216D99
		public virtual TaggedString PsychicRitualCanceledMessage(TaggedString reason)
		{
			if (reason.NullOrEmpty())
			{
				return "PsychicRitualCanceled".Translate(this.label);
			}
			return "PsychicRitualCanceledBecause".Translate(this.label, reason);
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x00218BD8 File Offset: 0x00216DD8
		public virtual TaggedString LeftPsychicRitualMessage(Pawn pawn, TaggedString reason)
		{
			if (reason.NullOrEmpty())
			{
				return "PsychicRitualLeft".Translate(pawn, this.label);
			}
			return "PsychicRitualLeftBecause".Translate(pawn, this.label, reason);
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x00218C2B File Offset: 0x00216E2B
		public virtual string GetPawnReport(PsychicRitual psychicRitual, Pawn pawn)
		{
			return "PsychicRitualAttending".Translate(this.label.Named("RITUALNAME"));
		}

		// Token: 0x06005B3C RID: 23356 RVA: 0x00218C4C File Offset: 0x00216E4C
		public virtual Lord MakeNewLord(PsychicRitualRoleAssignments assignments)
		{
			Lord lord = LordMaker.MakeNewLord(Faction.OfPlayer, new LordJob_PsychicRitual(this, assignments, -1f), Find.CurrentMap, assignments.AllAssignedPawns);
			Building building = assignments.Target.Thing as Building;
			if (building != null)
			{
				lord.AddBuilding(building);
			}
			return lord;
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x00218C9A File Offset: 0x00216E9A
		public virtual bool IsValidTarget(TargetInfo target, out AnyEnum reason)
		{
			reason = AnyEnum.None;
			return true;
		}

		// Token: 0x06005B3E RID: 23358 RVA: 0x00218CA8 File Offset: 0x00216EA8
		public virtual TaggedString InvalidTargetReason(AnyEnum reason)
		{
			if (reason == AnyEnum.None)
			{
				return TaggedString.Empty;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Unknown enum type ");
			defaultInterpolatedStringHandler.AppendFormatted(reason.enumType.ToStringSafe<Type>());
			defaultInterpolatedStringHandler.AppendLiteral("; did you forget to ");
			defaultInterpolatedStringHandler.AppendLiteral("override `");
			defaultInterpolatedStringHandler.AppendFormatted("InvalidTargetReason");
			defaultInterpolatedStringHandler.AppendLiteral("` in a child class?");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06005B3F RID: 23359 RVA: 0x00218D2C File Offset: 0x00216F2C
		public bool Visible
		{
			get
			{
				return this.playerCastable && (DebugSettings.godMode || this.researchPrerequisite == null || this.researchPrerequisite.IsFinished);
			}
		}

		// Token: 0x06005B40 RID: 23360 RVA: 0x00218D5C File Offset: 0x00216F5C
		public virtual void CalculateMaxPower(PsychicRitualRoleAssignments assignments, List<QualityFactor> powerFactorsOut, out float power)
		{
			power = 0f;
			int num = 0;
			IReadOnlyDictionary<PsychicRitualRoleDef, List<Pawn>> roleAssignments = assignments.RoleAssignments;
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in roleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				List<Pawn> list2 = list;
				if (psychicRitualRoleDef2.applyPowerOffset)
				{
					num += list2.Count;
				}
				if (psychicRitualRoleDef2.MaxCount != psychicRitualRoleDef2.MinCount)
				{
					float num2 = (float)(list2.Count - psychicRitualRoleDef2.MinCount) / (float)(psychicRitualRoleDef2.MaxCount - psychicRitualRoleDef2.MinCount);
					power += this.rolePowerFactor * num2;
					if (powerFactorsOut != null)
					{
						QualityFactor qualityFactor = new QualityFactor();
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
						defaultInterpolatedStringHandler.AppendFormatted<int>(list2.Count);
						defaultInterpolatedStringHandler.AppendLiteral(" / ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(psychicRitualRoleDef2.MaxCount);
						qualityFactor.count = defaultInterpolatedStringHandler.ToStringAndClear();
						qualityFactor.label = Find.ActiveLanguageWorker.Pluralize(psychicRitualRoleDef2.LabelCap, -1);
						qualityFactor.positive = (list2.Count >= psychicRitualRoleDef2.MinCount);
						qualityFactor.quality = this.rolePowerFactor * num2;
						qualityFactor.toolTip = psychicRitualRoleDef2.description.CapitalizeFirst().EndWithPeriod();
						powerFactorsOut.Add(qualityFactor);
					}
				}
			}
			if (num > 0)
			{
				float num3 = 0f;
				foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in roleAssignments)
				{
					PsychicRitualRoleDef psychicRitualRoleDef;
					List<Pawn> list;
					keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
					PsychicRitualRoleDef psychicRitualRoleDef3 = psychicRitualRoleDef;
					List<Pawn> list3 = list;
					if (psychicRitualRoleDef3.applyPowerOffset)
					{
						foreach (Pawn thing in list3)
						{
							num3 += thing.GetStatValue(StatDefOf.PsychicRitualQualityOffset, true, -1) / (float)num;
						}
					}
				}
				if (!Mathf.Approximately(num3, 0f))
				{
					power += num3;
					if (powerFactorsOut != null)
					{
						powerFactorsOut.Add(new QualityFactor
						{
							label = "PsychicRitualDef_InvocationCircle_QualityFactor_Ideoligion".Translate(),
							positive = (num3 > 0f),
							count = num3.ToStringPercent(),
							quality = num3,
							toolTip = "PsychicRitualDef_InvocationCircle_QualityFactor_Ideoligion_Tooltip".Translate()
						});
					}
				}
			}
			power = Mathf.Clamp01(power);
		}

		// Token: 0x06005B41 RID: 23361 RVA: 0x00219004 File Offset: 0x00217204
		public virtual void RemoveIncapablePawns(PsychicRitual psychicRitual)
		{
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in psychicRitual.assignments.RoleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				List<Pawn> collection = list;
				PsychicRitualDef.tmpPawnsIterationList.Clear();
				PsychicRitualDef.tmpPawnsIterationList.AddRange(collection);
				foreach (Pawn pawn in PsychicRitualDef.tmpPawnsIterationList)
				{
					PsychicRitualRoleDef.Reason reason;
					if (!psychicRitualRoleDef2.PawnCanDo(PsychicRitualRoleDef.Context.Runtime, pawn, psychicRitual.assignments.Target, out reason) && psychicRitual.LeaveOrCancelPsychicRitual(psychicRitualRoleDef2, pawn, reason.ToPlayerReadable()) == PsychicRitual.LeftOrCanceled.Canceled)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06005B42 RID: 23362 RVA: 0x002190E4 File Offset: 0x002172E4
		public virtual void CheckPsychicRitualCancelConditions(PsychicRitual psychicRitual)
		{
			TargetInfo target = psychicRitual.assignments.Target;
			if (target.ThingDestroyed)
			{
				psychicRitual.CancelPsychicRitual("PsychicRitualDef_TargetDestroyed".Translate(target.Thing.Named("TARGET")));
			}
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x00219127 File Offset: 0x00217327
		public virtual TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return TaggedString.Empty;
		}

		// Token: 0x06005B44 RID: 23364 RVA: 0x0021912E File Offset: 0x0021732E
		public virtual IEnumerable<TaggedString> OutcomeWarnings(PsychicRitualRoleAssignments assignments)
		{
			return Enumerable.Empty<TaggedString>();
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x00219135 File Offset: 0x00217335
		public virtual TaggedString TimeAndOfferingLabel()
		{
			return TaggedString.Empty;
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x0021913C File Offset: 0x0021733C
		public virtual void InitializeCast(Map map)
		{
		}

		// Token: 0x06005B47 RID: 23367 RVA: 0x00219140 File Offset: 0x00217340
		public virtual IntVec3 GetBestStandableRolePosition(bool playerRitual, IntVec3 origin, IntVec3 ritualPosition, Map map, float radius = 8f)
		{
			if (playerRitual)
			{
				return origin;
			}
			IntVec3 result = CellFinder.StandableCellNear(origin, map, radius, (IntVec3 c) => map.reachability.CanReach(ritualPosition, c, PathEndMode.OnCell, TraverseMode.NoPassClosedDoorsOrWater));
			if (result.IsValid)
			{
				return result;
			}
			return origin;
		}

		// Token: 0x06005B48 RID: 23368 RVA: 0x0021918E File Offset: 0x0021738E
		public virtual IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
		{
			return new PsychicRitualDef.<GetPawnTooltipExtras>d__49(-2);
		}

		// Token: 0x0400343A RID: 13370
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x0400343B RID: 13371
		public bool allowsDrafting;

		// Token: 0x0400343C RID: 13372
		public bool allowsFloatMenu;

		// Token: 0x0400343D RID: 13373
		public int cooldownHours;

		// Token: 0x0400343E RID: 13374
		public bool nonRequiredPawnsMayLeave;

		// Token: 0x0400343F RID: 13375
		public float rolePowerFactor = 0.2f;

		// Token: 0x04003440 RID: 13376
		public bool aiCastable;

		// Token: 0x04003441 RID: 13377
		public bool playerCastable = true;

		// Token: 0x04003442 RID: 13378
		public float minThreatPoints;

		// Token: 0x04003443 RID: 13379
		public bool castableOnPocketMaps = true;

		// Token: 0x04003444 RID: 13380
		public List<PlanetLayerDef> layerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x04003445 RID: 13381
		[MustTranslate]
		public string letterAICompleteLabel;

		// Token: 0x04003446 RID: 13382
		[MustTranslate]
		public string letterAICompleteText;

		// Token: 0x04003447 RID: 13383
		[MustTranslate]
		public string letterAIArrivedText;

		// Token: 0x04003448 RID: 13384
		[NoTranslate]
		public string iconPath;

		// Token: 0x04003449 RID: 13385
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x0400344A RID: 13386
		private List<PsychicRitualRoleDef> rolesBackingList = new List<PsychicRitualRoleDef>(8);

		// Token: 0x0400344B RID: 13387
		private static readonly List<Pawn> tmpPawnsIterationList = new List<Pawn>(16);
	}
}
