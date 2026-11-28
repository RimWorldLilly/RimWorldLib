using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200229C RID: 8860
	public class QuestPart_DarkenMap : QuestPart
	{
		// Token: 0x0600DAC5 RID: 56005 RVA: 0x00517847 File Offset: 0x00515A47
		public QuestPart_DarkenMap()
		{
		}

		// Token: 0x0600DAC6 RID: 56006 RVA: 0x0051784F File Offset: 0x00515A4F
		public QuestPart_DarkenMap(string inSignal, MapParent mapParent)
		{
			this.inSignal = inSignal;
			this.mapParent = mapParent;
		}

		// Token: 0x0600DAC7 RID: 56007 RVA: 0x00517868 File Offset: 0x00515A68
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.mapParent.Map == null)
			{
				return;
			}
			if (signal.tag == this.inSignal)
			{
				this.mapParent.Map.gameConditionManager.SetTargetBrightness(0f, 5f);
			}
		}

		// Token: 0x0600DAC8 RID: 56008 RVA: 0x005178B8 File Offset: 0x00515AB8
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.mapParent.Map != null)
			{
				GameCondition_UnnaturalDarkness activeCondition = this.mapParent.Map.GameConditionManager.GetActiveCondition<GameCondition_UnnaturalDarkness>();
				if (activeCondition != null)
				{
					activeCondition.Permanent = false;
					activeCondition.TicksLeft = 301;
				}
				this.mapParent.Map.gameConditionManager.SetTargetBrightness(1f, 5f);
			}
		}

		// Token: 0x0600DAC9 RID: 56009 RVA: 0x00517922 File Offset: 0x00515B22
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x04008C00 RID: 35840
		public string inSignal;

		// Token: 0x04008C01 RID: 35841
		public MapParent mapParent;
	}
}
