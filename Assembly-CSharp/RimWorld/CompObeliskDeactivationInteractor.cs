using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200191B RID: 6427
	public class CompObeliskDeactivationInteractor : CompInteractable
	{
		// Token: 0x17001BD3 RID: 7123
		// (get) Token: 0x0600A12C RID: 41260 RVA: 0x00399B98 File Offset: 0x00397D98
		private CompObelisk ObeliskComp
		{
			get
			{
				CompObelisk result;
				if ((result = this.obeliskComp) == null)
				{
					result = (this.obeliskComp = this.parent.GetComp<CompObelisk>());
				}
				return result;
			}
		}

		// Token: 0x17001BD4 RID: 7124
		// (get) Token: 0x0600A12D RID: 41261 RVA: 0x00399BC3 File Offset: 0x00397DC3
		private new CompProperties_ObeliskDeactivationInteractor Props
		{
			get
			{
				return (CompProperties_ObeliskDeactivationInteractor)this.props;
			}
		}

		// Token: 0x17001BD5 RID: 7125
		// (get) Token: 0x0600A12E RID: 41262 RVA: 0x00399BD0 File Offset: 0x00397DD0
		public override string ExposeKey
		{
			get
			{
				return "Deactivation";
			}
		}

		// Token: 0x17001BD6 RID: 7126
		// (get) Token: 0x0600A12F RID: 41263 RVA: 0x00399BD7 File Offset: 0x00397DD7
		public override bool CanCooldown
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600A130 RID: 41264 RVA: 0x00399BDC File Offset: 0x00397DDC
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (!this.ObeliskComp.StudyFinished || this.ObeliskComp.Activated || this.ObeliskComp.ActivityComp.Deactivated)
			{
				return false;
			}
			if (activateBy != null)
			{
				if (checkOptionalItems && !activateBy.HasReserved(ThingDefOf.Shard) && !ReservationUtility.ExistsUnreservedAmountOfDef(this.parent.MapHeld, ThingDefOf.Shard, Faction.OfPlayer, this.Props.shardsRequired, (Thing t) => activateBy.CanReserveAndReach(t, PathEndMode.Touch, Danger.None, 1, -1, null, false)))
				{
					return "ObeliskDeactivateMissingShards".Translate(this.Props.shardsRequired);
				}
			}
			else if (checkOptionalItems && !ReservationUtility.ExistsUnreservedAmountOfDef(this.parent.MapHeld, ThingDefOf.Shard, Faction.OfPlayer, this.Props.shardsRequired, null))
			{
				return "ObeliskDeactivateMissingShards".Translate(this.Props.shardsRequired);
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x0600A131 RID: 41265 RVA: 0x00399CF7 File Offset: 0x00397EF7
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompObeliskDeactivationInteractor.<CompGetGizmosExtra>d__10 <CompGetGizmosExtra>d__ = new CompObeliskDeactivationInteractor.<CompGetGizmosExtra>d__10(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A132 RID: 41266 RVA: 0x00399D07 File Offset: 0x00397F07
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompObeliskDeactivationInteractor.<CompFloatMenuOptions>d__11 <CompFloatMenuOptions>d__ = new CompObeliskDeactivationInteractor.<CompFloatMenuOptions>d__11(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600A133 RID: 41267 RVA: 0x00399D1E File Offset: 0x00397F1E
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (base.ValidateTarget(target, false))
			{
				this.OrderDeactivation(target.Pawn);
			}
		}

		// Token: 0x0600A134 RID: 41268 RVA: 0x00399D38 File Offset: 0x00397F38
		private void OrderDeactivation(Pawn pawn)
		{
			List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, ThingDefOf.Shard, this.Props.shardsRequired);
			if (list.NullOrEmpty<Thing>())
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent, list[0]);
			job.targetQueueB = (from i in list.Skip(1)
			select new LocalTargetInfo(i)).ToList<LocalTargetInfo>();
			job.count = this.Props.shardsRequired;
			job.playerForced = true;
			job.interactableIndex = 1;
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x0600A135 RID: 41269 RVA: 0x00399DF0 File Offset: 0x00397FF0
		protected override void OnInteracted(Pawn caster)
		{
			if (this.obeliskComp.Activated)
			{
				return;
			}
			this.obeliskComp.ActivityComp.Deactivate();
			CompObeliskTriggerInteractor comp = this.parent.GetComp<CompObeliskTriggerInteractor>();
			if (comp == null)
			{
				return;
			}
			comp.ResetCooldown(true);
		}

		// Token: 0x04005F32 RID: 24370
		private CompObelisk obeliskComp;
	}
}
