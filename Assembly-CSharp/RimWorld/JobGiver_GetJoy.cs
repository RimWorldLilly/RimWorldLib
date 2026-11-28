using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B74 RID: 2932
	public class JobGiver_GetJoy : ThinkNode_JobGiver
	{
		// Token: 0x17000F10 RID: 3856
		// (get) Token: 0x06005544 RID: 21828 RVA: 0x001FB4CF File Offset: 0x001F96CF
		protected virtual bool CanDoDuringMedicalRest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x001FB4D2 File Offset: 0x001F96D2
		protected virtual bool JoyGiverAllowed(JoyGiverDef def)
		{
			return true;
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x001FB4D5 File Offset: 0x001F96D5
		protected virtual Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJob(pawn);
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x001FB4E3 File Offset: 0x001F96E3
		public override void ResolveReferences()
		{
			this.joyGiverChances = new DefMap<JoyGiverDef, float>();
		}

		// Token: 0x06005548 RID: 21832 RVA: 0x001FB4F0 File Offset: 0x001F96F0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!this.CanDoDuringMedicalRest && pawn.InBed() && HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				return null;
			}
			if (pawn.needs.joy.CurLevel >= 0.99f)
			{
				return null;
			}
			List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
			JoyToleranceSet tolerances = pawn.needs.joy.tolerances;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyGiverDef joyGiverDef = allDefsListForReading[i];
				this.joyGiverChances[joyGiverDef] = 0f;
				if (this.JoyGiverAllowed(joyGiverDef) && !pawn.needs.joy.tolerances.BoredOf(joyGiverDef.joyKind) && joyGiverDef.Worker.CanBeGivenTo(pawn))
				{
					if (joyGiverDef.pctPawnsEverDo < 1f)
					{
						Rand.PushState(pawn.thingIDNumber ^ 63216713);
						if (Rand.Value >= joyGiverDef.pctPawnsEverDo)
						{
							Rand.PopState();
							goto IL_133;
						}
						Rand.PopState();
					}
					float num = tolerances[joyGiverDef.joyKind];
					float num2 = Mathf.Pow(1f - num, 5f);
					num2 = Mathf.Max(0.001f, num2);
					this.joyGiverChances[joyGiverDef] = joyGiverDef.Worker.GetChance(pawn) * num2;
				}
				IL_133:;
			}
			int num3 = 0;
			JoyGiverDef def;
			while (num3 < this.joyGiverChances.Count && allDefsListForReading.TryRandomElementByWeight((JoyGiverDef d) => this.joyGiverChances[d], out def))
			{
				Job job = this.TryGiveJobFromJoyGiverDefDirect(def, pawn);
				if (job != null)
				{
					return job;
				}
				this.joyGiverChances[def] = 0f;
				num3++;
			}
			return null;
		}

		// Token: 0x040032C8 RID: 13000
		[Unsaved(false)]
		private DefMap<JoyGiverDef, float> joyGiverChances;

		// Token: 0x040032C9 RID: 13001
		private const float JoyBuffer = 0.99f;
	}
}
