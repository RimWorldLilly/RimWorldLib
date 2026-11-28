using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000ED RID: 237
	public class PhysicalInteractionReservationManager : IExposable
	{
		// Token: 0x06000890 RID: 2192 RVA: 0x000309F0 File Offset: 0x0002EBF0
		public void Reserve(Pawn claimant, Job job, LocalTargetInfo target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				return;
			}
			PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = new PhysicalInteractionReservationManager.PhysicalInteractionReservation();
			physicalInteractionReservation.target = target;
			physicalInteractionReservation.claimant = claimant;
			physicalInteractionReservation.job = job;
			this.reservations.Add(physicalInteractionReservation);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00030A30 File Offset: 0x0002EC30
		public void Release(Pawn claimant, Job job, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant && physicalInteractionReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			string str = (claimant != null) ? claimant.ToString() : null;
			string str2 = " tried to release reservation on target ";
			LocalTargetInfo localTargetInfo = target;
			Log.Warning(str + str2 + localTargetInfo.ToString() + ", but it's not reserved by him.");
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00030ABD File Offset: 0x0002ECBD
		public void TryRelease(Pawn claimant, Job job, LocalTargetInfo target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				this.Release(claimant, job, target);
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00030AD4 File Offset: 0x0002ECD4
		public bool IsReservedBy(Pawn claimant, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00030B1E File Offset: 0x0002ED1E
		public bool IsReserved(LocalTargetInfo target)
		{
			return this.FirstReserverOf(target) != null;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00030B2C File Offset: 0x0002ED2C
		public Pawn FirstReserverOf(LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					return physicalInteractionReservation.claimant;
				}
			}
			return null;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00030B74 File Offset: 0x0002ED74
		public void ReserversOf(LocalTargetInfo target, HashSet<Pawn> reserversOut)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					reserversOut.Add(physicalInteractionReservation.claimant);
				}
			}
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00030BC0 File Offset: 0x0002EDC0
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00030C10 File Offset: 0x0002EE10
		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.target == target);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00030C44 File Offset: 0x0002EE44
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

		// Token: 0x0600089A RID: 2202 RVA: 0x00030CA0 File Offset: 0x0002EEA0
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

		// Token: 0x0600089B RID: 2203 RVA: 0x00030CE8 File Offset: 0x0002EEE8
		public void ExposeData()
		{
			Scribe_Collections.Look<PhysicalInteractionReservationManager.PhysicalInteractionReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some physical interaction reservations had null or destroyed claimant.");
				}
			}
		}

		// Token: 0x040003D3 RID: 979
		private List<PhysicalInteractionReservationManager.PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservationManager.PhysicalInteractionReservation>();

		// Token: 0x02002506 RID: 9478
		public class PhysicalInteractionReservation : IExposable
		{
			// Token: 0x0600EC83 RID: 60547 RVA: 0x00573AA7 File Offset: 0x00571CA7
			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x0400976B RID: 38763
			public LocalTargetInfo target;

			// Token: 0x0400976C RID: 38764
			public Pawn claimant;

			// Token: 0x0400976D RID: 38765
			public Job job;
		}
	}
}
