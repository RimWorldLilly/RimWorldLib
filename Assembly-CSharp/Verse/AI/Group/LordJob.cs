using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Unity.Collections;

namespace Verse.AI.Group
{
	// Token: 0x0200093A RID: 2362
	public abstract class LordJob : IExposable, IDisposable
	{
		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x0600487F RID: 18559 RVA: 0x001C7E50 File Offset: 0x001C6050
		public virtual bool LostImportantReferenceDuringLoading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06004880 RID: 18560 RVA: 0x001C7E53 File Offset: 0x001C6053
		public virtual bool AllowStartNewGatherings
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x06004881 RID: 18561 RVA: 0x001C7E56 File Offset: 0x001C6056
		public virtual bool AllowStartNewRituals
		{
			get
			{
				return this.AllowStartNewGatherings;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06004882 RID: 18562 RVA: 0x001C7E5E File Offset: 0x001C605E
		public virtual bool NeverInRestraints
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06004883 RID: 18563 RVA: 0x001C7E61 File Offset: 0x001C6061
		public virtual bool GuiltyOnDowned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06004884 RID: 18564 RVA: 0x001C7E64 File Offset: 0x001C6064
		public virtual bool CanBlockHostileVisitors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06004885 RID: 18565 RVA: 0x001C7E67 File Offset: 0x001C6067
		public virtual bool AddFleeToil
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06004886 RID: 18566 RVA: 0x001C7E6A File Offset: 0x001C606A
		public virtual bool OrganizerIsStartingPawn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06004887 RID: 18567 RVA: 0x001C7E6D File Offset: 0x001C606D
		public virtual bool KeepExistingWhileHasAnyBuilding
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06004888 RID: 18568 RVA: 0x001C7E70 File Offset: 0x001C6070
		public virtual bool AlwaysShowWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06004889 RID: 18569 RVA: 0x001C7E73 File Offset: 0x001C6073
		public virtual bool IsCaravanSendable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x0600488A RID: 18570 RVA: 0x001C7E76 File Offset: 0x001C6076
		public virtual bool ManagesRopableAnimals
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x0600488B RID: 18571 RVA: 0x001C7E79 File Offset: 0x001C6079
		public virtual bool DontInterruptLayingPawnsOnCleanup
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x0600488C RID: 18572 RVA: 0x001C7E7C File Offset: 0x001C607C
		public virtual bool CanAutoAddPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x001C7E7F File Offset: 0x001C607F
		public virtual bool ShouldExistWithoutPawns
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x0600488E RID: 18574 RVA: 0x001C7E82 File Offset: 0x001C6082
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x0600488F RID: 18575
		public abstract StateGraph CreateGraph();

		// Token: 0x06004890 RID: 18576 RVA: 0x001C7E94 File Offset: 0x001C6094
		public virtual void LordJobTick()
		{
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x001C7E96 File Offset: 0x001C6096
		public virtual void ExposeData()
		{
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x001C7E98 File Offset: 0x001C6098
		public virtual void Cleanup()
		{
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x001C7E9A File Offset: 0x001C609A
		public virtual void PostCleanup()
		{
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x001C7E9C File Offset: 0x001C609C
		public virtual void Notify_AddedToLord()
		{
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x001C7E9E File Offset: 0x001C609E
		public virtual void Notify_PawnAdded(Pawn p)
		{
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x001C7EA0 File Offset: 0x001C60A0
		public virtual void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x001C7EA2 File Offset: 0x001C60A2
		public virtual void Notify_PawnJobDone(Pawn p, JobCondition condition)
		{
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x001C7EA4 File Offset: 0x001C60A4
		public virtual void Notify_InMentalState(Pawn pawn, MentalStateDef stateDef)
		{
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x001C7EA6 File Offset: 0x001C60A6
		public virtual void Notify_BuildingAdded(Building b)
		{
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x001C7EA8 File Offset: 0x001C60A8
		public virtual void Notify_CorpseAdded(Corpse c)
		{
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x001C7EAA File Offset: 0x001C60AA
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x0600489C RID: 18588 RVA: 0x001C7EAC File Offset: 0x001C60AC
		public virtual void Notify_CorpseLost(Corpse c)
		{
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x001C7EAE File Offset: 0x001C60AE
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x001C7EB0 File Offset: 0x001C60B0
		public virtual void Notify_MapRemoved()
		{
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x001C7EB2 File Offset: 0x001C60B2
		public virtual void Notify_PawnUndowned(Pawn p)
		{
			LordToil curLordToil = this.lord.CurLordToil;
			if (curLordToil == null)
			{
				return;
			}
			curLordToil.UpdateAllDuties();
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x001C7EC9 File Offset: 0x001C60C9
		public virtual void Notify_PawnDowned(Pawn p)
		{
			LordToil curLordToil = this.lord.CurLordToil;
			if (curLordToil == null)
			{
				return;
			}
			curLordToil.UpdateAllDuties();
		}

		// Token: 0x060048A1 RID: 18593 RVA: 0x001C7EE0 File Offset: 0x001C60E0
		public virtual ThinkResult Notify_DutyConstantResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			return result;
		}

		// Token: 0x060048A2 RID: 18594 RVA: 0x001C7EE3 File Offset: 0x001C60E3
		public virtual ThinkResult Notify_DutyResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			return result;
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x001C7EE6 File Offset: 0x001C60E6
		public virtual string GetJobReport(Pawn pawn)
		{
			return null;
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x001C7EE9 File Offset: 0x001C60E9
		public virtual string GetReport(Pawn pawn)
		{
			return null;
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x001C7EEC File Offset: 0x001C60EC
		public virtual bool CanOpenAnyDoor(Pawn p)
		{
			return false;
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x001C7EEF File Offset: 0x001C60EF
		public virtual bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return true;
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x001C7EF2 File Offset: 0x001C60F2
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x001C7EF9 File Offset: 0x001C60F9
		public virtual bool EndPawnJobOnCleanup(Pawn p)
		{
			return true;
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x001C7EFC File Offset: 0x001C60FC
		public virtual bool BlocksSocialInteraction(Pawn pawn)
		{
			return false;
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x001C7EFF File Offset: 0x001C60FF
		public virtual bool DutyActiveWhenDown(Pawn pawn)
		{
			return false;
		}

		// Token: 0x060048AB RID: 18603 RVA: 0x001C7F02 File Offset: 0x001C6102
		public virtual AcceptanceReport AllowsFloatMenu(Pawn pawn)
		{
			return true;
		}

		// Token: 0x060048AC RID: 18604 RVA: 0x001C7F0A File Offset: 0x001C610A
		public virtual bool PrisonerSecure(Pawn pawn)
		{
			return false;
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x001C7F0D File Offset: 0x001C610D
		public virtual AcceptanceReport AbilityAllowed(Ability ability)
		{
			return true;
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x001C7F15 File Offset: 0x001C6115
		public virtual AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			return true;
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x001C7F1D File Offset: 0x001C611D
		public virtual bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			return true;
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x001C7F20 File Offset: 0x001C6120
		public virtual NativeBitArray GetWalkGrid(Pawn pawn)
		{
			return default(NativeBitArray);
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x001C7F36 File Offset: 0x001C6136
		public virtual void Dispose()
		{
		}

		// Token: 0x04002ED2 RID: 11986
		public Lord lord;
	}
}
