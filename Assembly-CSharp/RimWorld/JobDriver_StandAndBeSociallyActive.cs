using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009EB RID: 2539
	public class JobDriver_StandAndBeSociallyActive : JobDriver
	{
		// Token: 0x06004CEB RID: 19691 RVA: 0x001D86A4 File Offset: 0x001D68A4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004CEC RID: 19692 RVA: 0x001D86A7 File Offset: 0x001D68A7
		protected virtual Toil GetGotoToil()
		{
			return Toils_Goto.GotoCell(base.TargetLocA, PathEndMode.OnCell);
		}

		// Token: 0x06004CED RID: 19693 RVA: 0x001D86B5 File Offset: 0x001D68B5
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_StandAndBeSociallyActive.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_StandAndBeSociallyActive.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x001D86C8 File Offset: 0x001D68C8
		public static Pawn FindClosePawn(Pawn pawn)
		{
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			for (int i = 0; i < 24; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					Thing thing = intVec.GetThingList(map).Find((Thing x) => x is Pawn);
					if (thing != null && thing != pawn && GenSight.LineOfSight(position, intVec, map))
					{
						return (Pawn)thing;
					}
				}
			}
			return null;
		}
	}
}
