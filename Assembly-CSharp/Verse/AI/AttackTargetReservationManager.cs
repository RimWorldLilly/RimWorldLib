using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008B4 RID: 2228
	public class AttackTargetReservationManager : IExposable
	{
		// Token: 0x0600454F RID: 17743 RVA: 0x001B50F9 File Offset: 0x001B32F9
		public AttackTargetReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x001B5114 File Offset: 0x001B3314
		public void Reserve(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(((claimant != null) ? claimant.ToString() : null) + " tried to reserve null attack target.");
				return;
			}
			if (this.IsReservedBy(claimant, target))
			{
				return;
			}
			AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = new AttackTargetReservationManager.AttackTargetReservation();
			attackTargetReservation.target = target;
			attackTargetReservation.claimant = claimant;
			attackTargetReservation.job = job;
			this.reservations.Add(attackTargetReservation);
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x001B5174 File Offset: 0x001B3374
		public void Release(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(((claimant != null) ? claimant.ToString() : null) + " tried to release reservation on null attack target.");
				return;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant && attackTargetReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(string.Concat(new string[]
			{
				(claimant != null) ? claimant.ToString() : null,
				" with job ",
				(job != null) ? job.ToString() : null,
				" tried to release reservation on target ",
				(target != null) ? target.ToString() : null,
				", but it's not reserved by him."
			}));
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x001B5244 File Offset: 0x001B3444
		public bool CanReserve(Pawn claimant, IAttackTarget target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				return true;
			}
			int reservationsCount = this.GetReservationsCount(target, claimant.Faction);
			int maxPreferredReservationsCount = this.GetMaxPreferredReservationsCount(target);
			return reservationsCount < maxPreferredReservationsCount;
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x001B5278 File Offset: 0x001B3478
		public bool IsReservedBy(Pawn claimant, IAttackTarget target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x001B52C0 File Offset: 0x001B34C0
		public void ReleaseAllForTarget(IAttackTarget target)
		{
			this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == target);
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x001B52F4 File Offset: 0x001B34F4
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant && this.reservations[i].job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x001B5350 File Offset: 0x001B3550
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x001B5398 File Offset: 0x001B3598
		public IAttackTarget FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return null;
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x001B53E4 File Offset: 0x001B35E4
		public void ExposeData()
		{
			Scribe_Collections.Look<AttackTargetReservationManager.AttackTargetReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == null);
				if (this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some attack target reservations had null or destroyed claimant.");
				}
			}
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x001B5470 File Offset: 0x001B3670
		private int GetReservationsCount(IAttackTarget target, Faction faction)
		{
			int num = 0;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant.Faction == faction)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x001B54C0 File Offset: 0x001B36C0
		private int GetMaxPreferredReservationsCount(IAttackTarget target)
		{
			int num = 0;
			CellRect cellRect = target.Thing.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c) && c.InBounds(this.map) && c.Standable(this.map))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04002D45 RID: 11589
		private Map map;

		// Token: 0x04002D46 RID: 11590
		private List<AttackTargetReservationManager.AttackTargetReservation> reservations = new List<AttackTargetReservationManager.AttackTargetReservation>();

		// Token: 0x02002B9E RID: 11166
		public class AttackTargetReservation : IExposable
		{
			// Token: 0x06010AD8 RID: 68312 RVA: 0x005D6F6B File Offset: 0x005D516B
			public void ExposeData()
			{
				Scribe_References.Look<IAttackTarget>(ref this.target, "target", false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x0400B233 RID: 45619
			public IAttackTarget target;

			// Token: 0x0400B234 RID: 45620
			public Pawn claimant;

			// Token: 0x0400B235 RID: 45621
			public Job job;
		}
	}
}
