using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020019EA RID: 6634
	public class CompGravshipThruster : CompGravshipFacility
	{
		// Token: 0x17001D41 RID: 7489
		// (get) Token: 0x0600A706 RID: 42758 RVA: 0x003B811C File Offset: 0x003B631C
		public CompBreakdownable Breakdownable
		{
			get
			{
				CompBreakdownable result;
				if ((result = this.breakdownableComp) == null)
				{
					result = (this.breakdownableComp = this.parent.GetComp<CompBreakdownable>());
				}
				return result;
			}
		}

		// Token: 0x17001D42 RID: 7490
		// (get) Token: 0x0600A707 RID: 42759 RVA: 0x003B8147 File Offset: 0x003B6347
		public new CompProperties_GravshipThruster Props
		{
			get
			{
				return (CompProperties_GravshipThruster)this.props;
			}
		}

		// Token: 0x17001D43 RID: 7491
		// (get) Token: 0x0600A708 RID: 42760 RVA: 0x003B8154 File Offset: 0x003B6354
		public bool Blocked
		{
			get
			{
				if (this.blockedBy == null && !this.blockedBySubstructure)
				{
					bool? flag = this.outdoors;
					bool flag2 = false;
					return flag.GetValueOrDefault() == flag2 & flag != null;
				}
				return true;
			}
		}

		// Token: 0x17001D44 RID: 7492
		// (get) Token: 0x0600A709 RID: 42761 RVA: 0x003B8190 File Offset: 0x003B6390
		public override bool CanBeActive
		{
			get
			{
				if (!base.CanBeActive)
				{
					return false;
				}
				if (this.Blocked)
				{
					return false;
				}
				if (this.Breakdownable.BrokenDown)
				{
					return false;
				}
				if (this.outdoors == null)
				{
					this.RecalculateOutdoors();
				}
				return !base.LinkedBuildings.NullOrEmpty<Thing>() && this.outdoors.GetValueOrDefault();
			}
		}

		// Token: 0x0600A70A RID: 42762 RVA: 0x003B81ED File Offset: 0x003B63ED
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateBlocked();
			this.RecalculateOutdoors();
		}

		// Token: 0x0600A70B RID: 42763 RVA: 0x003B8202 File Offset: 0x003B6402
		public override void PostMapInit()
		{
			this.RecalculateBlocked();
			this.RecalculateOutdoors();
			base.PostMapInit();
		}

		// Token: 0x0600A70C RID: 42764 RVA: 0x003B8216 File Offset: 0x003B6416
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateBlocked();
			this.RecalculateOutdoors();
		}

		// Token: 0x0600A70D RID: 42765 RVA: 0x003B822A File Offset: 0x003B642A
		public override bool CanLink()
		{
			if (this.Blocked)
			{
				return false;
			}
			if (this.outdoors == null)
			{
				this.RecalculateOutdoors();
			}
			return this.outdoors.GetValueOrDefault();
		}

		// Token: 0x0600A70E RID: 42766 RVA: 0x003B8254 File Offset: 0x003B6454
		private void RecalculateBlocked()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.lastCheckedBlockedTick = Find.TickManager.TicksGame;
			bool flag = this.blockedBy != null || this.blockedBySubstructure;
			CompGravshipThruster.IsBlocked(this.parent.def, this.parent.Map, this.parent.Position, this.parent.Rotation, out this.blockedBy, out this.blockedBySubstructure);
			if (flag != (this.blockedBy != null || this.blockedBySubstructure))
			{
				base.Notify_ThingChanged();
			}
		}

		// Token: 0x0600A70F RID: 42767 RVA: 0x003B82E8 File Offset: 0x003B64E8
		private void RecalculateOutdoors()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			bool valueOrDefault = this.outdoors.GetValueOrDefault();
			this.outdoors = new bool?(CompGravshipThruster.IsOutdoors(this.parent.def, this.parent.Map, this.parent.Position, this.parent.Rotation));
			bool? flag = this.outdoors;
			bool flag2 = valueOrDefault;
			if (!(flag.GetValueOrDefault() == flag2 & flag != null))
			{
				base.Notify_ThingChanged();
			}
		}

		// Token: 0x0600A710 RID: 42768 RVA: 0x003B836C File Offset: 0x003B656C
		public static bool IsBlocked(ThingDef thrusterDef, Map map, IntVec3 position, Rot4 rotation, out Thing blockedBy, out bool blockedBySubstructure)
		{
			blockedBy = null;
			blockedBySubstructure = false;
			thrusterDef.GetCompProperties<CompProperties_GravshipThruster>().GetExclusionZone(position, rotation, ref CompGravshipThruster.tmpExclusionCells);
			for (int i = 0; i < CompGravshipThruster.tmpExclusionCells.Count; i++)
			{
				IntVec3 c = CompGravshipThruster.tmpExclusionCells[i];
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
					if (terrainDef != null && terrainDef.IsSubstructure)
					{
						blockedBySubstructure = true;
					}
					foreach (Thing thing in map.thingGrid.ThingsListAt(c))
					{
						if (thing.def.blockWind && !(thing is Plant))
						{
							blockedBy = thing;
							return true;
						}
					}
				}
			}
			return blockedBySubstructure;
		}

		// Token: 0x0600A711 RID: 42769 RVA: 0x003B844C File Offset: 0x003B664C
		public static bool IsOutdoors(ThingDef thrusterDef, Map map, IntVec3 position, Rot4 rotation)
		{
			if (!map.regionAndRoomUpdater.Enabled)
			{
				return false;
			}
			foreach (IntVec3 intVec in GenAdj.CellsAdjacentAlongEdge(position, rotation, thrusterDef.size, rotation.Opposite.AsInt))
			{
				if (intVec.InBounds(map))
				{
					Room room = intVec.GetRoom(map);
					if (room != null && !room.UsesOutdoorTemperature)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600A712 RID: 42770 RVA: 0x003B84DC File Offset: 0x003B66DC
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Props.showMaxSimultaneous)
			{
				stringBuilder.Append(this.MaxConnectedString);
				stringBuilder.Append(": " + this.Props.maxSimultaneous.ToString());
			}
			string text = "";
			if (this.parent.Spawned && Find.TickManager.TicksGame != this.lastCheckedBlockedTick)
			{
				this.RecalculateBlocked();
				this.RecalculateOutdoors();
			}
			if (this.Breakdownable.BrokenDown)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "BrokenDown".Translate();
			}
			if (this.blockedBy != null)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "ThrusterBlockedBy".Translate(this.blockedBy);
			}
			else if (this.blockedBySubstructure)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "BlockedBySubstructure".Translate();
			}
			bool? flag = this.outdoors;
			bool flag2 = false;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "MustBeOutside".Translate();
			}
			if (text.NullOrEmpty() && this.parent.Spawned && (base.LinkedBuildings.NullOrEmpty<Thing>() || !this.CanBeActive))
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "ThrusterNotConnected".Translate();
			}
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLineIfNotEmpty().Append(("ThrusterNotFunctional".Translate() + ": " + text.CapitalizeFirst()).Colorize(ColorLibrary.RedReadable));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04006266 RID: 25190
		[Unsaved(false)]
		private Thing blockedBy;

		// Token: 0x04006267 RID: 25191
		[Unsaved(false)]
		private bool blockedBySubstructure;

		// Token: 0x04006268 RID: 25192
		[Unsaved(false)]
		private bool? outdoors;

		// Token: 0x04006269 RID: 25193
		[Unsaved(false)]
		private int lastCheckedBlockedTick = -99999;

		// Token: 0x0400626A RID: 25194
		[Unsaved(false)]
		private CompBreakdownable breakdownableComp;

		// Token: 0x0400626B RID: 25195
		private static List<IntVec3> tmpExclusionCells = new List<IntVec3>();
	}
}
