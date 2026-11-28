using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001AAC RID: 6828
	public abstract class Building_Enterable : Building, IThingHolder, ISuspendableThingHolder, ISearchableContents
	{
		// Token: 0x17001ECD RID: 7885
		// (get) Token: 0x0600ACB1 RID: 44209 RVA: 0x003CE530 File Offset: 0x003CC730
		public bool Working
		{
			get
			{
				return this.startTick >= 0;
			}
		}

		// Token: 0x17001ECE RID: 7886
		// (get) Token: 0x0600ACB2 RID: 44210 RVA: 0x003CE53E File Offset: 0x003CC73E
		public virtual bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001ECF RID: 7887
		// (get) Token: 0x0600ACB3 RID: 44211 RVA: 0x003CE541 File Offset: 0x003CC741
		public bool AnyAcceptablePawns
		{
			get
			{
				return base.Spawned && base.Map.mapPawns.AllPawnsSpawned.Any((Pawn x) => this.CanAcceptPawn(x));
			}
		}

		// Token: 0x17001ED0 RID: 7888
		// (get) Token: 0x0600ACB4 RID: 44212 RVA: 0x003CE56E File Offset: 0x003CC76E
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x17001ED1 RID: 7889
		// (get) Token: 0x0600ACB5 RID: 44213 RVA: 0x003CE576 File Offset: 0x003CC776
		// (set) Token: 0x0600ACB6 RID: 44214 RVA: 0x003CE57E File Offset: 0x003CC77E
		public Pawn SelectedPawn
		{
			get
			{
				return this.selectedPawn;
			}
			set
			{
				this.selectedPawn = value;
			}
		}

		// Token: 0x0600ACB7 RID: 44215 RVA: 0x003CE587 File Offset: 0x003CC787
		public Building_Enterable()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600ACB8 RID: 44216
		public abstract AcceptanceReport CanAcceptPawn(Pawn p);

		// Token: 0x0600ACB9 RID: 44217
		public abstract void TryAcceptPawn(Pawn p);

		// Token: 0x17001ED2 RID: 7890
		// (get) Token: 0x0600ACBA RID: 44218
		public abstract Vector3 PawnDrawOffset { get; }

		// Token: 0x0600ACBB RID: 44219 RVA: 0x003CE5A2 File Offset: 0x003CC7A2
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600ACBC RID: 44220 RVA: 0x003CE5CB File Offset: 0x003CC7CB
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600ACBD RID: 44221 RVA: 0x003CE5D9 File Offset: 0x003CC7D9
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600ACBE RID: 44222 RVA: 0x003CE5E1 File Offset: 0x003CC7E1
		protected virtual void SelectPawn(Pawn pawn)
		{
			this.selectedPawn = pawn;
			if (pawn.IsPrisonerOfColony)
			{
				return;
			}
			if (pawn.Downed)
			{
				return;
			}
			pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.EnterBuilding, this), new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x0600ACBF RID: 44223 RVA: 0x003CE620 File Offset: 0x003CC820
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (!this.Working && this.selectedPawn != null && this.selectedPawn.Map == base.Map)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.selectedPawn.TrueCenter());
			}
		}

		// Token: 0x0600ACC0 RID: 44224 RVA: 0x003CE66C File Offset: 0x003CC86C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Enterable.<GetGizmos>d__24 <GetGizmos>d__ = new Building_Enterable.<GetGizmos>d__24(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600ACC1 RID: 44225 RVA: 0x003CE67C File Offset: 0x003CC87C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.startTick, "startTick", -1, false);
			Scribe_References.Look<Pawn>(ref this.selectedPawn, "selectedPawn", false);
		}

		// Token: 0x0400643B RID: 25659
		public ThingOwner innerContainer;

		// Token: 0x0400643C RID: 25660
		protected Pawn selectedPawn;

		// Token: 0x0400643D RID: 25661
		protected int startTick = -1;
	}
}
