using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B56 RID: 2902
	public class JobGiver_OptimizeApparel : ThinkNode_JobGiver
	{
		// Token: 0x060054CC RID: 21708 RVA: 0x001F8153 File Offset: 0x001F6353
		private static void SetNextOptimizeTick(Pawn pawn)
		{
			pawn.mindState.nextApparelOptimizeTick = Find.TickManager.TicksGame + Rand.Range(6000, 9000);
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x001F817C File Offset: 0x001F637C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.outfits == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" tried to run JobGiver_OptimizeApparel without an OutfitTracker");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 5643897);
				return null;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Non-colonist ");
				defaultInterpolatedStringHandler2.AppendFormatted<Pawn>(pawn);
				defaultInterpolatedStringHandler2.AppendLiteral(" tried to optimize apparel.");
				Log.ErrorOnce(defaultInterpolatedStringHandler2.ToStringAndClear(), 764323);
				return null;
			}
			if (pawn.IsMutant && pawn.mutant.Def.disableApparel)
			{
				return null;
			}
			if (pawn.IsQuestLodger())
			{
				return null;
			}
			if (!DebugViewSettings.debugApparelOptimize)
			{
				if (Find.TickManager.TicksGame < pawn.mindState.nextApparelOptimizeTick)
				{
					return null;
				}
			}
			else
			{
				JobGiver_OptimizeApparel.debugSb = new StringBuilder();
				StringBuilder stringBuilder = JobGiver_OptimizeApparel.debugSb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(17, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("Scanning for ");
				defaultInterpolatedStringHandler3.AppendFormatted<Pawn>(pawn);
				defaultInterpolatedStringHandler3.AppendLiteral(" at ");
				defaultInterpolatedStringHandler3.AppendFormatted<IntVec3>(pawn.Position);
				stringBuilder.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
			}
			Job result;
			if (ModsConfig.IdeologyActive && JobGiver_OptimizeApparel.TryCreateRecolorJob(pawn, out result, false))
			{
				return result;
			}
			ApparelPolicy currentApparelPolicy = pawn.outfits.CurrentApparelPolicy;
			List<Apparel> wornApparel = pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				bool flag = false;
				Apparel apparel = wornApparel[i];
				if (pawn.MapHeld.Biome.inVacuum && pawn.Position.GetVacuum(pawn.MapHeld) >= 0.5f)
				{
					flag = (apparel.def.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.VacuumResistance) > 0f);
				}
				if (!flag && !currentApparelPolicy.filter.Allows(apparel) && pawn.outfits.forcedHandler.AllowedToAutomaticallyDrop(apparel) && !pawn.apparel.IsLocked(apparel))
				{
					Job job = JobMaker.MakeJob(JobDefOf.RemoveApparel, apparel);
					job.haulDroppedApparel = true;
					return job;
				}
			}
			Thing thing = null;
			float num = 0f;
			JobGiver_OptimizeApparel.tmpApparelList.Clear();
			pawn.Map.listerThings.GetAllThings(JobGiver_OptimizeApparel.tmpApparelList, ThingRequestGroup.Apparel, null, true);
			foreach (IHaulSource haulSource in pawn.Map.haulDestinationManager.AllHaulSourcesListForReading)
			{
				foreach (Thing thing2 in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					Apparel apparel2 = thing2 as Apparel;
					if (apparel2 != null)
					{
						JobGiver_OptimizeApparel.tmpApparelList.Add(apparel2);
					}
				}
			}
			if (JobGiver_OptimizeApparel.tmpApparelList.Count == 0)
			{
				JobGiver_OptimizeApparel.SetNextOptimizeTick(pawn);
				return null;
			}
			JobGiver_OptimizeApparel.neededWarmth = PawnApparelGenerator.CalculateNeededWarmth(pawn, pawn.Map.TileInfo.tile, GenLocalDate.Twelfth(pawn));
			JobGiver_OptimizeApparel.wornApparelScores.Clear();
			for (int j = 0; j < wornApparel.Count; j++)
			{
				JobGiver_OptimizeApparel.wornApparelScores.Add(JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, wornApparel[j]));
			}
			for (int k = 0; k < JobGiver_OptimizeApparel.tmpApparelList.Count; k++)
			{
				Apparel apparel3 = (Apparel)JobGiver_OptimizeApparel.tmpApparelList[k];
				if (currentApparelPolicy.filter.Allows(apparel3) && apparel3.IsInAnyStorage() && !apparel3.IsForbidden(pawn) && !apparel3.IsBurning() && (apparel3.def.apparel.gender == Gender.None || apparel3.def.apparel.gender == pawn.gender))
				{
					float num2 = JobGiver_OptimizeApparel.ApparelScoreGain(pawn, apparel3, JobGiver_OptimizeApparel.wornApparelScores);
					if (DebugViewSettings.debugApparelOptimize)
					{
						StringBuilder stringBuilder2 = JobGiver_OptimizeApparel.debugSb;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(2, 2);
						defaultInterpolatedStringHandler4.AppendFormatted(apparel3.LabelCap);
						defaultInterpolatedStringHandler4.AppendLiteral(": ");
						defaultInterpolatedStringHandler4.AppendFormatted<float>(num2, "F2");
						stringBuilder2.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
					}
					if (num2 >= 0.05f && num2 >= num && (!CompBiocodable.IsBiocoded(apparel3) || CompBiocodable.IsBiocodedFor(apparel3, pawn)) && ApparelUtility.HasPartsToWear(pawn, apparel3.def))
					{
						LocalTargetInfo target = apparel3;
						IApparelSource apparelSource = apparel3.ParentHolder as IApparelSource;
						if (apparelSource != null)
						{
							Thing thing3 = apparelSource as Thing;
							if (thing3 != null)
							{
								if (!apparelSource.ApparelSourceEnabled)
								{
									goto IL_4A6;
								}
								target = thing3;
							}
						}
						if (pawn.CanReserveAndReach(target, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false) && apparel3.def.apparel.developmentalStageFilter.Has(pawn.DevelopmentalStage))
						{
							thing = apparel3;
							num = num2;
						}
					}
				}
				IL_4A6:;
			}
			JobGiver_OptimizeApparel.tmpApparelList.Clear();
			if (DebugViewSettings.debugApparelOptimize)
			{
				StringBuilder stringBuilder3 = JobGiver_OptimizeApparel.debugSb;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(6, 1);
				defaultInterpolatedStringHandler5.AppendLiteral("BEST: ");
				defaultInterpolatedStringHandler5.AppendFormatted<Thing>(thing);
				stringBuilder3.AppendLine(defaultInterpolatedStringHandler5.ToStringAndClear());
				Log.Message(JobGiver_OptimizeApparel.debugSb.ToString());
				JobGiver_OptimizeApparel.debugSb = null;
			}
			if (thing == null)
			{
				JobGiver_OptimizeApparel.SetNextOptimizeTick(pawn);
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Wear, thing);
		}

		// Token: 0x060054CE RID: 21710 RVA: 0x001F86D4 File Offset: 0x001F68D4
		public static bool TryCreateRecolorJob(Pawn pawn, out Job job, bool dryRun = false)
		{
			if (!ModLister.CheckIdeology("Apparel recoloring"))
			{
				job = null;
				return false;
			}
			if (pawn.apparel.AnyApparelNeedsRecoloring)
			{
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.StylingStation), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, false) && pawn.CanReserveSittableOrSpot(t.InteractionCell, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					try
					{
						foreach (Apparel apparel in pawn.apparel.WornApparel)
						{
							if (apparel.DesiredColor != null)
							{
								JobGiver_OptimizeApparel.tmpApparelToRecolor.Add(apparel);
							}
						}
						List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye);
						if (JobGiver_OptimizeApparel.tmpApparelToRecolor.Count > 0)
						{
							list.SortBy((Thing t) => t.Position.DistanceToSquared(pawn.Position));
							foreach (Thing thing2 in list)
							{
								if (pawn.CanReach(thing2, PathEndMode.Touch, Danger.Some, false, false, TraverseMode.ByPawn) && !thing2.IsForbidden(pawn))
								{
									int num = 0;
									while (num < thing2.stackCount && pawn.CanReserve(thing2, 1, num + 1, null, false))
									{
										JobGiver_OptimizeApparel.tmpQueueApparel.Add(JobGiver_OptimizeApparel.tmpApparelToRecolor[JobGiver_OptimizeApparel.tmpApparelToRecolor.Count - 1]);
										if (!JobGiver_OptimizeApparel.tmpQueueDye.Contains(thing2))
										{
											JobGiver_OptimizeApparel.tmpQueueDye.Add(thing2);
										}
										JobGiver_OptimizeApparel.tmpApparelToRecolor.RemoveAt(JobGiver_OptimizeApparel.tmpApparelToRecolor.Count - 1);
										if (JobGiver_OptimizeApparel.tmpApparelToRecolor.Count == 0)
										{
											break;
										}
										num++;
									}
									if (JobGiver_OptimizeApparel.tmpApparelToRecolor.Count == 0)
									{
										break;
									}
								}
							}
							if (JobGiver_OptimizeApparel.tmpQueueApparel.Count > 0)
							{
								if (dryRun)
								{
									job = null;
								}
								else
								{
									job = JobMaker.MakeJob(JobDefOf.RecolorApparel);
									List<LocalTargetInfo> targetQueue = job.GetTargetQueue(TargetIndex.A);
									List<LocalTargetInfo> targetQueue2 = job.GetTargetQueue(TargetIndex.B);
									targetQueue.AddRange(JobGiver_OptimizeApparel.tmpQueueDye);
									targetQueue2.AddRange(JobGiver_OptimizeApparel.tmpQueueApparel);
									job.SetTarget(TargetIndex.C, thing);
									job.count = JobGiver_OptimizeApparel.tmpQueueApparel.Count;
								}
								return true;
							}
						}
					}
					finally
					{
						JobGiver_OptimizeApparel.tmpApparelToRecolor.Clear();
						JobGiver_OptimizeApparel.tmpQueueApparel.Clear();
						JobGiver_OptimizeApparel.tmpQueueDye.Clear();
					}
				}
			}
			job = null;
			return false;
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x001F89F4 File Offset: 0x001F6BF4
		public static float ApparelScoreGain(Pawn pawn, Apparel ap, List<float> wornScoresCache)
		{
			if (ap.def == ThingDefOf.Apparel_ShieldBelt && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsWeaponUsingProjectiles)
			{
				return -1000f;
			}
			if (ap.def.apparel.ignoredByNonViolent && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return -1000f;
			}
			float num = JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, ap);
			List<Apparel> wornApparel = pawn.apparel.WornApparel;
			bool flag = false;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(wornApparel[i].def, ap.def, pawn.RaceProps.body))
				{
					if (!pawn.outfits.forcedHandler.AllowedToAutomaticallyDrop(wornApparel[i]) || pawn.apparel.IsLocked(wornApparel[i]))
					{
						return -1000f;
					}
					num -= wornScoresCache[i];
					flag = true;
				}
			}
			if (!flag)
			{
				num *= 10f;
			}
			return num;
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x001F8AF0 File Offset: 0x001F6CF0
		public static float ApparelScoreRaw(Pawn pawn, Apparel ap)
		{
			if (!ap.PawnCanWear(pawn, true))
			{
				return -10f;
			}
			if (ap.def.apparel.blocksVision)
			{
				return -10f;
			}
			if (ap.def.apparel.slaveApparel && !pawn.IsSlave)
			{
				return -10f;
			}
			if (ap.def.apparel.mechanitorApparel && pawn.mechanitor == null)
			{
				return -10f;
			}
			float num = 0.1f + ap.def.apparel.scoreOffset;
			float num2 = ap.GetStatValue(StatDefOf.ArmorRating_Sharp, true, -1) + ap.GetStatValue(StatDefOf.ArmorRating_Blunt, true, -1);
			num += num2;
			if (ap.def.useHitPoints)
			{
				float x = (float)ap.HitPoints / (float)ap.MaxHitPoints;
				num *= JobGiver_OptimizeApparel.HitPointsPercentScoreFactorCurve.Evaluate(x);
			}
			num += ap.GetSpecialApparelScoreOffset();
			float num3 = 1f;
			if (JobGiver_OptimizeApparel.neededWarmth == NeededWarmth.Warm)
			{
				float statValue = ap.GetStatValue(StatDefOf.Insulation_Cold, true, -1);
				num3 *= JobGiver_OptimizeApparel.InsulationColdScoreFactorCurve_NeedWarm.Evaluate(statValue);
			}
			num *= num3;
			if (ap.WornByCorpse && (pawn == null || ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.DeadMansApparel, true)))
			{
				num -= 0.5f;
				if (num > 0f)
				{
					num *= 0.1f;
				}
			}
			if (ap.Stuff == ThingDefOf.Human.race.leatherDef)
			{
				if (pawn.Ideo != null && pawn.Ideo.LikesHumanLeatherApparel)
				{
					num += 0.12f;
				}
				else
				{
					if (pawn == null || ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelSad, true))
					{
						num -= 0.5f;
						if (num > 0f)
						{
							num *= 0.1f;
						}
					}
					if (pawn != null && ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelHappy, true))
					{
						num += 0.12f;
					}
				}
			}
			if (pawn != null && !ap.def.apparel.CorrectGenderForWearing(pawn.gender))
			{
				num *= 0.01f;
			}
			bool flag = false;
			if (pawn != null)
			{
				foreach (ApparelRequirementWithSource apparelRequirementWithSource in pawn.apparel.AllRequirements)
				{
					foreach (BodyPartGroupDef item in apparelRequirementWithSource.requirement.bodyPartGroupsMatchAny)
					{
						if (ap.def.apparel.bodyPartGroups.Contains(item))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				foreach (ApparelRequirementWithSource apparelRequirementWithSource2 in pawn.apparel.AllRequirements)
				{
					if (apparelRequirementWithSource2.requirement.RequiredForPawn(pawn, ap.def, false))
					{
						flag2 = true;
					}
					if (apparelRequirementWithSource2.requirement.AllowedForPawn(pawn, ap.def, false))
					{
						flag3 = true;
					}
				}
				if (flag2)
				{
					num *= 25f;
				}
				else if (flag3)
				{
					num *= 10f;
				}
			}
			if (pawn != null && pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			{
				QualityCategory qualityCategory = QualityCategory.Awful;
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (royalTitle.def.requiredMinimumApparelQuality > qualityCategory)
					{
						qualityCategory = royalTitle.def.requiredMinimumApparelQuality;
					}
				}
				QualityCategory qualityCategory2;
				if (ap.TryGetQuality(out qualityCategory2) && qualityCategory2 < qualityCategory)
				{
					num *= 0.25f;
				}
			}
			return num;
		}

		// Token: 0x0400329D RID: 12957
		private static NeededWarmth neededWarmth;

		// Token: 0x0400329E RID: 12958
		private static StringBuilder debugSb;

		// Token: 0x0400329F RID: 12959
		private static readonly List<float> wornApparelScores = new List<float>();

		// Token: 0x040032A0 RID: 12960
		private static readonly List<Thing> tmpApparelList = new List<Thing>();

		// Token: 0x040032A1 RID: 12961
		private const int ApparelOptimizeCheckIntervalMin = 6000;

		// Token: 0x040032A2 RID: 12962
		private const int ApparelOptimizeCheckIntervalMax = 9000;

		// Token: 0x040032A3 RID: 12963
		private const float MinScoreGainToCare = 0.05f;

		// Token: 0x040032A4 RID: 12964
		private const float ScoreFactorIfNotReplacing = 10f;

		// Token: 0x040032A5 RID: 12965
		private static readonly SimpleCurve InsulationColdScoreFactorCurve_NeedWarm = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(30f, 8f),
				true
			}
		};

		// Token: 0x040032A6 RID: 12966
		private static readonly SimpleCurve HitPointsPercentScoreFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.2f, 0.2f),
				true
			},
			{
				new CurvePoint(0.22f, 0.3f),
				true
			},
			{
				new CurvePoint(0.5f, 0.3f),
				true
			},
			{
				new CurvePoint(0.52f, 1f),
				true
			}
		};

		// Token: 0x040032A7 RID: 12967
		private static readonly List<LocalTargetInfo> tmpQueueDye = new List<LocalTargetInfo>();

		// Token: 0x040032A8 RID: 12968
		private static readonly List<LocalTargetInfo> tmpQueueApparel = new List<LocalTargetInfo>();

		// Token: 0x040032A9 RID: 12969
		private static readonly List<Apparel> tmpApparelToRecolor = new List<Apparel>();
	}
}
