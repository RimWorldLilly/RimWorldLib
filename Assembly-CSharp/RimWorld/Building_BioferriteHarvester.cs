using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AA0 RID: 6816
	public class Building_BioferriteHarvester : Building
	{
		// Token: 0x17001EA0 RID: 7840
		// (get) Token: 0x0600AC0D RID: 44045 RVA: 0x003CC05C File Offset: 0x003CA25C
		public CompFacility FacilityComp
		{
			get
			{
				CompFacility result;
				if ((result = this.facilityComp) == null)
				{
					result = (this.facilityComp = base.GetComp<CompFacility>());
				}
				return result;
			}
		}

		// Token: 0x17001EA1 RID: 7841
		// (get) Token: 0x0600AC0E RID: 44046 RVA: 0x003CC084 File Offset: 0x003CA284
		public CompPowerTrader Power
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.powerComp) == null)
				{
					result = (this.powerComp = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17001EA2 RID: 7842
		// (get) Token: 0x0600AC0F RID: 44047 RVA: 0x003CC0AA File Offset: 0x003CA2AA
		public List<Thing> Platforms
		{
			get
			{
				return this.FacilityComp.LinkedBuildings;
			}
		}

		// Token: 0x17001EA3 RID: 7843
		// (get) Token: 0x0600AC10 RID: 44048 RVA: 0x003CC0B7 File Offset: 0x003CA2B7
		public bool ReadyForHauling
		{
			get
			{
				return Mathf.FloorToInt(this.containedBioferrite) >= 30;
			}
		}

		// Token: 0x17001EA4 RID: 7844
		// (get) Token: 0x0600AC11 RID: 44049 RVA: 0x003CC0CC File Offset: 0x003CA2CC
		private float BioferritePerDay
		{
			get
			{
				if (!this.Power.PowerOn)
				{
					return 0f;
				}
				float num = 0f;
				foreach (Thing thing in this.Platforms)
				{
					Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
					if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
					{
						num += CompProducesBioferrite.BioferritePerDay(building_HoldingPlatform.HeldPawn);
					}
				}
				return num;
			}
		}

		// Token: 0x17001EA5 RID: 7845
		// (get) Token: 0x0600AC12 RID: 44050 RVA: 0x003CC150 File Offset: 0x003CA350
		public CompCableConnection CableConnection
		{
			get
			{
				CompCableConnection result;
				if ((result = this.cableConnection) == null)
				{
					result = (this.cableConnection = base.GetComp<CompCableConnection>());
				}
				return result;
			}
		}

		// Token: 0x0600AC13 RID: 44051 RVA: 0x003CC176 File Offset: 0x003CA376
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600AC14 RID: 44052 RVA: 0x003CC18C File Offset: 0x003CA38C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.FacilityComp.OnLinkAdded -= this.OnLinkAdded;
			this.FacilityComp.OnLinkRemoved -= this.OnLinkRemoved;
			this.initalized = false;
			Sustainer sustainer = this.workingSustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.workingSustainer = null;
		}

		// Token: 0x0600AC15 RID: 44053 RVA: 0x003CC1F0 File Offset: 0x003CA3F0
		private void Initialize()
		{
			if (this.initalized)
			{
				return;
			}
			this.initalized = true;
			this.FacilityComp.OnLinkAdded += this.OnLinkAdded;
			this.FacilityComp.OnLinkRemoved += this.OnLinkRemoved;
			foreach (Thing thing in this.Platforms)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
				}
			}
			this.RebuildCables();
		}

		// Token: 0x0600AC16 RID: 44054 RVA: 0x003CC2A0 File Offset: 0x003CA4A0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (!this.initalized)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600AC17 RID: 44055 RVA: 0x003CC2B8 File Offset: 0x003CA4B8
		private void OnLinkRemoved(CompFacility facility, Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return;
			}
			building_HoldingPlatform.innerContainer.OnContentsChanged -= this.RebuildCables;
			this.RebuildCables();
		}

		// Token: 0x0600AC18 RID: 44056 RVA: 0x003CC2F0 File Offset: 0x003CA4F0
		private void OnLinkAdded(CompFacility facility, Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return;
			}
			building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
			this.RebuildCables();
		}

		// Token: 0x0600AC19 RID: 44057 RVA: 0x003CC328 File Offset: 0x003CA528
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				this.containedBioferrite = Mathf.Min(this.containedBioferrite + this.BioferritePerDay / 60000f * 250f, 60f);
			}
			if (this.IsWorking())
			{
				if (this.workingSustainer == null)
				{
					this.workingSustainer = SoundDefOf.BioferriteHarvester_Ambient.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.None));
				}
				this.workingSustainer.Maintain();
				return;
			}
			Sustainer sustainer = this.workingSustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.workingSustainer = null;
		}

		// Token: 0x0600AC1A RID: 44058 RVA: 0x003CC3C1 File Offset: 0x003CA5C1
		public override bool IsWorking()
		{
			return this.BioferritePerDay != 0f;
		}

		// Token: 0x0600AC1B RID: 44059 RVA: 0x003CC3D4 File Offset: 0x003CA5D4
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("BioferriteHarvesterContained".Translate());
			StringBuilder stringBuilder2 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 3);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.containedBioferrite, "F2");
			defaultInterpolatedStringHandler.AppendLiteral(" / ");
			defaultInterpolatedStringHandler.AppendFormatted(60f.ToString());
			defaultInterpolatedStringHandler.AppendLiteral(" (+");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.BioferritePerDay, "F2");
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			StringBuilder stringBuilder3 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(1, 1);
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("BioferriteHarvesterPerDay".Translate());
			defaultInterpolatedStringHandler2.AppendLiteral(")");
			stringBuilder3.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
			return stringBuilder.ToString();
		}

		// Token: 0x0600AC1C RID: 44060 RVA: 0x003CC4D4 File Offset: 0x003CA6D4
		public Thing TakeOutBioferrite()
		{
			int num = Mathf.FloorToInt(this.containedBioferrite);
			if (num == 0)
			{
				return null;
			}
			this.containedBioferrite -= (float)num;
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Bioferrite, null);
			thing.stackCount = num;
			return thing;
		}

		// Token: 0x0600AC1D RID: 44061 RVA: 0x003CC513 File Offset: 0x003CA713
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_BioferriteHarvester.<GetGizmos>d__31 <GetGizmos>d__ = new Building_BioferriteHarvester.<GetGizmos>d__31(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AC1E RID: 44062 RVA: 0x003CC524 File Offset: 0x003CA724
		private void EjectContents()
		{
			Thing thing = this.TakeOutBioferrite();
			if (thing == null)
			{
				return;
			}
			GenPlace.TryPlaceThing(thing, base.Position, base.Map, ThingPlaceMode.Near, null, null, null, 1);
		}

		// Token: 0x0600AC1F RID: 44063 RVA: 0x003CC55C File Offset: 0x003CA75C
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			this.RebuildCables();
		}

		// Token: 0x0600AC20 RID: 44064 RVA: 0x003CC56A File Offset: 0x003CA76A
		private void RebuildCables()
		{
			this.CableConnection.RebuildCables(this.Platforms, delegate(Thing thing)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				return building_HoldingPlatform != null && building_HoldingPlatform.Occupied;
			});
		}

		// Token: 0x0600AC21 RID: 44065 RVA: 0x003CC59C File Offset: 0x003CA79C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.containedBioferrite, "containedBioferrite", 0f, false);
			Scribe_Values.Look<bool>(ref this.unloadingEnabled, "unloadingEnabled", false, false);
		}

		// Token: 0x04006412 RID: 25618
		public float containedBioferrite;

		// Token: 0x04006413 RID: 25619
		public bool unloadingEnabled = true;

		// Token: 0x04006414 RID: 25620
		private bool initalized;

		// Token: 0x04006415 RID: 25621
		private const int HaulingThreshold = 30;

		// Token: 0x04006416 RID: 25622
		private const float MaxCapacity = 60f;

		// Token: 0x04006417 RID: 25623
		private CompFacility facilityComp;

		// Token: 0x04006418 RID: 25624
		private CompPowerTrader powerComp;

		// Token: 0x04006419 RID: 25625
		private CompCableConnection cableConnection;

		// Token: 0x0400641A RID: 25626
		private Sustainer workingSustainer;
	}
}
