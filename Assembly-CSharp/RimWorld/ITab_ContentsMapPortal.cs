using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001D67 RID: 7527
	public class ITab_ContentsMapPortal : ITab_ContentsBase
	{
		// Token: 0x1700241A RID: 9242
		// (get) Token: 0x0600C303 RID: 49923 RVA: 0x00471D1B File Offset: 0x0046FF1B
		public override IList<Thing> container
		{
			get
			{
				return this.tmpContainer;
			}
		}

		// Token: 0x1700241B RID: 9243
		// (get) Token: 0x0600C304 RID: 49924 RVA: 0x00471D23 File Offset: 0x0046FF23
		public override bool UseDiscardMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700241C RID: 9244
		// (get) Token: 0x0600C305 RID: 49925 RVA: 0x00471D26 File Offset: 0x0046FF26
		public MapPortal Portal
		{
			get
			{
				return base.SelThing as MapPortal;
			}
		}

		// Token: 0x1700241D RID: 9245
		// (get) Token: 0x0600C306 RID: 49926 RVA: 0x00471D33 File Offset: 0x0046FF33
		public override bool IsVisible
		{
			get
			{
				return this.Portal != null && this.Portal.LoadInProgress;
			}
		}

		// Token: 0x1700241E RID: 9246
		// (get) Token: 0x0600C307 RID: 49927 RVA: 0x00471D4A File Offset: 0x0046FF4A
		public override IntVec3 DropOffset
		{
			get
			{
				return IntVec3.Zero;
			}
		}

		// Token: 0x0600C308 RID: 49928 RVA: 0x00471D51 File Offset: 0x0046FF51
		public ITab_ContentsMapPortal()
		{
			this.labelKey = "TabMapPortalContents";
			this.containedItemsKey = "";
		}

		// Token: 0x0600C309 RID: 49929 RVA: 0x00471D7C File Offset: 0x0046FF7C
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			MapPortal portal = this.Portal;
			Text.Font = GameFont.Small;
			bool flag = false;
			float num = 0f;
			Widgets.BeginGroup(inRect);
			Rect viewRect = inRect;
			viewRect.y = 0f;
			viewRect.height = this.lastHeight;
			if (this.lastHeight > inRect.height)
			{
				viewRect.width -= 16f;
			}
			Widgets.BeginScrollView(inRect, ref this.scroll, viewRect, true);
			Widgets.ListSeparator(ref num, inRect.width, this.Portal.EnteringString);
			if (portal.leftToLoad != null)
			{
				for (int i = 0; i < portal.leftToLoad.Count; i++)
				{
					TransferableOneWay t = portal.leftToLoad[i];
					if (t.CountToTransfer > 0 && t.HasAnyThing)
					{
						flag = true;
						base.DoThingRow(t.ThingDef, t.CountToTransfer, t.things, viewRect.width, ref num, delegate(int x)
						{
							this.OnDropToLoadThing(t, x);
						});
					}
				}
			}
			this.lastHeight = num;
			Widgets.EndScrollView();
			if (!flag)
			{
				Widgets.NoneLabel(ref num, inRect.width, null);
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600C30A RID: 49930 RVA: 0x00471ED8 File Offset: 0x004700D8
		protected override void OnDropThing(Thing t, int count)
		{
			base.OnDropThing(t, count);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				this.RemovePawnFromLoadLord(pawn);
			}
		}

		// Token: 0x0600C30B RID: 49931 RVA: 0x00471F00 File Offset: 0x00470100
		private void RemovePawnFromLoadLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_LoadAndEnterTransporters)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}

		// Token: 0x0600C30C RID: 49932 RVA: 0x00471F38 File Offset: 0x00470138
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

		// Token: 0x0600C30D RID: 49933 RVA: 0x00471FA8 File Offset: 0x004701A8
		private void EndJobForEveryoneHauling(TransferableOneWay t)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = base.SelThing.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
				{
					JobDriver_HaulToPortal jobDriver_HaulToPortal = (JobDriver_HaulToPortal)allPawnsSpawned[i].jobs.curDriver;
					if (jobDriver_HaulToPortal.MapPortal == this.Portal && jobDriver_HaulToPortal.ThingToCarry != null && jobDriver_HaulToPortal.ThingToCarry.def == t.ThingDef)
					{
						allPawnsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x04006F7D RID: 28541
		private List<Thing> tmpContainer = new List<Thing>();

		// Token: 0x04006F7E RID: 28542
		private Vector2 scroll;

		// Token: 0x04006F7F RID: 28543
		private float lastHeight;
	}
}
