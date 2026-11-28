using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200083D RID: 2109
	public static class ToilFailConditions
	{
		// Token: 0x06004251 RID: 16977 RVA: 0x001A2D94 File Offset: 0x001A0F94
		public static Toil FailOn(this Toil toil, Func<Toil, bool> condition)
		{
			toil.AddEndCondition(delegate
			{
				if (condition(toil))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return toil;
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x001A2DD4 File Offset: 0x001A0FD4
		public static T FailOn<T>(this T f, Func<bool> condition) where T : IJobEndable
		{
			ref T ptr = ref f;
			if (default(T) == null)
			{
				T t = f;
				ptr = ref t;
			}
			ptr.AddEndCondition(delegate
			{
				if (condition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x001A2E20 File Offset: 0x001A1020
		public static T FailOnDestroyedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass2_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass2_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass2_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing.DestroyedOrNull())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x001A2E7C File Offset: 0x001A107C
		public static T FailOnInvalidOrDestroyed<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass3_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass3_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass3_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				LocalTargetInfo target = CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind);
				if (!target.IsValid || target.ThingDestroyed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x001A2ED8 File Offset: 0x001A10D8
		public static bool DespawnedOrNull(LocalTargetInfo target, Pawn actor)
		{
			Thing thing = target.Thing;
			return (thing != null || !target.IsValid) && (thing == null || !thing.Spawned || thing.Map != actor.Map);
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x001A2F18 File Offset: 0x001A1118
		public static T FailOnDespawnedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass5_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass5_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass5_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (ToilFailConditions.DespawnedOrNull(CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind), CS$<>8__locals1.f.GetActor()))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x001A2F74 File Offset: 0x001A1174
		public static bool SelfAndParentsDespawnedOrNull(LocalTargetInfo target, Pawn actor)
		{
			Thing thing = target.Thing;
			return (thing != null || !target.IsValid) && (thing == null || !thing.SpawnedOrAnyParentSpawned || thing.MapHeld != actor.Map);
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x001A2FB4 File Offset: 0x001A11B4
		public static T FailOnSelfAndParentsDespawnedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass7_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass7_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass7_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (ToilFailConditions.SelfAndParentsDespawnedOrNull(CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind), CS$<>8__locals1.f.GetActor()))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x001A3010 File Offset: 0x001A1210
		public static T EndOnDespawnedOrNull<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass8_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass8_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			CS$<>8__locals1.endCondition = endCondition;
			ToilFailConditions.<>c__DisplayClass8_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (ToilFailConditions.DespawnedOrNull(CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind), CS$<>8__locals1.f.GetActor()))
				{
					return CS$<>8__locals1.endCondition;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x001A3074 File Offset: 0x001A1274
		public static T EndOnNoTargetInQueue<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass9_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass9_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			CS$<>8__locals1.endCondition = endCondition;
			ToilFailConditions.<>c__DisplayClass9_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (CS$<>8__locals1.f.GetActor().jobs.curJob.GetTargetQueue(CS$<>8__locals1.ind).NullOrEmpty<LocalTargetInfo>())
				{
					return CS$<>8__locals1.endCondition;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x001A30D8 File Offset: 0x001A12D8
		public static T FailOnDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass10_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass10_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass10_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (((Pawn)CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing).Downed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x001A3134 File Offset: 0x001A1334
		public static T FailOnMobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass11_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass11_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass11_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (((Pawn)CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing).health.State == PawnHealthState.Mobile)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x001A3190 File Offset: 0x001A1390
		public static T FailOnNotDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass12_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass12_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass12_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (!((Pawn)CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing).Downed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x001A31EC File Offset: 0x001A13EC
		public static T FailOnNotAwake<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass13_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass13_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass13_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (!((Pawn)CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing).Awake())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x001A3248 File Offset: 0x001A1448
		public static T FailOnNotCasualInterruptible<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass14_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass14_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass14_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (!((Pawn)CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing).CanCasuallyInteractNow(false, false, false, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x001A32A4 File Offset: 0x001A14A4
		public static T FailOnMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass15_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass15_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass15_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn pawn = CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing as Pawn;
				if (pawn != null && pawn.InMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x001A3300 File Offset: 0x001A1500
		public static T FailOnAggroMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass16_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass16_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass16_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn pawn = CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing as Pawn;
				if (pawn != null && pawn.InAggroMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x001A335C File Offset: 0x001A155C
		public static T FailOnAggroMentalStateAndHostile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass17_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass17_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass17_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn pawn = CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing as Pawn;
				if (pawn != null && pawn.InAggroMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false) && pawn.HostileTo(CS$<>8__locals1.f.GetActor()))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x001A33B8 File Offset: 0x001A15B8
		public static T FailOnSomeonePhysicallyInteracting<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass18_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass18_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass18_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn actor = CS$<>8__locals1.f.GetActor();
				Thing thing = actor.jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing;
				if (thing != null && actor.Map.physicalInteractionReservationManager.IsReserved(thing) && !actor.Map.physicalInteractionReservationManager.IsReservedBy(actor, thing))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x001A3414 File Offset: 0x001A1614
		public static T FailOnForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass19_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass19_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass19_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn actor = CS$<>8__locals1.f.GetActor();
				if (actor.Faction != Faction.OfPlayer)
				{
					return JobCondition.Ongoing;
				}
				if (actor.jobs.curJob.ignoreForbidden)
				{
					return JobCondition.Ongoing;
				}
				IntVec3 cell = actor.jobs.curJob.GetTarget(CS$<>8__locals1.ind).Cell;
				Thing thing = actor.jobs.curJob.GetTarget(CS$<>8__locals1.ind).Thing;
				IThingHolder thingHolder = (thing != null) ? thing.ParentHolder : null;
				if (!(thingHolder is Pawn_CarryTracker) && !(thingHolder is Pawn_InventoryTracker) && cell.IsValid && cell.IsForbidden(actor))
				{
					return JobCondition.Incompletable;
				}
				if (thing == null)
				{
					return JobCondition.Ongoing;
				}
				if (thing.IsForbidden(actor))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x001A3470 File Offset: 0x001A1670
		public static T FailOnDespawnedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDespawnedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x001A3483 File Offset: 0x001A1683
		public static T FailOnDestroyedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDestroyedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x001A3498 File Offset: 0x001A1698
		public static T FailOnThingMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass22_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass22_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			CS$<>8__locals1.desDef = desDef;
			ToilFailConditions.<>c__DisplayClass22_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn actor = CS$<>8__locals1.f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = curJob.GetTarget(CS$<>8__locals1.ind).Thing;
				if (thing == null || actor.Map.designationManager.DesignationOn(thing, CS$<>8__locals1.desDef) == null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x001A34FC File Offset: 0x001A16FC
		public static T FailOnThingHavingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass23_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass23_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			CS$<>8__locals1.desDef = desDef;
			ToilFailConditions.<>c__DisplayClass23_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn actor = CS$<>8__locals1.f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = curJob.GetTarget(CS$<>8__locals1.ind).Thing;
				if (thing == null || actor.Map.designationManager.DesignationOn(thing, CS$<>8__locals1.desDef) != null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x001A3560 File Offset: 0x001A1760
		public static T FailOnCellMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass24_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass24_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			CS$<>8__locals1.desDef = desDef;
			ToilFailConditions.<>c__DisplayClass24_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn actor = CS$<>8__locals1.f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				if (actor.Map.designationManager.DesignationAt(curJob.GetTarget(CS$<>8__locals1.ind).Cell, CS$<>8__locals1.desDef) == null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x001A35C4 File Offset: 0x001A17C4
		public static T FailOnBurningImmobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass25_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass25_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			ToilFailConditions.<>c__DisplayClass25_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn actor = CS$<>8__locals1.f.GetActor();
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(CS$<>8__locals1.ind);
				if (target.IsValid && target.ToTargetInfo(actor.Map).IsBurning())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x001A3620 File Offset: 0x001A1820
		public static T FailOnCannotReach<T>(this T f, TargetIndex ind, PathEndMode mode) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass26_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass26_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			CS$<>8__locals1.mode = mode;
			ToilFailConditions.<>c__DisplayClass26_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn actor = CS$<>8__locals1.f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (actor.IsHashIntervalTick(300) && !actor.CanReach(curJob.GetTarget(CS$<>8__locals1.ind), CS$<>8__locals1.mode, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x001A3684 File Offset: 0x001A1884
		public static T FailOnCannotTouch<T>(this T f, TargetIndex ind, PathEndMode peMode) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass27_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass27_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.ind = ind;
			CS$<>8__locals1.peMode = peMode;
			ToilFailConditions.<>c__DisplayClass27_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (!CS$<>8__locals1.f.GetActor().CanReachImmediate(CS$<>8__locals1.f.GetActor().jobs.curJob.GetTarget(CS$<>8__locals1.ind), CS$<>8__locals1.peMode))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x001A36E8 File Offset: 0x001A18E8
		public static T FailOnIncapable<T>(this T f, PawnCapacityDef pawnCapacity) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass28_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass28_0<T>();
			CS$<>8__locals1.f = f;
			CS$<>8__locals1.pawnCapacity = pawnCapacity;
			ToilFailConditions.<>c__DisplayClass28_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				if (!CS$<>8__locals1.f.GetActor().health.capacities.CapableOf(CS$<>8__locals1.pawnCapacity))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x001A3744 File Offset: 0x001A1944
		public static T FailOnChildLearningConditions<T>(this T f) where T : IJobEndable
		{
			ToilFailConditions.<>c__DisplayClass29_0<T> CS$<>8__locals1 = new ToilFailConditions.<>c__DisplayClass29_0<T>();
			CS$<>8__locals1.f = f;
			ToilFailConditions.<>c__DisplayClass29_0<T> CS$<>8__locals2 = CS$<>8__locals1;
			ref T ptr = ref CS$<>8__locals2.f;
			if (default(T) == null)
			{
				T f2 = CS$<>8__locals2.f;
				ptr = ref f2;
			}
			ptr.AddEndCondition(delegate
			{
				Pawn actor = CS$<>8__locals1.f.GetActor();
				if (!actor.DevelopmentalStage.Juvenile() || PawnUtility.WillSoonHaveBasicNeed(actor, -0.05f))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return CS$<>8__locals1.f;
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x001A379C File Offset: 0x001A199C
		public static Toil FailOnDespawnedNullOrForbiddenPlacedThings(this Toil toil, TargetIndex containerIndex = TargetIndex.None)
		{
			toil.AddFailCondition(delegate
			{
				if (toil.actor.jobs.curJob.placedThings == null)
				{
					return false;
				}
				for (int i = 0; i < toil.actor.jobs.curJob.placedThings.Count; i++)
				{
					ThingCountClass thingCountClass = toil.actor.jobs.curJob.placedThings[i];
					Thing thing = toil.actor.jobs.curJob.GetTarget(containerIndex).Thing;
					ThingOwner thingOwner = (thing != null) ? thing.TryGetInnerInteractableThingOwner() : null;
					if (thingCountClass.thing == null || (!thingCountClass.thing.Spawned && (thingOwner == null || !thingOwner.Contains(thingCountClass.thing))) || thingCountClass.thing.MapHeld != toil.actor.Map || (!toil.actor.CurJob.ignoreForbidden && thingCountClass.thing.IsForbidden(toil.actor)))
					{
						return true;
					}
				}
				return false;
			});
			return toil;
		}
	}
}
