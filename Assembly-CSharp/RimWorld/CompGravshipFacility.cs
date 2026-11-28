using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E8 RID: 6632
	public class CompGravshipFacility : CompFacility
	{
		// Token: 0x17001D3D RID: 7485
		// (get) Token: 0x0600A6F8 RID: 42744 RVA: 0x003B7CCF File Offset: 0x003B5ECF
		public new CompProperties_GravshipFacility Props
		{
			get
			{
				return (CompProperties_GravshipFacility)this.props;
			}
		}

		// Token: 0x17001D3E RID: 7486
		// (get) Token: 0x0600A6F9 RID: 42745 RVA: 0x003B7CDC File Offset: 0x003B5EDC
		protected override string MaxConnectedString
		{
			get
			{
				return "GravFacilityMaxSimultaneousConnections".Translate();
			}
		}

		// Token: 0x17001D3F RID: 7487
		// (get) Token: 0x0600A6FA RID: 42746 RVA: 0x003B7CF0 File Offset: 0x003B5EF0
		public override bool CanBeActive
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return false;
				}
				if (!base.CanBeActive)
				{
					return false;
				}
				if (Find.TickManager.TicksGame != this.lastGotEnginesTick)
				{
					this.tmpEngines.Clear();
					this.tmpEngines.AddRange(this.parent.Map.listerBuildings.AllBuildingsColonistOfClass<Building_GravEngine>());
					this.lastGotEnginesTick = Find.TickManager.TicksGame;
				}
				foreach (Building_GravEngine building_GravEngine in this.tmpEngines)
				{
					if (!this.parent.Position.InHorDistOf(building_GravEngine.Position, this.Props.maxDistance))
					{
						return false;
					}
					if (this.Props.onlyRequiresLooseConnection && building_GravEngine.LooselyConnectedToGravEngine(this.parent))
					{
						return true;
					}
					if (building_GravEngine.OnValidSubstructure(this.parent))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600A6FB RID: 42747 RVA: 0x003B7DFC File Offset: 0x003B5FFC
		public override void PostSpawnSetup(bool respawningAfterReload)
		{
			base.OnLinkAdded += this.Notify_LinkAdded;
			base.OnLinkRemoved += this.Notify_LinkRemoved;
			base.PostSpawnSetup(respawningAfterReload);
		}

		// Token: 0x0600A6FC RID: 42748 RVA: 0x003B7E2B File Offset: 0x003B602B
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			base.OnLinkAdded -= this.Notify_LinkAdded;
			base.OnLinkRemoved -= this.Notify_LinkRemoved;
		}

		// Token: 0x0600A6FD RID: 42749 RVA: 0x003B7E5C File Offset: 0x003B605C
		protected virtual void Notify_LinkAdded(CompFacility facility, Thing thing)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Building_GravEngine building_GravEngine = thing as Building_GravEngine;
			if (building_GravEngine != null && this.engine == null)
			{
				this.engine = building_GravEngine;
				this.engine.AddComponent(this);
			}
		}

		// Token: 0x0600A6FE RID: 42750 RVA: 0x003B7E98 File Offset: 0x003B6098
		protected virtual void Notify_LinkRemoved(CompFacility facility, Thing thing)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Building_GravEngine building_GravEngine = thing as Building_GravEngine;
			if (building_GravEngine != null && building_GravEngine == this.engine)
			{
				this.engine.RemoveComponent(this);
				this.engine = null;
			}
		}

		// Token: 0x0600A6FF RID: 42751 RVA: 0x003B7ED4 File Offset: 0x003B60D4
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder(base.CompInspectStringExtra());
			if (!this.parent.Spawned)
			{
				return stringBuilder.ToString();
			}
			if (this.parent.Spawned && (base.LinkedBuildings.NullOrEmpty<Thing>() || !this.CanBeActive))
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("NotConnectedToGravEngine".Translate().Colorize(ColorLibrary.RedReadable));
			}
			bool flag = false;
			foreach (Building_GravEngine building_GravEngine in this.parent.Map.listerBuildings.AllBuildingsColonistOfClass<Building_GravEngine>())
			{
				if (this.parent.Position.InHorDistOf(building_GravEngine.Position, this.Props.maxDistance))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				stringBuilder.AppendLineIfNotEmpty().Append("MessageMustBePlacedInRangeOfGravEngine".Translate().Colorize(ColorLibrary.RedReadable));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400625B RID: 25179
		[Unsaved(false)]
		public Building_GravEngine engine;

		// Token: 0x0400625C RID: 25180
		private List<Building_GravEngine> tmpEngines = new List<Building_GravEngine>();

		// Token: 0x0400625D RID: 25181
		private int lastGotEnginesTick = -1;
	}
}
