using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D20 RID: 7456
	public class FloatMenuOptionProvider_CleanRoom : FloatMenuOptionProvider
	{
		// Token: 0x17002338 RID: 9016
		// (get) Token: 0x0600C0E4 RID: 49380 RVA: 0x00465366 File Offset: 0x00463566
		protected override bool Drafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002339 RID: 9017
		// (get) Token: 0x0600C0E5 RID: 49381 RVA: 0x00465369 File Offset: 0x00463569
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700233A RID: 9018
		// (get) Token: 0x0600C0E6 RID: 49382 RVA: 0x0046536C File Offset: 0x0046356C
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700233B RID: 9019
		// (get) Token: 0x0600C0E7 RID: 49383 RVA: 0x0046536F File Offset: 0x0046356F
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0E8 RID: 49384 RVA: 0x00465374 File Offset: 0x00463574
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.ClickedRoom != null && !context.ClickedRoom.PsychologicallyOutdoors && context.ClickedRoom.ProperRoom && !context.ClickedRoom.TouchesMapEdge && !context.ClickedRoom.IsHuge && (!context.map.IsPocketMap || context.map.generatorDef.pocketMapProperties.canBeCleaned) && !context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Cleaning);
		}

		// Token: 0x0600C0E9 RID: 49385 RVA: 0x004653FC File Offset: 0x004635FC
		protected override FloatMenuOption GetSingleOption(FloatMenuContext context)
		{
			List<Filth> filth = FloatMenuOptionProvider_CleanRoom.GetRoomFilthCleanableByPawn(context.ClickedRoom, context.FirstSelectedPawn);
			if (filth.Count == 0)
			{
				return null;
			}
			string roomRoleLabel = context.ClickedRoom.GetRoomRoleLabel();
			return new FloatMenuOption("CleanRoom".Translate(roomRoleLabel), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.Clean);
				foreach (Filth t in filth)
				{
					job.AddQueuedTarget(TargetIndex.A, t);
				}
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.VeryLow, null, null, 0f, null, null, true, 0);
		}

		// Token: 0x0600C0EA RID: 49386 RVA: 0x0046548C File Offset: 0x0046368C
		private static List<Filth> GetRoomFilthCleanableByPawn(Room room, Pawn pawn)
		{
			if (pawn.Map.IsPocketMap && !pawn.Map.generatorDef.pocketMapProperties.canBeCleaned)
			{
				return null;
			}
			if (room == null || room.Dereferenced || room.Fogged)
			{
				return null;
			}
			if (room.IsHuge || room.TouchesMapEdge)
			{
				return null;
			}
			FloatMenuOptionProvider_CleanRoom.tmpFilth.Clear();
			foreach (Filth filth in room.ContainedAndAdjacentThings.OfType<Filth>())
			{
				if (!filth.IsForbidden(pawn) && filth.Map.areaManager.Home[filth.Position] && pawn.CanReach(filth, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn) && pawn.CanReserve(filth, 1, -1, null, false))
				{
					FloatMenuOptionProvider_CleanRoom.tmpFilth.Add(filth);
				}
			}
			return FloatMenuOptionProvider_CleanRoom.tmpFilth;
		}

		// Token: 0x04006ED6 RID: 28374
		private static List<Filth> tmpFilth = new List<Filth>();
	}
}
