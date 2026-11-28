using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB5 RID: 6837
	public class Building_Grave : Building_CorpseCasket, INotifyHauledTo
	{
		// Token: 0x17001EFA RID: 7930
		// (get) Token: 0x0600AD45 RID: 44357 RVA: 0x003D13E2 File Offset: 0x003CF5E2
		public Pawn AssignedPawn
		{
			get
			{
				if (this.CompAssignableToPawn == null || !this.CompAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
				{
					return null;
				}
				return this.CompAssignableToPawn.AssignedPawnsForReading[0];
			}
		}

		// Token: 0x17001EFB RID: 7931
		// (get) Token: 0x0600AD46 RID: 44358 RVA: 0x003D1411 File Offset: 0x003CF611
		public CompAssignableToPawn_Grave CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Grave>();
			}
		}

		// Token: 0x17001EFC RID: 7932
		// (get) Token: 0x0600AD47 RID: 44359 RVA: 0x003D141C File Offset: 0x003CF61C
		public override Graphic Graphic
		{
			get
			{
				if (!base.HasCorpse)
				{
					return base.Graphic;
				}
				if (this.def.building.fullGraveGraphicData == null)
				{
					return base.Graphic;
				}
				if (this.cachedGraphicFull == null)
				{
					this.cachedGraphicFull = this.def.building.fullGraveGraphicData.GraphicColoredFor(this);
				}
				return this.cachedGraphicFull;
			}
		}

		// Token: 0x17001EFD RID: 7933
		// (get) Token: 0x0600AD48 RID: 44360 RVA: 0x003D147B File Offset: 0x003CF67B
		public override bool StorageTabVisible
		{
			get
			{
				return base.StorageTabVisible && this.AssignedPawn == null;
			}
		}

		// Token: 0x0600AD49 RID: 44361 RVA: 0x003D1490 File Offset: 0x003CF690
		public override void EjectContents()
		{
			base.EjectContents();
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600AD4A RID: 44362 RVA: 0x003D14C0 File Offset: 0x003CF6C0
		public virtual void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			CompArt comp = base.GetComp<CompArt>();
			if (comp != null && !comp.Active && hauler.RaceProps.Humanlike)
			{
				comp.JustCreatedBy(hauler);
				comp.InitializeArt(base.Corpse.InnerPawn);
			}
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Things);
			hauler.records.Increment(RecordDefOf.CorpsesBuried);
			TaleDef buriedCorpse = TaleDefOf.BuriedCorpse;
			object[] array = new object[2];
			array[0] = hauler;
			int num = 1;
			Corpse corpse = base.Corpse;
			array[num] = ((corpse != null) ? corpse.InnerPawn : null);
			TaleRecorder.RecordTale(buriedCorpse, array);
		}

		// Token: 0x0600AD4B RID: 44363 RVA: 0x003D156C File Offset: 0x003CF76C
		public override bool Accepts(Thing thing)
		{
			if (!base.Accepts(thing))
			{
				return false;
			}
			if (base.HasCorpse)
			{
				return false;
			}
			if (this.AssignedPawn != null)
			{
				Corpse corpse = thing as Corpse;
				if (corpse == null)
				{
					return false;
				}
				if (corpse.InnerPawn != this.AssignedPawn)
				{
					return false;
				}
			}
			else if (!this.storageSettings.AllowedToAccept(thing))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600AD4C RID: 44364 RVA: 0x003D15C4 File Offset: 0x003CF7C4
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && corpse.InnerPawn.ownership != null && corpse.InnerPawn.ownership.AssignedGrave != this)
				{
					corpse.InnerPawn.ownership.UnclaimGrave();
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600AD4D RID: 44365 RVA: 0x003D1641 File Offset: 0x003CF841
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Grave.<GetGizmos>d__13 <GetGizmos>d__ = new Building_Grave.<GetGizmos>d__13(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AD4E RID: 44366 RVA: 0x003D1651 File Offset: 0x003CF851
		public override void Notify_ColorChanged()
		{
			base.Notify_ColorChanged();
			this.cachedGraphicFull = null;
		}

		// Token: 0x0600AD4F RID: 44367 RVA: 0x003D1660 File Offset: 0x003CF860
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (base.HasCorpse)
			{
				if (base.Tile.Valid)
				{
					string value = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(base.Corpse.timeOfDeath), Find.WorldGrid.LongLatOf(base.Tile));
					stringBuilder.AppendLine();
					stringBuilder.Append("DiedOn".Translate(value));
				}
			}
			else if (this.AssignedPawn != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AssignedColonist".Translate());
				stringBuilder.Append(": ");
				stringBuilder.Append(this.AssignedPawn.LabelCap);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04006477 RID: 25719
		private Graphic cachedGraphicFull;
	}
}
