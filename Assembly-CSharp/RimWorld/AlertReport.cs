using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B9E RID: 7070
	public struct AlertReport
	{
		// Token: 0x170020CE RID: 8398
		// (get) Token: 0x0600B584 RID: 46468 RVA: 0x003FE918 File Offset: 0x003FCB18
		public bool AnyCulpritValid
		{
			get
			{
				if (!this.culpritsThings.NullOrEmpty<Thing>() || !this.culpritsPawns.NullOrEmpty<Pawn>() || !this.culpritsCaravans.NullOrEmpty<Caravan>())
				{
					return true;
				}
				if (this.culpritTarget != null && this.culpritTarget.Value.IsValid)
				{
					return true;
				}
				if (this.culpritsTargets != null)
				{
					for (int i = 0; i < this.culpritsTargets.Count; i++)
					{
						if (this.culpritsTargets[i].IsValid)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170020CF RID: 8399
		// (get) Token: 0x0600B585 RID: 46469 RVA: 0x003FE9A9 File Offset: 0x003FCBA9
		public IEnumerable<GlobalTargetInfo> AllCulprits
		{
			get
			{
				AlertReport.<get_AllCulprits>d__9 <get_AllCulprits>d__ = new AlertReport.<get_AllCulprits>d__9(-2);
				<get_AllCulprits>d__.<>3__<>4__this = this;
				return <get_AllCulprits>d__;
			}
		}

		// Token: 0x0600B586 RID: 46470 RVA: 0x003FE9C0 File Offset: 0x003FCBC0
		public static AlertReport CulpritIs(GlobalTargetInfo culp)
		{
			AlertReport result = default(AlertReport);
			result.active = culp.IsValid;
			if (culp.IsValid)
			{
				result.culpritTarget = new GlobalTargetInfo?(culp);
			}
			return result;
		}

		// Token: 0x0600B587 RID: 46471 RVA: 0x003FE9FC File Offset: 0x003FCBFC
		public static AlertReport CulpritsAre(List<Thing> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsThings = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x0600B588 RID: 46472 RVA: 0x003FEA28 File Offset: 0x003FCC28
		public static AlertReport CulpritsAre(List<Pawn> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsPawns = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x0600B589 RID: 46473 RVA: 0x003FEA54 File Offset: 0x003FCC54
		public static AlertReport CulpritsAre(List<Caravan> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsCaravans = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x0600B58A RID: 46474 RVA: 0x003FEA80 File Offset: 0x003FCC80
		public static AlertReport CulpritsAre(List<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsTargets = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x0600B58B RID: 46475 RVA: 0x003FEAAC File Offset: 0x003FCCAC
		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		// Token: 0x0600B58C RID: 46476 RVA: 0x003FEACA File Offset: 0x003FCCCA
		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x0600B58D RID: 46477 RVA: 0x003FEAD7 File Offset: 0x003FCCD7
		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x0600B58E RID: 46478 RVA: 0x003FEAE4 File Offset: 0x003FCCE4
		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x170020D0 RID: 8400
		// (get) Token: 0x0600B58F RID: 46479 RVA: 0x003FEAEC File Offset: 0x003FCCEC
		public static AlertReport Active
		{
			get
			{
				return new AlertReport
				{
					active = true
				};
			}
		}

		// Token: 0x170020D1 RID: 8401
		// (get) Token: 0x0600B590 RID: 46480 RVA: 0x003FEB0C File Offset: 0x003FCD0C
		public static AlertReport Inactive
		{
			get
			{
				return new AlertReport
				{
					active = false
				};
			}
		}

		// Token: 0x04006868 RID: 26728
		public bool active;

		// Token: 0x04006869 RID: 26729
		public List<Thing> culpritsThings;

		// Token: 0x0400686A RID: 26730
		public List<Pawn> culpritsPawns;

		// Token: 0x0400686B RID: 26731
		public List<Caravan> culpritsCaravans;

		// Token: 0x0400686C RID: 26732
		public List<GlobalTargetInfo> culpritsTargets;

		// Token: 0x0400686D RID: 26733
		public GlobalTargetInfo? culpritTarget;
	}
}
