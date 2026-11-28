using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193B RID: 6459
	public class CompPollutionPump : ThingComp
	{
		// Token: 0x17001BFF RID: 7167
		// (get) Token: 0x0600A1F8 RID: 41464 RVA: 0x0039E0C4 File Offset: 0x0039C2C4
		public CompProperties_PollutionPump Props
		{
			get
			{
				return (CompProperties_PollutionPump)this.props;
			}
		}

		// Token: 0x17001C00 RID: 7168
		// (get) Token: 0x0600A1F9 RID: 41465 RVA: 0x0039E0D4 File Offset: 0x0039C2D4
		private bool Active
		{
			get
			{
				return this.parent.Spawned && !this.disabledByArtificialBuildings && (this.compPower == null || this.compPower.PowerOn) && this.GetCellToUnpollute().IsValid;
			}
		}

		// Token: 0x0600A1FA RID: 41466 RVA: 0x0039E124 File Offset: 0x0039C324
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Pollution pump"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.ticksUntilPump = this.Props.intervalTicks;
			}
			this.compPower = this.parent.GetComp<CompPowerTrader>();
		}

		// Token: 0x0600A1FB RID: 41467 RVA: 0x0039E184 File Offset: 0x0039C384
		private IntVec3 GetCellToUnpollute()
		{
			int num = GenRadial.NumCellsInRadius(this.Props.radius);
			Map map = this.parent.Map;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && intVec.CanUnpollute(map))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0600A1FC RID: 41468 RVA: 0x0039E1F0 File Offset: 0x0039C3F0
		private void Pump()
		{
			IntVec3 cellToUnpollute = this.GetCellToUnpollute();
			if (cellToUnpollute.IsValid)
			{
				Map map = this.parent.Map;
				map.pollutionGrid.SetPolluted(cellToUnpollute, false, false);
				this.currentIntervalPumps++;
				EffecterDef pumpEffecterDef = this.Props.pumpEffecterDef;
				if (pumpEffecterDef != null)
				{
					pumpEffecterDef.Spawn(this.parent.Position, map, 1f).Cleanup();
				}
				if (this.Props.pumpsPerWastepack > 0 && this.currentIntervalPumps % this.Props.pumpsPerWastepack == 0)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Wastepack, null), this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, delegate(IntVec3 p)
					{
						Building firstBuilding = p.GetFirstBuilding(map);
						return firstBuilding == null || firstBuilding.IsClearableFreeBuilding;
					}, null, 1);
					this.currentIntervalPumps = 0;
				}
				MoteMaker.MakeAttachedOverlay(this.parent, ThingDefOf.Mote_PollutionPump, Vector3.zero, 1f, -1f);
			}
		}

		// Token: 0x0600A1FD RID: 41469 RVA: 0x0039E301 File Offset: 0x0039C501
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPollutionPump.<CompGetGizmosExtra>d__12 <CompGetGizmosExtra>d__ = new CompPollutionPump.<CompGetGizmosExtra>d__12(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A1FE RID: 41470 RVA: 0x0039E314 File Offset: 0x0039C514
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.Active)
			{
				stringBuilder.Append("AbsorbingPollutionNext".Translate(this.ticksUntilPump.ToStringTicksToPeriod(true, false, true, true, false)));
			}
			else if (this.disabledByArtificialBuildings)
			{
				stringBuilder.Append("CannotAbsorbPollution".Translate());
			}
			else
			{
				stringBuilder.Append("CanAbsorbPollution".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A1FF RID: 41471 RVA: 0x0039E3B8 File Offset: 0x0039C5B8
		public override void CompTick()
		{
			base.CompTick();
			if (this.Props.disabledByArtificialBuildings && this.parent.IsHashIntervalTick(60))
			{
				this.disabledByArtificialBuildings = (this.parent.Map.listerArtificialBuildingsForMeditation.GetForCell(this.parent.Position, this.Props.radius).Count > 0);
			}
			if (this.parent.IsHashIntervalTick(60) && this.Active)
			{
				this.ticksUntilPump -= 60;
				if (this.ticksUntilPump <= 0)
				{
					this.ticksUntilPump = this.Props.intervalTicks;
					this.Pump();
				}
			}
		}

		// Token: 0x0600A200 RID: 41472 RVA: 0x0039E466 File Offset: 0x0039C666
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksUntilPump, "ticksUntilPump", 0, false);
			Scribe_Values.Look<int>(ref this.currentIntervalPumps, "currentIntervalPumps", 0, false);
			Scribe_Values.Look<bool>(ref this.disabledByArtificialBuildings, "disabledByArtificialBuildings", false, false);
		}

		// Token: 0x04005FAD RID: 24493
		private const int CheckInterval = 60;

		// Token: 0x04005FAE RID: 24494
		private CompPowerTrader compPower;

		// Token: 0x04005FAF RID: 24495
		private int ticksUntilPump;

		// Token: 0x04005FB0 RID: 24496
		private int currentIntervalPumps;

		// Token: 0x04005FB1 RID: 24497
		[LoadAlias("disabledByArtificalBuildings")]
		private bool disabledByArtificialBuildings;
	}
}
