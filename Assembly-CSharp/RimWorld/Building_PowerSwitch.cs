using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A66 RID: 6758
	[StaticConstructorOnStartup]
	public class Building_PowerSwitch : Building
	{
		// Token: 0x17001DDC RID: 7644
		// (get) Token: 0x0600A9D0 RID: 43472 RVA: 0x003C350D File Offset: 0x003C170D
		public override bool TransmitsPowerNow
		{
			get
			{
				return FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x17001DDD RID: 7645
		// (get) Token: 0x0600A9D1 RID: 43473 RVA: 0x003C3515 File Offset: 0x003C1715
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x0600A9D2 RID: 43474 RVA: 0x003C3522 File Offset: 0x003C1722
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x0600A9D3 RID: 43475 RVA: 0x003C3538 File Offset: 0x003C1738
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.flickableComp == null)
				{
					this.flickableComp = base.GetComp<CompFlickable>();
				}
				this.wantsOnOld = !FlickUtility.WantsToBeOn(this);
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x0600A9D4 RID: 43476 RVA: 0x003C3571 File Offset: 0x003C1771
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "FlickedOn" || signal == "ScheduledOn" || signal == "ScheduledOff")
			{
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x0600A9D5 RID: 43477 RVA: 0x003C35B0 File Offset: 0x003C17B0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("PowerSwitch_Power".Translate() + ": ");
			if (FlickUtility.WantsToBeOn(this))
			{
				stringBuilder.Append("On".Translate().ToLower());
			}
			else
			{
				stringBuilder.Append("Off".Translate().ToLower());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A9D6 RID: 43478 RVA: 0x003C364B File Offset: 0x003C184B
		private void UpdatePowerGrid()
		{
			if (FlickUtility.WantsToBeOn(this) != this.wantsOnOld)
			{
				if (base.Spawned)
				{
					base.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(base.PowerComp);
				}
				this.wantsOnOld = FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x04006361 RID: 25441
		private bool wantsOnOld = true;

		// Token: 0x04006362 RID: 25442
		private CompFlickable flickableComp;
	}
}
