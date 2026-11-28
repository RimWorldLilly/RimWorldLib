using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001979 RID: 6521
	public class CompSmokeCloudMaker : ThingComp
	{
		// Token: 0x17001C72 RID: 7282
		// (get) Token: 0x0600A3CE RID: 41934 RVA: 0x003A877E File Offset: 0x003A697E
		private CompProperties_SmokeCloudMaker Props
		{
			get
			{
				return (CompProperties_SmokeCloudMaker)this.props;
			}
		}

		// Token: 0x0600A3CF RID: 41935 RVA: 0x003A878C File Offset: 0x003A698C
		public override void CompTick()
		{
			if (!ModLister.CheckIdeology("Smoke cloud maker"))
			{
				return;
			}
			if (!this.parent.Spawned)
			{
				return;
			}
			CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			if ((compRefuelable == null || compRefuelable.HasFuel) & (compPowerTrader == null || compPowerTrader.PowerOn))
			{
				if (this.effecter == null)
				{
					this.effecter = this.Props.sourceStreamEffect.Spawn();
					this.effecter.Trigger(this.parent, this.parent, -1);
				}
				this.effecter.EffectTick(this.parent, this.parent);
				if (Rand.MTBEventOccurs(this.Props.fleckSpawnMTB, 1f, 1f))
				{
					CompSmokeCloudMaker.tmpValidCells.Clear();
					Room room = this.parent.GetRoom(RegionType.Set_All);
					CellRect cellRect = this.parent.OccupiedRect();
					foreach (IntVec3 intVec in cellRect.ExpandedBy(Mathf.FloorToInt(this.Props.cloudRadius + 1f)))
					{
						if (cellRect.ClosestCellTo(intVec).DistanceTo(intVec) <= this.Props.cloudRadius && intVec.GetRoom(this.parent.Map) == room)
						{
							CompSmokeCloudMaker.tmpValidCells.Add(intVec);
						}
					}
					if (CompSmokeCloudMaker.tmpValidCells.Count > 0)
					{
						Vector3 loc = CompSmokeCloudMaker.tmpValidCells.RandomElement<IntVec3>().ToVector3ShiftedWithAltitude(AltitudeLayer.MoteOverhead);
						this.parent.Map.flecks.CreateFleck(FleckMaker.GetDataStatic(loc, this.parent.Map, this.Props.cloudFleck, this.Props.fleckScale));
						return;
					}
				}
			}
			else
			{
				Effecter effecter = this.effecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effecter = null;
			}
		}

		// Token: 0x0600A3D0 RID: 41936 RVA: 0x003A89A4 File Offset: 0x003A6BA4
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x040060C6 RID: 24774
		private Effecter effecter;

		// Token: 0x040060C7 RID: 24775
		private static List<IntVec3> tmpValidCells = new List<IntVec3>();
	}
}
