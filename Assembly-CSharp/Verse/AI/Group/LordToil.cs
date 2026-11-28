using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000942 RID: 2370
	public abstract class LordToil : IDisposable
	{
		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x060048DA RID: 18650 RVA: 0x001C8D8C File Offset: 0x001C6F8C
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x060048DB RID: 18651 RVA: 0x001C8D9E File Offset: 0x001C6F9E
		public virtual IntVec3 FlagLoc
		{
			get
			{
				return IntVec3.Invalid;
			}
		}

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x060048DC RID: 18652 RVA: 0x001C8DA5 File Offset: 0x001C6FA5
		public virtual bool AllowSatisfyLongNeeds
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x060048DD RID: 18653 RVA: 0x001C8DA8 File Offset: 0x001C6FA8
		public virtual float? CustomWakeThreshold
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x060048DE RID: 18654 RVA: 0x001C8DBE File Offset: 0x001C6FBE
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x060048DF RID: 18655 RVA: 0x001C8DC1 File Offset: 0x001C6FC1
		public virtual bool AllowAggressiveTargetingOfRoamers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x060048E0 RID: 18656 RVA: 0x001C8DC4 File Offset: 0x001C6FC4
		public virtual bool AllowSelfTend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x060048E1 RID: 18657 RVA: 0x001C8DC8 File Offset: 0x001C6FC8
		public virtual bool ShouldFail
		{
			get
			{
				for (int i = 0; i < this.failConditions.Count; i++)
				{
					if (this.failConditions[i]())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x060048E2 RID: 18658 RVA: 0x001C8E01 File Offset: 0x001C7001
		public virtual bool ForceHighStoryDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x060048E3 RID: 18659 RVA: 0x001C8E04 File Offset: 0x001C7004
		public virtual bool AssignsDuties
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x001C8E07 File Offset: 0x001C7007
		public virtual void Init()
		{
		}

		// Token: 0x060048E5 RID: 18661
		public abstract void UpdateAllDuties();

		// Token: 0x060048E6 RID: 18662 RVA: 0x001C8E09 File Offset: 0x001C7009
		public virtual void LordToilTick()
		{
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x001C8E0B File Offset: 0x001C700B
		public virtual void Cleanup()
		{
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x001C8E0D File Offset: 0x001C700D
		public virtual ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.None;
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x001C8E10 File Offset: 0x001C7010
		public virtual void DrawPawnGUIOverlay(Pawn pawn)
		{
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x001C8E12 File Offset: 0x001C7012
		public virtual IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn target, Pawn forPawn)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x001C8E19 File Offset: 0x001C7019
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x001C8E20 File Offset: 0x001C7020
		public virtual IEnumerable<Gizmo> GetBuildingGizmos(Building building)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x001C8E27 File Offset: 0x001C7027
		public virtual bool CanAddPawn(Pawn p)
		{
			return true;
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x001C8E2A File Offset: 0x001C702A
		public virtual void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x001C8E2C File Offset: 0x001C702C
		public virtual void Notify_PawnJobDone(Pawn p, JobCondition condition)
		{
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x001C8E2E File Offset: 0x001C702E
		public virtual void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x001C8E30 File Offset: 0x001C7030
		public virtual void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x001C8E32 File Offset: 0x001C7032
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x001C8E34 File Offset: 0x001C7034
		public virtual void Notify_CorpseLost(Corpse c)
		{
		}

		// Token: 0x060048F4 RID: 18676 RVA: 0x001C8E36 File Offset: 0x001C7036
		public virtual void Notify_ReachedDutyLocation(Pawn pawn)
		{
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x001C8E38 File Offset: 0x001C7038
		public virtual void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x001C8E3A File Offset: 0x001C703A
		public virtual void Notify_ConstructionCompleted(Pawn pawn, Building building)
		{
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x001C8E3C File Offset: 0x001C703C
		public virtual void Notify_BuildingSpawnedOnMap(Building b)
		{
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x001C8E3E File Offset: 0x001C703E
		public virtual void Notify_BuildingDespawnedOnMap(Building b)
		{
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x001C8E40 File Offset: 0x001C7040
		public void AddFailCondition(Func<bool> failCondition)
		{
			this.failConditions.Add(failCondition);
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x001C8E50 File Offset: 0x001C7050
		public override string ToString()
		{
			string text = base.GetType().ToString();
			if (text.Contains('.'))
			{
				text = text.Substring(text.LastIndexOf('.') + 1);
			}
			if (text.Contains('_'))
			{
				text = text.Substring(text.LastIndexOf('_') + 1);
			}
			return text;
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x001C8EA0 File Offset: 0x001C70A0
		public void Dispose()
		{
			IDisposable disposable = this.data as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x04002EE6 RID: 12006
		public Lord lord;

		// Token: 0x04002EE7 RID: 12007
		public LordToilData data;

		// Token: 0x04002EE8 RID: 12008
		private List<Func<bool>> failConditions = new List<Func<bool>>();

		// Token: 0x04002EE9 RID: 12009
		public bool useAvoidGrid;
	}
}
