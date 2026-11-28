using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200159F RID: 5535
	public class Pawn_Ownership : IExposable
	{
		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x06008871 RID: 34929 RVA: 0x00315D1B File Offset: 0x00313F1B
		// (set) Token: 0x06008872 RID: 34930 RVA: 0x00315D23 File Offset: 0x00313F23
		public Building_Bed OwnedBed
		{
			get
			{
				return this.intOwnedBed;
			}
			private set
			{
				if (this.intOwnedBed != value)
				{
					this.intOwnedBed = value;
					ThoughtUtility.RemovePositiveBedroomThoughts(this.pawn);
				}
			}
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x06008873 RID: 34931 RVA: 0x00315D40 File Offset: 0x00313F40
		// (set) Token: 0x06008874 RID: 34932 RVA: 0x00315D48 File Offset: 0x00313F48
		public Building_Grave AssignedGrave { get; private set; }

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x06008875 RID: 34933 RVA: 0x00315D51 File Offset: 0x00313F51
		// (set) Token: 0x06008876 RID: 34934 RVA: 0x00315D59 File Offset: 0x00313F59
		public Building_Throne AssignedThrone { get; private set; }

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x06008877 RID: 34935 RVA: 0x00315D62 File Offset: 0x00313F62
		// (set) Token: 0x06008878 RID: 34936 RVA: 0x00315D6A File Offset: 0x00313F6A
		public Building AssignedMeditationSpot { get; private set; }

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x06008879 RID: 34937 RVA: 0x00315D73 File Offset: 0x00313F73
		// (set) Token: 0x0600887A RID: 34938 RVA: 0x00315D7B File Offset: 0x00313F7B
		public Building_Bed AssignedDeathrestCasket { get; private set; }

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x0600887B RID: 34939 RVA: 0x00315D84 File Offset: 0x00313F84
		public Room OwnedRoom
		{
			get
			{
				if (this.OwnedBed == null)
				{
					return null;
				}
				Room room = this.OwnedBed.GetRoom(RegionType.Set_All);
				if (room == null)
				{
					return null;
				}
				if (room.Owners.Contains(this.pawn))
				{
					return room;
				}
				return null;
			}
		}

		// Token: 0x17001773 RID: 6003
		// (get) Token: 0x0600887C RID: 34940 RVA: 0x00315DC4 File Offset: 0x00313FC4
		public Room Bedroom
		{
			get
			{
				if (this.OwnedBed == null)
				{
					return null;
				}
				return this.OwnedBed.GetRoom(RegionType.Set_All);
			}
		}

		// Token: 0x0600887D RID: 34941 RVA: 0x00315DDD File Offset: 0x00313FDD
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600887E RID: 34942 RVA: 0x00315DEC File Offset: 0x00313FEC
		public void ExposeData()
		{
			Building_Grave assignedGrave = this.AssignedGrave;
			Building_Throne assignedThrone = this.AssignedThrone;
			Building assignedMeditationSpot = this.AssignedMeditationSpot;
			Building_Bed assignedDeathrestCasket = this.AssignedDeathrestCasket;
			Scribe_References.Look<Building_Bed>(ref this.intOwnedBed, "ownedBed", false);
			Scribe_References.Look<Building>(ref assignedMeditationSpot, "assignedMeditationSpot", false);
			Scribe_References.Look<Building_Grave>(ref assignedGrave, "assignedGrave", false);
			Scribe_References.Look<Building_Throne>(ref assignedThrone, "assignedThrone", false);
			Scribe_References.Look<Building_Bed>(ref assignedDeathrestCasket, "assignedDeathrestCasket", false);
			this.AssignedGrave = assignedGrave;
			this.AssignedThrone = assignedThrone;
			this.AssignedMeditationSpot = assignedMeditationSpot;
			this.AssignedDeathrestCasket = assignedDeathrestCasket;
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.intOwnedBed != null)
				{
					CompAssignableToPawn compAssignableToPawn = this.intOwnedBed.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn != null && !compAssignableToPawn.AssignedPawns.Contains(this.pawn))
					{
						Building_Bed newBed = this.intOwnedBed;
						this.UnclaimBed();
						this.ClaimBedIfNonMedical(newBed);
					}
				}
				if (this.AssignedGrave != null)
				{
					CompAssignableToPawn compAssignableToPawn2 = this.AssignedGrave.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn2 != null && !compAssignableToPawn2.AssignedPawns.Contains(this.pawn))
					{
						Building_Grave assignedGrave2 = this.AssignedGrave;
						this.UnclaimGrave();
						this.ClaimGrave(assignedGrave2);
					}
				}
				if (this.AssignedThrone != null)
				{
					CompAssignableToPawn compAssignableToPawn3 = this.AssignedThrone.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn3 != null && !compAssignableToPawn3.AssignedPawns.Contains(this.pawn))
					{
						Building_Throne assignedThrone2 = this.AssignedThrone;
						this.UnclaimThrone();
						this.ClaimThrone(assignedThrone2);
					}
				}
			}
		}

		// Token: 0x0600887F RID: 34943 RVA: 0x00315F50 File Offset: 0x00314150
		public bool ClaimBedIfNonMedical(Building_Bed newBed)
		{
			if (newBed.IsOwner(this.pawn) || newBed.Medical)
			{
				return false;
			}
			if (ModsConfig.BiotechActive && newBed.def == ThingDefOf.DeathrestCasket)
			{
				this.UnclaimDeathrestCasket();
				newBed.CompAssignableToPawn.ForceAddPawn(this.pawn);
				this.AssignedDeathrestCasket = newBed;
				return true;
			}
			this.UnclaimBed();
			if (newBed.OwnersForReading.Count == newBed.SleepingSlotsCount)
			{
				newBed.OwnersForReading[newBed.OwnersForReading.Count - 1].ownership.UnclaimBed();
			}
			newBed.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.OwnedBed = newBed;
			if (this.pawn.IsFreeman && newBed.CompAssignableToPawn.IdeoligionForbids(this.pawn))
			{
				Log.Error("Assigned " + this.pawn.GetUniqueLoadID() + " to a bed against their or occupants' ideo.");
			}
			if (newBed.Medical)
			{
				Log.Warning(this.pawn.LabelCap + " claimed medical bed.");
				this.UnclaimBed();
			}
			return true;
		}

		// Token: 0x06008880 RID: 34944 RVA: 0x00316068 File Offset: 0x00314268
		public bool UnclaimBed()
		{
			if (this.OwnedBed == null)
			{
				return false;
			}
			this.OwnedBed.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.OwnedBed = null;
			return true;
		}

		// Token: 0x06008881 RID: 34945 RVA: 0x00316094 File Offset: 0x00314294
		public bool ClaimGrave(Building_Grave newGrave)
		{
			if (newGrave.AssignedPawn == this.pawn)
			{
				return false;
			}
			this.UnclaimGrave();
			if (newGrave.AssignedPawn != null)
			{
				newGrave.AssignedPawn.ownership.UnclaimGrave();
			}
			newGrave.CompAssignableToPawn.ForceAddPawn(this.pawn);
			newGrave.GetStoreSettings().Priority = StoragePriority.Critical;
			this.AssignedGrave = newGrave;
			return true;
		}

		// Token: 0x06008882 RID: 34946 RVA: 0x003160F6 File Offset: 0x003142F6
		public bool UnclaimGrave()
		{
			if (this.AssignedGrave == null)
			{
				return false;
			}
			this.AssignedGrave.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedGrave.GetStoreSettings().Priority = StoragePriority.Important;
			this.AssignedGrave = null;
			return true;
		}

		// Token: 0x06008883 RID: 34947 RVA: 0x00316134 File Offset: 0x00314334
		public bool ClaimThrone(Building_Throne newThrone)
		{
			if (newThrone.AssignedPawn == this.pawn)
			{
				return false;
			}
			this.UnclaimThrone();
			if (newThrone.AssignedPawn != null)
			{
				newThrone.AssignedPawn.ownership.UnclaimThrone();
			}
			newThrone.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.AssignedThrone = newThrone;
			return true;
		}

		// Token: 0x06008884 RID: 34948 RVA: 0x0031618A File Offset: 0x0031438A
		public bool UnclaimThrone()
		{
			if (this.AssignedThrone == null)
			{
				return false;
			}
			this.AssignedThrone.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedThrone = null;
			return true;
		}

		// Token: 0x06008885 RID: 34949 RVA: 0x003161B4 File Offset: 0x003143B4
		public bool ClaimMeditationSpot(Building newSpot)
		{
			if (newSpot.GetAssignedPawn() == this.pawn)
			{
				return false;
			}
			this.UnclaimMeditationSpot();
			if (newSpot.GetAssignedPawn() != null)
			{
				newSpot.GetAssignedPawn().ownership.UnclaimMeditationSpot();
			}
			newSpot.TryGetComp<CompAssignableToPawn>().ForceAddPawn(this.pawn);
			this.AssignedMeditationSpot = newSpot;
			return true;
		}

		// Token: 0x06008886 RID: 34950 RVA: 0x0031620A File Offset: 0x0031440A
		public bool UnclaimMeditationSpot()
		{
			if (this.AssignedMeditationSpot == null)
			{
				return false;
			}
			this.AssignedMeditationSpot.TryGetComp<CompAssignableToPawn>().ForceRemovePawn(this.pawn);
			this.AssignedMeditationSpot = null;
			return true;
		}

		// Token: 0x06008887 RID: 34951 RVA: 0x00316234 File Offset: 0x00314434
		public bool ClaimDeathrestCasket(Building_Bed deathrestCasket)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (deathrestCasket.CompAssignableToPawn.AssignedPawns.Contains(this.pawn))
			{
				return false;
			}
			this.UnclaimDeathrestCasket();
			if (deathrestCasket.GetAssignedPawn() != null)
			{
				deathrestCasket.GetAssignedPawn().ownership.UnclaimDeathrestCasket();
			}
			deathrestCasket.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.AssignedDeathrestCasket = deathrestCasket;
			return true;
		}

		// Token: 0x06008888 RID: 34952 RVA: 0x0031629D File Offset: 0x0031449D
		public bool UnclaimDeathrestCasket()
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (this.AssignedDeathrestCasket == null)
			{
				return false;
			}
			this.AssignedDeathrestCasket.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedDeathrestCasket = null;
			return true;
		}

		// Token: 0x06008889 RID: 34953 RVA: 0x003162D0 File Offset: 0x003144D0
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
			this.UnclaimThrone();
			this.UnclaimDeathrestCasket();
		}

		// Token: 0x0600888A RID: 34954 RVA: 0x003162F0 File Offset: 0x003144F0
		public void Notify_ChangedGuestStatus()
		{
			if (this.OwnedBed != null && ((this.OwnedBed.ForPrisoners && !this.pawn.IsPrisoner && !PawnUtility.IsBeingArrested(this.pawn)) || (!this.OwnedBed.ForPrisoners && this.pawn.IsPrisoner) || (this.OwnedBed.ForColonists && this.pawn.HostFaction == null)))
			{
				this.UnclaimBed();
			}
			if (this.AssignedDeathrestCasket != null && ((this.AssignedDeathrestCasket.ForPrisoners && !this.pawn.IsPrisoner && !PawnUtility.IsBeingArrested(this.pawn)) || (!this.AssignedDeathrestCasket.ForPrisoners && this.pawn.IsPrisoner) || (this.AssignedDeathrestCasket.ForColonists && this.pawn.HostFaction == null)))
			{
				this.UnclaimDeathrestCasket();
			}
		}

		// Token: 0x0400541E RID: 21534
		private Pawn pawn;

		// Token: 0x0400541F RID: 21535
		private Building_Bed intOwnedBed;
	}
}
