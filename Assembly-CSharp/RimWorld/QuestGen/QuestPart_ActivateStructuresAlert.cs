using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022A8 RID: 8872
	public class QuestPart_ActivateStructuresAlert : QuestPart_Alert
	{
		// Token: 0x0600DB0B RID: 56075 RVA: 0x005198F0 File Offset: 0x00517AF0
		public QuestPart_ActivateStructuresAlert()
		{
		}

		// Token: 0x0600DB0C RID: 56076 RVA: 0x005198F8 File Offset: 0x00517AF8
		public QuestPart_ActivateStructuresAlert(List<Thing> structures, string inSignalEnable, string inSignalDisable)
		{
			this.structures = structures;
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
			this.lookTargets = structures;
		}

		// Token: 0x1700262B RID: 9771
		// (get) Token: 0x0600DB0D RID: 56077 RVA: 0x00519924 File Offset: 0x00517B24
		public override string AlertLabel
		{
			get
			{
				return "StructuresActivatedAlertLabel".Translate() + ": " + this.structures.Count(delegate(Thing s)
				{
					bool? flag;
					if (s == null)
					{
						flag = null;
					}
					else
					{
						CompVoidStructure compVoidStructure = s.TryGetComp<CompVoidStructure>();
						flag = ((compVoidStructure != null) ? new bool?(compVoidStructure.Active) : null);
					}
					bool? flag2 = flag;
					return flag2.GetValueOrDefault();
				}).ToString() + "/" + this.structures.Count.ToString();
			}
		}

		// Token: 0x1700262C RID: 9772
		// (get) Token: 0x0600DB0E RID: 56078 RVA: 0x00519999 File Offset: 0x00517B99
		public override string AlertExplanation
		{
			get
			{
				return "StructuresActivatedAlertDescription".Translate();
			}
		}

		// Token: 0x0600DB0F RID: 56079 RVA: 0x005199AA File Offset: 0x00517BAA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.structures, "structures", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0600DB10 RID: 56080 RVA: 0x005199C8 File Offset: 0x00517BC8
		public override void Cleanup()
		{
			base.Cleanup();
			this.structures.Clear();
		}

		// Token: 0x04008C3C RID: 35900
		private List<Thing> structures;
	}
}
