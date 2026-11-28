using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A4F RID: 6735
	public class MinifiedTree : MinifiedThing, IThingHolderTickable, IThingHolder
	{
		// Token: 0x17001D98 RID: 7576
		// (get) Token: 0x0600A8BE RID: 43198 RVA: 0x003BEB12 File Offset: 0x003BCD12
		public bool ShouldTickContents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D99 RID: 7577
		// (get) Token: 0x0600A8BF RID: 43199 RVA: 0x003BEB15 File Offset: 0x003BCD15
		public int TicksTillDeath
		{
			get
			{
				return this.ticksTillDeath;
			}
		}

		// Token: 0x17001D9A RID: 7578
		// (get) Token: 0x0600A8C0 RID: 43200 RVA: 0x003BEB1D File Offset: 0x003BCD1D
		public Plant InnerTree
		{
			get
			{
				return (Plant)base.InnerThing;
			}
		}

		// Token: 0x17001D9B RID: 7579
		// (get) Token: 0x0600A8C1 RID: 43201 RVA: 0x003BEB2C File Offset: 0x003BCD2C
		public override Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.cachedGraphic = base.InnerThing.Graphic.ExtractInnerGraphicFor(base.InnerThing, null);
					Vector2 minifiedDrawSize = base.GetMinifiedDrawSize(base.InnerThing.def.size.ToVector2(), 1.1f);
					Vector2 newDrawSize = new Vector2(minifiedDrawSize.x / (float)base.InnerThing.def.size.x * this.cachedGraphic.drawSize.x, minifiedDrawSize.y / (float)base.InnerThing.def.size.z * this.cachedGraphic.drawSize.y);
					this.cachedGraphic = this.cachedGraphic.GetCopy(newDrawSize, ShaderTypeDefOf.Cutout.Shader);
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x0600A8C2 RID: 43202 RVA: 0x003BEC0F File Offset: 0x003BCE0F
		protected override Graphic LoadCrateFrontGraphic()
		{
			return GraphicDatabase.Get<Graphic_Single>("Things/Item/Minified/BurlapBag", ShaderDatabase.Cutout, base.GetMinifiedDrawSize(base.InnerThing.def.size.ToVector2(), 1.1f) * 1.16f, Color.white);
		}

		// Token: 0x0600A8C3 RID: 43203 RVA: 0x003BEC50 File Offset: 0x003BCE50
		private void RecordTreeDeath(LookTargets lookTargets)
		{
			if (base.InnerThing.def.plant.treeLoversCareIfChopped)
			{
				using (IEnumerator<Ideo> enumerator = Faction.OfPlayer.ideos.AllIdeos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.WarnPlayerOnDesignateChopTree)
						{
							Messages.Message("MessageMinifiedTreeDied".Translate(), lookTargets, MessageTypeDefOf.NegativeEvent, true);
							break;
						}
					}
				}
			}
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.MinifiedTreeDied, new SignalArgs(base.InnerThing.Named(HistoryEventArgsNames.Victim))), true);
		}

		// Token: 0x0600A8C4 RID: 43204 RVA: 0x003BED04 File Offset: 0x003BCF04
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (base.InnerThing == null)
			{
				base.Destroy(mode);
				return;
			}
			Caravan anyParent = ThingOwnerUtility.GetAnyParent<Caravan>(this);
			ActiveTransporterInfo activeTransporterInfo = base.ParentHolder as ActiveTransporterInfo;
			this.RecordTreeDeath((anyParent == null) ? new LookTargets(base.PositionHeld, base.MapHeld) : anyParent);
			ThingDef harvestedThingDef = base.InnerThing.def.plant.harvestedThingDef;
			int i = (int)((float)((Plant)base.InnerThing).YieldNow() * 0.5f);
			List<Thing> list = new List<Thing>();
			while (i > 0)
			{
				int num = Mathf.Min(i, harvestedThingDef.stackLimit);
				Thing thing = ThingMaker.MakeThing(harvestedThingDef, null);
				thing.stackCount = num;
				list.Add(thing);
				i -= num;
			}
			IntVec3 center = (anyParent == null) ? base.PositionHeld : IntVec3.Invalid;
			Map map = (anyParent == null) ? base.MapHeld : null;
			if (base.ParentHolder != null)
			{
				base.ParentHolder.GetDirectlyHeldThings().Remove(this);
			}
			base.Destroy(mode);
			if (anyParent != null)
			{
				using (List<Thing>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing thing2 = enumerator.Current;
						anyParent.AddPawnOrItem(thing2, true);
					}
					return;
				}
			}
			if (activeTransporterInfo != null)
			{
				using (List<Thing>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing item = enumerator.Current;
						activeTransporterInfo.innerContainer.TryAdd(item, true);
					}
					return;
				}
			}
			if (map != null)
			{
				foreach (Thing thing3 in list)
				{
					GenPlace.TryPlaceThing(thing3, center, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
		}

		// Token: 0x0600A8C5 RID: 43205 RVA: 0x003BEEEC File Offset: 0x003BD0EC
		public override void PostMake()
		{
			base.PostMake();
			this.ticksTillDeath = 420000;
		}

		// Token: 0x0600A8C6 RID: 43206 RVA: 0x003BEEFF File Offset: 0x003BD0FF
		protected override void Tick()
		{
			base.Tick();
			this.ticksTillDeath--;
			if (this.ticksTillDeath <= 0)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600A8C7 RID: 43207 RVA: 0x003BEF25 File Offset: 0x003BD125
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MinifiedTree.<GetGizmos>d__16 <GetGizmos>d__ = new MinifiedTree.<GetGizmos>d__16(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A8C8 RID: 43208 RVA: 0x003BEF35 File Offset: 0x003BD135
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			this.RecordTreeDeath(null);
		}

		// Token: 0x0600A8C9 RID: 43209 RVA: 0x003BEF44 File Offset: 0x003BD144
		public override string GetInspectString()
		{
			return "MinifiedTreeWillDieIn".Translate(this.ticksTillDeath.ToStringTicksToPeriod(true, false, true, true, false).Named("time"));
		}

		// Token: 0x0600A8CA RID: 43210 RVA: 0x003BEF6F File Offset: 0x003BD16F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksTillDeath, "ticksTillDeath", 0, false);
		}

		// Token: 0x04006309 RID: 25353
		private int ticksTillDeath;

		// Token: 0x0400630A RID: 25354
		public const int InitialTicksTillDeath = 420000;

		// Token: 0x0400630B RID: 25355
		public const float DyingYieldPercentage = 0.5f;
	}
}
