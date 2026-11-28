using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D5 RID: 2517
	public class JobDriver_Uninstall : JobDriver_RemoveBuilding
	{
		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06004C69 RID: 19561 RVA: 0x001D6F3A File Offset: 0x001D513A
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06004C6A RID: 19562 RVA: 0x001D6F44 File Offset: 0x001D5144
		protected override float TotalNeededWork
		{
			get
			{
				return base.TargetA.Thing.def.building.uninstallWork;
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06004C6B RID: 19563 RVA: 0x001D6F6E File Offset: 0x001D516E
		protected override EffecterDef WorkEffecter
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x001D6F71 File Offset: 0x001D5171
		protected override void FinishedRemoving()
		{
			base.Building.Uninstall();
			this.pawn.records.Increment(RecordDefOf.ThingsUninstalled);
		}
	}
}
