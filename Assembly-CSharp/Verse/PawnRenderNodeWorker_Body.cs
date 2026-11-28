using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020003C2 RID: 962
	public class PawnRenderNodeWorker_Body : PawnRenderNodeWorker
	{
		// Token: 0x06001C9C RID: 7324 RVA: 0x000B0180 File Offset: 0x000AE380
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			if (!base.CanDrawNow(node, parms))
			{
				return false;
			}
			if (!parms.Portrait)
			{
				if (parms.flags.FlagSet(PawnRenderFlags.NoBody))
				{
					return false;
				}
				if (parms.posture == PawnPosture.Standing)
				{
					return true;
				}
				Pawn_MindState mindState = parms.pawn.mindState;
				bool flag;
				if (mindState == null)
				{
					flag = false;
				}
				else
				{
					PawnDuty duty = mindState.duty;
					bool? flag2;
					if (duty == null)
					{
						flag2 = null;
					}
					else
					{
						DutyDef def = duty.def;
						flag2 = ((def != null) ? new bool?(def.drawBodyOverride != null) : null);
					}
					bool? flag3 = flag2;
					flag = flag3.GetValueOrDefault();
				}
				if (flag)
				{
					return parms.pawn.mindState.duty.def.drawBodyOverride.Value;
				}
				if (parms.bed != null && parms.pawn.RaceProps.Humanlike)
				{
					return parms.bed.def.building.bed_showSleeperBody;
				}
			}
			return true;
		}
	}
}
