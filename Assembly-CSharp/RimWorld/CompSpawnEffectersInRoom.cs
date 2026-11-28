using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200209C RID: 8348
	public class CompSpawnEffectersInRoom : ThingComp
	{
		// Token: 0x170025C9 RID: 9673
		// (get) Token: 0x0600D25E RID: 53854 RVA: 0x004D43AB File Offset: 0x004D25AB
		private CompProperties_SpawnEffectersInRoom Props
		{
			get
			{
				return (CompProperties_SpawnEffectersInRoom)this.props;
			}
		}

		// Token: 0x0600D25F RID: 53855 RVA: 0x004D43B8 File Offset: 0x004D25B8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.effecters.Clear();
		}

		// Token: 0x0600D260 RID: 53856 RVA: 0x004D43CC File Offset: 0x004D25CC
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			foreach (Effecter effecter in this.effecters.Values)
			{
				effecter.Cleanup();
			}
		}

		// Token: 0x0600D261 RID: 53857 RVA: 0x004D442C File Offset: 0x004D262C
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			Room room = this.parent.GetRoom(RegionType.Set_All);
			if (room == null || room.TouchesMapEdge)
			{
				return;
			}
			if (this.parent.IsRitualTarget())
			{
				foreach (IntVec3 cell in room.Cells)
				{
					if (cell.InHorDistOf(this.parent.Position, this.Props.radius))
					{
						this.CheckEffecter(cell);
					}
				}
			}
		}

		// Token: 0x0600D262 RID: 53858 RVA: 0x004D44D0 File Offset: 0x004D26D0
		private void CheckEffecter(IntVec3 cell)
		{
			if (this.effecters.ContainsKey(cell))
			{
				if (this.effecters[cell] == null)
				{
					this.effecters[cell] = this.Props.effecter.Spawn();
					this.effecters[cell].Trigger(new TargetInfo(cell, this.parent.Map, false), TargetInfo.Invalid, -1);
				}
			}
			else
			{
				this.effecters[cell] = this.Props.effecter.Spawn();
				this.effecters[cell].Trigger(new TargetInfo(cell, this.parent.Map, false), TargetInfo.Invalid, -1);
			}
			this.effecters[cell].EffectTick(new TargetInfo(cell, this.parent.Map, false), TargetInfo.Invalid);
		}

		// Token: 0x04008547 RID: 34119
		private readonly Dictionary<IntVec3, Effecter> effecters = new Dictionary<IntVec3, Effecter>();
	}
}
