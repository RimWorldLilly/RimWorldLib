using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B75 RID: 2933
	public class JobGiver_GetJoyInBed : JobGiver_GetJoy
	{
		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x0600554B RID: 21835 RVA: 0x001FB6A7 File Offset: 0x001F98A7
		protected override bool CanDoDuringMedicalRest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600554C RID: 21836 RVA: 0x001FB6AA File Offset: 0x001F98AA
		protected override bool JoyGiverAllowed(JoyGiverDef def)
		{
			return def.canDoWhileInBed;
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x001FB6B2 File Offset: 0x001F98B2
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJobWhileInBed(pawn);
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x001FB6C0 File Offset: 0x001F98C0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.CurJob == null || !pawn.InBed() || !pawn.Awake() || pawn.needs.joy == null)
			{
				return null;
			}
			if (pawn.health.hediffSet.InLabor(true))
			{
				return null;
			}
			if (pawn.needs.joy.CurLevel > 0.5f)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x040032CA RID: 13002
		private const float MaxJoyLevel = 0.5f;
	}
}
