using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA7 RID: 6823
	public class Building_Crate : Building_Casket, IThingGlower
	{
		// Token: 0x17001EB1 RID: 7857
		// (get) Token: 0x0600AC5A RID: 44122 RVA: 0x003CCFDE File Offset: 0x003CB1DE
		public override int OpenTicks
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x0600AC5B RID: 44123 RVA: 0x003CCFE2 File Offset: 0x003CB1E2
		public bool ShouldBeLitNow()
		{
			return base.HasAnyContents;
		}

		// Token: 0x17001EB2 RID: 7858
		// (get) Token: 0x0600AC5C RID: 44124 RVA: 0x003CCFEA File Offset: 0x003CB1EA
		public override bool CanOpen
		{
			get
			{
				if (this.hackable == null)
				{
					this.hackable = base.GetComp<CompHackable>();
				}
				return (this.hackable == null || this.hackable.IsHacked) && base.CanOpen;
			}
		}

		// Token: 0x0600AC5D RID: 44125 RVA: 0x003CD01D File Offset: 0x003CB21D
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.def == ThingDefOf.AncientSecurityCrate && !ModLister.CheckIdeology("Ancient security crate"))
			{
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600AC5E RID: 44126 RVA: 0x003CD041 File Offset: 0x003CB241
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				base.BroadcastCompSignal("CrateContentsChanged");
				return true;
			}
			return false;
		}

		// Token: 0x0600AC5F RID: 44127 RVA: 0x003CD05C File Offset: 0x003CB25C
		public override void EjectContents()
		{
			this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, (IntVec3 c) => c.GetEdifice(base.Map) == null, true);
			this.contentsKnown = true;
			if (this.def.building.openingEffect != null)
			{
				Effecter effecter = this.def.building.openingEffect.Spawn();
				effecter.Trigger(new TargetInfo(base.Position, base.Map, false), null, -1);
				effecter.Cleanup();
			}
			base.BroadcastCompSignal("CrateContentsChanged");
		}

		// Token: 0x0600AC60 RID: 44128 RVA: 0x003CD0EE File Offset: 0x003CB2EE
		protected override void ReceiveCompSignal(string signal)
		{
			base.ReceiveCompSignal(signal);
			if (signal == "Hacked")
			{
				this.Open();
			}
		}

		// Token: 0x0600AC61 RID: 44129 RVA: 0x003CD10A File Offset: 0x003CB30A
		public override void Open()
		{
			this.contentsKnown = true;
			if (!this.CanOpen)
			{
				return;
			}
			base.Open();
		}

		// Token: 0x0600AC62 RID: 44130 RVA: 0x003CD122 File Offset: 0x003CB322
		public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			Building_Crate.<GetMultiSelectFloatMenuOptions>d__13 <GetMultiSelectFloatMenuOptions>d__ = new Building_Crate.<GetMultiSelectFloatMenuOptions>d__13(-2);
			<GetMultiSelectFloatMenuOptions>d__.<>4__this = this;
			<GetMultiSelectFloatMenuOptions>d__.<>3__selPawns = selPawns;
			return <GetMultiSelectFloatMenuOptions>d__;
		}

		// Token: 0x0600AC63 RID: 44131 RVA: 0x003CD139 File Offset: 0x003CB339
		private bool IsCapableOfOpening(Pawn pawn)
		{
			return !pawn.IsSubhuman && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}

		// Token: 0x04006420 RID: 25632
		public const string CrateContentsChanged = "CrateContentsChanged";

		// Token: 0x04006421 RID: 25633
		private CompHackable hackable;

		// Token: 0x04006422 RID: 25634
		private static List<Pawn> tmpAllowedPawns = new List<Pawn>();
	}
}
