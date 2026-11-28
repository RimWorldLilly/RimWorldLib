using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A92 RID: 6802
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x17001E88 RID: 7816
		// (get) Token: 0x0600ABA1 RID: 43937 RVA: 0x003C9CE1 File Offset: 0x003C7EE1
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x17001E89 RID: 7817
		// (get) Token: 0x0600ABA2 RID: 43938 RVA: 0x003C9CF0 File Offset: 0x003C7EF0
		private VacuumComponent Vacuum
		{
			get
			{
				VacuumComponent result;
				if ((result = this.intVacuum) == null)
				{
					result = (this.intVacuum = base.Map.GetComponent<VacuumComponent>());
				}
				return result;
			}
		}

		// Token: 0x17001E8A RID: 7818
		// (get) Token: 0x0600ABA3 RID: 43939 RVA: 0x003C9D1B File Offset: 0x003C7F1B
		public override bool ExchangeVacuum
		{
			get
			{
				return base.ExchangeVacuum || FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x0600ABA4 RID: 43940 RVA: 0x003C9D2D File Offset: 0x003C7F2D
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x0600ABA5 RID: 43941 RVA: 0x003C9D43 File Offset: 0x003C7F43
		public override void TickRare()
		{
			if (!FlickUtility.WantsToBeOn(this))
			{
				return;
			}
			GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			base.Map.gasGrid.EqualizeGasThroughBuilding(this, true);
		}

		// Token: 0x0600ABA6 RID: 43942 RVA: 0x003C9D6C File Offset: 0x003C7F6C
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOn" || signal == "FlickedOff")
			{
				this.Vacuum.Dirty();
			}
		}

		// Token: 0x0600ABA7 RID: 43943 RVA: 0x003C9D94 File Offset: 0x003C7F94
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (!FlickUtility.WantsToBeOn(this))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("VentClosed".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040063ED RID: 25581
		private CompFlickable flickableComp;

		// Token: 0x040063EE RID: 25582
		private VacuumComponent intVacuum;
	}
}
