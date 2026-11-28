using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181D RID: 6173
	public class CompAssignableToPawn : ThingComp
	{
		// Token: 0x17001A47 RID: 6727
		// (get) Token: 0x06009A5E RID: 39518 RVA: 0x0037B6FB File Offset: 0x003798FB
		public CompProperties_AssignableToPawn Props
		{
			get
			{
				return (CompProperties_AssignableToPawn)this.props;
			}
		}

		// Token: 0x17001A48 RID: 6728
		// (get) Token: 0x06009A5F RID: 39519 RVA: 0x0037B708 File Offset: 0x00379908
		public int MaxAssignedPawnsCount
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x17001A49 RID: 6729
		// (get) Token: 0x06009A60 RID: 39520 RVA: 0x0037B718 File Offset: 0x00379918
		public bool PlayerCanSeeAssignments
		{
			get
			{
				if (this.parent.Faction == Faction.OfPlayer)
				{
					return true;
				}
				for (int i = 0; i < this.assignedPawns.Count; i++)
				{
					if (this.assignedPawns[i].Faction == Faction.OfPlayer || this.assignedPawns[i].HostFaction == Faction.OfPlayer)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001A4A RID: 6730
		// (get) Token: 0x06009A61 RID: 39521 RVA: 0x0037B782 File Offset: 0x00379982
		public virtual IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return this.parent.Map.mapPawns.FreeColonists;
			}
		}

		// Token: 0x17001A4B RID: 6731
		// (get) Token: 0x06009A62 RID: 39522 RVA: 0x0037B7AC File Offset: 0x003799AC
		public List<Pawn> AssignedPawnsForReading
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x17001A4C RID: 6732
		// (get) Token: 0x06009A63 RID: 39523 RVA: 0x0037B7B4 File Offset: 0x003799B4
		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x17001A4D RID: 6733
		// (get) Token: 0x06009A64 RID: 39524 RVA: 0x0037B7BC File Offset: 0x003799BC
		public bool HasFreeSlot
		{
			get
			{
				return this.assignedPawns.Count < this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x17001A4E RID: 6734
		// (get) Token: 0x06009A65 RID: 39525 RVA: 0x0037B7D6 File Offset: 0x003799D6
		public int TotalSlots
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x06009A66 RID: 39526 RVA: 0x0037B7E3 File Offset: 0x003799E3
		protected virtual bool CanDrawOverlayForPawn(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06009A67 RID: 39527 RVA: 0x0037B7E8 File Offset: 0x003799E8
		public override void DrawGUIOverlay()
		{
			if (!this.Props.drawAssignmentOverlay || (!this.Props.drawUnownedAssignmentOverlay && !this.assignedPawns.Any<Pawn>()))
			{
				return;
			}
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeAssignments)
			{
				Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
				if (!this.assignedPawns.Any<Pawn>())
				{
					GenMapUI.DrawThingLabel(this.parent, "Unowned".Translate(), defaultThingLabelColor);
				}
				if (this.assignedPawns.Count == 1)
				{
					Pawn pawn = this.assignedPawns[0];
					if (!this.CanDrawOverlayForPawn(pawn))
					{
						return;
					}
					if (pawn.RaceProps.Animal && !Prefs.AnimalNameMode.ShouldDisplayAnimalName(pawn))
					{
						return;
					}
					GenMapUI.DrawThingLabel(this.parent, this.assignedPawns[0].LabelShort, defaultThingLabelColor);
				}
			}
		}

		// Token: 0x06009A68 RID: 39528 RVA: 0x0037B8C4 File Offset: 0x00379AC4
		protected virtual void SortAssignedPawns()
		{
			this.assignedPawns.RemoveAll((Pawn x) => x == null);
			this.assignedPawns.SortBy((Pawn x) => x.thingIDNumber);
		}

		// Token: 0x06009A69 RID: 39529 RVA: 0x0037B926 File Offset: 0x00379B26
		public virtual void ForceAddPawn(Pawn pawn)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Add(pawn);
			}
			this.SortAssignedPawns();
		}

		// Token: 0x06009A6A RID: 39530 RVA: 0x0037B948 File Offset: 0x00379B48
		public virtual void ForceRemovePawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Remove(pawn);
			}
			this.uninstalledAssignedPawns.Remove(pawn);
			this.SortAssignedPawns();
		}

		// Token: 0x06009A6B RID: 39531 RVA: 0x0037B978 File Offset: 0x00379B78
		public virtual AcceptanceReport CanAssignTo(Pawn pawn)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06009A6C RID: 39532 RVA: 0x0037B97F File Offset: 0x00379B7F
		public virtual bool IdeoligionForbids(Pawn pawn)
		{
			return false;
		}

		// Token: 0x06009A6D RID: 39533 RVA: 0x0037B982 File Offset: 0x00379B82
		public virtual void TryAssignPawn(Pawn pawn)
		{
			this.uninstalledAssignedPawns.Remove(pawn);
			if (this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Add(pawn);
			this.SortAssignedPawns();
		}

		// Token: 0x06009A6E RID: 39534 RVA: 0x0037B9B4 File Offset: 0x00379BB4
		public virtual void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Remove(pawn);
			if (uninstall && pawn != null && !this.uninstalledAssignedPawns.Contains(pawn))
			{
				this.uninstalledAssignedPawns.Add(pawn);
			}
			if (sort)
			{
				this.SortAssignedPawns();
			}
		}

		// Token: 0x06009A6F RID: 39535 RVA: 0x0037BA06 File Offset: 0x00379C06
		public virtual bool AssignedAnything(Pawn pawn)
		{
			return this.assignedPawns.Contains(pawn);
		}

		// Token: 0x06009A70 RID: 39536 RVA: 0x0037BA14 File Offset: 0x00379C14
		protected virtual bool ShouldShowAssignmentGizmo()
		{
			return this.parent.Faction == Faction.OfPlayer;
		}

		// Token: 0x06009A71 RID: 39537 RVA: 0x0037BA28 File Offset: 0x00379C28
		protected virtual string GetAssignmentGizmoLabel()
		{
			return "CommandThingSetOwnerLabel".Translate();
		}

		// Token: 0x06009A72 RID: 39538 RVA: 0x0037BA39 File Offset: 0x00379C39
		protected virtual string GetAssignmentGizmoDesc()
		{
			return "";
		}

		// Token: 0x06009A73 RID: 39539 RVA: 0x0037BA40 File Offset: 0x00379C40
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompAssignableToPawn.<CompGetGizmosExtra>d__31 <CompGetGizmosExtra>d__ = new CompAssignableToPawn.<CompGetGizmosExtra>d__31(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009A74 RID: 39540 RVA: 0x0037BA50 File Offset: 0x00379C50
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.assignedPawns, "assignedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.uninstalledAssignedPawns, "uninstalledAssignedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedPawns.RemoveAll((Pawn x) => x == null);
				this.uninstalledAssignedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06009A75 RID: 39541 RVA: 0x0037BAF0 File Offset: 0x00379CF0
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				for (int i = this.assignedPawns.Count - 1; i >= 0; i--)
				{
					this.TryUnassignPawn(this.assignedPawns[i], false, !this.parent.DestroyedOrNull());
				}
			}
		}

		// Token: 0x06009A76 RID: 39542 RVA: 0x0037BB3C File Offset: 0x00379D3C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			for (int i = this.uninstalledAssignedPawns.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.uninstalledAssignedPawns[i];
				if (this.CanSetUninstallAssignedPawn(pawn))
				{
					this.TryAssignPawn(pawn);
				}
			}
			this.uninstalledAssignedPawns.Clear();
		}

		// Token: 0x06009A77 RID: 39543 RVA: 0x0037BB90 File Offset: 0x00379D90
		public override void PostSwapMap()
		{
			for (int i = this.assignedPawns.Count - 1; i >= 0; i--)
			{
				if (this.assignedPawns[i].DestroyedOrNull() || !this.assignedPawns[i].SpawnedOrAnyParentSpawned)
				{
					this.TryUnassignPawn(this.assignedPawns[i], true, false);
				}
			}
		}

		// Token: 0x06009A78 RID: 39544 RVA: 0x0037BBEF File Offset: 0x00379DEF
		protected virtual bool CanSetUninstallAssignedPawn(Pawn pawn)
		{
			return false;
		}

		// Token: 0x04005BCB RID: 23499
		protected List<Pawn> assignedPawns = new List<Pawn>();

		// Token: 0x04005BCC RID: 23500
		protected List<Pawn> uninstalledAssignedPawns = new List<Pawn>();
	}
}
