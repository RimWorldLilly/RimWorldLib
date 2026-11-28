using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B1F RID: 6943
	[StaticConstructorOnStartup]
	public class AncientHatch : MapPortal
	{
		// Token: 0x1700200B RID: 8203
		// (get) Token: 0x0600B1CD RID: 45517 RVA: 0x003EA4C8 File Offset: 0x003E86C8
		private CompHackable Hackable
		{
			get
			{
				CompHackable result;
				if ((result = this.hackableInt) == null)
				{
					result = (this.hackableInt = base.GetComp<CompHackable>());
				}
				return result;
			}
		}

		// Token: 0x0600B1CE RID: 45518 RVA: 0x003EA4EE File Offset: 0x003E86EE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<TileMutatorWorker_Stockpile.StockpileType>(ref this.stockpileType, "stockpileType", TileMutatorWorker_Stockpile.StockpileType.Medicine, false);
			Scribe_Defs.Look<LayoutDef>(ref this.layout, "layout");
		}

		// Token: 0x0600B1CF RID: 45519 RVA: 0x003EA518 File Offset: 0x003E8718
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.openGraphicData = new GraphicData();
			this.openGraphicData.CopyFrom(this.def.graphicData);
			this.openGraphicData.texPath = "Things/Building/AncientHatch/AncientHatch_Open";
		}

		// Token: 0x0600B1D0 RID: 45520 RVA: 0x003EA554 File Offset: 0x003E8754
		public override void Print(SectionLayer layer)
		{
			string text;
			if (this.IsEnterable(out text))
			{
				this.openGraphicData.Graphic.Print(layer, this, 0f);
				return;
			}
			this.Graphic.Print(layer, this, 0f);
		}

		// Token: 0x0600B1D1 RID: 45521 RVA: 0x003EA595 File Offset: 0x003E8795
		protected override IEnumerable<GenStepWithParams> GetExtraGenSteps()
		{
			AncientHatch.<GetExtraGenSteps>d__10 <GetExtraGenSteps>d__ = new AncientHatch.<GetExtraGenSteps>d__10(-2);
			<GetExtraGenSteps>d__.<>4__this = this;
			return <GetExtraGenSteps>d__;
		}

		// Token: 0x0600B1D2 RID: 45522 RVA: 0x003EA5A5 File Offset: 0x003E87A5
		public override bool IsEnterable(out string reason)
		{
			if (!this.Hackable.IsHacked)
			{
				reason = "Locked".Translate();
				return false;
			}
			return base.IsEnterable(out reason);
		}

		// Token: 0x0600B1D3 RID: 45523 RVA: 0x003EA5D0 File Offset: 0x003E87D0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
			if (this.Hackable.IsHacked)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("HatchUnlocked".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B1D4 RID: 45524 RVA: 0x003EA619 File Offset: 0x003E8819
		public override IEnumerable<Gizmo> GetGizmos()
		{
			AncientHatch.<GetGizmos>d__13 <GetGizmos>d__ = new AncientHatch.<GetGizmos>d__13(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x040066A9 RID: 26281
		public TileMutatorWorker_Stockpile.StockpileType stockpileType;

		// Token: 0x040066AA RID: 26282
		public LayoutDef layout;

		// Token: 0x040066AB RID: 26283
		private CompHackable hackableInt;

		// Token: 0x040066AC RID: 26284
		private GraphicData openGraphicData;

		// Token: 0x040066AD RID: 26285
		private const string OpenTexturePath = "Things/Building/AncientHatch/AncientHatch_Open";
	}
}
