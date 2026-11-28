using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A58 RID: 6744
	public class Building_PassengerShuttle : Building, IRenameable
	{
		// Token: 0x17001DB1 RID: 7601
		// (get) Token: 0x0600A92D RID: 43309 RVA: 0x003C0114 File Offset: 0x003BE314
		public CompRefuelable RefuelableComp
		{
			get
			{
				CompRefuelable result;
				if ((result = this.cachedRefuelableComp) == null)
				{
					result = (this.cachedRefuelableComp = base.GetComp<CompRefuelable>());
				}
				return result;
			}
		}

		// Token: 0x17001DB2 RID: 7602
		// (get) Token: 0x0600A92E RID: 43310 RVA: 0x003C013C File Offset: 0x003BE33C
		public CompLaunchable LaunchableComp
		{
			get
			{
				CompLaunchable result;
				if ((result = this.cachedLaunchableComp) == null)
				{
					result = (this.cachedLaunchableComp = base.GetComp<CompLaunchable>());
				}
				return result;
			}
		}

		// Token: 0x17001DB3 RID: 7603
		// (get) Token: 0x0600A92F RID: 43311 RVA: 0x003C0164 File Offset: 0x003BE364
		public CompTransporter TransporterComp
		{
			get
			{
				CompTransporter result;
				if ((result = this.cachedTransporterComp) == null)
				{
					result = (this.cachedTransporterComp = base.GetComp<CompTransporter>());
				}
				return result;
			}
		}

		// Token: 0x17001DB4 RID: 7604
		// (get) Token: 0x0600A930 RID: 43312 RVA: 0x003C018C File Offset: 0x003BE38C
		public CompShuttle ShuttleComp
		{
			get
			{
				CompShuttle result;
				if ((result = this.cachedShuttleComp) == null)
				{
					result = (this.cachedShuttleComp = base.GetComp<CompShuttle>());
				}
				return result;
			}
		}

		// Token: 0x17001DB5 RID: 7605
		// (get) Token: 0x0600A931 RID: 43313 RVA: 0x003C01B2 File Offset: 0x003BE3B2
		// (set) Token: 0x0600A932 RID: 43314 RVA: 0x003C01C4 File Offset: 0x003BE3C4
		public string RenamableLabel
		{
			get
			{
				return this.shuttleName ?? this.BaseLabel;
			}
			set
			{
				this.shuttleName = value;
			}
		}

		// Token: 0x17001DB6 RID: 7606
		// (get) Token: 0x0600A933 RID: 43315 RVA: 0x003C01CD File Offset: 0x003BE3CD
		public string BaseLabel
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17001DB7 RID: 7607
		// (get) Token: 0x0600A934 RID: 43316 RVA: 0x003C01DF File Offset: 0x003BE3DF
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17001DB8 RID: 7608
		// (get) Token: 0x0600A935 RID: 43317 RVA: 0x003C01E7 File Offset: 0x003BE3E7
		public override string Label
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17001DB9 RID: 7609
		// (get) Token: 0x0600A936 RID: 43318 RVA: 0x003C01EF File Offset: 0x003BE3EF
		public float FuelLevel
		{
			get
			{
				return this.RefuelableComp.Fuel;
			}
		}

		// Token: 0x17001DBA RID: 7610
		// (get) Token: 0x0600A937 RID: 43319 RVA: 0x003C01FC File Offset: 0x003BE3FC
		public float MaxFuelLevel
		{
			get
			{
				return this.RefuelableComp.Props.fuelCapacity;
			}
		}

		// Token: 0x0600A938 RID: 43320 RVA: 0x003C020E File Offset: 0x003BE40E
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ShuttleComp.shipParent.Start();
			}
		}

		// Token: 0x0600A939 RID: 43321 RVA: 0x003C022B File Offset: 0x003BE42B
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_PassengerShuttle.<GetGizmos>d__28 <GetGizmos>d__ = new Building_PassengerShuttle.<GetGizmos>d__28(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A93A RID: 43322 RVA: 0x003C023B File Offset: 0x003BE43B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.shuttleName, "shuttleName", null, false);
		}

		// Token: 0x0600A93B RID: 43323 RVA: 0x003C0258 File Offset: 0x003BE458
		private float FuelInShuttle()
		{
			float num = 0f;
			foreach (Thing thing in ((IEnumerable<Thing>)this.TransporterComp.innerContainer))
			{
				if (this.RefuelableComp.Props.fuelFilter.Allows(thing))
				{
					num += (float)thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x0600A93C RID: 43324 RVA: 0x003C02CC File Offset: 0x003BE4CC
		private void ConsumeFuelFromInventory(int fuelAmount)
		{
			Building_PassengerShuttle.tmpContainedThings.Clear();
			Building_PassengerShuttle.tmpContainedThings.AddRange(this.TransporterComp.innerContainer);
			int num = fuelAmount;
			for (int i = Building_PassengerShuttle.tmpContainedThings.Count - 1; i >= 0; i--)
			{
				Thing thing = Building_PassengerShuttle.tmpContainedThings[i];
				if (this.RefuelableComp.Props.fuelFilter.Allows(thing))
				{
					Thing thing2 = thing.SplitOff(Mathf.Min(num, thing.stackCount));
					num -= thing2.stackCount;
				}
				if (num <= 0)
				{
					break;
				}
			}
		}

		// Token: 0x04006315 RID: 25365
		private string shuttleName;

		// Token: 0x04006316 RID: 25366
		private CompRefuelable cachedRefuelableComp;

		// Token: 0x04006317 RID: 25367
		private CompLaunchable cachedLaunchableComp;

		// Token: 0x04006318 RID: 25368
		private CompTransporter cachedTransporterComp;

		// Token: 0x04006319 RID: 25369
		private CompShuttle cachedShuttleComp;

		// Token: 0x0400631A RID: 25370
		public static CachedTexture RefuelFromCargoIcon = new CachedTexture("UI/Commands/RefuelPassengerShuttle");

		// Token: 0x0400631B RID: 25371
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
