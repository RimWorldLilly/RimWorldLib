using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AAC RID: 2732
	public class JobDriver_DropEquipment : JobDriver
	{
		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x0600524C RID: 21068 RVA: 0x001EAC34 File Offset: 0x001E8E34
		private ThingWithComps TargetEquipment
		{
			get
			{
				return (ThingWithComps)base.TargetA.Thing;
			}
		}

		// Token: 0x0600524D RID: 21069 RVA: 0x001EAC54 File Offset: 0x001E8E54
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600524E RID: 21070 RVA: 0x001EAC57 File Offset: 0x001E8E57
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DropEquipment.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_DropEquipment.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040031C8 RID: 12744
		private const int DurationTicks = 30;
	}
}
