using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A9D RID: 6813
	public class Building_Bed : Building
	{
		// Token: 0x17001E8D RID: 7821
		// (get) Token: 0x0600ABCE RID: 43982 RVA: 0x003CA74F File Offset: 0x003C894F
		public List<Pawn> OwnersForReading
		{
			get
			{
				return this.CompAssignableToPawn.AssignedPawnsForReading;
			}
		}

		// Token: 0x17001E8E RID: 7822
		// (get) Token: 0x0600ABCF RID: 43983 RVA: 0x003CA75C File Offset: 0x003C895C
		public CompAssignableToPawn CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn>();
			}
		}

		// Token: 0x17001E8F RID: 7823
		// (get) Token: 0x0600ABD0 RID: 43984 RVA: 0x003CA764 File Offset: 0x003C8964
		// (set) Token: 0x0600ABD1 RID: 43985 RVA: 0x003CA770 File Offset: 0x003C8970
		public bool ForPrisoners
		{
			get
			{
				return this.forOwnerType == BedOwnerType.Prisoner;
			}
			set
			{
				if (value == this.ForPrisoners || !this.def.building.bed_humanlike || this.ForHumanBabies)
				{
					return;
				}
				if (Current.ProgramState != ProgramState.Playing && Scribe.mode != LoadSaveMode.Inactive)
				{
					Log.Error("Tried to set ForPrisoners while game mode was " + Current.ProgramState.ToString());
					return;
				}
				this.RemoveAllOwners(false);
				if (value)
				{
					this.forOwnerType = BedOwnerType.Prisoner;
				}
				else
				{
					this.forOwnerType = BedOwnerType.Colonist;
					Log.Error("Bed ForPrisoners=false, but should it be for for colonists or slaves?  Set ForOwnerType instead.");
				}
				this.Notify_ColorChanged();
				this.NotifyRoomBedTypeChanged();
			}
		}

		// Token: 0x17001E90 RID: 7824
		// (get) Token: 0x0600ABD2 RID: 43986 RVA: 0x003CA803 File Offset: 0x003C8A03
		public bool ForSlaves
		{
			get
			{
				return this.ForOwnerType == BedOwnerType.Slave;
			}
		}

		// Token: 0x17001E91 RID: 7825
		// (get) Token: 0x0600ABD3 RID: 43987 RVA: 0x003CA80E File Offset: 0x003C8A0E
		public bool ForColonists
		{
			get
			{
				return this.ForOwnerType == BedOwnerType.Colonist;
			}
		}

		// Token: 0x17001E92 RID: 7826
		// (get) Token: 0x0600ABD4 RID: 43988 RVA: 0x003CA819 File Offset: 0x003C8A19
		public bool ForHumanBabies
		{
			get
			{
				return this.def.building.bed_humanlike && this.def.building.bed_maxBodySize < LifeStageDefOf.HumanlikeChild.bodySizeFactor;
			}
		}

		// Token: 0x17001E93 RID: 7827
		// (get) Token: 0x0600ABD5 RID: 43989 RVA: 0x003CA84B File Offset: 0x003C8A4B
		// (set) Token: 0x0600ABD6 RID: 43990 RVA: 0x003CA854 File Offset: 0x003C8A54
		public BedOwnerType ForOwnerType
		{
			get
			{
				return this.forOwnerType;
			}
			set
			{
				if (value == this.forOwnerType || !this.def.building.bed_humanlike || this.ForHumanBabies)
				{
					return;
				}
				if (value == BedOwnerType.Slave && !ModLister.CheckIdeology("Slavery"))
				{
					return;
				}
				this.RemoveAllOwners(false);
				this.forOwnerType = value;
				this.Notify_ColorChanged();
				this.NotifyRoomBedTypeChanged();
			}
		}

		// Token: 0x17001E94 RID: 7828
		// (get) Token: 0x0600ABD7 RID: 43991 RVA: 0x003CA8B0 File Offset: 0x003C8AB0
		// (set) Token: 0x0600ABD8 RID: 43992 RVA: 0x003CA8B8 File Offset: 0x003C8AB8
		public bool Medical
		{
			get
			{
				return this.medicalInt;
			}
			set
			{
				if (value == this.medicalInt)
				{
					return;
				}
				if (value && !this.def.building.bed_canBeMedical)
				{
					return;
				}
				this.RemoveAllOwners(false);
				this.medicalInt = value;
				this.Notify_ColorChanged();
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
					this.NotifyRoomBedTypeChanged();
				}
				this.FacilityChanged();
			}
		}

		// Token: 0x17001E95 RID: 7829
		// (get) Token: 0x0600ABD9 RID: 43993 RVA: 0x003CA92D File Offset: 0x003C8B2D
		public bool AnyUnownedSleepingSlot
		{
			get
			{
				if (this.Medical)
				{
					Log.Warning("Tried to check for unowned sleeping slot on medical bed " + ((this != null) ? this.ToString() : null));
					return false;
				}
				return this.CompAssignableToPawn.HasFreeSlot;
			}
		}

		// Token: 0x17001E96 RID: 7830
		// (get) Token: 0x0600ABDA RID: 43994 RVA: 0x003CA960 File Offset: 0x003C8B60
		public int TotalSleepingSlots
		{
			get
			{
				if (this.Medical)
				{
					Log.Warning("Tried to check for total sleeping slots on medical bed " + ((this != null) ? this.ToString() : null));
					return 0;
				}
				return this.CompAssignableToPawn.TotalSlots;
			}
		}

		// Token: 0x17001E97 RID: 7831
		// (get) Token: 0x0600ABDB RID: 43995 RVA: 0x003CA994 File Offset: 0x003C8B94
		public bool AnyUnoccupiedSleepingSlot
		{
			get
			{
				for (int i = 0; i < this.SleepingSlotsCount; i++)
				{
					if (this.GetCurOccupant(i) == null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001E98 RID: 7832
		// (get) Token: 0x0600ABDC RID: 43996 RVA: 0x003CA9BE File Offset: 0x003C8BBE
		public IEnumerable<Pawn> CurOccupants
		{
			get
			{
				Building_Bed.<get_CurOccupants>d__38 <get_CurOccupants>d__ = new Building_Bed.<get_CurOccupants>d__38(-2);
				<get_CurOccupants>d__.<>4__this = this;
				return <get_CurOccupants>d__;
			}
		}

		// Token: 0x17001E99 RID: 7833
		// (get) Token: 0x0600ABDD RID: 43997 RVA: 0x003CA9D0 File Offset: 0x003C8BD0
		public bool AnyOccupants
		{
			get
			{
				for (int i = 0; i < this.SleepingSlotsCount; i++)
				{
					if (this.GetCurOccupant(i) != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001E9A RID: 7834
		// (get) Token: 0x0600ABDE RID: 43998 RVA: 0x003CA9FA File Offset: 0x003C8BFA
		public override Color DrawColor
		{
			get
			{
				if (this.def.MadeFromStuff)
				{
					return base.DrawColor;
				}
				return this.DrawColorTwo;
			}
		}

		// Token: 0x17001E9B RID: 7835
		// (get) Token: 0x0600ABDF RID: 43999 RVA: 0x003CAA18 File Offset: 0x003C8C18
		public override Color DrawColorTwo
		{
			get
			{
				if (this.def.building != null && !this.def.building.bed_UseSheetColor)
				{
					return base.DrawColorTwo;
				}
				bool medical = this.Medical;
				BedOwnerType bedOwnerType = this.forOwnerType;
				if (bedOwnerType != BedOwnerType.Prisoner)
				{
					if (bedOwnerType != BedOwnerType.Slave)
					{
						if (medical)
						{
							return Building_Bed.SheetColorMedical;
						}
						if (this.def == ThingDefOf.RoyalBed)
						{
							return Building_Bed.SheetColorRoyal;
						}
						return Building_Bed.SheetColorNormal;
					}
					else
					{
						if (!medical)
						{
							return Building_Bed.SheetColorForSlave;
						}
						return Building_Bed.SheetColorMedicalForSlave;
					}
				}
				else
				{
					if (!medical)
					{
						return Building_Bed.SheetColorForPrisoner;
					}
					return Building_Bed.SheetColorMedicalForPrisoner;
				}
			}
		}

		// Token: 0x17001E9C RID: 7836
		// (get) Token: 0x0600ABE0 RID: 44000 RVA: 0x003CAAA2 File Offset: 0x003C8CA2
		public int SleepingSlotsCount
		{
			get
			{
				return BedUtility.GetSleepingSlotsCount(this.def.size);
			}
		}

		// Token: 0x17001E9D RID: 7837
		// (get) Token: 0x0600ABE1 RID: 44001 RVA: 0x003CAAB4 File Offset: 0x003C8CB4
		private bool PlayerCanSeeOwners
		{
			get
			{
				return this.CompAssignableToPawn.PlayerCanSeeAssignments;
			}
		}

		// Token: 0x0600ABE2 RID: 44002 RVA: 0x003CAAC4 File Offset: 0x003C8CC4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!this.alreadySetDefaultMed)
			{
				this.alreadySetDefaultMed = true;
				if (this.def.building.bed_defaultMedical)
				{
					this.Medical = true;
				}
			}
			if (!respawningAfterLoad)
			{
				Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(base.Position);
				if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.Room.IsPrisonCell)
				{
					this.ForPrisoners = true;
				}
				District district = this.GetDistrict(RegionType.Set_Passable);
				if (district != null)
				{
					district.Notify_RoomShapeOrContainedBedsChanged();
					district.Room.Notify_RoomShapeChanged();
				}
			}
		}

		// Token: 0x0600ABE3 RID: 44003 RVA: 0x003CAB4C File Offset: 0x003C8D4C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				if (mode != DestroyMode.Vanish)
				{
					this.RemoveAllOwners(mode == DestroyMode.KillFinalize);
				}
				else if (InstallBlueprintUtility.ExistingBlueprintFor(this) == null)
				{
					foreach (Pawn pawn in this.OwnersForReading)
					{
						Messages.Message("MessageBedLostAssignment".Translate(this.def, pawn), new LookTargets(new TargetInfo[]
						{
							this,
							pawn
						}), MessageTypeDefOf.CautionInput, false);
					}
				}
				this.ForOwnerType = BedOwnerType.Colonist;
				this.Medical = false;
				this.alreadySetDefaultMed = false;
			}
			District district = this.GetDistrict(RegionType.Set_Passable);
			base.DeSpawn(mode);
			if (district != null)
			{
				district.Notify_RoomShapeOrContainedBedsChanged();
				district.Room.Notify_RoomShapeChanged();
			}
		}

		// Token: 0x0600ABE4 RID: 44004 RVA: 0x003CAC44 File Offset: 0x003C8E44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.medicalInt, "medical", false, false);
			Scribe_Values.Look<bool>(ref this.alreadySetDefaultMed, "alreadySetDefaultMed", false, false);
			Scribe_Values.Look<BedOwnerType>(ref this.forOwnerType, "forOwnerType", BedOwnerType.Colonist, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600ABE5 RID: 44005 RVA: 0x003CAC94 File Offset: 0x003C8E94
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Room room = this.GetRoom(RegionType.Set_All);
			if (room != null && Building_Bed.RoomCanBePrisonCell(room))
			{
				room.DrawFieldEdges();
			}
		}

		// Token: 0x0600ABE6 RID: 44006 RVA: 0x003CACC1 File Offset: 0x003C8EC1
		public static bool RoomCanBePrisonCell(Room r)
		{
			return r.ProperRoom && !r.IsHuge;
		}

		// Token: 0x0600ABE7 RID: 44007 RVA: 0x003CACD6 File Offset: 0x003C8ED6
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Bed.<GetGizmos>d__54 <GetGizmos>d__ = new Building_Bed.<GetGizmos>d__54(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600ABE8 RID: 44008 RVA: 0x003CACE8 File Offset: 0x003C8EE8
		public void SetBedOwnerTypeByInterface(BedOwnerType ownerType)
		{
			if (Building_Bed.lastBedOwnerSetChangeFrame == Time.frameCount)
			{
				return;
			}
			Building_Bed.lastBedOwnerSetChangeFrame = Time.frameCount;
			((this.ForOwnerType != ownerType) ? SoundDefOf.Checkbox_TurnedOn : SoundDefOf.Checkbox_TurnedOff).PlayOneShotOnCamera(null);
			List<Building_Bed> bedsToAffect = new List<Building_Bed>();
			foreach (Building_Bed building_Bed in Find.Selector.SelectedObjects.OfType<Building_Bed>())
			{
				if (building_Bed.ForOwnerType != ownerType)
				{
					Room room = building_Bed.GetRoom(RegionType.Set_All);
					if (room == null && ownerType != BedOwnerType.Prisoner)
					{
						if (!bedsToAffect.Contains(building_Bed))
						{
							bedsToAffect.Add(building_Bed);
						}
					}
					else
					{
						foreach (Building_Bed building_Bed2 in room.ContainedBeds)
						{
							if (building_Bed2.ForOwnerType != ownerType)
							{
								if (building_Bed2.ForOwnerType == BedOwnerType.Prisoner && !bedsToAffect.Contains(building_Bed2))
								{
									bedsToAffect.Add(building_Bed2);
								}
								else if (ownerType == BedOwnerType.Prisoner && Building_Bed.RoomCanBePrisonCell(room) && !bedsToAffect.Contains(building_Bed2))
								{
									bedsToAffect.Add(building_Bed2);
								}
								else if (building_Bed2 == building_Bed && !bedsToAffect.Contains(building_Bed2))
								{
									bedsToAffect.Add(building_Bed2);
								}
							}
						}
					}
				}
			}
			Action action = delegate()
			{
				List<District> list = new List<District>();
				List<Room> list2 = new List<Room>();
				foreach (Building_Bed building_Bed4 in bedsToAffect)
				{
					District district = building_Bed4.GetDistrict(RegionType.Set_Passable);
					Room room2 = district.Room;
					if (ownerType == BedOwnerType.Prisoner && room2.TouchesMapEdge)
					{
						building_Bed4.ForOwnerType = BedOwnerType.Colonist;
					}
					else
					{
						building_Bed4.ForOwnerType = ownerType;
					}
					if (!room2.TouchesMapEdge)
					{
						if (!list2.Contains(room2))
						{
							list2.Add(room2);
						}
						if (!list.Contains(district))
						{
							list.Add(district);
						}
					}
				}
				foreach (District district2 in list)
				{
					district2.Notify_RoomShapeOrContainedBedsChanged();
				}
				foreach (Room room3 in list2)
				{
					room3.Notify_RoomShapeChanged();
				}
			};
			if ((from b in bedsToAffect
			where b.OwnersForReading.Any((Pawn owner) => owner.RaceProps.Humanlike) && b != this
			select b).Count<Building_Bed>() == 0)
			{
				action();
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (!ModsConfig.IdeologyActive)
			{
				if (ownerType == BedOwnerType.Prisoner)
				{
					stringBuilder.Append("TurningOnPrisonerBedWarning".Translate());
				}
				else
				{
					stringBuilder.Append("TurningOffPrisonerBedWarning".Translate());
				}
			}
			else
			{
				stringBuilder.Append("ChangingOwnerTypeBedWarning".Translate());
			}
			stringBuilder.AppendLine();
			foreach (Building_Bed building_Bed3 in bedsToAffect)
			{
				if (ownerType != building_Bed3.ForOwnerType)
				{
					for (int i = 0; i < building_Bed3.OwnersForReading.Count; i++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(building_Bed3.OwnersForReading[i].LabelShort);
					}
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("AreYouSure".Translate());
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), action, false, null, WindowLayer.Dialog));
		}

		// Token: 0x0600ABE9 RID: 44009 RVA: 0x003CB03C File Offset: 0x003C923C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.def.building.bed_humanlike && this.def.building.bed_DisplayOwnerType && base.Faction == Faction.OfPlayer)
			{
				switch (this.ForOwnerType)
				{
				case BedOwnerType.Colonist:
					stringBuilder.AppendInNewLine("ForColonistUse".Translate());
					break;
				case BedOwnerType.Prisoner:
					stringBuilder.AppendInNewLine("ForPrisonerUse".Translate());
					break;
				case BedOwnerType.Slave:
					stringBuilder.AppendInNewLine("ForSlaveUse".Translate());
					break;
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unknown bed owner type: ");
					defaultInterpolatedStringHandler.AppendFormatted<BedOwnerType>(this.ForOwnerType);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					break;
				}
				}
			}
			if (this.Medical)
			{
				stringBuilder.AppendInNewLine("MedicalBed".Translate());
				if (base.Spawned)
				{
					stringBuilder.AppendInNewLine("RoomInfectionChanceFactor".Translate() + ": " + this.GetRoom(RegionType.Set_All).GetStat(RoomStatDefOf.InfectionChanceFactor).ToStringPercent());
				}
			}
			else if (this.PlayerCanSeeOwners && this.def.building.bed_DisplayOwnersInInspectString)
			{
				if (this.OwnersForReading.Count == 0)
				{
					stringBuilder.AppendInNewLine("Owner".Translate() + ": " + "Nobody".Translate());
				}
				else if (this.OwnersForReading.Count == 1)
				{
					stringBuilder.AppendInNewLine("Owner".Translate() + ": " + this.OwnersForReading[0].Label);
				}
				else
				{
					stringBuilder.AppendInNewLine("Owners".Translate() + ": ");
					bool flag = false;
					for (int i = 0; i < this.OwnersForReading.Count; i++)
					{
						if (flag)
						{
							stringBuilder.Append(", ");
						}
						flag = true;
						stringBuilder.Append(this.OwnersForReading[i].LabelShort);
					}
				}
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if (this.OwnersForReading.Count == 1 && ChildcareUtility.CanSuckle(this.OwnersForReading[0], out breastfeedFailReason))
				{
					Pawn p = this.OwnersForReading[0];
					float ambientTemperature = base.AmbientTemperature;
					if (!p.SafeTemperatureRange().IncludesEpsilon(ambientTemperature))
					{
						stringBuilder.AppendInNewLine("BedUnsafeTemperature".Translate().Colorize(ColoredText.WarningColor));
					}
					else if (!p.ComfortableTemperatureRange().IncludesEpsilon(ambientTemperature))
					{
						stringBuilder.AppendInNewLine("BedUncomfortableTemperature".Translate());
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600ABEA RID: 44010 RVA: 0x003CB33C File Offset: 0x003C953C
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			Building_Bed.<GetFloatMenuOptions>d__57 <GetFloatMenuOptions>d__ = new Building_Bed.<GetFloatMenuOptions>d__57(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__myPawn = myPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600ABEB RID: 44011 RVA: 0x003CB354 File Offset: 0x003C9554
		public FloatMenuOption GetBedRestFloatMenuOption(Pawn myPawn)
		{
			if (!myPawn.RaceProps.Humanlike || this.ForPrisoners || !this.Medical || myPawn.Drafted || base.Faction != Faction.OfPlayer || !RestUtility.CanUseBedEver(myPawn, this.def))
			{
				return null;
			}
			if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn))
			{
				if (myPawn.health.surgeryBills.AnyShouldDoNow && !WorkGiver_PatientGoToBedTreatment.AnyAvailableDoctorFor(myPawn))
				{
					return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NoDoctor".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotInjured".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				if (myPawn.IsSlaveOfColony && !this.ForSlaves)
				{
					return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotForSlaves".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				Action action = delegate()
				{
					if (!this.ForPrisoners && this.Medical && myPawn.CanReserveAndReach(this, PathEndMode.ClosestTouch, Danger.Deadly, this.SleepingSlotsCount, -1, null, true))
					{
						if (myPawn.CurJobDef == JobDefOf.LayDown && myPawn.CurJob.GetTarget(TargetIndex.A).Thing == this)
						{
							myPawn.CurJob.restUntilHealed = true;
						}
						else
						{
							Job job = JobMaker.MakeJob(JobDefOf.LayDown, this);
							job.restUntilHealed = true;
							myPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}
						myPawn.mindState.ResetLastDisturbanceTick();
					}
				};
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), myPawn, this, (this.AnyUnoccupiedSleepingSlot ? "ReservedBy" : "SomeoneElseSleeping").CapitalizeFirst(), null);
			}
		}

		// Token: 0x0600ABEC RID: 44012 RVA: 0x003CB54C File Offset: 0x003C974C
		public override void DrawGUIOverlay()
		{
			if (this.Medical)
			{
				return;
			}
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeOwners)
			{
				Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
				if (!this.OwnersForReading.Any<Pawn>())
				{
					GenMapUI.DrawThingLabel(this, "Unowned".Translate(), defaultThingLabelColor);
					return;
				}
				if (this.OwnersForReading.Count == 1)
				{
					Pawn pawn = this.OwnersForReading[0];
					if (pawn.InBed() && pawn.CurrentBed() == this)
					{
						return;
					}
					if (pawn.RaceProps.Animal && !Prefs.AnimalNameMode.ShouldDisplayAnimalName(pawn))
					{
						return;
					}
					GenMapUI.DrawThingLabel(this, pawn.LabelShort, defaultThingLabelColor);
					return;
				}
				else
				{
					for (int i = 0; i < this.OwnersForReading.Count; i++)
					{
						Pawn pawn2 = this.OwnersForReading[i];
						if (!pawn2.InBed() || this.OwnersForReading[i].CurrentBed() != this || !(pawn2.Position == this.GetSleepingSlotPos(i)))
						{
							if (pawn2.RaceProps.Animal && !Prefs.AnimalNameMode.ShouldDisplayAnimalName(pawn2))
							{
								return;
							}
							GenMapUI.DrawThingLabel(this.GetMultiOwnersLabelScreenPosFor(i), pawn2.LabelShort, defaultThingLabelColor);
						}
					}
				}
			}
		}

		// Token: 0x0600ABED RID: 44013 RVA: 0x003CB688 File Offset: 0x003C9888
		public Pawn GetCurOccupant(int slotIndex)
		{
			if (!base.Spawned)
			{
				return null;
			}
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			List<Thing> list = base.Map.thingGrid.ThingsListAt(sleepingSlotPos);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.CurJob != null && pawn.GetPosture().InBed())
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x0600ABEE RID: 44014 RVA: 0x003CB6F4 File Offset: 0x003C98F4
		public int GetCurOccupantSlotIndex(Pawn curOccupant)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetCurOccupant(i) == curOccupant)
				{
					return i;
				}
			}
			Log.Error("Could not find pawn " + ((curOccupant != null) ? curOccupant.ToString() : null) + " on any of sleeping slots.");
			return 0;
		}

		// Token: 0x0600ABEF RID: 44015 RVA: 0x003CB740 File Offset: 0x003C9940
		public Pawn GetCurOccupantAt(IntVec3 pos)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetSleepingSlotPos(i) == pos)
				{
					return this.GetCurOccupant(i);
				}
			}
			return null;
		}

		// Token: 0x0600ABF0 RID: 44016 RVA: 0x003CB776 File Offset: 0x003C9976
		public IntVec3 GetSleepingSlotPos(int index)
		{
			return BedUtility.GetSleepingSlotPos(index, base.Position, base.Rotation, this.def.size);
		}

		// Token: 0x0600ABF1 RID: 44017 RVA: 0x003CB795 File Offset: 0x003C9995
		public IntVec3 GetFootSlotPos(int index)
		{
			return BedUtility.GetFeetSlotPos(index, base.Position, base.Rotation, this.def.size);
		}

		// Token: 0x0600ABF2 RID: 44018 RVA: 0x003CB7B4 File Offset: 0x003C99B4
		public bool IsOwner(Pawn p)
		{
			int? num;
			return this.IsOwner(p, out num);
		}

		// Token: 0x0600ABF3 RID: 44019 RVA: 0x003CB7CC File Offset: 0x003C99CC
		public bool IsOwner(Pawn p, out int? assignedSleepingSlot)
		{
			int num = base.GetComp<CompAssignableToPawn>().AssignedPawnsForReading.IndexOf(p);
			if (num >= 0)
			{
				assignedSleepingSlot = new int?(num);
				return true;
			}
			assignedSleepingSlot = null;
			return false;
		}

		// Token: 0x0600ABF4 RID: 44020 RVA: 0x003CB808 File Offset: 0x003C9A08
		private void RemoveAllOwners(bool destroyed = false)
		{
			for (int i = this.OwnersForReading.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.OwnersForReading[i];
				pawn.ownership.UnclaimBed();
				string key = "MessageBedLostAssignment";
				if (destroyed)
				{
					key = "MessageBedDestroyed";
				}
				Messages.Message(key.Translate(this.def, pawn), new LookTargets(new TargetInfo[]
				{
					this,
					pawn
				}), MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0600ABF5 RID: 44021 RVA: 0x003CB8A4 File Offset: 0x003C9AA4
		private void NotifyRoomBedTypeChanged()
		{
			Room room = this.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				room.Notify_BedTypeChanged();
			}
		}

		// Token: 0x0600ABF6 RID: 44022 RVA: 0x003CB8C4 File Offset: 0x003C9AC4
		public void NotifyRoomAssignedPawnsChanged()
		{
			Room room = this.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				room.Notify_BedTypeChanged();
			}
		}

		// Token: 0x0600ABF7 RID: 44023 RVA: 0x003CB8E4 File Offset: 0x003C9AE4
		private void FacilityChanged()
		{
			CompFacility compFacility = this.TryGetComp<CompFacility>();
			CompAffectedByFacilities compAffectedByFacilities = this.TryGetComp<CompAffectedByFacilities>();
			if (compFacility != null)
			{
				compFacility.Notify_ThingChanged();
			}
			if (compAffectedByFacilities != null)
			{
				compAffectedByFacilities.Notify_ThingChanged();
			}
		}

		// Token: 0x0600ABF8 RID: 44024 RVA: 0x003CB914 File Offset: 0x003C9B14
		private Vector3 GetMultiOwnersLabelScreenPosFor(int slotIndex)
		{
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			Vector3 drawPos = this.DrawPos;
			if (base.Rotation.IsHorizontal)
			{
				drawPos.z = (float)sleepingSlotPos.z + 0.6f;
			}
			else
			{
				drawPos.x = (float)sleepingSlotPos.x + 0.5f;
				drawPos.z += -0.4f;
			}
			Vector2 v = drawPos.MapToUIPosition();
			if (!base.Rotation.IsHorizontal && this.SleepingSlotsCount == 2)
			{
				v = this.AdjustOwnerLabelPosToAvoidOverlapping(v, slotIndex);
			}
			return v;
		}

		// Token: 0x0600ABF9 RID: 44025 RVA: 0x003CB9B4 File Offset: 0x003C9BB4
		private Vector3 AdjustOwnerLabelPosToAvoidOverlapping(Vector3 screenPos, int slotIndex)
		{
			Text.Font = GameFont.Tiny;
			float num = Text.CalcSize(this.OwnersForReading[slotIndex].LabelShort).x + 1f;
			Vector2 vector = this.DrawPos.MapToUIPosition();
			float num2 = Mathf.Abs(screenPos.x - vector.x);
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			if (num > num2 * 2f)
			{
				float num3;
				if (slotIndex == 0)
				{
					num3 = (float)this.GetSleepingSlotPos(1).x;
				}
				else
				{
					num3 = (float)this.GetSleepingSlotPos(0).x;
				}
				if ((float)sleepingSlotPos.x < num3)
				{
					screenPos.x -= (num - num2 * 2f) / 2f;
				}
				else
				{
					screenPos.x += (num - num2 * 2f) / 2f;
				}
			}
			return screenPos;
		}

		// Token: 0x0600ABFA RID: 44026 RVA: 0x003CBA88 File Offset: 0x003C9C88
		private bool RemoveLeastDesirableInteractionCells(List<IntVec3> cells, Map map)
		{
			cells.RemoveAll(map, (Map innerMap, IntVec3 cell) => !cell.Standable(innerMap) || !TouchPathEndModeUtility.IsAdjacentOrInsideAndAllowedToTouch(cell, new LocalTargetInfo(this), map.pathing.Normal), false);
			return cells.RemoveAll_IfNotAll(map, delegate(Map innerMap, IntVec3 cell)
			{
				Building building = map.edificeGrid[cell];
				if (building == null)
				{
					return true;
				}
				ThingDef def = building.def;
				return !((def != null) ? new bool?(def.IsBed) : null).GetValueOrDefault();
			}) && cells.RemoveAll_IfNotAll(map, (Map innerMap, IntVec3 cell) => cell.GetDoor(map) == null);
		}

		// Token: 0x0600ABFB RID: 44027 RVA: 0x003CBAF8 File Offset: 0x003C9CF8
		public IntVec3? FindPreferredInteractionCell(IntVec3 occupantLocation, CellSearchPattern customSearchPattern = null)
		{
			CellRect cellRect = this.OccupiedRect();
			if (!cellRect.Contains(occupantLocation))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("interiorLocation ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(occupantLocation);
				defaultInterpolatedStringHandler.AppendLiteral(" is not within the bounds of this bed ");
				defaultInterpolatedStringHandler.AppendFormatted<CellRect>(cellRect);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return null;
			}
			Building_Bed.tmpOrderedInteractionCells.Clear();
			(customSearchPattern ?? Building_Bed.defaultBedInteractionCellsOrder).AddCellsToList(Building_Bed.tmpOrderedInteractionCells, this, cellRect, occupantLocation, base.Rotation);
			this.RemoveLeastDesirableInteractionCells(Building_Bed.tmpOrderedInteractionCells, base.Map);
			if (Building_Bed.tmpOrderedInteractionCells.Count == 0)
			{
				return null;
			}
			return new IntVec3?(Building_Bed.tmpOrderedInteractionCells[0]);
		}

		// Token: 0x17001E9E RID: 7838
		// (get) Token: 0x0600ABFC RID: 44028 RVA: 0x003CBBCC File Offset: 0x003C9DCC
		public override IntVec3 InteractionCell
		{
			get
			{
				IntVec3? intVec = this.FindPreferredInteractionCell(base.Position, null);
				if (intVec == null)
				{
					return base.InteractionCell;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x04006404 RID: 25604
		private BedOwnerType forOwnerType;

		// Token: 0x04006405 RID: 25605
		private bool medicalInt;

		// Token: 0x04006406 RID: 25606
		private bool alreadySetDefaultMed;

		// Token: 0x04006407 RID: 25607
		private static int lastBedOwnerSetChangeFrame = -1;

		// Token: 0x04006408 RID: 25608
		private static List<IntVec3> tmpOrderedInteractionCells = new List<IntVec3>(8);

		// Token: 0x04006409 RID: 25609
		private static readonly Color SheetColorNormal = new Color(0.6313726f, 0.8352941f, 0.7058824f);

		// Token: 0x0400640A RID: 25610
		private static readonly Color SheetColorRoyal = new Color(0.67058825f, 0.9137255f, 0.74509805f);

		// Token: 0x0400640B RID: 25611
		public static readonly Color SheetColorForPrisoner = new Color(1f, 0.7176471f, 0.12941177f);

		// Token: 0x0400640C RID: 25612
		private static readonly Color SheetColorMedical = new Color(0.3882353f, 0.62352943f, 0.8862745f);

		// Token: 0x0400640D RID: 25613
		private static readonly Color SheetColorMedicalForPrisoner = new Color(0.654902f, 0.3764706f, 0.15294118f);

		// Token: 0x0400640E RID: 25614
		private static readonly Color SheetColorForSlave = new Color32(252, 244, 3, byte.MaxValue);

		// Token: 0x0400640F RID: 25615
		private static readonly Color SheetColorMedicalForSlave = new Color32(153, 148, 0, byte.MaxValue);

		// Token: 0x04006410 RID: 25616
		private static readonly BedInteractionCellSearchPattern defaultBedInteractionCellsOrder = new BedInteractionCellSearchPattern();
	}
}
