using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D69 RID: 7529
	public class ITab_ContentsTransporter : ITab_ContentsBase
	{
		// Token: 0x17002423 RID: 9251
		// (get) Token: 0x0600C318 RID: 49944 RVA: 0x00472312 File Offset: 0x00470512
		public override IList<Thing> container
		{
			get
			{
				return this.Transporter.innerContainer;
			}
		}

		// Token: 0x17002424 RID: 9252
		// (get) Token: 0x0600C319 RID: 49945 RVA: 0x0047231F File Offset: 0x0047051F
		public override bool UseDiscardMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002425 RID: 9253
		// (get) Token: 0x0600C31A RID: 49946 RVA: 0x00472322 File Offset: 0x00470522
		public CompTransporter Transporter
		{
			get
			{
				return base.SelThing.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x17002426 RID: 9254
		// (get) Token: 0x0600C31B RID: 49947 RVA: 0x00472330 File Offset: 0x00470530
		public override bool IsVisible
		{
			get
			{
				return (base.SelThing.Faction == null || base.SelThing.Faction == Faction.OfPlayer) && this.Transporter != null && (this.Transporter.LoadingInProgressOrReadyToLaunch || this.Transporter.innerContainer.Any);
			}
		}

		// Token: 0x17002427 RID: 9255
		// (get) Token: 0x0600C31C RID: 49948 RVA: 0x00472385 File Offset: 0x00470585
		public override IntVec3 DropOffset
		{
			get
			{
				if (this.Transporter.Shuttle != null)
				{
					return base.SelThing.def.interactionCellOffset.RotatedBy(base.SelThing.Rotation);
				}
				return base.DropOffset;
			}
		}

		// Token: 0x0600C31D RID: 49949 RVA: 0x004723BB File Offset: 0x004705BB
		public ITab_ContentsTransporter()
		{
			this.labelKey = "TabTransporterContents";
			this.containedItemsKey = "ContainedItems";
		}

		// Token: 0x0600C31E RID: 49950 RVA: 0x004723DC File Offset: 0x004705DC
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			CompTransporter transporter = this.Transporter;
			Rect rect = new Rect(0f, curY, (inRect.width - 10f) / 2f, inRect.height);
			Text.Font = GameFont.Small;
			bool flag = false;
			float a = 0f;
			Widgets.BeginGroup(rect);
			Widgets.ListSeparator(ref a, rect.width, "ItemsToLoad".Translate());
			if (transporter.leftToLoad != null)
			{
				for (int i = 0; i < transporter.leftToLoad.Count; i++)
				{
					TransferableOneWay t = transporter.leftToLoad[i];
					if (t.CountToTransfer > 0 && t.HasAnyThing)
					{
						flag = true;
						base.DoThingRow(t.ThingDef, t.CountToTransfer, t.things, rect.width, ref a, delegate(int x)
						{
							this.OnDropToLoadThing(t, x);
						});
					}
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref a, rect.width, null);
			}
			Widgets.EndGroup();
			Rect inRect2 = new Rect((inRect.width + 10f) / 2f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float b = 0f;
			base.DoItemsLists(inRect2, ref b);
			curY += Mathf.Max(a, b);
		}

		// Token: 0x0600C31F RID: 49951 RVA: 0x00472564 File Offset: 0x00470764
		protected override void OnDropThing(Thing t, int count)
		{
			base.OnDropThing(t, count);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				this.RemovePawnFromLoadLord(pawn);
			}
			this.Transporter.Notify_ThingRemoved(t);
		}

		// Token: 0x0600C320 RID: 49952 RVA: 0x00472598 File Offset: 0x00470798
		private void RemovePawnFromLoadLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_LoadAndEnterTransporters)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}

		// Token: 0x0600C321 RID: 49953 RVA: 0x004725D0 File Offset: 0x004707D0
		private void OnDropToLoadThing(TransferableOneWay t, int count)
		{
			t.ForceTo(t.CountToTransfer - count);
			this.EndJobForEveryoneHauling(t);
			foreach (Thing thing in t.things)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					this.RemovePawnFromLoadLord(pawn);
				}
			}
		}

		// Token: 0x0600C322 RID: 49954 RVA: 0x00472640 File Offset: 0x00470840
		private void EndJobForEveryoneHauling(TransferableOneWay t)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = base.SelThing.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
				{
					JobDriver_HaulToTransporter jobDriver_HaulToTransporter = (JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver;
					if (jobDriver_HaulToTransporter.Transporter == this.Transporter && jobDriver_HaulToTransporter.ThingToCarry != null && jobDriver_HaulToTransporter.ThingToCarry.def == t.ThingDef)
					{
						allPawnsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}
	}
}
