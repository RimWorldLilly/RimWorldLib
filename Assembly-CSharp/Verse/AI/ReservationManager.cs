using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020008B5 RID: 2229
	[StaticConstructorOnStartup]
	public sealed class ReservationManager : IExposable
	{
		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x0600455B RID: 17755 RVA: 0x001B5550 File Offset: 0x001B3750
		public List<ReservationManager.Reservation> ReservationsReadOnly
		{
			get
			{
				return this.reservations;
			}
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x001B5558 File Offset: 0x001B3758
		public ReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x001B5580 File Offset: 0x001B3780
		public void ExposeData()
		{
			Scribe_Collections.Look<ReservationManager.Reservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.reservations.Count - 1; i >= 0; i--)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (reservation.Target.Thing != null && reservation.Target.Thing.Destroyed)
					{
						string str = "Loaded reservation with destroyed target: ";
						ReservationManager.Reservation reservation2 = reservation;
						Log.Error(str + ((reservation2 != null) ? reservation2.ToString() : null) + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant != null && reservation.Claimant.Destroyed)
					{
						string str2 = "Loaded reservation with destroyed claimant: ";
						ReservationManager.Reservation reservation3 = reservation;
						Log.Error(str2 + ((reservation3 != null) ? reservation3.ToString() : null) + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant == null)
					{
						string str3 = "Loaded reservation with null claimant: ";
						ReservationManager.Reservation reservation4 = reservation;
						Log.Error(str3 + ((reservation4 != null) ? reservation4.ToString() : null) + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
					if (reservation.Job == null)
					{
						string str4 = "Loaded reservation with null job: ";
						ReservationManager.Reservation reservation5 = reservation;
						Log.Error(str4 + ((reservation5 != null) ? reservation5.ToString() : null) + ". Deleting it...");
						this.reservations.Remove(reservation);
					}
				}
			}
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x001B56DC File Offset: 0x001B38DC
		public bool IsReserved(LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				if (this.reservations[i].Target == target)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x001B571C File Offset: 0x001B391C
		public void ReserversOf(LocalTargetInfo target, HashSet<Pawn> reserversOut)
		{
			this.map.physicalInteractionReservationManager.ReserversOf(target, reserversOut);
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target)
				{
					reserversOut.Add(reservation.Claimant);
				}
			}
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x001B577C File Offset: 0x001B397C
		public bool HasReservedOfDef(Pawn claimant, ThingDef thingDef)
		{
			foreach (ReservationManager.Reservation reservation in this.reservations)
			{
				if (reservation.Target.HasThing && reservation.Target.Thing.def == thingDef && reservation.Claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x001B5800 File Offset: 0x001B3A00
		public bool CanReserve(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant");
				return false;
			}
			if (!claimant.Spawned || claimant.Map != this.map)
			{
				return false;
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return false;
			}
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned && target.Thing.MapHeld != this.map)
			{
				return false;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = (stackCount == -1) ? num : stackCount;
			if (num2 > num)
			{
				return false;
			}
			if (!ignoreOtherReservations)
			{
				if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					return false;
				}
				if (this.IsAlreadyReserved(claimant, target, layer, num2))
				{
					return true;
				}
				if (target.HasThing)
				{
					Building building = target.Thing as Building;
					if (building != null && building.def.hasInteractionCell)
					{
						IntVec3 interactionCell = building.InteractionCell;
						Building edifice = interactionCell.GetEdifice(claimant.Map);
						if (edifice != null)
						{
							Pawn pawn;
							if (claimant.Map.reservationManager.TryGetReserver(edifice, claimant.Faction, out pawn) && pawn.Spawned && pawn != claimant)
							{
								return false;
							}
							goto IL_189;
						}
						else
						{
							Pawn pawn2;
							if (claimant.Map.reservationManager.TryGetReserver(interactionCell, claimant.Faction, out pawn2) && pawn2.Spawned && pawn2 != claimant)
							{
								return false;
							}
							goto IL_189;
						}
					}
				}
				if (this.IsAlreadyReserved(claimant, target, layer, num2))
				{
					return true;
				}
				IL_189:
				this.tmpReservers.Clear();
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < this.reservations.Count; i++)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (!(reservation.Target != target) && reservation.Layer == layer && reservation.Claimant != claimant && !this.tmpReservers.Contains(reservation.Claimant) && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
					{
						if (reservation.MaxPawns != maxPawns)
						{
							return false;
						}
						this.tmpReservers.Add(reservation.Claimant);
						num3++;
						if (reservation.StackCount == -1)
						{
							num4 += num;
						}
						else
						{
							num4 += reservation.StackCount;
						}
						if (num3 >= maxPawns || num2 + num4 > num)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x001B5A70 File Offset: 0x001B3C70
		private bool IsAlreadyReserved(Pawn claimant, LocalTargetInfo target, ReservationLayerDef layer, int toReserveResolved)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Layer == layer && reservation.Claimant == claimant && (reservation.StackCount == -1 || reservation.StackCount >= toReserveResolved))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x001B5AD8 File Offset: 0x001B3CD8
		public int CanReserveStack(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant");
				return 0;
			}
			if (!claimant.Spawned || claimant.Map != this.map)
			{
				return 0;
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return 0;
			}
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned && target.Thing.MapHeld != this.map)
			{
				return 0;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = 0;
			if (!ignoreOtherReservations)
			{
				if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					return 0;
				}
				int num3 = 0;
				for (int i = 0; i < this.reservations.Count; i++)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (!(reservation.Target != target) && reservation.Layer == layer && reservation.Claimant != claimant && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
					{
						if (reservation.MaxPawns != maxPawns)
						{
							return 0;
						}
						num3++;
						if (reservation.StackCount == -1)
						{
							num2 += num;
						}
						else
						{
							num2 += reservation.StackCount;
						}
						if (num3 >= maxPawns || num2 >= num)
						{
							return 0;
						}
					}
				}
			}
			return Mathf.Max(num - num2, 0);
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x001B5C34 File Offset: 0x001B3E34
		public bool Reserve(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool errorOnFailed = true, bool ignoreOtherReservations = false, bool canReserversStartJobs = true)
		{
			if (maxPawns > 1 && stackCount == -1)
			{
				Log.ErrorOnce("Reserving with maxPawns > 1 and stackCount = All; this will not have a useful effect (suppressing future warnings)", 83269);
			}
			if (job == null)
			{
				Log.Warning(claimant.ToStringSafe<Pawn>() + " tried to reserve thing " + target.ToStringSafe<LocalTargetInfo>() + " without a valid job");
				return false;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = (stackCount == -1) ? num : stackCount;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && reservation.Job == job && reservation.Layer == layer && (reservation.StackCount == -1 || reservation.StackCount >= num2))
				{
					return true;
				}
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return false;
			}
			if (this.CanReserve(claimant, target, maxPawns, stackCount, layer, ignoreOtherReservations))
			{
				ReservationManager.Reservation reservation2 = new ReservationManager.Reservation(claimant, job, maxPawns, stackCount, target, layer);
				this.reservations.Add(reservation2);
				this.map.events.Notify_ReservationAdded(reservation2);
				return true;
			}
			if (job.playerForced && this.CanReserve(claimant, target, maxPawns, stackCount, layer, true))
			{
				this.reservations.Add(new ReservationManager.Reservation(claimant, job, maxPawns, stackCount, target, layer));
				foreach (ReservationManager.Reservation reservation3 in this.reservations.ToList<ReservationManager.Reservation>())
				{
					if (reservation3.Claimant != claimant && reservation3.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, reservation3.Claimant))
					{
						if (reservation3.Target == target)
						{
							reservation3.Claimant.jobs.EndCurrentOrQueuedJob(reservation3.Job, JobCondition.InterruptForced, canReserversStartJobs, true);
						}
						else if (target.HasThing)
						{
							Building building = target.Thing as Building;
							if (building != null && building.def.hasInteractionCell)
							{
								IntVec3 interactionCell = building.InteractionCell;
								Building edifice = interactionCell.GetEdifice(claimant.Map);
								if (reservation3.Claimant != claimant && reservation3.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, reservation3.Claimant))
								{
									if (edifice != null && reservation3.Target == edifice)
									{
										reservation3.Claimant.jobs.EndCurrentOrQueuedJob(reservation3.Job, JobCondition.InterruptForced, true, true);
									}
									else if (reservation3.Target == interactionCell)
									{
										reservation3.Claimant.jobs.EndCurrentOrQueuedJob(reservation3.Job, JobCondition.InterruptForced, true, true);
									}
								}
							}
						}
					}
				}
				return true;
			}
			if (errorOnFailed)
			{
				this.LogCouldNotReserveError(claimant, job, target, maxPawns, stackCount, layer);
			}
			return false;
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x001B5F40 File Offset: 0x001B4140
		public void Release(LocalTargetInfo target, Pawn claimant, Job job)
		{
			if (target.ThingDestroyed)
			{
				string str = "Releasing destroyed thing ";
				LocalTargetInfo localTargetInfo = target;
				Log.Warning(str + localTargetInfo.ToString() + " for " + ((claimant != null) ? claimant.ToString() : null));
			}
			ReservationManager.Reservation reservation = null;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation2 = this.reservations[i];
				if (reservation2.Target == target && reservation2.Claimant == claimant && reservation2.Job == job)
				{
					reservation = reservation2;
					break;
				}
			}
			if (reservation == null && !target.ThingDestroyed)
			{
				string[] array = new string[5];
				array[0] = "Tried to release ";
				int num = 1;
				LocalTargetInfo localTargetInfo = target;
				array[num] = localTargetInfo.ToString();
				array[2] = " that wasn't reserved by ";
				array[3] = ((claimant != null) ? claimant.ToString() : null);
				array[4] = ".";
				Log.Error(string.Concat(array));
				return;
			}
			this.reservations.Remove(reservation);
			this.map.events.Notify_ReservationRemoved(reservation);
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x001B6044 File Offset: 0x001B4244
		public void ReleaseAllForTarget(Thing t)
		{
			if (t == null)
			{
				return;
			}
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Target.Thing == t)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x001B6098 File Offset: 0x001B4298
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant && this.reservations[i].Job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x001B60F4 File Offset: 0x001B42F4
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			if (claimant == null)
			{
				return;
			}
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x001B6140 File Offset: 0x001B4340
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			if (claimant == null)
			{
				return LocalTargetInfo.Invalid;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					return this.reservations[i].Target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x001B6198 File Offset: 0x001B4398
		public bool IsReservedByAnyoneOf(LocalTargetInfo target, Faction faction)
		{
			Pawn pawn;
			return this.TryGetReserver(target, faction, out pawn);
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x001B61B0 File Offset: 0x001B43B0
		public bool TryGetReserver(LocalTargetInfo target, Faction faction, out Pawn reserver)
		{
			reserver = null;
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant.Faction == faction)
				{
					reserver = reservation.Claimant;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x001B6218 File Offset: 0x001B4418
		public bool OnlyReservationsForJobDef(LocalTargetInfo target, JobDef requiredJobDef, bool requireAtLeastOne = false)
		{
			if (!target.IsValid)
			{
				return false;
			}
			bool flag = false;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (!(reservation.Target != target))
				{
					if (reservation.Job.def != requiredJobDef)
					{
						return false;
					}
					flag = true;
				}
			}
			return !requireAtLeastOne || flag;
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x001B627B File Offset: 0x001B447B
		public bool IsReservedAndRespected(LocalTargetInfo target, Pawn claimant)
		{
			return this.FirstRespectedReserver(target, claimant, null) != null;
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x001B628C File Offset: 0x001B448C
		public bool IsReservedAndRespected(LocalTargetInfo target, Faction faction)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (!(reservation.Target != target) && ReservationManager.RespectsReservationsOf(faction, reservation.Claimant))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x001B62E8 File Offset: 0x001B44E8
		public Pawn FirstRespectedReserver(LocalTargetInfo target, Pawn claimant, ReservationLayerDef layer = null)
		{
			if (!target.IsValid)
			{
				return null;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && (layer == null || reservation.Layer == layer) && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
				{
					return reservation.Claimant;
				}
			}
			return null;
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x001B6354 File Offset: 0x001B4554
		public bool ReservedBy(LocalTargetInfo target, Pawn claimant, Job job = null)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && (job == null || reservation.Job == job))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x001B63B8 File Offset: 0x001B45B8
		public bool ReservedBy<TDriver>(LocalTargetInfo target, Pawn claimant, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && reservation.Job != null && typeof(TDriver).IsAssignableFrom(reservation.Job.def.driverClass) && (targetAIsNot == null || reservation.Job.targetA != targetAIsNot) && (targetBIsNot == null || reservation.Job.targetB != targetBIsNot) && (targetCIsNot == null || reservation.Job.targetC != targetCIsNot))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x001B64E0 File Offset: 0x001B46E0
		public IEnumerable<Thing> AllReservedThings()
		{
			return from res in this.reservations
			select res.Target.Thing;
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x001B650C File Offset: 0x001B470C
		private static bool RespectsReservationsOf(Faction faction, Pawn claimant)
		{
			return faction != null && ((claimant != null) ? claimant.Faction : null) != null && (faction == claimant.Faction || !faction.HostileTo(claimant.Faction) || faction == claimant.HostFaction);
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x001B6548 File Offset: 0x001B4748
		private static bool RespectsReservationsOf(Pawn newClaimant, Pawn oldClaimant)
		{
			if (newClaimant == oldClaimant)
			{
				return true;
			}
			if (newClaimant.Faction == null || oldClaimant.Faction == null)
			{
				return false;
			}
			if (newClaimant.Faction == oldClaimant.Faction)
			{
				return true;
			}
			if (!newClaimant.Faction.HostileTo(oldClaimant.Faction))
			{
				return true;
			}
			if (oldClaimant.HostFaction != null && oldClaimant.HostFaction == newClaimant.HostFaction)
			{
				return true;
			}
			if (newClaimant.HostFaction != null)
			{
				if (oldClaimant.HostFaction != null)
				{
					return true;
				}
				if (newClaimant.HostFaction == oldClaimant.Faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x001B65D0 File Offset: 0x001B47D0
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All reservation in ReservationManager:");
			for (int i = 0; i < this.reservations.Count; i++)
			{
				stringBuilder.AppendLine("[" + i.ToString() + "] " + this.reservations[i].ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x001B663C File Offset: 0x001B483C
		internal void DebugDrawReservations()
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target.Thing != null)
				{
					if (reservation.Target.Thing.Spawned)
					{
						Thing thing = reservation.Target.Thing;
						Vector3 s = new Vector3((float)thing.RotatedSize.x, 1f, (float)thing.RotatedSize.z);
						Matrix4x4 matrix = default(Matrix4x4);
						matrix.SetTRS(thing.DrawPos + Vector3.up * 0.1f, Quaternion.identity, s);
						Graphics.DrawMesh(MeshPool.plane10, matrix, ReservationManager.DebugReservedThingIcon, 0);
						GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Thing.DrawPos);
					}
					else
					{
						Graphics.DrawMesh(MeshPool.plane03, reservation.Claimant.DrawPos + Vector3.up + new Vector3(0.5f, 0f, 0.5f), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					}
				}
				else
				{
					Graphics.DrawMesh(MeshPool.plane10, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays));
				}
			}
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x001B67D8 File Offset: 0x001B49D8
		private void LogCouldNotReserveError(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns, int stackCount, ReservationLayerDef layer)
		{
			Job curJob = claimant.CurJob;
			string text = "null";
			int num = -1;
			if (curJob != null)
			{
				text = curJob.ToString();
				if (claimant.jobs.curDriver != null)
				{
					num = claimant.jobs.curDriver.CurToilIndex;
				}
			}
			string text2;
			if (target.HasThing && target.Thing.def.stackLimit != 1)
			{
				text2 = "(current stack count: " + target.Thing.stackCount.ToString() + ")";
			}
			else
			{
				text2 = "";
			}
			string text3 = string.Concat(new string[]
			{
				"Could not reserve ",
				target.ToStringSafe<LocalTargetInfo>(),
				text2,
				" (layer: ",
				layer.ToStringSafe<ReservationLayerDef>(),
				") for ",
				claimant.ToStringSafe<Pawn>(),
				" for job ",
				job.ToStringSafe<Job>(),
				" (now doing job ",
				text,
				"(curToil=",
				num.ToString(),
				")) for maxPawns ",
				maxPawns.ToString(),
				" and stackCount ",
				stackCount.ToString(),
				"."
			});
			int num2 = 0;
			bool flag = false;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && (layer == null || reservation.Layer == layer) && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
				{
					if (!flag)
					{
						text3 += "\nExisting reservers:";
						flag = true;
					}
					string value = "null";
					int value2 = -1;
					Job curJob2 = reservation.Claimant.CurJob;
					if (curJob2 != null)
					{
						value = curJob2.ToStringSafe<Job>();
						if (reservation.Claimant.jobs.curDriver != null)
						{
							value2 = reservation.Claimant.jobs.curDriver.CurToilIndex;
						}
					}
					string str = text3;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 6);
					defaultInterpolatedStringHandler.AppendLiteral("\n   [");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num2++);
					defaultInterpolatedStringHandler.AppendLiteral("] ");
					defaultInterpolatedStringHandler.AppendFormatted(reservation.Claimant.ToStringSafe<Pawn>());
					defaultInterpolatedStringHandler.AppendLiteral(" (job: ");
					defaultInterpolatedStringHandler.AppendFormatted(value);
					defaultInterpolatedStringHandler.AppendLiteral(", toil: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(value2);
					defaultInterpolatedStringHandler.AppendLiteral(", maxPawns: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(reservation.MaxPawns);
					defaultInterpolatedStringHandler.AppendLiteral(", stackCount: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(reservation.StackCount);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					text3 = str + defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			if (!flag)
			{
				text3 += " No existing reserver.";
			}
			Pawn pawn = this.map.physicalInteractionReservationManager.FirstReserverOf(target);
			if (pawn != null)
			{
				text3 = text3 + " Physical interaction reserver: " + pawn.ToStringSafe<Pawn>();
			}
			Log.Error(text3);
		}

		// Token: 0x04002D47 RID: 11591
		private Map map;

		// Token: 0x04002D48 RID: 11592
		private List<ReservationManager.Reservation> reservations = new List<ReservationManager.Reservation>();

		// Token: 0x04002D49 RID: 11593
		private static readonly Material DebugReservedThingIcon = MaterialPool.MatFrom("UI/Overlays/ReservedForWork", ShaderDatabase.Cutout);

		// Token: 0x04002D4A RID: 11594
		public const int StackCount_All = -1;

		// Token: 0x04002D4B RID: 11595
		private List<Pawn> tmpReservers = new List<Pawn>();

		// Token: 0x02002BA1 RID: 11169
		public class Reservation : IExposable
		{
			// Token: 0x17002C6F RID: 11375
			// (get) Token: 0x06010AE0 RID: 68320 RVA: 0x005D6FEC File Offset: 0x005D51EC
			public Pawn Claimant
			{
				get
				{
					return this.claimant;
				}
			}

			// Token: 0x17002C70 RID: 11376
			// (get) Token: 0x06010AE1 RID: 68321 RVA: 0x005D6FF4 File Offset: 0x005D51F4
			public Job Job
			{
				get
				{
					return this.job;
				}
			}

			// Token: 0x17002C71 RID: 11377
			// (get) Token: 0x06010AE2 RID: 68322 RVA: 0x005D6FFC File Offset: 0x005D51FC
			public LocalTargetInfo Target
			{
				get
				{
					return this.target;
				}
			}

			// Token: 0x17002C72 RID: 11378
			// (get) Token: 0x06010AE3 RID: 68323 RVA: 0x005D7004 File Offset: 0x005D5204
			public ReservationLayerDef Layer
			{
				get
				{
					return this.layer;
				}
			}

			// Token: 0x17002C73 RID: 11379
			// (get) Token: 0x06010AE4 RID: 68324 RVA: 0x005D700C File Offset: 0x005D520C
			public int MaxPawns
			{
				get
				{
					return this.maxPawns;
				}
			}

			// Token: 0x17002C74 RID: 11380
			// (get) Token: 0x06010AE5 RID: 68325 RVA: 0x005D7014 File Offset: 0x005D5214
			public int StackCount
			{
				get
				{
					return this.stackCount;
				}
			}

			// Token: 0x17002C75 RID: 11381
			// (get) Token: 0x06010AE6 RID: 68326 RVA: 0x005D701C File Offset: 0x005D521C
			public Faction Faction
			{
				get
				{
					return this.claimant.Faction;
				}
			}

			// Token: 0x06010AE7 RID: 68327 RVA: 0x005D7029 File Offset: 0x005D5229
			public Reservation()
			{
			}

			// Token: 0x06010AE8 RID: 68328 RVA: 0x005D7038 File Offset: 0x005D5238
			public Reservation(Pawn claimant, Job job, int maxPawns, int stackCount, LocalTargetInfo target, ReservationLayerDef layer)
			{
				this.claimant = claimant;
				this.job = job;
				this.maxPawns = maxPawns;
				this.stackCount = stackCount;
				this.target = target;
				this.layer = layer;
			}

			// Token: 0x06010AE9 RID: 68329 RVA: 0x005D7074 File Offset: 0x005D5274
			public void ExposeData()
			{
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_Values.Look<int>(ref this.maxPawns, "maxPawns", 0, false);
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Defs.Look<ReservationLayerDef>(ref this.layer, "layer");
			}

			// Token: 0x06010AEA RID: 68330 RVA: 0x005D70E8 File Offset: 0x005D52E8
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					(this.claimant != null) ? this.claimant.LabelShort : "null",
					":",
					this.job.ToStringSafe<Job>(),
					", ",
					this.target.ToStringSafe<LocalTargetInfo>(),
					", ",
					this.layer.ToStringSafe<ReservationLayerDef>(),
					", ",
					this.maxPawns.ToString(),
					", ",
					this.stackCount.ToString()
				});
			}

			// Token: 0x0400B23A RID: 45626
			private Pawn claimant;

			// Token: 0x0400B23B RID: 45627
			private Job job;

			// Token: 0x0400B23C RID: 45628
			private LocalTargetInfo target;

			// Token: 0x0400B23D RID: 45629
			private ReservationLayerDef layer;

			// Token: 0x0400B23E RID: 45630
			private int maxPawns;

			// Token: 0x0400B23F RID: 45631
			private int stackCount = -1;
		}
	}
}
