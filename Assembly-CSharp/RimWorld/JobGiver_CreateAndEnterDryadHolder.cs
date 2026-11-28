using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BD0 RID: 3024
	public abstract class JobGiver_CreateAndEnterDryadHolder : ThinkNode_JobGiver
	{
		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06005705 RID: 22277
		public abstract JobDef JobDef { get; }

		// Token: 0x06005706 RID: 22278 RVA: 0x00204D4C File Offset: 0x00202F4C
		public virtual bool ExtraValidator(Pawn pawn, CompTreeConnection connectionComp)
		{
			return false;
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x00204D50 File Offset: 0x00202F50
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return null;
			}
			if (pawn.connections == null || pawn.connections.ConnectedThings.NullOrEmpty<Thing>())
			{
				return null;
			}
			Predicate<IntVec3> <>9__0;
			foreach (Thing thing in pawn.connections.ConnectedThings)
			{
				CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && this.ExtraValidator(pawn, compTreeConnection) && pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					IntVec3 position = thing.Position;
					Map map = pawn.Map;
					int squareRadius = 4;
					Predicate<IntVec3> validator;
					if ((validator = <>9__0) == null)
					{
						validator = (<>9__0 = ((IntVec3 c) => GauranlenUtility.CocoonAndPodCellValidator(c, pawn.Map, null)));
					}
					IntVec3 intVec;
					if (CellFinder.TryFindRandomCellNear(position, map, squareRadius, validator, out intVec, -1))
					{
						return JobMaker.MakeJob(this.JobDef, thing);
					}
				}
			}
			return null;
		}

		// Token: 0x0400332A RID: 13098
		public const int SquareRadius = 4;
	}
}
