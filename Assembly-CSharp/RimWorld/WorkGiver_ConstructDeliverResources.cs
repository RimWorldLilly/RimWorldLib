using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC4 RID: 3012
	public abstract class WorkGiver_ConstructDeliverResources : WorkGiver_Scanner
	{
		// Token: 0x060056BB RID: 22203 RVA: 0x002035E9 File Offset: 0x002017E9
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060056BC RID: 22204 RVA: 0x002035EC File Offset: 0x002017EC
		public static void ResetStaticData()
		{
			WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated = "ForbiddenLower".Translate();
			WorkGiver_ConstructDeliverResources.NoPathTranslated = "NoPath".Translate();
		}

		// Token: 0x060056BD RID: 22205 RVA: 0x00203616 File Offset: 0x00201816
		private static bool ResourceValidator(Pawn pawn, ThingDefCountClass need, Thing th)
		{
			return WorkGiver_ConstructDeliverResources.ResourceValidator_NewTemp(pawn, need, th, false);
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x00203621 File Offset: 0x00201821
		private static bool ResourceValidator_NewTemp(Pawn pawn, ThingDefCountClass need, Thing th, bool forced)
		{
			return th.def == need.thingDef && !th.IsForbidden(pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast_NewTemp(pawn, th, forced, false);
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x0020364C File Offset: 0x0020184C
		private static bool NoReachableResourceFailedThisTick(Pawn pawn, ThingDef need, bool forced)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (WorkGiver_ConstructDeliverResources.noReachableResourceCacheTick != ticksGame)
			{
				WorkGiver_ConstructDeliverResources.noReachableResourceCache.Clear();
				WorkGiver_ConstructDeliverResources.noReachableResourceCacheTick = ticksGame;
			}
			return WorkGiver_ConstructDeliverResources.noReachableResourceCache.Contains(new WorkGiver_ConstructDeliverResources.NoReachableResourceKey(pawn, need, forced));
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x00203690 File Offset: 0x00201890
		private static void NotifyNoReachableResourceFound(Pawn pawn, ThingDef need, bool forced)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (WorkGiver_ConstructDeliverResources.noReachableResourceCacheTick != ticksGame)
			{
				WorkGiver_ConstructDeliverResources.noReachableResourceCache.Clear();
				WorkGiver_ConstructDeliverResources.noReachableResourceCacheTick = ticksGame;
			}
			WorkGiver_ConstructDeliverResources.noReachableResourceCache.Add(new WorkGiver_ConstructDeliverResources.NoReachableResourceKey(pawn, need, forced));
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x002036D4 File Offset: 0x002018D4
		private bool CanUseCarriedResource(Pawn pawn, IConstructible c, ThingDefCountClass need)
		{
			WorkGiver_ConstructDeliverResources.<>c__DisplayClass15_0 CS$<>8__locals1;
			CS$<>8__locals1.pawn = pawn;
			Thing carriedThing = CS$<>8__locals1.pawn.carryTracker.CarriedThing;
			if (((carriedThing != null) ? carriedThing.def : null) != need.thingDef)
			{
				return false;
			}
			if (!KeyBindingDefOf.QueueOrder.IsDownEvent)
			{
				return true;
			}
			if (CS$<>8__locals1.pawn.CurJob != null && !WorkGiver_ConstructDeliverResources.<CanUseCarriedResource>g__IsValidJob|15_0(CS$<>8__locals1.pawn.CurJob, ref CS$<>8__locals1))
			{
				return false;
			}
			using (IEnumerator<QueuedJob> enumerator = CS$<>8__locals1.pawn.jobs.jobQueue.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!WorkGiver_ConstructDeliverResources.<CanUseCarriedResource>g__IsValidJob|15_0(enumerator.Current.job, ref CS$<>8__locals1))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060056C2 RID: 22210 RVA: 0x00203798 File Offset: 0x00201998
		protected Job ResourceDeliverJobFor(Pawn pawn, IConstructible c, bool canRemoveExistingFloorUnderNearbyNeeders = true, bool forced = false)
		{
			Blueprint_Install blueprint_Install = c as Blueprint_Install;
			if (blueprint_Install != null)
			{
				return this.InstallJob(pawn, blueprint_Install);
			}
			WorkGiver_ConstructDeliverResources.missingResources.Clear();
			using (List<ThingDefCountClass>.Enumerator enumerator = c.TotalMaterialCost().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDefCountClass need = enumerator.Current;
					if (forced)
					{
						goto IL_A3;
					}
					IHaulEnroute haulEnroute = c as IHaulEnroute;
					if (haulEnroute == null)
					{
						goto IL_A3;
					}
					int num = haulEnroute.GetSpaceRemainingWithEnroute(need.thingDef, pawn);
					IL_B6:
					if (num <= 0)
					{
						continue;
					}
					if (!pawn.Map.itemAvailability.ThingsAvailableAnywhere(need.thingDef, num, pawn))
					{
						WorkGiver_ConstructDeliverResources.missingResources.Add(need.thingDef, num);
						if (FloatMenuMakerMap.makingFor != pawn)
						{
							break;
						}
						continue;
					}
					else
					{
						Thing foundRes;
						if (this.CanUseCarriedResource(pawn, c, need))
						{
							foundRes = pawn.carryTracker.CarriedThing;
						}
						else if (WorkGiver_ConstructDeliverResources.NoReachableResourceFailedThisTick(pawn, need.thingDef, forced))
						{
							foundRes = null;
						}
						else
						{
							foundRes = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(need.thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, forced ? Danger.Deadly : pawn.NormalMaxDanger(), TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing r) => WorkGiver_ConstructDeliverResources.ResourceValidator_NewTemp(pawn, need, r, forced), null, 0, -1, false, RegionType.Set_Passable, false, false);
							if (foundRes == null)
							{
								WorkGiver_ConstructDeliverResources.NotifyNoReachableResourceFound(pawn, need.thingDef, forced);
							}
						}
						if (foundRes == null)
						{
							WorkGiver_ConstructDeliverResources.missingResources.Add(need.thingDef, num);
							if (FloatMenuMakerMap.makingFor != pawn)
							{
								break;
							}
							continue;
						}
						else
						{
							int num2;
							this.FindAvailableNearbyResources(foundRes, pawn, out num2);
							int num3;
							Job job;
							HashSet<Thing> hashSet = this.FindNearbyNeeders(pawn, need.thingDef, c, num, num2, canRemoveExistingFloorUnderNearbyNeeders, out num3, out job);
							if (job != null)
							{
								return job;
							}
							hashSet.Add((Thing)c);
							Thing thing;
							if (hashSet.Count > 0)
							{
								thing = GenCollection.MinBy<Thing, int>(hashSet, (Thing needer) => IntVec3Utility.ManhattanDistanceFlat(foundRes.Position, needer.Position));
								hashSet.Remove(thing);
							}
							else
							{
								thing = (Thing)c;
							}
							int num4 = 0;
							int i = 0;
							do
							{
								num4 += WorkGiver_ConstructDeliverResources.resourcesAvailable[i].stackCount;
								num4 = Mathf.Min(num4, Mathf.Min(num2, num3));
								i++;
							}
							while (num4 < num3 && num4 < num2 && i < WorkGiver_ConstructDeliverResources.resourcesAvailable.Count);
							WorkGiver_ConstructDeliverResources.resourcesAvailable.RemoveRange(i, WorkGiver_ConstructDeliverResources.resourcesAvailable.Count - i);
							WorkGiver_ConstructDeliverResources.resourcesAvailable.Remove(foundRes);
							Job job2 = JobMaker.MakeJob(JobDefOf.HaulToContainer);
							job2.targetA = foundRes;
							job2.targetQueueA = new List<LocalTargetInfo>();
							for (i = 0; i < WorkGiver_ConstructDeliverResources.resourcesAvailable.Count; i++)
							{
								job2.targetQueueA.Add(WorkGiver_ConstructDeliverResources.resourcesAvailable[i]);
							}
							job2.targetC = (Thing)c;
							job2.targetB = thing;
							if (hashSet.Count > 0)
							{
								job2.targetQueueB = new List<LocalTargetInfo>();
								foreach (Thing t in hashSet)
								{
									job2.targetQueueB.Add(t);
								}
							}
							job2.count = num4;
							job2.haulMode = HaulMode.ToContainer;
							return job2;
						}
					}
					IL_A3:
					num = c.ThingCountNeeded(need.thingDef);
					goto IL_B6;
				}
			}
			if (WorkGiver_ConstructDeliverResources.missingResources.Count > 0 && FloatMenuMakerMap.makingFor == pawn)
			{
				JobFailReason.Is("MissingMaterials".Translate(WorkGiver_ConstructDeliverResources.missingResources.Select(delegate(KeyValuePair<ThingDef, int> kvp)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(kvp.Value);
					defaultInterpolatedStringHandler.AppendLiteral("x ");
					defaultInterpolatedStringHandler.AppendFormatted(kvp.Key.label);
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}).ToCommaList(false, false)), null);
			}
			return null;
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x00203CE0 File Offset: 0x00201EE0
		private void FindAvailableNearbyResources(Thing firstFoundResource, Pawn pawn, out int resTotalAvailable)
		{
			int num = pawn.carryTracker.MaxStackSpaceEver(firstFoundResource.def);
			resTotalAvailable = 0;
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Clear();
			WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(firstFoundResource);
			resTotalAvailable += firstFoundResource.stackCount;
			if (resTotalAvailable < num)
			{
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(firstFoundResource.PositionHeld, firstFoundResource.MapHeld, 5f, false))
				{
					if (resTotalAvailable >= num)
					{
						resTotalAvailable = num;
						break;
					}
					if (thing.def == firstFoundResource.def && GenAI.CanUseItemForWork(pawn, thing))
					{
						WorkGiver_ConstructDeliverResources.resourcesAvailable.Add(thing);
						resTotalAvailable += thing.stackCount;
					}
				}
			}
			resTotalAvailable = Mathf.Min(resTotalAvailable, num);
		}

		// Token: 0x060056C4 RID: 22212 RVA: 0x00203DB4 File Offset: 0x00201FB4
		private HashSet<Thing> FindNearbyNeeders(Pawn pawn, ThingDef stuff, IConstructible c, int resNeeded, int resTotalAvailable, bool canRemoveExistingFloorUnderNearbyNeeders, out int neededTotal, out Job jobToMakeNeederAvailable)
		{
			neededTotal = resNeeded;
			HashSet<Thing> hashSet = new HashSet<Thing>();
			Thing thing = (Thing)c;
			foreach (Thing thing2 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 8f, true))
			{
				if (neededTotal >= resTotalAvailable)
				{
					break;
				}
				if (this.IsNewValidNearbyNeeder(thing2, hashSet, c, pawn))
				{
					Blueprint blueprint = thing2 as Blueprint;
					if (blueprint == null || !WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
					{
						int num = 0;
						IHaulEnroute haulEnroute = thing2 as IHaulEnroute;
						if (haulEnroute != null)
						{
							num = haulEnroute.GetSpaceRemainingWithEnroute(stuff, pawn);
						}
						else
						{
							IConstructible constructible = thing2 as IConstructible;
							if (constructible != null)
							{
								num = constructible.ThingCountNeeded(stuff);
							}
						}
						if (num > 0)
						{
							hashSet.Add(thing2);
							neededTotal += num;
						}
					}
				}
			}
			Blueprint blueprint2 = c as Blueprint;
			if (blueprint2 != null && blueprint2.def.entityDefToBuild is TerrainDef && canRemoveExistingFloorUnderNearbyNeeders && neededTotal < resTotalAvailable)
			{
				foreach (Thing thing3 in GenRadial.RadialDistinctThingsAround(thing.Position, thing.Map, 3f, false))
				{
					if (this.IsNewValidNearbyNeeder(thing3, hashSet, c, pawn))
					{
						Blueprint blueprint3 = thing3 as Blueprint;
						if (blueprint3 != null)
						{
							Job job = this.RemoveExistingFloorJob(pawn, blueprint3);
							if (job != null)
							{
								jobToMakeNeederAvailable = job;
								return hashSet;
							}
						}
					}
				}
			}
			jobToMakeNeederAvailable = null;
			return hashSet;
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x00203F50 File Offset: 0x00202150
		private bool IsNewValidNearbyNeeder(Thing t, HashSet<Thing> nearbyNeeders, IConstructible constructible, Pawn pawn)
		{
			return t is IConstructible && t != constructible && t.Faction == pawn.Faction && t.Isnt<Blueprint_Install>() && !nearbyNeeders.Contains(t) && !t.IsForbidden(pawn) && GenConstruct.CanConstruct(t, pawn, false, false, JobDefOf.HaulToContainer);
		}

		// Token: 0x060056C6 RID: 22214 RVA: 0x00203FA4 File Offset: 0x002021A4
		protected static bool ShouldRemoveExistingFloorFirst(Pawn pawn, Blueprint blue)
		{
			return blue.def.entityDefToBuild is TerrainDef && pawn.Map.terrainGrid.CanRemoveTopLayerAt(blue.Position);
		}

		// Token: 0x060056C7 RID: 22215 RVA: 0x00203FD0 File Offset: 0x002021D0
		protected Job RemoveExistingFloorJob(Pawn pawn, Blueprint blue)
		{
			if (!WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blue))
			{
				return null;
			}
			if (!pawn.CanReserve(blue.Position, 1, -1, ReservationLayerDefOf.Floor, false))
			{
				return null;
			}
			if (pawn.WorkTypeIsDisabled(WorkGiverDefOf.ConstructRemoveFloors.workType))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.RemoveFloor, blue.Position);
			job.ignoreDesignations = true;
			return job;
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x00204038 File Offset: 0x00202238
		private Job InstallJob(Pawn pawn, Blueprint_Install install)
		{
			Thing miniToInstallOrBuildingToReinstall = install.MiniToInstallOrBuildingToReinstall;
			IThingHolder parentHolder = miniToInstallOrBuildingToReinstall.ParentHolder;
			if (parentHolder != null)
			{
				Pawn_CarryTracker pawn_CarryTracker = parentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null)
				{
					JobFailReason.Is("BeingCarriedBy".Translate(pawn_CarryTracker.pawn), null);
					return null;
				}
			}
			if (miniToInstallOrBuildingToReinstall.IsForbidden(pawn))
			{
				JobFailReason.Is(WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated, null);
				return null;
			}
			if (!pawn.CanReach(miniToInstallOrBuildingToReinstall, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				JobFailReason.Is(WorkGiver_ConstructDeliverResources.NoPathTranslated, null);
				return null;
			}
			if (!pawn.CanReserve(miniToInstallOrBuildingToReinstall, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(miniToInstallOrBuildingToReinstall, pawn, null);
				if (pawn2 != null)
				{
					JobFailReason.Is("ReservedBy".Translate(pawn2.LabelShort, pawn2), null);
				}
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToContainer);
			job.targetA = miniToInstallOrBuildingToReinstall;
			job.targetB = install;
			job.count = 1;
			job.haulMode = HaulMode.ToContainer;
			return job;
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x00204171 File Offset: 0x00202371
		[CompilerGenerated]
		internal static bool <CanUseCarriedResource>g__IsValidJob|15_0(Job job, ref WorkGiver_ConstructDeliverResources.<>c__DisplayClass15_0 A_1)
		{
			return job.def != JobDefOf.HaulToContainer || job.targetA != A_1.pawn.carryTracker.CarriedThing;
		}

		// Token: 0x04003320 RID: 13088
		private static readonly List<Thing> resourcesAvailable = new List<Thing>();

		// Token: 0x04003321 RID: 13089
		private static readonly Dictionary<ThingDef, int> missingResources = new Dictionary<ThingDef, int>();

		// Token: 0x04003322 RID: 13090
		private static readonly HashSet<WorkGiver_ConstructDeliverResources.NoReachableResourceKey> noReachableResourceCache = new HashSet<WorkGiver_ConstructDeliverResources.NoReachableResourceKey>();

		// Token: 0x04003323 RID: 13091
		private static int noReachableResourceCacheTick = -1;

		// Token: 0x04003324 RID: 13092
		private const float MultiPickupRadius = 5f;

		// Token: 0x04003325 RID: 13093
		private const float NearbyConstructScanRadius = 8f;

		// Token: 0x04003326 RID: 13094
		protected static string ForbiddenLowerTranslated;

		// Token: 0x04003327 RID: 13095
		protected static string NoPathTranslated;

		// Token: 0x02002E23 RID: 11811
		private readonly struct NoReachableResourceKey : IEquatable<WorkGiver_ConstructDeliverResources.NoReachableResourceKey>
		{
			// Token: 0x06011785 RID: 71557 RVA: 0x00604943 File Offset: 0x00602B43
			public NoReachableResourceKey(Pawn pawn, ThingDef need, bool forced)
			{
				this.pawnId = pawn.thingIDNumber;
				this.need = need;
				this.forced = forced;
			}

			// Token: 0x06011786 RID: 71558 RVA: 0x00604960 File Offset: 0x00602B60
			public override bool Equals(object obj)
			{
				if (obj is WorkGiver_ConstructDeliverResources.NoReachableResourceKey)
				{
					WorkGiver_ConstructDeliverResources.NoReachableResourceKey noReachableResourceKey = (WorkGiver_ConstructDeliverResources.NoReachableResourceKey)obj;
					return this.pawnId == noReachableResourceKey.pawnId && this.need == noReachableResourceKey.need && this.forced == noReachableResourceKey.forced;
				}
				return false;
			}

			// Token: 0x06011787 RID: 71559 RVA: 0x006049AC File Offset: 0x00602BAC
			public override int GetHashCode()
			{
				return Gen.HashCombine<bool>(Gen.HashCombine<ThingDef>(Gen.HashCombine<int>(0, this.pawnId), this.need), this.forced);
			}

			// Token: 0x06011788 RID: 71560 RVA: 0x006049D0 File Offset: 0x00602BD0
			public bool Equals(WorkGiver_ConstructDeliverResources.NoReachableResourceKey other)
			{
				return this.pawnId == other.pawnId && object.Equals(this.need, other.need) && this.forced == other.forced;
			}

			// Token: 0x0400BB1A RID: 47898
			private readonly int pawnId;

			// Token: 0x0400BB1B RID: 47899
			private readonly ThingDef need;

			// Token: 0x0400BB1C RID: 47900
			private readonly bool forced;
		}
	}
}
