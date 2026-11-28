using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020018D3 RID: 6355
	public class CompLabyrinthDoor : CompInteractable, IJammedDoorDrawer
	{
		// Token: 0x17001B61 RID: 7009
		// (get) Token: 0x06009F54 RID: 40788 RVA: 0x00392107 File Offset: 0x00390307
		public Building_JammedDoor Door
		{
			get
			{
				return (Building_JammedDoor)this.parent;
			}
		}

		// Token: 0x17001B62 RID: 7010
		// (get) Token: 0x06009F55 RID: 40789 RVA: 0x00392114 File Offset: 0x00390314
		public new CompProperties_LabyrinthDoor Props
		{
			get
			{
				return (CompProperties_LabyrinthDoor)this.props;
			}
		}

		// Token: 0x06009F56 RID: 40790 RVA: 0x00392124 File Offset: 0x00390324
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Labyrinth door"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && Rand.Chance(this.Props.unlockedChance))
			{
				this.Door.UnlockDoor();
			}
		}

		// Token: 0x06009F57 RID: 40791 RVA: 0x00392171 File Offset: 0x00390371
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.OrderActivation(target.Pawn);
		}

		// Token: 0x06009F58 RID: 40792 RVA: 0x00392180 File Offset: 0x00390380
		public override string CompInspectStringExtra()
		{
			if (!this.Door.Jammed)
			{
				return null;
			}
			return "GrayDoorJammed".Translate();
		}

		// Token: 0x06009F59 RID: 40793 RVA: 0x003921A0 File Offset: 0x003903A0
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompLabyrinthDoor.<CompFloatMenuOptions>d__7 <CompFloatMenuOptions>d__ = new CompLabyrinthDoor.<CompFloatMenuOptions>d__7(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x06009F5A RID: 40794 RVA: 0x003921B7 File Offset: 0x003903B7
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompLabyrinthDoor.<CompGetGizmosExtra>d__8 <CompGetGizmosExtra>d__ = new CompLabyrinthDoor.<CompGetGizmosExtra>d__8(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009F5B RID: 40795 RVA: 0x003921C7 File Offset: 0x003903C7
		protected override void OnInteracted(Pawn caster)
		{
			this.Door.UnlockAndOpenDoor();
			if (caster.IsColonist)
			{
				this.parent.Map.fogGrid.FloodUnfogAdjacent(this.parent.Position, false);
			}
		}

		// Token: 0x06009F5C RID: 40796 RVA: 0x00392200 File Offset: 0x00390400
		private void OrderActivation(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent);
			job.count = 1;
			job.playerForced = true;
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06009F5D RID: 40797 RVA: 0x00392248 File Offset: 0x00390448
		public void DrawJammed(Rot4 rotation)
		{
			Vector3 drawPos = this.Door.DrawPos;
			drawPos.y = AltitudeLayer.DoorMoveable.AltitudeFor();
			this.Props.jammed.Graphic.Draw(drawPos, rotation, this.Door, 0f);
		}
	}
}
