using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200155E RID: 5470
	public class Pawn_MechanitorTracker : IExposable
	{
		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x060085D8 RID: 34264 RVA: 0x00305EC4 File Offset: 0x003040C4
		public List<Pawn> OverseenPawns
		{
			get
			{
				this.overseenPawns.Clear();
				List<DirectPawnRelation> directRelations = this.pawn.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (directRelations[i].def == PawnRelationDefOf.Overseer)
					{
						this.overseenPawns.Add(directRelations[i].otherPawn);
					}
				}
				return this.overseenPawns;
			}
		}

		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x060085D9 RID: 34265 RVA: 0x00305F2E File Offset: 0x0030412E
		public List<Pawn> ControlledPawns
		{
			get
			{
				return this.controlledPawns;
			}
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x060085DA RID: 34266 RVA: 0x00305F36 File Offset: 0x00304136
		public int UsedBandwidth
		{
			get
			{
				return this.UsedBandwidthFromSubjects + this.UsedBandwidthFromGestation;
			}
		}

		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x060085DB RID: 34267 RVA: 0x00305F48 File Offset: 0x00304148
		public int UsedBandwidthFromSubjects
		{
			get
			{
				return (int)(from p in this.OverseenPawns
				where !p.IsGestating()
				select p).Sum((Pawn p) => p.GetStatValue(StatDefOf.BandwidthCost, true, -1));
			}
		}

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x060085DC RID: 34268 RVA: 0x00305FA4 File Offset: 0x003041A4
		public int UsedBandwidthFromGestation
		{
			get
			{
				List<Bill_Mech> list = this.ActiveMechBills;
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num += (int)list[i].BandwidthCost;
				}
				return num;
			}
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x060085DD RID: 34269 RVA: 0x00305FDC File Offset: 0x003041DC
		public int TotalBandwidth
		{
			get
			{
				return (int)this.pawn.GetStatValue(StatDefOf.MechBandwidth, true, -1);
			}
		}

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x060085DE RID: 34270 RVA: 0x00305FF1 File Offset: 0x003041F1
		public int TotalAvailableControlGroups
		{
			get
			{
				return (int)this.pawn.GetStatValue(StatDefOf.MechControlGroups, true, -1);
			}
		}

		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x060085DF RID: 34271 RVA: 0x00306008 File Offset: 0x00304208
		public List<Bill_Mech> ActiveMechBills
		{
			get
			{
				this.activeMechBills.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.MechGestator);
					for (int j = 0; j < list.Count; j++)
					{
						Building_MechGestator building_MechGestator = list[j] as Building_MechGestator;
						if (building_MechGestator != null && building_MechGestator.ActiveMechBill != null && building_MechGestator.ActiveMechBill.BoundPawn == this.pawn)
						{
							this.activeMechBills.Add(building_MechGestator.ActiveMechBill);
						}
					}
				}
				this.activeMechBills.SortBy((Bill_Mech b) => b.StartedTick);
				return this.activeMechBills;
			}
		}

		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x060085E0 RID: 34272 RVA: 0x003060CD File Offset: 0x003042CD
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x060085E1 RID: 34273 RVA: 0x003060D8 File Offset: 0x003042D8
		public bool AnySelectedDraftedMechs
		{
			get
			{
				List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
				for (int i = 0; i < selectedPawns.Count; i++)
				{
					if (selectedPawns[i].GetOverseer() == this.pawn && selectedPawns[i].Drafted)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170016A7 RID: 5799
		// (get) Token: 0x060085E2 RID: 34274 RVA: 0x00306128 File Offset: 0x00304328
		public AcceptanceReport CanControlMechs
		{
			get
			{
				if (this.pawn.Downed)
				{
					return "MechControllerDowned".Translate(this.pawn.Named("PAWN"));
				}
				if (this.pawn.IsPrisoner)
				{
					return "MechControllerImprisoned".Translate(this.pawn.Named("PAWN"));
				}
				if (!this.pawn.Spawned)
				{
					Thing spawnedParentOrMe = this.pawn.SpawnedParentOrMe;
					if (spawnedParentOrMe is Building)
					{
						return "MechControllerInsideContainer".Translate(this.pawn.Named("PAWN"), spawnedParentOrMe.Named("CONTAINER"));
					}
					return false;
				}
				else
				{
					if (this.pawn.InMentalState)
					{
						return "MechControllerMentalState".Translate(this.pawn.Named("PAWN"), this.pawn.MentalStateDef.Named("MENTALSTATE"));
					}
					return true;
				}
			}
		}

		// Token: 0x060085E3 RID: 34275 RVA: 0x0030622C File Offset: 0x0030442C
		public Pawn_MechanitorTracker()
		{
		}

		// Token: 0x060085E4 RID: 34276 RVA: 0x00306284 File Offset: 0x00304484
		public Pawn_MechanitorTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060085E5 RID: 34277 RVA: 0x003062E0 File Offset: 0x003044E0
		public bool CanOverseeSubject(Pawn subject)
		{
			return subject.OverseerSubject != null && subject.GetStatValue(StatDefOf.BandwidthCost, true, -1) <= (float)this.TotalBandwidth;
		}

		// Token: 0x060085E6 RID: 34278 RVA: 0x00306308 File Offset: 0x00304508
		public MechanitorControlGroup GetControlGroup(Pawn mech)
		{
			foreach (MechanitorControlGroup mechanitorControlGroup in this.controlGroups)
			{
				if (mechanitorControlGroup.MechsForReading.Contains(mech))
				{
					return mechanitorControlGroup;
				}
			}
			return null;
		}

		// Token: 0x060085E7 RID: 34279 RVA: 0x0030636C File Offset: 0x0030456C
		public void AssignPawnControlGroup(Pawn pawn, MechWorkModeDef preferredWorkMode = null)
		{
			if (this.controlGroups.Count == 0)
			{
				this.Notify_ControlGroupAmountMayChanged();
				if (this.controlGroups.Count == 0)
				{
					Log.Warning("Wants to assign pawn to a control group, but there are none!");
					return;
				}
			}
			if (preferredWorkMode != null)
			{
				if (this.controlGroups.Any((MechanitorControlGroup c) => c.WorkMode == preferredWorkMode))
				{
					this.controlGroups.First((MechanitorControlGroup c) => c.WorkMode == preferredWorkMode).Assign(pawn);
				}
				else if (this.controlGroups.Any((MechanitorControlGroup c) => c.MechsForReading.Count == 0))
				{
					MechanitorControlGroup mechanitorControlGroup = this.controlGroups.First((MechanitorControlGroup c) => c.MechsForReading.Count == 0);
					mechanitorControlGroup.SetWorkMode(preferredWorkMode);
					mechanitorControlGroup.Assign(pawn);
				}
				else
				{
					this.controlGroups[0].Assign(pawn);
				}
			}
			else
			{
				this.controlGroups[0].Assign(pawn);
			}
			this.Notify_BandwidthChanged();
		}

		// Token: 0x060085E8 RID: 34280 RVA: 0x00306490 File Offset: 0x00304690
		public void UnassignPawnFromAnyControlGroup(Pawn pawn)
		{
			foreach (MechanitorControlGroup mechanitorControlGroup in this.controlGroups)
			{
				mechanitorControlGroup.TryUnassign(pawn);
			}
		}

		// Token: 0x060085E9 RID: 34281 RVA: 0x003064E4 File Offset: 0x003046E4
		private void Notify_ControlGroupAmountMayChanged()
		{
			int totalAvailableControlGroups = this.TotalAvailableControlGroups;
			while (this.controlGroups.Count < totalAvailableControlGroups)
			{
				this.controlGroups.Add(new MechanitorControlGroup(this));
			}
			List<Pawn> list = null;
			while (this.controlGroups.Count > totalAvailableControlGroups)
			{
				MechanitorControlGroup mechanitorControlGroup = this.controlGroups[this.controlGroups.Count - 1];
				if (this.controlGroups.Count > 1)
				{
					if (!mechanitorControlGroup.MechsForReading.NullOrEmpty<Pawn>())
					{
						if (list == null)
						{
							list = new List<Pawn>();
						}
						list.AddRange(mechanitorControlGroup.MechsForReading);
					}
				}
				else
				{
					Log.Warning("Removed last mechanitor control group");
				}
				this.controlGroups.RemoveAt(this.controlGroups.Count - 1);
			}
			if (list != null && this.controlGroups.Count > 0)
			{
				foreach (Pawn pawn in list)
				{
					this.controlGroups[0].Assign(pawn);
				}
			}
			this.controlGroupGizmos.Clear();
			foreach (MechanitorControlGroup controlGroup in this.controlGroups)
			{
				this.controlGroupGizmos.Add(new MechanitorControlGroupGizmo(controlGroup));
			}
		}

		// Token: 0x060085EA RID: 34282 RVA: 0x00306654 File Offset: 0x00304854
		public void Notify_PawnSpawned(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				this.Notify_BandwidthChanged();
				this.Notify_ControlGroupAmountMayChanged();
			}
		}

		// Token: 0x060085EB RID: 34283 RVA: 0x00306668 File Offset: 0x00304868
		public void Notify_BandwidthChanged()
		{
			this.CheckAvailableBandwidthForBills();
			Pawn_MechanitorTracker.tmpPrevControlledMechs.Clear();
			Pawn_MechanitorTracker.tmpPrevControlledMechs.AddRange(this.controlledPawns);
			this.controlledPawns.Clear();
			this.tmpMechsInAssignedOrder.Clear();
			MechanitorUtility.GetMechsInAssignedOrder(this.pawn, ref this.tmpMechsInAssignedOrder);
			float num = 0f;
			for (int i = this.tmpMechsInAssignedOrder.Count - 1; i >= 0; i--)
			{
				num += this.tmpMechsInAssignedOrder[i].GetStatValue(StatDefOf.BandwidthCost, true, -1);
				if (num <= (float)this.TotalBandwidth)
				{
					this.controlledPawns.Add(this.tmpMechsInAssignedOrder[i]);
				}
				else if (Pawn_MechanitorTracker.tmpPrevControlledMechs.Contains(this.tmpMechsInAssignedOrder[i]))
				{
					CompOverseerSubject overseerSubject = this.tmpMechsInAssignedOrder[i].OverseerSubject;
					if (overseerSubject != null)
					{
						overseerSubject.Notify_DisconnectedFromOverseer();
					}
				}
			}
			Pawn_MechanitorTracker.tmpPrevControlledMechs.Clear();
			this.tmpMechsInAssignedOrder.Clear();
		}

		// Token: 0x060085EC RID: 34284 RVA: 0x00306764 File Offset: 0x00304964
		public void CheckAvailableBandwidthForBills()
		{
			List<Bill_Mech> list = this.ActiveMechBills;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].suspended = !this.HasBandwidthForBill(list[i]);
			}
		}

		// Token: 0x060085ED RID: 34285 RVA: 0x003067A8 File Offset: 0x003049A8
		public void Notify_MechlinkRemoved()
		{
			List<Pawn> list = this.OverseenPawns;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.pawn.relations.TryRemoveDirectRelation(PawnRelationDefOf.Overseer, list[i]);
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
		}

		// Token: 0x060085EE RID: 34286 RVA: 0x003067F8 File Offset: 0x003049F8
		public void Notify_ChangedGuestStatus()
		{
			if (this.pawn.IsPrisoner)
			{
				this.UndraftAllMechs();
				for (int i = 0; i < this.controlGroups.Count; i++)
				{
					this.controlGroups[i].SetWorkMode(MechWorkModeDefOf.SelfShutdown);
				}
			}
		}

		// Token: 0x060085EF RID: 34287 RVA: 0x00306844 File Offset: 0x00304A44
		public void Notify_ApparelChanged()
		{
			this.Notify_BandwidthChanged();
			this.Notify_ControlGroupAmountMayChanged();
		}

		// Token: 0x060085F0 RID: 34288 RVA: 0x00306852 File Offset: 0x00304A52
		public void Notify_HediffStateChange(Hediff hediff)
		{
			if (hediff != null)
			{
				this.Notify_BandwidthChanged();
				this.Notify_ControlGroupAmountMayChanged();
			}
		}

		// Token: 0x060085F1 RID: 34289 RVA: 0x00306864 File Offset: 0x00304A64
		public void Notify_ChangedFaction()
		{
			List<Pawn> list = this.OverseenPawns;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetFaction(this.pawn.Faction, null);
				list[i].Notify_DisabledWorkTypesChanged();
			}
		}

		// Token: 0x060085F2 RID: 34290 RVA: 0x003068AD File Offset: 0x00304AAD
		public void Notify_Downed()
		{
			this.UndraftAllMechs();
		}

		// Token: 0x060085F3 RID: 34291 RVA: 0x003068B5 File Offset: 0x00304AB5
		public void Notify_DeSpawned(DestroyMode mode)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.UndraftAllMechs();
			}
		}

		// Token: 0x060085F4 RID: 34292 RVA: 0x003068C4 File Offset: 0x00304AC4
		public void UndraftAllMechs()
		{
			this.tmpMechsInAssignedOrder.Clear();
			MechanitorUtility.GetMechsInAssignedOrder(this.pawn, ref this.tmpMechsInAssignedOrder);
			for (int i = 0; i < this.tmpMechsInAssignedOrder.Count; i++)
			{
				if (this.tmpMechsInAssignedOrder[i].Drafted)
				{
					this.tmpMechsInAssignedOrder[i].drafter.Drafted = false;
				}
			}
			this.tmpMechsInAssignedOrder.Clear();
		}

		// Token: 0x060085F5 RID: 34293 RVA: 0x00306938 File Offset: 0x00304B38
		public bool HasBandwidthForBill(Bill_Mech bill)
		{
			int totalBandwidth = this.TotalBandwidth;
			int num = this.UsedBandwidthFromSubjects;
			float bandwidthCost = bill.BandwidthCost;
			if ((float)totalBandwidth < bandwidthCost + (float)num)
			{
				return false;
			}
			List<Bill_Mech> list = this.ActiveMechBills;
			for (int i = 0; i < list.Count; i++)
			{
				num += (int)list[i].BandwidthCost;
				if (list[i] == bill && num <= totalBandwidth)
				{
					return true;
				}
				if (num >= totalBandwidth)
				{
					return false;
				}
			}
			return (float)num + bandwidthCost <= (float)totalBandwidth;
		}

		// Token: 0x060085F6 RID: 34294 RVA: 0x003069B4 File Offset: 0x00304BB4
		public bool HasBandwidthToResurrect(Corpse corpse)
		{
			float statValue = corpse.InnerPawn.GetStatValue(StatDefOf.BandwidthCost, true, -1);
			return (float)this.UsedBandwidth + statValue <= (float)this.TotalBandwidth;
		}

		// Token: 0x060085F7 RID: 34295 RVA: 0x003069E9 File Offset: 0x00304BE9
		public void DrawCommandRadius()
		{
			if (this.pawn.Spawned && this.AnySelectedDraftedMechs)
			{
				GenDraw.DrawRadiusRing(this.pawn.Position, 24.9f, Color.white, (IntVec3 c) => this.CanCommandTo(c));
			}
		}

		// Token: 0x060085F8 RID: 34296 RVA: 0x00306A26 File Offset: 0x00304C26
		public bool CanCommandTo(LocalTargetInfo target)
		{
			return target.Cell.InBounds(this.pawn.MapHeld) && (float)this.pawn.Position.DistanceToSquared(target.Cell) < 620.01f;
		}

		// Token: 0x060085F9 RID: 34297 RVA: 0x00306A62 File Offset: 0x00304C62
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_MechanitorTracker.<GetGizmos>d__56 <GetGizmos>d__ = new Pawn_MechanitorTracker.<GetGizmos>d__56(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x060085FA RID: 34298 RVA: 0x00306A74 File Offset: 0x00304C74
		public void ExposeData()
		{
			Scribe_Collections.Look<MechanitorControlGroup>(ref this.controlGroups, "controlGroups", LookMode.Deep, new object[]
			{
				this
			});
			Scribe_Collections.Look<Pawn>(ref this.controlledPawns, "controlledPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.controlledPawns.RemoveAll((Pawn x) => x == null);
				if (this.controlGroups == null)
				{
					this.controlGroups = new List<MechanitorControlGroup>();
				}
				this.Notify_ControlGroupAmountMayChanged();
			}
		}

		// Token: 0x04005280 RID: 21120
		private const float MechCommandRange = 24.9f;

		// Token: 0x04005281 RID: 21121
		private static readonly CachedTexture SelectAllMechsIcon = new CachedTexture("UI/Icons/SelectAllMechs");

		// Token: 0x04005282 RID: 21122
		private Pawn pawn;

		// Token: 0x04005283 RID: 21123
		private MechanitorBandwidthGizmo bandwidthGizmo;

		// Token: 0x04005284 RID: 21124
		private Command_CallBossgroup callBossgroupGizmo;

		// Token: 0x04005285 RID: 21125
		private List<MechanitorControlGroupGizmo> controlGroupGizmos = new List<MechanitorControlGroupGizmo>();

		// Token: 0x04005286 RID: 21126
		private List<Pawn> tmpMechsInAssignedOrder = new List<Pawn>();

		// Token: 0x04005287 RID: 21127
		private List<Pawn> overseenPawns = new List<Pawn>();

		// Token: 0x04005288 RID: 21128
		private List<Pawn> controlledPawns = new List<Pawn>();

		// Token: 0x04005289 RID: 21129
		public List<MechanitorControlGroup> controlGroups = new List<MechanitorControlGroup>();

		// Token: 0x0400528A RID: 21130
		private List<Bill_Mech> activeMechBills = new List<Bill_Mech>();

		// Token: 0x0400528B RID: 21131
		private static List<Pawn> tmpPrevControlledMechs = new List<Pawn>();
	}
}
